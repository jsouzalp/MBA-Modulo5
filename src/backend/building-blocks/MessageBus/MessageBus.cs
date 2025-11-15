using Core.Messages;
using Core.Messages.Integration;
using EasyNetQ;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace MessageBus;

[ExcludeFromCodeCoverage]
public class MessageBus : IMessageBus
{
    private IBus? _bus;

    private readonly string _connectionString;

    private readonly int _requestTimeoutSeconds;

    public MessageBus(string connectionString)
    {
        _connectionString = connectionString;
        _requestTimeoutSeconds = 120;
        TryConnect();
    }

    public bool IsConnected => _bus?.IsConnected ?? false;
    public IAdvancedBus AdvancedBus => _bus?.Advanced!;

    public void Publish<T>(T message) where T : IntegrationEvent
    {
        TryConnect();

        if (message is AlunoRegistradoIntegrationEvent)
        {
            _bus?.Publish(message);
        }
        else
        {
            _bus?.Publish(message);
        }
    }

    public async Task PublishAsync<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        if (_bus != null)
            await _bus.PublishAsync(message);
    }

    public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
    {
        TryConnect();
        _bus?.Subscribe(subscriptionId, onMessage);
    }

    public void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
    {
        TryConnect();
        _bus?.SubscribeAsync(subscriptionId, onMessage);
    }

    public TResponse Request<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus?.Request<TRequest, TResponse>(request) ?? throw new InvalidOperationException("Bus not connected");
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();

        if (_bus == null)
            throw new InvalidOperationException("Bus not connected");

        var retryPolicy = Policy.Handle<TimeoutException>()
            .Or<EasyNetQ.EasyNetQException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryAttempt))));

        return await retryPolicy.ExecuteAsync(async () =>
        {
            var requestTask = _bus.RequestAsync<TRequest, TResponse>(request);
            var completed = await Task.WhenAny(requestTask, Task.Delay(TimeSpan.FromSeconds(_requestTimeoutSeconds)));
            if (completed == requestTask)
            {
                return await requestTask;
            }
            throw new TimeoutException($"Request timed out after {_requestTimeoutSeconds} seconds.");
        });
    }

    public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder) where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus?.Respond(responder) ?? throw new InvalidOperationException("Bus not connected");
    }

    public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder) where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus?.RespondAsync(responder) ?? throw new InvalidOperationException("Bus not connected");
    }

    private void TryConnect()
    {
        if (IsConnected) return;

        var policy = Policy.Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        policy.Execute(() =>
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
        });
    }

    public void Dispose() => _bus?.Dispose();
}
