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

using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests
{
    [TestClass]
    public class SupportingTypesFixture
    {
        
        [TestMethod]
        public void TypeConverterRejectsNull()
        {
            Assert.IsFalse(new RequiredIdentifierConverter().IsValid(null));
        }

        [TestMethod]
        public void TypeConverterRejectsEmpty()
        {
            Assert.IsFalse(new RequiredIdentifierConverter().IsValid(string.Empty));
        }

        [TestMethod]
        public void TypeConverterRejectsNonString()
        {
            Assert.IsFalse(new RequiredIdentifierConverter().IsValid(5));
        }

        [TestMethod]
        public void TypeConverterAcceptsIdentifier()
        {
            Assert.IsTrue(new RequiredIdentifierConverter().IsValid("Property"));
        }
    }
}
