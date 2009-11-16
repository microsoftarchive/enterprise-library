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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class SymmetricAlgorithmProviderDataViewModel: CollectionElementViewModel
    {
        public SymmetricAlgorithmProviderDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
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
            SymmetricAlgorithmProviderData configuration;

            public ProtectedKeySettingsProperty(IServiceProvider serviceProvider, SymmetricAlgorithmProviderData configuration)
                :base(serviceProvider, "Key")
            {
                this.configuration = configuration;
            }
            private ProtectedKey key;

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
                    configuration.ProtectedKeyFilename = keySettings.Filename;
                    configuration.ProtectedKeyProtectionScope = keySettings.Scope;

                    key = keySettings.ProtectedKey;
                }
            }

            public override bool HasEditor
            {
                get
                {
                    return true;
                }
            }

            public override bool TextReadOnly
            {
                get
                {
                    return true;
                }
            }

            public override EditorBehavior EditorBehavior
            {
                get
                {
                    return EditorBehavior.ModalPopup;
                }
            }

            public override System.Drawing.Design.UITypeEditor PopupEditor
            {
                get
                {
                    return new KeyManagerEditor();
                }
            }

            public ProtectedKeySettings KeySettings
            {
                get { return TypedValue; }
            }

            public IKeyCreator KeyCreator
            {
                get { return new SymmetricAlgorithmKeyCreator(configuration.AlgorithmType); }
            }
        }
    }
}
