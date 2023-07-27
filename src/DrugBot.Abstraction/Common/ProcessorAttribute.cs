using JetBrains.Annotations;

namespace DrugBot.Core.Common;

[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse]
public class ProcessorAttribute : Attribute
{
}