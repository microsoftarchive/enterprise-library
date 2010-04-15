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
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ValidationModel"/> tracks all <see cref="ValidationResult"/> instances
    /// by elements in <see cref="ElementLookup"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ValidationModel"/> monitors changes in <see cref="ElementLookup"/> and as new <see cref="ElementViewModel"/> are
    /// added begins montoring their <see cref="ElementViewModel.ValidationResults"/> and their properties' validation results.
    /// <br/>
    /// As results are added or removed for each element, this model keeps this list up-to-date for display in the design-time user interface.
    /// </remarks>
    public class ValidationModel
    {
        private readonly ElementLookup lookup;
        private readonly CompositeValidationResultsCollection resultsCollection = new CompositeValidationResultsCollection();
        private readonly NavigateValidationResultCommand navigateValidationResultCommand;
        private readonly List<ElementViewModel> elementsMonitored = new List<ElementViewModel>();
        private bool populating = false;

        ///<summary>
        /// Initializes a new instance of <see cref="ValidationModel"/>.
        ///</summary>
        ///<param name="lookup">The element loookup service to use for monitoring the addition and removal of <see cref="ElementViewModel"/> items.</param>
        public ValidationModel(ElementLookup lookup)
        {
            this.lookup = lookup;
            this.navigateValidationResultCommand = new NavigateValidationResultCommand(this);
            ResetCollection();
            lookup.CollectionChanged += ElementsChanged;
        }

        private void ElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var elementViewModel = item as ElementViewModel;
                    if (elementViewModel == null) continue;
                    AddElementResultsCollections(elementViewModel);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var elementViewModel = item as ElementViewModel;
                    if (elementViewModel == null) continue;
                    RemoveElementResultsCollections(elementViewModel);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetCollection();
            }
        }

        private void ResetCollection()
        {
            resultsCollection.NotifyEnabled = false;
            ClearElementsMonitored();
            resultsCollection.Clear();
            PopulateCollection();
            resultsCollection.NotifyEnabled = true;
        }

        private void PopulateCollection()
        {
            populating = true;

            foreach (var section in lookup.FindInstancesOfConfigurationType(typeof(ConfigurationSection)))
            {
                AddElementResultsCollections(section);
            }

            populating = false;
        }

        private void RemoveElementResultsCollections(ElementViewModel element)
        {
            resultsCollection.Remove(element.ValidationResults);

            foreach (var prop in element.Properties)
            {
                resultsCollection.Remove(prop.ValidationResults);
            }

            foreach (var child in element.ChildElements)
            {
                RemoveElementResultsCollections(child);
            }

            RemoveElementPropertyMonitoring(element);

        }

        private void AddElementResultsCollections(ElementViewModel element)
        {
            resultsCollection.Add(element.ValidationResults);

            foreach (var prop in element.Properties)
            {
                resultsCollection.Add(prop.ValidationResults);
            }

            foreach (var child in element.ChildElements)
            {
                AddElementResultsCollections(child);
            }

            AddElementPropertyMonitoring(element);
        }

        /// <summary>
        /// Orientates the designer to the configuration element associated with the given <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="validationResult">The <see cref="ValidationResult"/> that should be navigated to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public void Navigate(ValidationResult validationResult)
        {
            Guard.ArgumentNotNull(validationResult, "validationResult");

            var elementWithError = lookup.GetElementById(validationResult.ElementId);
            if (elementWithError != null)
            {
                if (!(elementWithError is SectionViewModel))
                {
                    elementWithError.ContainingSection.ExpandSection();
                }
                elementWithError.PropertiesShown = true;
                elementWithError.Select();
            }
        }

        /// <summary>
        /// Provides an <see cref="ICommand"/> implementation that allows to execute the <see cref="ValidationModel.Navigate"/> method from within XAML.
        /// </summary>
        public ICommand NavigateCommand
        {
            get { return this.navigateValidationResultCommand; }
        }

        ///<summary>
        /// Gets the set of validation results collected from all elements.
        ///</summary>
        public IEnumerable<ValidationResult> ValidationResults
        {
            get { return resultsCollection; }
        }

        private void RemoveElementPropertyMonitoring(ElementViewModel element)
        {
            this.elementsMonitored.Remove(element);
            element.Properties.CollectionChanged -= ElementPropertiesCollectionChangedHandler;
        }

        private void AddElementPropertyMonitoring(ElementViewModel element)
        {
            if (!elementsMonitored.Contains(element))
            {
                this.elementsMonitored.Add(element);
                element.Properties.CollectionChanged += ElementPropertiesCollectionChangedHandler;
            }
        }


        private void ClearElementsMonitored()
        {
            foreach (var element in elementsMonitored)
            {
                element.Properties.CollectionChanged -= ElementPropertiesCollectionChangedHandler;
            }

            elementsMonitored.Clear();
        }

        private void ElementPropertiesCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (populating) return;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetCollection();
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        resultsCollection.Remove(((Property)oldItem).ValidationResults);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var newItem in e.NewItems)
                    {
                        resultsCollection.Add(((Property)newItem).ValidationResults);
                    }
                }
            }
        }

        private class NavigateValidationResultCommand : ICommand
        {
            private readonly ValidationModel validationModel;

            public NavigateValidationResultCommand(ValidationModel validationModel)
            {
                this.validationModel = validationModel;
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ValidationResult;
            }

#pragma warning disable 67
            public event EventHandler CanExecuteChanged;
#pragma warning restore 67

            public void Execute(object parameter)
            {
                ValidationResult result = (ValidationResult)parameter;
                validationModel.Navigate(result);
            }
        }

    }
}
