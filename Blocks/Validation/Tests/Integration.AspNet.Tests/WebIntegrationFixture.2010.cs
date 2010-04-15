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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Tests
{
    [TestClass]
    public class WebIntegrationFixture : WebIntegrationFixtureBase
    {
        [TestInitialize]
        public new void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithLocalType.aspx")]
        public new void CanUseValidatorWithAttributesWithTypeLocalToWebApp()
        {
            base.CanUseValidatorWithAttributesWithTypeLocalToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationUsingAttributesWithNonLocalType.aspx")]
        public new void CanUseValidatorWithAttributesWithTypeFromReferencedAssemblyToWebApp()
        {
            base.CanUseValidatorWithAttributesWithTypeFromReferencedAssemblyToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationUsingConfigurationWithNonLocalType.aspx")]
        public new void CanUseValidatorFromConfigurationWithTypeFromReferencedAssemblyToWebApp()
        {
            base.CanUseValidatorFromConfigurationWithTypeFromReferencedAssemblyToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithDefaultTypeConversion.aspx")]
        public new void UsingValidatorWithDefaultTypeConversionWillValidateTheConvertedTargetControlValue()
        {
            base.UsingValidatorWithDefaultTypeConversionWillValidateTheConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithDefaultTypeConversionForEnum.aspx")]
        public new void UsingValidatorWithDefaultTypeConversionForEnumWillValidateTheConvertedTargetControlValue()
        {
            base.UsingValidatorWithDefaultTypeConversionForEnumWillValidateTheConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithCustomTypeConversion.aspx")]
        public new void UsingValidatorWithCustomTypeConversionWillValidateTheCustomConvertedTargetControlValue()
        {
            base.UsingValidatorWithCustomTypeConversionWillValidateTheCustomConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithFailingCustomTypeConversion.aspx")]
        public new void UsingValidatorWithFailingCustomTypeConversionWillLogValidationErrorWithSuppliedConversionErrorMessage()
        {
            base.UsingValidatorWithFailingCustomTypeConversionWillLogValidationErrorWithSuppliedConversionErrorMessage();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        public new void CanGetValueForPropertyMappedToProvidedValidator()
        {
            base.CanGetValueForPropertyMappedToProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        public new void CanGetValueForPropertyMappedToOtherValidator()
        {
            base.CanGetValueForPropertyMappedToOtherValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        public new void CanGetValueForPropertyMappedToValidatorInSameNamingContainerAsProvidedValidator()
        {
            base.CanGetValueForPropertyMappedToValidatorInSameNamingContainerAsProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        public new void ValueRequestForNonMappedPropertyReturnsFailure()
        {
            base.ValueRequestForNonMappedPropertyReturnsFailure();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccessValueConvert.aspx")]
        public new void CanGetConvertedValueForPropertyMappedToProvidedValidator()
        {
            base.CanGetConvertedValueForPropertyMappedToProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web.2010", "/Web")]
        [UrlToTest("http://localhost/Web/CrossFieldValidationWithLocalType.aspx")]
        public new void CanPerformCrossFieldValidation()
        {
            base.CanPerformCrossFieldValidation();
        }
    }
}
