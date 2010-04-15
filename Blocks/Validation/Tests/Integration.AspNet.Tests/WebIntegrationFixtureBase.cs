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

using System;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Tests
{
    public class WebIntegrationFixtureBase
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            MockValidator<object>.ResetCaches();
        }

        public void CanUseValidatorWithAttributesWithTypeLocalToWebApp()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.IsFalse(page.IsValid);
            Assert.IsTrue(((BaseValidator)page.FindControl("PropertyValidatorProxyControl")).ErrorMessage.Contains("invalid name from attribute"));
        }

        public void CanUseValidatorWithAttributesWithTypeFromReferencedAssemblyToWebApp()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.IsFalse(page.IsValid);
            Assert.IsTrue(((BaseValidator)page.FindControl("PropertyValidatorProxyControl")).ErrorMessage.Contains("message1"));
        }

        public void CanUseValidatorFromConfigurationWithTypeFromReferencedAssemblyToWebApp()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.IsFalse(page.IsValid);
            Assert.IsTrue(((BaseValidator)page.FindControl("PropertyValidatorProxyControl")).ErrorMessage.Contains("message-from-config1"));
        }

        public void UsingValidatorWithDefaultTypeConversionWillValidateTheConvertedTargetControlValue()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
            MockValidator<object> validator = MockValidator<object>.CreatedValidators[0];
            Assert.AreEqual(012345678, validator.ValidatedTargets[0]);
        }

        public void UsingValidatorWithDefaultTypeConversionForEnumWillValidateTheConvertedTargetControlValue()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
            MockValidator<object> validator = MockValidator<object>.CreatedValidators[0];
            Assert.AreEqual(TraceOptions.LogicalOperationStack, validator.ValidatedTargets[0]);
        }

        public void UsingValidatorWithCustomTypeConversionWillValidateTheCustomConvertedTargetControlValue()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
            MockValidator<object> validator = MockValidator<object>.CreatedValidators[0];
            Assert.AreEqual(876543210, validator.ValidatedTargets[0]); // reversed by custom type conversion
        }

        public void UsingValidatorWithFailingCustomTypeConversionWillLogValidationErrorWithSuppliedConversionErrorMessage()
        {
            Page page = TestContext.RequestedPage;
            page.Validate();

            Assert.AreEqual(1, MockValidator<object>.CreatedValidators.Count);
            Assert.IsFalse(page.IsValid);
            Assert.IsTrue(((BaseValidator)page.FindControl("PropertyValidatorProxyControl")).ErrorMessage.Contains("custom conversion error message"));
        }

        public void CanGetValueForPropertyMappedToProvidedValidator()
        {
            Page page = TestContext.RequestedPage;
            PropertyProxyValidator validator = (PropertyProxyValidator)page.FindControl("NameValidator1");

            ValueAccess valueAccess = new PropertyMappedValidatorValueAccess("Name");
            object value;
            string valueAccessFailureMessage;

            bool accessStatus = valueAccess.GetValue(validator, out value, out valueAccessFailureMessage);

            Assert.IsTrue(accessStatus);
            Assert.AreEqual("0123456789012345", value);
        }

        public void CanGetValueForPropertyMappedToOtherValidator()
        {
            Page page = TestContext.RequestedPage;
            PropertyProxyValidator validator = (PropertyProxyValidator)page.FindControl("NameValidator1");

            ValueAccess valueAccess = new PropertyMappedValidatorValueAccess("SurName");
            object value;
            string valueAccessFailureMessage;

            bool accessStatus = valueAccess.GetValue(validator, out value, out valueAccessFailureMessage);

            Assert.IsTrue(accessStatus);
            Assert.AreEqual("LogicalOperationStack", value);
        }

        public void CanGetValueForPropertyMappedToValidatorInSameNamingContainerAsProvidedValidator()
        {
            Page page = TestContext.RequestedPage;

            ValueAccess nameValueAccess = new PropertyMappedValidatorValueAccess("Name");
            ValueAccess surNameValueAccess = new PropertyMappedValidatorValueAccess("SurName");

            foreach (BaseValidator validator in page.Validators)
            {
                PropertyProxyValidator propertyProxyValidator = validator as PropertyProxyValidator;

                if (propertyProxyValidator != null && propertyProxyValidator.ID.Equals("Name_Validator"))
                {
                    object nameValue;
                    object surNameValue;
                    string targetSurNameValue = null;
                    string valueAccessFailureMessage;

                    bool accessStatus
                        = nameValueAccess.GetValue(propertyProxyValidator, out nameValue, out valueAccessFailureMessage);
                    Assert.IsTrue(accessStatus);

                    switch ((string)nameValue)
                    {
                        case "abc":
                            targetSurNameValue = "123";
                            break;
                        case "def":
                            targetSurNameValue = "456";
                            break;
                        case "ghi":
                            targetSurNameValue = "789";
                            break;
                    }

                    accessStatus
                        = surNameValueAccess.GetValue(propertyProxyValidator, out surNameValue, out valueAccessFailureMessage);
                    Assert.IsTrue(accessStatus);
                    Assert.AreEqual(targetSurNameValue, surNameValue);
                }
            }
        }

        public void ValueRequestForNonMappedPropertyReturnsFailure()
        {
            Page page = TestContext.RequestedPage;
            PropertyProxyValidator validator = (PropertyProxyValidator)page.FindControl("NameValidator1");

            ValueAccess valueAccess = new PropertyMappedValidatorValueAccess("InvalidProperty");
            object value;
            string valueAccessFailureMessage;

            bool accessStatus = valueAccess.GetValue(validator, out value, out valueAccessFailureMessage);

            Assert.IsFalse(accessStatus);
        }

        public void CanGetConvertedValueForPropertyMappedToProvidedValidator()
        {
            Page page = TestContext.RequestedPage;
            PropertyProxyValidator validator = (PropertyProxyValidator)page.FindControl("NumberPropertyValidator");

            ValueAccess valueAccess = new PropertyMappedValidatorValueAccess("NumberProperty");
            object value;
            string valueAccessFailureMessage;

            bool accessStatus = valueAccess.GetValue(validator, out value, out valueAccessFailureMessage);

            Assert.IsTrue(accessStatus);
            Assert.AreEqual(012345, value);
        }

        public void CanPerformCrossFieldValidation()
        {
            Page page = TestContext.RequestedPage;
            TextBox nameTextBox = (TextBox)page.FindControl("NameText");

            page.Validate();

            Assert.IsFalse(page.IsValid);

            nameTextBox.Text = "not 123";
            page.Validate();

            Assert.IsTrue(page.IsValid);
        }
    }
}
