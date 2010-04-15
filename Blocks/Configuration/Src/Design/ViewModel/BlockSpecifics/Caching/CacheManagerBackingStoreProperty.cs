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
using System.Configuration;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class CacheManagerBackingStoreProperty : ElementReferenceProperty
    {
        BackingStoreReferenceConverter backingStoreReferenceConverter;
        CacheManagerSectionViewModel cacheManagerViewModel;

        public CacheManagerBackingStoreProperty(IServiceProvider serviceProvider, ElementLookup elementLookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, elementLookup, parent, declaringProperty)
        {
            
        }

        public override TypeConverter Converter
        {
            get
            {
                return backingStoreReferenceConverter;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "False positive from build server.")]
        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                return base.SuggestedValues.Union(new[] {string.Empty}).OrderBy(x => x);
            }
        }

        protected override string GetEmptyValue()
        {
            cacheManagerViewModel = (CacheManagerSectionViewModel)ContainingSection;
            return cacheManagerViewModel.NullBackingStoreName;
        }

        public override void Initialize(InitializeContext context)
        {
            cacheManagerViewModel = (CacheManagerSectionViewModel)ContainingSection;
            backingStoreReferenceConverter = new BackingStoreReferenceConverter(cacheManagerViewModel.NullBackingStoreName);

            base.Initialize(context);
        }

        public override IEnumerable<Validator> GetValidators()
        {
            return base.GetValidators()
                .Where(v => v.GetType() != typeof(ElementReferenceValidator))
                .Union(
                    new Validator[] 
                        {new CacheManagerBackingStoreReferenceValidator(cacheManagerViewModel.NullBackingStoreName)}
                    );
        }

        private class CacheManagerBackingStoreReferenceValidator : ElementReferenceValidator
        {
            private readonly string nullBackingStoreName;

            public CacheManagerBackingStoreReferenceValidator(string nullBackingStoreName)
            {
                this.nullBackingStoreName = nullBackingStoreName;
            }

            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
            {
                var property = instance as Property;
                if (property == null) return;

                var convertedValue = property.ConvertFromBindableValue(value);
                if (convertedValue.ToString() == nullBackingStoreName) return;

                base.ValidateCore(instance, value, results);
            }
        }
    }

#pragma warning restore 1591
}
