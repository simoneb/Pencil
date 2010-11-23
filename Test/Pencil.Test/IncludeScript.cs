namespace Pencil.Test
{
    public class IncludeScript : Project
    {
        public int IncludeProperty { get { return 2; } }

        public void IncludeTarget()
        {
            Logger.WriteLine("Include output");
        }
    }
}