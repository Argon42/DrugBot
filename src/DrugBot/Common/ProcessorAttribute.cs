using System;
using JetBrains.Annotations;

namespace DrugBot.Common;

[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse]
public class ProcessorAttribute : Attribute
{
}