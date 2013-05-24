using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Config
{
    public class MockLogFilter : ILogFilter
    {
        public bool Filter(LogEntry log)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "Mock"; }
        }
    }
}
