using Core.DomainObjects;

namespace Core.Data;

public interface IRepository<T> : IDisposable where T : IRaizAgregacao
{
    IUnitOfWork UnitOfWork { get; }
}