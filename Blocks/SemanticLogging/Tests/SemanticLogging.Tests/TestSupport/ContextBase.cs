#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport
{
    [TestClass]
    public abstract class ContextBase
    {
        [TestInitialize]
        public void Initialize()
        {
            this.Given();
            this.When();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.OnCleanup();
        }

        protected virtual void Given()
        {
        }

        protected virtual void When()
        {
        }

        protected virtual void OnCleanup()
        {
        }
    }
}
