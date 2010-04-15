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
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// The <see cref="WizardProperty"/> provides a custom property for use in displaying as part of a <see cref="WizardStep"/>.
    /// </summary>
    /// <remarks>
    /// WizardProperties allow a wizard step to attribute a class and provide infrastructure for
    /// validation and element references, similar to that used by <see cref="ElementViewModel"/> elements.
    /// <seealso cref="ConfigurationWizardStep"/>
    /// <seealso cref="WizardStep"/>
    /// </remarks>
    /// <example>
    ///    
    /// <![CDATA[
    /// class PickExceptionTypeData
    /// {
    ///    [EditorAttribute(typeof(TypeSelectionEditor), typeof(UITypeEditor))]
    ///    [BaseType(typeof(Exception), TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.BaseType)]
    ///    [Validation(typeof(RequiredFieldValidator))]
    ///    [ResourceDisplayName(ResourceName = "PickExceptionTypeStepExceptionTypeDisplayName", ResourceType = typeof(Resources))]
    ///    [EditorWithReadOnlyText(true)]
    ///    public string ExceptionType { get; set; }
    ///    
    ///    [Validation(typeof(RequiredFieldValidator))]
    ///    [Reference(typeof(ExceptionHandlingSettings), typeof(ExceptionPolicyData))]
    ///    [ResourceDisplayName(ResourceName = "PickExceptionTypeStepPolicyDisplayName", ResourceType = typeof(Resources))]
    ///    public string Policy { get; set; }
    ///    
    ///    public string DatabaseName { get; set; }
    ///    
    ///    public string LogCategory { get; set; }
    /// }
    /// 
    /// var structure = new PickExceptionTypeData();
    /// var policyProperty = new WizardProperty(serviceProvider,
    ///                                     structure,
    ///                                     TypeDescriptor.GetProperties(wizardData)[Policy],
    ///                                     validatorFactory,
    ///                                     elementLookup);
    /// 
    /// 
    /// ]]></example>
    public class WizardProperty : Property
    {
        private IResolver<Validator> validatorResolver;
        private readonly ElementLookup elementLookup;

        /// <summary>
        /// Creates a new instance of a <see cref="WizardProperty"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="component">The <see cref="object"/> storing the value for this property as described by <paramref name="declaringProperty"/>.</param>
        /// <param name="declaringProperty">The property definition for this property.</param>
        /// <param name="validatorResolver">A factory for creating <see cref="Validator"/> instances.</param>
        /// <param name="elementLookup">A locator to locate <see cref="ElementViewModel"/> items in the current <see cref="ApplicationViewModel"/>.</param>
        /// <param name="additionalAttributes">Additional <see cref="Attribute"/> items for this element.</param>
        [InjectionConstructor]
        public WizardProperty(IServiceProvider serviceProvider, 
            object component, 
            PropertyDescriptor declaringProperty, 
            IResolver<Validator> validatorResolver,
            ElementLookup elementLookup,
            params Attribute[] additionalAttributes
            )
            : base(serviceProvider, component, declaringProperty, additionalAttributes)
        {
            this.validatorResolver = validatorResolver;
            this.elementLookup = elementLookup;
        }

        /// <summary>
        /// Gets the set of validators for this property.
        /// </summary>
        /// <remarks>
        /// Validators may be added by deriving from this and returning additional <see cref="Validator"/> objects.  
        /// Or, they can be added by providing <see cref="ValidationAttribute"/> attributes to the 
        /// underlying <see cref="Property.Component"/> or during the construction of <see cref="Property"/>.<br/>
        /// Validators specified by <see cref="ValidationAttribute"/> are created by the containing <see cref="SectionViewModel"/>.
        /// </remarks>
        /// <returns>An <see cref="IEnumerable{Validator}"/> containing default property validators, obtained from <see cref="Property.GetDefaultPropertyValidators"/>
        /// and additional validators provided through <see cref="ValidationAttribute"/>.</returns>
        public override IEnumerable<Validator> GetValidators()
        {
            var validations = GetDefaultPropertyValidators()
               .Union(Attributes.OfType<ValidationAttribute>()
                          .Select(v => validatorResolver.Resolve(v.ValidatorType)));

            return validations;
        }

        private bool HasElementReference
        {
            get { return Attributes.OfType<ReferenceAttribute>().FirstOrDefault() != null;  }
        }

        /// <summary>
        /// Returns a value indicating that this property has suggested values.
        /// </summary>
        /// <value>Returns <see langword="true"/> if there are suggested values for this property. 
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        /// <seealso cref="Property.SuggestedValues"/>
        public override bool HasSuggestedValues
        {
            get
            {
                return HasElementReference || base.HasSuggestedValues;
            }
        }

        private IEnumerable<ElementViewModel> GetReferencedElements()
        {
            var referenceAttribute = Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            if (referenceAttribute != null)
            {
                return elementLookup.FindInstancesOfConfigurationType(referenceAttribute.ScopeType,
                                                                                referenceAttribute.TargetType);
            }

            return Enumerable.Empty<ElementViewModel>();
        }

        /// <summary>
        /// Get a list of suggested values.
        /// </summary>
        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                if (HasElementReference)
                {
                    var referencedElements = GetReferencedElements();
                    
                    return referencedElements
                            .Select(vm => vm.Name)
                            .Cast<object>();
                }
                return base.SuggestedValues;
            }
        }
        
        ///<summary>
        /// Gets the suggested element based on the selected item from <see cref="SuggestedValues"/>.
        ///</summary>
        public ElementViewModel SuggestedElement
        {
            get
            {
                if (HasSuggestedValues && Value is string && SuggestedValues.Contains((string)Value))
                {
                    return GetReferencedElements().Where(vm => vm.Name == (string)Value).FirstOrDefault();
                }

                return null;
            }
        }

    }


    /// <summary>
    /// Represents a property associated with another <see cref="WizardProperty"/>.  
    /// This property associates itself with another property and marks itself as ReadOnly 
    /// if the associated property value is not from that properties suggested value list.
    /// </summary>
    public class AssociatedWizardProperty : WizardProperty
    {
        private readonly WizardProperty associatedProperty;

        /// <summary>
        /// Creates an instance of <see cref="AssociatedWizardProperty"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="component"></param>
        /// <param name="declaringProperty"></param>
        /// <param name="validatorResolver"></param>
        /// <param name="elementLookup"></param>
        /// <param name="associatedProperty"></param>
        /// <param name="additionalAttributes"></param>
        public AssociatedWizardProperty(
            IServiceProvider serviceProvider, 
            object component, 
            PropertyDescriptor declaringProperty, 
            IResolver<Validator> validatorResolver, 
            ElementLookup elementLookup,
            WizardProperty associatedProperty,
            params Attribute[] additionalAttributes) 
            : base(serviceProvider, component, declaringProperty, validatorResolver, elementLookup, additionalAttributes)
        {
            this.associatedProperty = associatedProperty;

            associatedProperty.PropertyChanged += AssociatedPropertyChangedHandler;
        }

        private void AssociatedPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var isASuggestedValue = associatedProperty.SuggestedValues.Contains(associatedProperty.Value);
            BindableProperty.ReadOnly = isASuggestedValue;

            if (isASuggestedValue)
            {
                Value = associatedProperty.SuggestedElement.Property(base.DeclaringProperty.Name).Value;
            }
        }
    }
}
