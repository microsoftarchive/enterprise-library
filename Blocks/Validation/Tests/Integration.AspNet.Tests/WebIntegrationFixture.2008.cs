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
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithLocalType.aspx")]
        [TestCategory("Integration")]
        public new void CanUseValidatorWithAttributesWithTypeLocalToWebApp()
        {
            base.CanUseValidatorWithAttributesWithTypeLocalToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationUsingAttributesWithNonLocalType.aspx")]
        [TestCategory("Integration")]
        public new void CanUseValidatorWithAttributesWithTypeFromReferencedAssemblyToWebApp()
        {
            base.CanUseValidatorWithAttributesWithTypeFromReferencedAssemblyToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationUsingConfigurationWithNonLocalType.aspx")]
        [TestCategory("Integration")]
        public new void CanUseValidatorFromConfigurationWithTypeFromReferencedAssemblyToWebApp()
        {
            base.CanUseValidatorFromConfigurationWithTypeFromReferencedAssemblyToWebApp();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithDefaultTypeConversion.aspx")]
        [TestCategory("Integration")]
        public new void UsingValidatorWithDefaultTypeConversionWillValidateTheConvertedTargetControlValue()
        {
            base.UsingValidatorWithDefaultTypeConversionWillValidateTheConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithDefaultTypeConversionForEnum.aspx")]
        [TestCategory("Integration")]
        public new void UsingValidatorWithDefaultTypeConversionForEnumWillValidateTheConvertedTargetControlValue()
        {
            base.UsingValidatorWithDefaultTypeConversionForEnumWillValidateTheConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithCustomTypeConversion.aspx")]
        [TestCategory("Integration")]
        public new void UsingValidatorWithCustomTypeConversionWillValidateTheCustomConvertedTargetControlValue()
        {
            base.UsingValidatorWithCustomTypeConversionWillValidateTheCustomConvertedTargetControlValue();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValidationWithFailingCustomTypeConversion.aspx")]
        [TestCategory("Integration")]
        public new void UsingValidatorWithFailingCustomTypeConversionWillLogValidationErrorWithSuppliedConversionErrorMessage()
        {
            base.UsingValidatorWithFailingCustomTypeConversionWillLogValidationErrorWithSuppliedConversionErrorMessage();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        [TestCategory("Integration")]
        public new void CanGetValueForPropertyMappedToProvidedValidator()
        {
            base.CanGetValueForPropertyMappedToProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        [TestCategory("Integration")]
        public new void CanGetValueForPropertyMappedToOtherValidator()
        {
            base.CanGetValueForPropertyMappedToOtherValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        [TestCategory("Integration")]
        public new void CanGetValueForPropertyMappedToValidatorInSameNamingContainerAsProvidedValidator()
        {
            base.CanGetValueForPropertyMappedToValidatorInSameNamingContainerAsProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccess.aspx")]
        [TestCategory("Integration")]
        public new void ValueRequestForNonMappedPropertyReturnsFailure()
        {
            base.ValueRequestForNonMappedPropertyReturnsFailure();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/ValueAccessValueConvert.aspx")]
        [TestCategory("Integration")]
        public new void CanGetConvertedValueForPropertyMappedToProvidedValidator()
        {
            base.CanGetConvertedValueForPropertyMappedToProvidedValidator();
        }

        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("%PathToWebRoot%\\Web", "/Web")]
        [UrlToTest("http://localhost/Web/CrossFieldValidationWithLocalType.aspx")]
        [TestCategory("Integration")]
        public new void CanPerformCrossFieldValidation()
        {
            base.CanPerformCrossFieldValidation();
        }
    }
}
