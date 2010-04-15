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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Console.Wpf.Tests.VSTS.DevTests.given_exception_throwing_validator
{
    [TestClass]
    public class when_validator_executed : ArrangeActAssert
    {
        private TestableProperty property;

        protected override void Arrange()
        {
            base.Arrange();

            property = new TestableProperty(null, new object(), null);

        }

        protected override void Act()
        {
            property.Validate();
        }

        [TestMethod]
        public void then_results_displays_exception()
        {
            Assert.IsTrue(
                property.ValidationResults.Any(x => x.Message.StartsWith("An error occurred")));
        }

    }

    public class TestableProperty : Property
    {
        public TestableProperty(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty) 
            : base(serviceProvider, component, declaringProperty)
        {

        }

        public override string PropertyName
        {
            get
            {
                return "TestName";
            }
        }

        public override object Value
        {
            get
            {
                return string.Empty;
            }
            set
            {
                //
            }
        }
        public override IEnumerable<Validator> GetValidators()
        {
            yield return new ExceptionThrowingValidator();
        }
    }
}
