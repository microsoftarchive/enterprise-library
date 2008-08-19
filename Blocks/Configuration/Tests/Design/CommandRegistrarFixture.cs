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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class CommandRegistrarFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstrutingWithNullServiceProviderThrows()
        {
            new MockCommandRegistrar();
        }

        class MockCommandRegistrar : CommandRegistrar
        {
            public MockCommandRegistrar()
                : base(null) {}

            public override void Register() {}
        }
    }
}