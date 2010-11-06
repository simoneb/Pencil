using System;

namespace Pencil
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class DependsOnAttribute : Attribute
	{
		string name;
		public DependsOnAttribute(string name)
		{
			this.name = name;
		}

		public string Name { get { return name; } }
	}
}