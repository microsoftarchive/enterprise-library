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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class EditableKeyValueFixture
    {
        [TestMethod]
        public void DefaultKeyAndValueAreNull()
        {
            EditableKeyValue keyValue = new EditableKeyValue();
            Assert.IsNull(keyValue.Key);
            Assert.IsNull(keyValue.Value);
        }

        [TestMethod]
        public void DisplayTextContainsKeyAndValue()
        {
            string someKey = "someKey";
            string someValue = "someValue";

            EditableKeyValue keyValue = new EditableKeyValue(someKey, someValue);
            string displayText = keyValue.ToString();

            Assert.IsFalse(-1 == displayText.IndexOf(someKey));
            Assert.IsFalse(-1 == displayText.IndexOf(someValue));
        }
    }
}