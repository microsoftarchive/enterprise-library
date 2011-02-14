using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class ConfigurationErrorsException : Exception
    {
        public ConfigurationErrorsException() 
        { }

        public ConfigurationErrorsException(string message) : base(message) 
        { }

        public ConfigurationErrorsException(string message, Exception inner) : base(message, inner) 
        { }
    }
}
