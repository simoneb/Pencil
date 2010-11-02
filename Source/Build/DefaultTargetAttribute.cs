using System;

namespace Pencil.Build
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class DefaultTargetAttribute : Attribute
	{}
}