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

using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Utility
{
    [TestClass]
    public class XamlActivatorFixture
    {
        [TestMethod]
        public void WhenNameIsNotValidXName_ThenReturnsNull()
        {
            var name = "and invalid name ;";

            var actual = XamlActivator.CreateInstance<object>(name);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenNameIsNotExistingType_ThenReturnsNull()
        {
            var name = "{clr-namespace:Microsoft.Practices.EnterpriseLibrary.Common.Tests.Utility;assembly=Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests}XamlActivatorFixturex";

            var actual = XamlActivator.CreateInstance<object>(name);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenNameIsNotExistingAssembly_ThenReturnsNull()
        {
            var name = "{clr-namespace:Microsoft.Practices.EnterpriseLibrary.Common.Tests.Utility;assembly=Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Testsx}XamlActivatorFixturex";

            var actual = XamlActivator.CreateInstance<object>(name);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenNameIsExistingType_ThenReturnsInstanceOfType()
        {
            var name = "{clr-namespace:Microsoft.Practices.EnterpriseLibrary.Common.Tests.Utility;assembly=Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests}XamlActivatorFixture";

            var actual = XamlActivator.CreateInstance<object>(name);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(XamlActivatorFixture));
        }
    }
}
