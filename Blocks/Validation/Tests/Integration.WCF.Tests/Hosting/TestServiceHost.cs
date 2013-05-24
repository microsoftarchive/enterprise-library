//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.Hosting
{
    /// <summary>
    /// This class hosts a WCF service in process and provides
    /// a proxy to said service. This allows us to write unit
    /// tests without having to run a separate WCF host process.
    /// </summary>
    /// <typeparam name="TService">Service implementation class to host.</typeparam>
    /// <typeparam name="TContract">Service Contract interface exposed by TService.</typeparam>
    class TestServiceHost<TService, TContract> : IDisposable
        where TService : class
        where TContract : class
    {
        private ChannelFactory<TContract> factory;
        private ServiceHost serviceHost;
        private TContract proxy;

        public TestServiceHost(string serviceAddress)
        {
            serviceHost = new ServiceHost(typeof(TService), new Uri(serviceAddress));

            try
            {
                serviceHost.Open();
            }
            catch (AddressAccessDeniedException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }

            EndpointAddress address = new EndpointAddress(new Uri(serviceAddress));
            factory = new ChannelFactory<TContract>(new BasicHttpBinding(), address);
            proxy = factory.CreateChannel();
        }

        public TContract Proxy
        {
            get { return proxy; }
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            CloseFactory();
            CloseServiceHost();
        }

        #endregion

        private void CloseFactory()
        {
            if(factory != null)
            {
                if(factory.State == CommunicationState.Opened)
                {
                    factory.Close();
                }
                factory = null;
            }
        }

        private void CloseServiceHost()
        {
            if(serviceHost != null)
            {
                if(serviceHost.State == CommunicationState.Opened)
                {
                    serviceHost.Close();
                }
                serviceHost = null;
            }
        }
    }
}
