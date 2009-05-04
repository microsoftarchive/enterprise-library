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
    public class FieldValueAccessFixture
    {
        [TestMethod]
        public void CanGetValueFromFieldForInstanceOfDeclaringClass()
        {
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field1");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);
            BaseTestDomainObject domainObject = new BaseTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base1Value, value);
        }

        [TestMethod]
        public void CanGetValueFromFieldForInstanceOfDerivedClass()
        {
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field1");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base1Value, value);
        }

        [TestMethod]
        public void RetrievalOfValueForInstanceOfDerivedTypeThroughBaseAliasedFieldReturnsValueFromInheritedField()
        {
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field3");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);
            BaseTestDomainObject domainObject = new DerivedTestDomainObject();

            object value;
            string valueAccessRetrievalFailure;
            bool status = valueAccess.GetValue(domainObject, out value, out valueAccessRetrievalFailure);

            Assert.IsTrue(status);
            Assert.AreEqual(BaseTestDomainObject.Base3Value, value);
        }

        [TestMethod]
        public void RetrievalOfValueForInstanceOfDerivedTypeThroughNewAliasedFieldReturnsValueFromNewField()
        {
            FieldInfo fieldInfo = typeof(DerivedTestDomainObject).GetField("Field3");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);
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
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field1");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);

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
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field1");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);

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
            FieldInfo fieldInfo = typeof(BaseTestDomainObject).GetField("Field3");
            ValueAccess valueAccess = new FieldValueAccess(fieldInfo);

            Assert.AreEqual("Field3", valueAccess.Key);
        }
    }
}
