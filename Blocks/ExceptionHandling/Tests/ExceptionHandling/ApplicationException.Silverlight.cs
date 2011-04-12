using System;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class ApplicationException : Exception
    {
        public ApplicationException()
            : base()
        { }

        public ApplicationException(string message)
            : base(message)
        { }

        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
