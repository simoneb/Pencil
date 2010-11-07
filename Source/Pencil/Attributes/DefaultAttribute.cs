using System;

namespace Pencil.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class DefaultAttribute : Attribute
	{}
}