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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_cryptography_settings
{
    [TestClass]
    public class when_editing_symmetric_crypto_provider_element : given_crypto_configuration
    {
        ElementViewModel symmetricCryptoProvider;

        protected override void Arrange()
        {
           base.Arrange();

           symmetricCryptoProvider = base.CryptographyModel.GetDescendentsOfType<SymmetricAlgorithmProviderData>().First();
        }

        [TestMethod]
        public void then_element_has_no_protected_key_properties()
        {
            Assert.IsTrue(symmetricCryptoProvider.Property("ProtectedKeyFilename").Hidden);
            Assert.IsTrue(symmetricCryptoProvider.Property("ProtectedKeyProtectionScope").Hidden);
        }

        [TestMethod]
        public void then_element_has_key_property()
        {
            var keyProperty = symmetricCryptoProvider.Property("Key");
            Assert.IsNotNull(keyProperty);
            Assert.AreEqual(typeof(ProtectedKeySettings), keyProperty.PropertyType);
            Assert.IsTrue(((PopupEditorBindableProperty)keyProperty.BindableProperty).TextReadOnly);
        }

        [TestMethod]
        public void then_key_property_validates()
        {
            var keyProperty = symmetricCryptoProvider.Property("Key");
            keyProperty.Validate();
            Assert.IsFalse(keyProperty.ValidationErrors.Any());
        }

    }



}
