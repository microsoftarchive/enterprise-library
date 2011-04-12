using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [ServiceContract(Namespace = "")]
    public interface ILoggingService
    {
        /// <summary>
        /// Adds log entries into to the server log.
        /// </summary>
        /// <param name="entries">The client log entries to log in the server.</param>
        [OperationContract(IsOneWay = true)]
        void Add(LogEntryMessage[] entries);
    }
}
