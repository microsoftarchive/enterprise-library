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
