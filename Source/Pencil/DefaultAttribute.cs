using System;

namespace Pencil
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class DefaultAttribute : Attribute
	{}
}