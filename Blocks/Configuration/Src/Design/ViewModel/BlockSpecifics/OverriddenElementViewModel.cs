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
using Console.Wpf.ViewModel.Services;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class OverriddenElementViewModel : INotifyPropertyChanged
    {
        private readonly EnvironmentMergeSection environmentSection;
        private readonly ElementLookup lookup;
        private readonly ReferenceContainer elementReference;

        private EnvironmentNodeMergeElement mergeElement;

        public OverriddenElementViewModel(ElementLookup lookup, EnvironmentMergeSection environmentSection, EnvironmentNodeMergeElement mergeElement)
            :this(lookup, environmentSection)
        {
            this.mergeElement = mergeElement;
            this.elementReference.InitializeReference(mergeElement.ConfigurationNodePath, mergeElement);
        }

        public OverriddenElementViewModel(ElementLookup lookup, EnvironmentMergeSection environmentSection)
        {
            this.lookup = lookup;
            this.environmentSection = environmentSection;
            this.elementReference = new ReferenceContainer(this);
        }

        public ElementViewModel Subject
        {
            get { return elementReference.Subject; }
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

        public object GetValue(Property property, object defaultValue)
        {
            if (mergeElement == null) return defaultValue;
            if (!mergeElement.OverriddenProperties.AllKeys.Contains(property.PropertyName)) return defaultValue;

            string overriddenValue = mergeElement.OverriddenProperties[property.PropertyName].Value;
            return property.Converter.ConvertFromString(property, CultureInfo.CurrentUICulture, overriddenValue);
        }

        public void SetValue(Property property, object value)
        {
            if (mergeElement == null) throw new InvalidOperationException("TODO");

            string valueToSet = property.Converter.ConvertToString(property, CultureInfo.CurrentUICulture, value);
            string propertyName = property.PropertyName;

            if (!mergeElement.OverriddenProperties.AllKeys.Contains(propertyName))
            {
                var overriddenProperty = new NameValueConfigurationElement(property.PropertyName, valueToSet);
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

            OverriddenElementViewModel overrides;
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
                elementReference.PathChanged += new PropertyValueChangedEventHandler<string>(elementReference_PathChanged);
                
                needsInitialization = false;
            }

            void elementReference_PathChanged(object sender, PropertyValueChangedEventArgs<string> args)
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

            public bool NeedsInitialization
            {
                get { return needsInitialization; }
            }
        }
    }
}
