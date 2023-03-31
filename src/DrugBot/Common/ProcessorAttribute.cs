using System;
using JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse]
public class ProcessorAttribute : Attribute
{
}