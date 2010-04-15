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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class OverriddenElementViewModel : INotifyPropertyChanged
    {
        private readonly EnvironmentMergeSection environmentSection;
        private readonly ElementLookup lookup;
        private readonly ReferenceContainer elementReference;
        private ElementViewModel subject;
        private EnvironmentNodeMergeElement mergeElement;

        public OverriddenElementViewModel(ElementLookup lookup, EnvironmentMergeSection environmentSection, EnvironmentNodeMergeElement mergeElement)
        {
            this.lookup = lookup;
            this.environmentSection = environmentSection;
            this.mergeElement = mergeElement;
            this.elementReference = new ReferenceContainer(this);
            this.elementReference.InitializeReference(mergeElement.ConfigurationNodePath, mergeElement);
        }

        public OverriddenElementViewModel(ElementLookup lookup, EnvironmentMergeSection environmentSection, ElementViewModel subject)
        {
            this.lookup = lookup;
            this.environmentSection = environmentSection;
            this.elementReference = new ReferenceContainer(this);
            this.subject = subject;
        }

        public ElementViewModel Subject
        {
            get { return subject ?? elementReference.Subject; }
        }

        public void SetOverrideProperties(ElementViewModel element, bool overrideProperties)
        {
            if (mergeElement == null)
            {
                mergeElement = new EnvironmentNodeMergeElement()
                {
                    ConfigurationNodePath = element.Path,
                    OverrideProperties = overrideProperties
                };
                environmentSection.MergeElements.Add(mergeElement);

                this.elementReference.InitializeReference(element.Path, mergeElement);
            }
            else
            {
                mergeElement.OverrideProperties = overrideProperties;
            }
            OnPropertyChanged("OverrideProperties");
        }

        public bool OverrideProperties
        {
            get
            {
                if (mergeElement == null) return false;
                return mergeElement.OverrideProperties;
            }
        }

        public object GetValue(ElementProperty property, object defaultValue)
        {
            if (mergeElement == null) return defaultValue;
            if (!mergeElement.OverriddenProperties.AllKeys.Contains(property.ConfigurationName)) return defaultValue;

            string overriddenValue = mergeElement.OverriddenProperties[property.ConfigurationName].Value;
            return property.Converter.ConvertFromString(property, CultureInfo.CurrentUICulture, overriddenValue);
        }

        public void SetValue(ElementProperty property, object value)
        {
            if (mergeElement == null) throw new InvalidOperationException("TODO");

            string valueToSet = (string) Convert.ChangeType(value, typeof(string));
            string propertyName = property.ConfigurationName;

            if (!mergeElement.OverriddenProperties.AllKeys.Contains(propertyName))
            {
                var overriddenProperty = new KeyValueConfigurationElement(propertyName, valueToSet);
                this.mergeElement.OverriddenProperties.Add(overriddenProperty);
            }
            else
            {
                mergeElement.OverriddenProperties[propertyName].Value = valueToSet;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private class ReferenceContainer
        {
            bool needsInitialization = true;

            readonly OverriddenElementViewModel overrides;
            ElementReference elementReference;
            EnvironmentNodeMergeElement mergeElement;

            public ReferenceContainer(OverriddenElementViewModel overrides)
            {
                this.overrides = overrides;
            }

            public void InitializeReference(string elementPath, EnvironmentNodeMergeElement mergeElement)
            {
                this.mergeElement = mergeElement;
                this.mergeElement.ConfigurationNodePath = elementPath;

                elementReference = overrides.lookup.CreateReference(elementPath);
                elementReference.ElementDeleted += new EventHandler(elementReference_ElementDeleted);
                elementReference.ElementFound += new EventHandler(elementReference_ElementFound);
                elementReference.PathChanged += new PropertyValueChangedEventHandler<string>(ElementReferencePathChanged);
                
                needsInitialization = false;
            }

            void ElementReferencePathChanged(object sender, PropertyValueChangedEventArgs<string> args)
            {
                this.mergeElement.ConfigurationNodePath = args.Value;
            }

            public ElementViewModel Subject
            {
                get
                {
                    if (needsInitialization) return null;
                    return elementReference.Element;
                }
            }

            void elementReference_ElementFound(object sender, EventArgs e)
            {
            }

            void elementReference_ElementDeleted(object sender, EventArgs e)
            {
                if (overrides.mergeElement != null)
                {
                    overrides.environmentSection.MergeElements.Remove(this.mergeElement);
                }
            }
        }
    }
#pragma warning restore 1591
}
