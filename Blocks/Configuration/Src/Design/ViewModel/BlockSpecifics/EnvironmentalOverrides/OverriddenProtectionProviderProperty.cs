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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class OverriddenProtectionProviderProperty : ProtectionProviderProperty
    {
        readonly EnvironmentOverriddenElementPayload payload;
        readonly ProtectionProviderProperty originalProperty;
        readonly EnvironmentOverriddenElementProperty overridesProperty;

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public OverriddenProtectionProviderProperty(IServiceProvider serviceProvider, EnvironmentOverriddenElementProperty overridesProperty, ProtectionProviderProperty originalProperty, EnvironmentOverriddenElementPayload payload)
            : base(serviceProvider)
        {
            Guard.ArgumentNotNull(overridesProperty, "overridesProperty");
            Guard.ArgumentNotNull(originalProperty, "originalPropery");

            this.payload = payload;
            this.originalProperty = originalProperty;
            this.overridesProperty = overridesProperty;

            this.overridesProperty.PropertyChanged += OverridesPropertyPropertyChanged;
            this.originalProperty.PropertyChanged += OriginalProperyPropertyChanged;
        }

        void OriginalProperyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                OnPropertyChanged("Value");
            }
        }

        void OverridesPropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                bool isOverridden = (bool)overridesProperty.Value;
                this.BindableProperty.ReadOnly = !isOverridden;
                if (!isOverridden)
                {
                    payload.RemoveProtectionProvider(originalProperty.ContainingSection);
                }
                else
                {
                    payload.EnsureProtectionProvider(originalProperty.ContainingSection, (string)originalProperty.Value);
                }
            }
        }

        public override BindableProperty CreateBindableProperty()
        {
            var bindable = base.CreateBindableProperty();
            bindable.ReadOnly = !(bool)overridesProperty.Value;

            return bindable;
        }

        protected override object GetValue()
        {
            return payload.GetProtectionProvider(originalProperty.ContainingSection, originalProperty);
        }

        protected override void SetValue(object value)
        {
            payload.SetProtectionProvider(originalProperty.ContainingSection, originalProperty, (string)value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.overridesProperty.PropertyChanged -= OverridesPropertyPropertyChanged;
                this.originalProperty.PropertyChanged -= OriginalProperyPropertyChanged;
            }

            base.Dispose(disposing);
        }
    }

#pragma warning restore 1591
}
