using System;

namespace Pencil
{
	public class TargetFailedException : Exception
	{
		public TargetFailedException(Exception inner): base(string.Empty, inner){}
	}
}