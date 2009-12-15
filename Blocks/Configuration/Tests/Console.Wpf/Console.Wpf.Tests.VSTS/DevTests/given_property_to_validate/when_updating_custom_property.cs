using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    [TestClass]
    public class when_updating_custom_property : ArrangeActAssert
    {
        private TestableCustomProperty property;

        protected override void Arrange()
        {
            base.Arrange();

            var serviceProvider = new Mock<IServiceProvider>();

            property = new TestableCustomProperty(serviceProvider.Object, "TestProperty");
        }

        protected override void Act()
        {
            property.Value = "blah";
        }

        [TestMethod]
        public void then_property_invokes_validation()
        {
            Assert.IsTrue(property.ValidationInvoked);
        }

        class TestableCustomProperty : CustomProperty<string>
        {
            public TestableCustomProperty(IServiceProvider serviceProvider, string propertyName) 
                : base(serviceProvider, propertyName)
            {
            }

            public override void Validate()
            {
                base.Validate();
                ValidationInvoked = true;
            }

            public bool ValidationInvoked { get; set; }
        }
    }

    
}
