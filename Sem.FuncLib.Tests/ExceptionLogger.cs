// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionLogger.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ExceptionLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System;
    using System.Collections.Generic;

    public class ExceptionLogger
    {
        private readonly List<string> list = new List<string>();
        
        private readonly List<int> intList = new List<int>();

        public IEnumerable<string> List
        {
            get
            {
                return this.list;
            }
        }

        public List<int> IntList
        {
            get
            {
                return this.intList;
            }
        }

        public void HandleException(Exception ex)
        {
            this.list.Add(ex.ToString());
        }

        public void LogInts(int value)
        {
            this.IntList.Add(value);
        }
    }
}