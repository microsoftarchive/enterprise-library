//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.AsyncConfigurationScenarios.given_a_null_current_container
{
    [TestClass]
    public class when_providing_credentials_or_state : Context
    {
        protected Uri ConfigurationFileUri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/NonExistent.xaml", UriKind.Relative);

        [TestMethod]
        [Asynchronous]
        public void then_state_is_null_by_default()
        {
            Common.Configuration.EnterpriseLibraryContainer.ConfigureAsync(ConfigurationFileUri);
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.IsNull(this.EventArgs.State));
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void then_state_can_be_retrieved()
        {
            var state = new object(); 
            Common.Configuration.EnterpriseLibraryContainer.ConfigureAsync(ConfigurationFileUri, state);
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.AreSame(state, this.EventArgs.State));
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void then_can_provide_credential_without_state()
        {
            var credential = new NetworkCredential("username", "password");
            Common.Configuration.EnterpriseLibraryContainer.ConfigureAsync(ConfigurationFileUri, credential);
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.IsNull(this.EventArgs.State));
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void then_state_can_be_retrieved_when_using_credential()
        {
            var state = new object();
            var credential = new NetworkCredential("username", "password");
            Common.Configuration.EnterpriseLibraryContainer.ConfigureAsync(ConfigurationFileUri, credential, state);
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.AreSame(state, this.EventArgs.State));
            this.EnqueueTestComplete();
        }
    }
}
