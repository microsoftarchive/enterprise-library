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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// The <see cref="ConfigurationWizardStep"/> defines a step in a configuration wizard and
    /// provides facilities for managing <see cref="WizardProperty"/> collections.
    /// </summary>
    /// <remarks>
    /// The <see cref="ConfigurationWizardStep"/> represents a step in the wizard
    /// with a number of properties that participate in the step.
    /// 
    /// This class helps re-use the <see cref="Property"/> infrastructure in 
    /// configuration tool from within a wizard, by allowing the wizard to
    /// take advantage of services like validation and element references.
    /// </remarks>
    public abstract class ConfigurationWizardStep : WizardStep
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IResolver<Validator> validatorFactory;
        private readonly ElementLookup elementLookup;
        private readonly WizardStepPropertyCollection propertyList;


        /// <summary>
        /// Creates a new <see cref="ConfigurationWizardStep"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="validatorFactory">A factory for creating <see cref="Validator"/> instances.</param>
        /// <param name="elementLookup">A service for locating elements within the current <see cref="ApplicationViewModel"/>.</param>
        protected ConfigurationWizardStep(
            IServiceProvider serviceProvider,
            IResolver<Validator> validatorFactory,
            ElementLookup elementLookup)
        {
            this.serviceProvider = serviceProvider;
            this.validatorFactory = validatorFactory;
            this.elementLookup = elementLookup;
            propertyList = new WizardStepPropertyCollection(this);
        }


        /// <summary>
        /// Invoked when any of the properties managed by this step change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StepPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Value")
            {
                OnPropertyChanged("IsValid");
            }
        }


        /// <summary>
        /// Creates a <see cref="WizardProperty"/> based on a data structure.
        /// </summary>
        /// <param name="wizardData">The attributed structure to use for storing wizard data.  See <see cref="WizardProperty"/>.</param>
        /// <param name="propertyName">The name of the property that must be on <paramref name="wizardData"/></param>
        /// <returns>An initialized wizard property based on <paramref name="wizardData"/>.</returns>
        protected WizardProperty AddReflectedProperty(object wizardData, string propertyName)
        {
            var property = new WizardProperty(serviceProvider,
                                              wizardData,
                                              TypeDescriptor.GetProperties(wizardData)[propertyName],
                                              validatorFactory,
                                              elementLookup
                );


            PropertyList.Add(property);
            return property;
        }

        /// <summary>
        /// Returns true if all properties are valid.
        /// </summary>
        public override bool IsValid
        {
            get { return PropertyList.All(p => p.IsValid); }
        }

        /// <summary>
        /// The set of <see cref="WizardProperty"/> properties this <see cref="ConfigurationWizardStep"/> manages.
        /// </summary>
        protected WizardStepPropertyCollection PropertyList
        {
            get { return propertyList; }
        }

        /// <summary>
        /// The properties managed by <see cref="ConfigurationWizardStep"/> as <see cref="Property"/> enumerable.
        /// </summary>
        public IEnumerable<Property> Properties
        {
            get { return propertyList.Cast<Property>(); }
        }

        /// <summary>
        /// The collection of <see cref="WizardProperty"/> elements.
        /// </summary>
        protected class WizardStepPropertyCollection : Collection<WizardProperty>
        {
            private readonly ConfigurationWizardStep parentStep;


            /// <summary>
            /// Creates a new instance of this step.
            /// </summary>
            /// <param name="parentStep"></param>
            public WizardStepPropertyCollection(ConfigurationWizardStep parentStep)
            {
                this.parentStep = parentStep;
            }

            /// <summary>
            /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.
            ///                 </param><param name="item">The object to insert. The value can be null for reference types.
            ///                 </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.
            ///                     -or-
            ///                 <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
            ///                 </exception>
            protected override void InsertItem(int index, WizardProperty item)
            {
                base.InsertItem(index, item);
                item.PropertyChanged += parentStep.StepPropertyChanged;
                item.Validate();
            }

            /// <summary>
            /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
            /// </summary>
            /// <param name="index">The zero-based index of the element to remove.
            ///                 </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.
            ///                     -or-
            ///                 <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
            ///                 </exception>
            protected override void RemoveItem(int index)
            {
                base[index].PropertyChanged -= parentStep.StepPropertyChanged;
                base.RemoveItem(index);
            }

            /// <summary>
            /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
            /// </summary>
            protected override void ClearItems()
            {
                foreach (var item in base.Items)
                {
                    item.PropertyChanged -= parentStep.StepPropertyChanged;
                }
                base.ClearItems();
            }


            /// <summary>
            /// Retrieves the wizard property based on <paramref name="propertyName"/>
            /// </summary>
            /// <param name="propertyName"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException">Thrown when the <paramref name="propertyName"/> does not exist.</exception>
            public WizardProperty this[string propertyName]
            {
                get { return Items.Where(p => p.PropertyName == propertyName).Single(); }
            }
        }
    }
}
