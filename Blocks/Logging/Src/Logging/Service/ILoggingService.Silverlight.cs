using System;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [ServiceContract(Namespace = "")]
    public interface ILoggingService
    {
        [OperationContract(AsyncPattern = true, IsOneWay = true)]
        IAsyncResult BeginSendLogEntries(LogEntryMessage[] entries, AsyncCallback callback, object asyncState);

        void EndSendLogEntries(IAsyncResult asyncResult);
    }
}
