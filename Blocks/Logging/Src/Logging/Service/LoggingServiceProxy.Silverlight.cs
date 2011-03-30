using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    public class LoggingServiceProxy : ILoggingService, IDisposable
    {
        private ILoggingService channel;
        private ChannelFactory<ILoggingService> factory;

        public LoggingServiceProxy(string endpointConfigurationName, EndpointAddress remoteAddress)
            : this(new ChannelFactory<ILoggingService>(endpointConfigurationName, remoteAddress))
        {
        }

        public LoggingServiceProxy(string endpointConfigurationName)
            : this(new ChannelFactory<ILoggingService>(endpointConfigurationName))
        {
        }

        public LoggingServiceProxy(Binding binding, EndpointAddress remoteAddress)
            : this(new ChannelFactory<ILoggingService>(binding, remoteAddress))
        {
        }

        public LoggingServiceProxy(ChannelFactory<ILoggingService> factory)
        {
            this.factory = factory;
            this.channel = factory.CreateChannel();
        }

        public IAsyncResult BeginSendLogEntries(LogEntryMessage[] entries, AsyncCallback callback, object asyncState)
        {
            return this.channel.BeginSendLogEntries(entries, callback, asyncState);
        }

        public void EndSendLogEntries(IAsyncResult asyncResult)
        {
            this.channel.EndSendLogEntries(asyncResult);
        }

        public void Dispose()
        {
            var clientChannel = (IClientChannel)this.channel;
            try
            {
                clientChannel.Close();
                this.factory.Close();
            }
            catch (CommunicationException)
            {
                clientChannel.Abort();
            }
            catch (TimeoutException)
            {
                clientChannel.Abort();
            }
            catch (Exception)
            {
                clientChannel.Abort();
                throw;
            }

            using (this.channel as IDisposable) this.channel = null;
            using (this.factory as IDisposable) this.factory = null;
        }
    }
}
