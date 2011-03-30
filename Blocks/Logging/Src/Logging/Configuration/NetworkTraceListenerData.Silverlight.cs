using System;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration for the NotificationTraceListener.
    /// </summary>
    public class NetworkTraceListenerData : TraceListenerData
    {
        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <returns>A <see cref="Expression"/> that creates an <see cref="NotificationTraceListener"/></returns>
        protected override Expression<Func<Diagnostics.TraceListener>> GetCreationExpression()
        {
            // TODO verify if we want to provide default binding config for usability reasons (no need to have a separate ServiceReferences.ClientConfig file)

            //BinaryMessageEncodingBindingElement binaryMessageEncoding = new BinaryMessageEncodingBindingElement(); 
            //HttpTransportBindingElement httpTransport = new HttpTransportBindingElement() { MaxBufferSize = int.MaxValue, MaxReceivedMessageSize = int.MaxValue };
            //// add the binding elements into a Custom BindingCustomBinding customBinding = new CustomBinding(binaryMessageEncoding,httpTransport);
            //CustomBinding binding = new CustomBinding(binaryMessageEncoding, httpTransport);
            
            //var endpointAddress = new EndpointAddress("../LoggingService.svc");
            //return () => new NetworkTraceListener(() => new LoggingServiceProxy(binding, endpointAddress));

            if (string.IsNullOrEmpty(EndpointConfigurationName))
                throw new ArgumentException("EndpointConfigurationName");

            return () => new NetworkTraceListener(() => new LoggingServiceProxy(EndpointConfigurationName));
        }

        /// <summary>
        /// The wcf endpoint configuration name. This configuration must be present in the
        /// ServiceReferences.ClientConfig file of the main application.
        /// </summary>
        public string EndpointConfigurationName { get; set; }
    }
}
