using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Core.Utils;

[ExcludeFromCodeCoverage]
public static class EnumeratorExtension
{
    public static string ObterDescricao(this Enum value)
    {
        if (value is null) return "Não informado";

        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo is null) return string.Empty;

        var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
        return !string.IsNullOrEmpty(description) ? description : value.ToString();
    }
}
