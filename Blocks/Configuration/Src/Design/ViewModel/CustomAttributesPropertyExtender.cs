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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="CustomAttributesPropertyExtender"/> provides extended properties for configuration elements with attributes
    /// provided through the <see cref="ICustomProviderData"/> interface.
    /// </summary>
    public class CustomAttributesPropertyExtender : ElementViewModel, IElementExtendedPropertyProvider
    {

        /// <summary>
        /// Initializes a new instance of <see cref="CustomAttributesPropertyExtender"/>.
        /// </summary>
        public CustomAttributesPropertyExtender()
            :base(null, (ConfigurationElement)null, new Attribute[]{new EnvironmentalOverridesAttribute(false)})
        {
        }

        /// <summary>
        /// Gets a value that indicates this extender provides <see cref="Property"/>
        /// elements to a <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="subject">The <see cref="ElementViewModel"/> to determine properties for.</param>
        /// <returns>
        /// Should return <see langword="true"/> if this extender provides properties for the <paramref name="subject"/>.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        public bool CanExtend(ElementViewModel subject)
        {
            return typeof(ICustomProviderData).IsAssignableFrom(subject.ConfigurationType) && TypeDescriptor.GetAttributes(subject.ConfigurationElement).OfType<OmitCustomAttributesPropertyAttribute>().Count() == 0;
        }

        /// <summary>
        /// Returns the set of <see cref="Property"/> elements to add to the <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="subject">The <see cref="ElementViewModel"/> to provide properties for.</param>
        /// <returns>
        /// Returns the set of additional <see cref="Property"/> items to add to the <paramref name="subject"/>.
        /// </returns>
        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            var attributesPropertyDescriptor = TypeDescriptor.GetProperties(subject.ConfigurationType).OfType<PropertyDescriptor>().FirstOrDefault(x => x.Name == "Attributes");
            
            yield return subject.ContainingSection.CreateProperty<CustomAttributesProperty>(
                    new DependencyOverride<ElementViewModel>(subject),
                    new DependencyOverride<PropertyDescriptor>(attributesPropertyDescriptor));

        }

        /// <summary>
        /// Gets the type of the configuration element this <see cref="ElementViewModel"/> was created for. <br/>
        /// </summary>
        /// <remarks>
        /// A <see cref="ElementViewModel"/>'s ConfigurationType is often used to identify or attach behavior to <see cref="ElementViewModel"/> instances.
        /// </remarks>
        public override Type ConfigurationType
        {
            get
            {
                return typeof(object);
            }
        }

        /// <summary>
        /// Gets a string that can be used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        public override string Path
        {
            get
            {
                return "<custom attributes>";
            }
        }

        private class CustomAttributesProperty : Property, ILogicalPropertyContainerElement
        {
            ElementViewModel subject;

            public CustomAttributesProperty(IServiceProvider serviceProvider, ElementViewModel subject, PropertyDescriptor declaringProperty)
                : base(serviceProvider, subject.ConfigurationElement, declaringProperty, 
                    new Attribute[] { 
                        new DisplayNameAttribute(Resources.CustomAttributesPropertyDisplayName),
                        new DescriptionAttribute(Resources.CustomAttributesPropertyDescription),
                        new ReadOnlyAttribute(false), 
                        ResourceCategoryAttribute.General, 
                        new EnvironmentalOverridesAttribute(false), 
                        new EditorAttribute(typeof(CustomAttributesEditor), typeof(FrameworkElement)),
                        new ValidationAttribute(typeof(CustomAttributesPropertyValidator))
                    })
            {
                this.subject = subject;
            }

            public override string PropertyName
            {
                get { return "Attributes"; }
            }

            protected override IEnumerable<Validator> GetDefaultPropertyValidators()
            {
                return Enumerable.Empty<Validator>();
            }

            protected override void SetValue(object value)
            {
                var actualNameValues = ((NameValueCollection)base.Value);
                actualNameValues.Clear();

                NameValueCollection newNameValues = (NameValueCollection)value;
                foreach (string key in newNameValues.AllKeys)
                {
                    actualNameValues.Add(key, newNameValues[key]);
                }

                Validate();
            }

            #region ILogicalPropertyContainerElement Members

            ElementViewModel ILogicalPropertyContainerElement.ContainingElement
            {
                get { return subject; }
            }

            string ILogicalPropertyContainerElement.ContainingElementDisplayName
            {
                get { return subject.Name; }
            }

            #endregion
        }

        private class CustomAttributesPropertyValidator : Validator
        {
            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
            {
                CustomAttributesProperty property = instance as CustomAttributesProperty;
                if (property != null)
                {
                    var bindable = (FrameworkEditorBindableProperty)property.BindableProperty;
                    
                    foreach(CustomAttributesEditor editor in bindable.CreatedEditorReferences)
                    {
                        var attributes = editor.GetEditorAttributes().ToArray();
                        var groupedAttributes = attributes.GroupBy(x => x.Key);
                        var duplicateKeys = groupedAttributes.Where(x => x.Count() > 1).Select(x => x.Key);

                        foreach (var duplicateKey in duplicateKeys)
                        {
                            results.Add(new PropertyValidationResult(property,
                                    string.Format(CultureInfo.CurrentCulture, Resources.ErrorCustomAttributesDuplicateKey, duplicateKey),
                                    false));
                        }
                    }
                }
            }
        }
    }
}
