namespace Sem.FuncLib.Tests
{
    using System;
    using System.Collections.Generic;

    public class ExceptionLogger
    {
        private readonly List<string> list = new List<string>();

        public IEnumerable<string> List
        {
            get
            {
                return this.list;
            }
        }

        public void HandleException(Exception ex)
        {
            this.list.Add(ex.ToString());
        }
    }
}