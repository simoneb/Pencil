namespace Pencil
{
    public class Include
    {
        public static implicit operator Include(string file)
        {
            return null;
        }

        public T Property<T>(string propertyName)
        {
            return default(T);
        }
    }
}