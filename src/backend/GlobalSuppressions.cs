// 🎯 SUPRESSÕES GLOBAIS INTELIGENTES
// Este arquivo define supressões de análise de código que se aplicam a toda a solution
// Usado para suprimir warnings específicos que são aceitáveis no contexto do projeto

using System.Diagnostics.CodeAnalysis;

// 🔇 NULLABLE REFERENCE TYPES - Supressões específicas para casos legítimos
[assembly: SuppressMessage("Nullable", "CS8618:Non-nullable field must contain a non-null value when exiting constructor", 
    Justification = "Propriedades são inicializadas pelo Entity Framework ou por construtores específicos")]

[assembly: SuppressMessage("Nullable", "CS8632:The annotation for nullable reference types should only be used in code within a '#nullable' annotations context", 
    Justification = "EditorConfig gerencia o contexto nullable globalmente")]

// 🧪 TESTES - Supressões para projetos de teste
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", 
    Scope = "namespaceanddescendants", Target = "~N:*.Tests", 
    Justification = "Métodos de teste não precisam ser estáticos")]

// 📝 DOCUMENTAÇÃO - Supressões para membros internos
[assembly: SuppressMessage("Documentation", "CS1591:Missing XML comment for publicly visible type or member", 
    Scope = "namespaceanddescendants", Target = "~N:*.Infrastructure", 
    Justification = "Infraestrutura não requer documentação XML pública")]
