//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    /// <summary>
    /// Summary description for ExceptionShieldingBehaviorFixture
    /// </summary>
    [TestClass]
    public class ExceptionShieldingBehaviorFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExceptionPolicy.Reset();
        }

        [TestMethod]
        public void ShouldSetShieldingWithNonIncludeExceptionDetailInFaults()
        {
            // create a mock service and its endpoint.
            Uri serviceUri = new Uri("http://tests:30003");
            ServiceHost host = new ServiceHost(typeof(MockService), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new WSHttpBinding(), serviceUri);
            try
            {
                host.Open();
            }
            catch (AddressAccessDeniedException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }
            try
            {
                // check that we have no ErrorHandler loaded into each channel that
                // has IncludeExceptionDetailInFaults turned off.
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(0, dispatcher.ErrorHandlers.Count);
                    Assert.IsFalse(dispatcher.IncludeExceptionDetailInFaults);
                }
                ExceptionShieldingBehavior behavior = new ExceptionShieldingBehavior();
                behavior.ApplyDispatchBehavior(null, host);
                // check that the ExceptionShieldingErrorHandler was assigned to each channel
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(1, dispatcher.ErrorHandlers.Count);
                    Assert.IsTrue(dispatcher.ErrorHandlers[0].GetType().IsAssignableFrom(typeof(ExceptionShieldingErrorHandler)));
                }
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        [TestMethod]
        public void ShouldFilterMexHttpEndpointsAndAddOneInstance()
        {
            FilterMexEndpointsAndAddOneInstance(Uri.UriSchemeHttp);
        }

        [TestMethod]
        public void ShouldFilterMexHttpsEndpointsAndAddOneInstance()
        {
            FilterMexEndpointsAndAddOneInstance(Uri.UriSchemeHttps);
        }

        void FilterMexEndpointsAndAddOneInstance(string mexScheme)
        {
            Uri serviceUri = new Uri(mexScheme + "://tests:30003");
            ServiceHost host = new ServiceHost(typeof(MockServiceWithShielding), serviceUri);
            SecurityMode securityMode = SetMexAndSecurity(host, mexScheme);
            WSHttpBinding mockServiceBinding = new WSHttpBinding(securityMode);
            host.AddServiceEndpoint(typeof(IMockService), mockServiceBinding, serviceUri);
            try
            {
                host.Open();
            }
            catch (AddressAccessDeniedException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }
            try
            {
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Assert.AreEqual(dispatcher.BindingName.Contains(mockServiceBinding.Name) ? 1 : 0, dispatcher.ErrorHandlers.Count);
                    Assert.IsFalse(dispatcher.IncludeExceptionDetailInFaults);
                }
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        SecurityMode SetMexAndSecurity(ServiceHost host,
                                       string mexScheme)
        {
            ServiceMetadataBehavior mexHttpBehavior = new ServiceMetadataBehavior();
            Uri mexUri = new Uri("/mex", UriKind.Relative);
            Binding binding;
            SecurityMode securityMode;
            if (mexScheme == Uri.UriSchemeHttp)
            {
                mexHttpBehavior.HttpGetEnabled = true;
                binding = MetadataExchangeBindings.CreateMexHttpBinding();
                securityMode = SecurityMode.Message;
            }
            else
            {
                mexHttpBehavior.HttpsGetEnabled = true;
                binding = MetadataExchangeBindings.CreateMexHttpsBinding();
                securityMode = SecurityMode.TransportWithMessageCredential;
            }
            host.Description.Behaviors.Add(mexHttpBehavior);
            host.AddServiceEndpoint(typeof(IMetadataExchange), binding, mexUri);

            return securityMode;
        }
    }
}
