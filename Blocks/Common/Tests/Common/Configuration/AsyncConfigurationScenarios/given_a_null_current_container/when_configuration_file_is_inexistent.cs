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
    public class when_configuration_file_is_inexistent : Context
    {
        protected Uri InexistentConfigurationFileUri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/NonExistent.xaml", UriKind.Relative);
        
        protected override void Act()
        {
            base.Act();
            Common.Configuration.EnterpriseLibraryContainer.ConfigureAsync(InexistentConfigurationFileUri);
        }

        [TestMethod]
        [Asynchronous]
        public void then_configured_successfully_flag_is_false()
        {
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.IsFalse(this.EventArgs.ConfiguredSuccessfully));
            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void then_original_webexception_can_be_retrieve()
        {
            this.EnqueueConditional(() => this.EventArgs != null);
            this.EnqueueCallback(() => Assert.IsNotNull(this.EventArgs.Error));
            this.EnqueueCallback(() => Assert.IsInstanceOfType(this.EventArgs.Error, typeof(WebException)));
            this.EnqueueTestComplete();
        }
    }
}
