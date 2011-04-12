using System;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [ServiceContract(Namespace = "")]
    public interface ILoggingService
    {
        /// <summary>
        /// Asynchronously submits log entries to the server log.
        /// </summary>
        /// <param name="entries">The log entries to send to the server.</param>
        /// <param name="callback">The delegate to call when the operation finishes.</param>
        /// <param name="asyncState">The user-defined state object used to pass context data to the callback method.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the status of the asynchronous operation.</returns>
        [OperationContract(AsyncPattern = true, IsOneWay = true)]
        IAsyncResult BeginAdd(LogEntryMessage[] entries, AsyncCallback callback, object asyncState);

        /// <summary>
        /// Called to complete the <see cref="BeginAdd"/> operation.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult"/> that represents the status of the asynchronous operation.</param>
        void EndAdd(IAsyncResult asyncResult);
    }
}
