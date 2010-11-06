using System;

namespace Pencil
{
    public class DuplicateTargetException : Exception
    {
        public DuplicateTargetException(string existingTarget, string newTarget, Exception innerException) : base(BuildMessage(existingTarget, newTarget), innerException)
        {
        }

        private static string BuildMessage(string existingTarget, string newTarget)
        {
            return string.Format("Target names are case insensitive, duplicates not allowed ('{0}', '{1}')", existingTarget, newTarget);
        }
    }
}