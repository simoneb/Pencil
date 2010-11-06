using System;

namespace Pencil.Build
{
	public class TargetFailedException : Exception
	{
		public TargetFailedException(Exception inner): base(string.Empty, inner){}
	}
}