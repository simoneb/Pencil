namespace Pencil
{
    public abstract class Future<T>
    {
        private T result;
        private bool evaluated;

        public T Value { get { return evaluated ? result : EvaluateInternal(); } }

        private T EvaluateInternal()
        {
            result = Evaluate();
            evaluated = true;
            return result;
        }

        protected abstract T Evaluate();
    }
}