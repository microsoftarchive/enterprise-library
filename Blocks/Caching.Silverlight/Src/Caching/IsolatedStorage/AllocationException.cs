using System;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class AllocationException : IOException
    {
        public AllocationException() { }

        public AllocationException(string message) : base(message) { }

        public AllocationException(string message, Exception inner) : base(message, inner) { }
    }
}
