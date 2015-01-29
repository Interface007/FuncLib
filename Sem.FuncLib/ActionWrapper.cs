namespace Sem.FuncLib
{
    using System;

    public struct ActionWrapper<TValue, TRight>
    {
        private readonly Func<Func<TValue, TRight>, TValue, TRight> func;

        private readonly TValue value;

        public ActionWrapper(Func<Func<TValue, TRight>, TValue, TRight> func, TValue value)
        {
            this.func = func;
            this.value = value;
        }

        public TRight Execute(Func<TValue, TRight> action)
        {
            return this.func(action, this.value);
        }

        public Func<Func<TValue, TRight>, TValue, TRight> Func
        {
            get
            {
                return this.func;
            }
        }

        public TValue Value
        {
            get
            {
                return this.value;
            }
        }
    }
}