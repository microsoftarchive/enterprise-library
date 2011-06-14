//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_locked_file
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_getting_entries : Context
    {
        private Exception exception;

        protected override void Act()
        {
            try
            {
                this.repository.RetrieveEntries();
            }
            catch (InvalidOperationException e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_exception_is_thrown()
        {
            Assert.IsNotNull(this.exception);
        }
    }
}
