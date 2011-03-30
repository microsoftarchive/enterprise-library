using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [ServiceContract(Namespace = "")]
    public interface ILoggingService
    {
        [OperationContract(IsOneWay = true)]
        void SendLogEntries(LogEntryMessage[] entries);
    }
}
