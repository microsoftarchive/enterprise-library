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

using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class PropertyValueAccessFixture
    {
        [TestMethod]
        public void CanGetValueFromPropertyForInstanceOfDeclaringClass()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property1");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);
            BaseTestDomainObject domainObject = new BaseTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base1Value, value);
        }

        [TestMethod]
        public void CanGetValueFromPropertyForInstanceOfDerivedClass()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property1");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base1Value, value);
        }

        [TestMethod]
        public void CanGetValueFromOverridenPropertyForInstanceOfDerivedClass()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property2");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(DerivedTestDomainObject.Derived2Value, value);
        }

        [TestMethod]
        public void RetrievalOfValueForInstanceOfDerivedTypeThroughBaseAliasedPropertyReturnsValueFromInheritedProperty()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property3");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base3Value, value);
        }

        [TestMethod]
        public void RetrievalOfValueForInstanceOfDerivedTypeThroughNewAliasedPropertyReturnsValueFromNewProperty()
        {
            PropertyInfo propertyInfo = typeof(DerivedTestDomainObject).GetProperty("Property3");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(DerivedTestDomainObject.Derived3Value, value);
        }

        [TestMethod]
        public void RetrievalOfValueForInstanceOfNonRelatedTypeReturnsFailure()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property1");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue("a string", out value, out valueAccessRetrievalFailure);

            Assert.IsFalse(status);
            Assert.IsNull(value);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.ErrorValueAccessInvalidType, valueAccessRetrievalFailure));
        }

        [TestMethod]
        public void RetrievalOfValueForNullReferenceReturnsFailure()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property1");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(null, out value, out valueAccessRetrievalFailure);

            Assert.IsFalse(status);
            Assert.IsNull(value);
            Assert.IsTrue(TemplateStringTester.IsMatch(Resources.ErrorValueAccessNull, valueAccessRetrievalFailure));
        }

        [TestMethod]
        public void CanGetKeyFromValueAccess()
        {
            PropertyInfo propertyInfo = typeof(BaseTestDomainObject).GetProperty("Property3");
            ValueAccess valueAccess = new PropertyValueAccess(propertyInfo);

            Assert.AreEqual("Property3", valueAccess.Key);
        }
    }
}
