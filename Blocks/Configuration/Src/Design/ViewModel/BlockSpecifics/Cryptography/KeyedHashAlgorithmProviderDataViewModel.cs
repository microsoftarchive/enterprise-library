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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Configuration;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class KeyedHashAlgorithmProviderDataViewModel : CollectionElementViewModel
    {
        public KeyedHashAlgorithmProviderDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(
                new Property[]{ 
                    ContainingSection.CreateProperty<ProtectedKeySettingsProperty>( 
                    new ParameterOverride("configuration", ConfigurationElement))
                });
        }

        private class ProtectedKeySettingsProperty : CustomProperty<ProtectedKeySettings>, ICryptographicKeyProperty
        {
            KeyedHashAlgorithmProviderData configuration;
            private ProtectedKey key;

            public ProtectedKeySettingsProperty(IServiceProvider serviceProvider, KeyedHashAlgorithmProviderData configuration)
                : base(serviceProvider, "Key", 
                new EditorAttribute(typeof(KeyManagerEditor), typeof(UITypeEditor)),
                new EditorWithReadOnlyTextAttribute(true))
            {
                this.configuration = configuration;
            }

            public override object Value
            {
                get
                {
                    return new ProtectedKeySettings(configuration.ProtectedKeyFilename, configuration.ProtectedKeyProtectionScope)
                        {
                            ProtectedKey = key
                        };
                }
                set
                {
                    ProtectedKeySettings keySettings = (ProtectedKeySettings)value;
                    configuration.ProtectedKeyFilename = keySettings.FileName;
                    configuration.ProtectedKeyProtectionScope = keySettings.Scope;

                    key = keySettings.ProtectedKey;
                }
            }

            public ProtectedKeySettings KeySettings
            {
                get { return TypedValue; }
            }

            public IKeyCreator KeyCreator
            {
                get { return new KeyedHashAlgorithmKeyCreator(configuration.AlgorithmType); }
            }

            protected override IEnumerable<Validator> GetDefaultPropertyValidators()
            {
                return Enumerable.Empty<Validator>();
            }
        }
    }

#pragma warning restore 1591
}
