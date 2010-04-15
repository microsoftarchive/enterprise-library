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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ValidationSectionViewModel : SectionViewModel
    {
        private IElementChangeScope changeScope;

        public ValidationSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, ElementLookup lookup)
            : base(builder, sectionName, section)
        {
            changeScope = lookup.CreateChangeScope(e => e.ContainingSection == this && typeof(ValidatorData).IsAssignableFrom(e.ConfigurationType));
        }

        protected override object CreateBindable()
        {
            var validationTypes = DescendentElements().Where(x => x.ConfigurationType == typeof(ValidatedTypeReferenceCollection)).First();

            return new HorizontalListLayout(
                            changeScope,
                            new HeaderLayout(validationTypes.Name, validationTypes.AddCommands),
                            new HeaderLayout(Resources.ValidationRuleSetsHeader),
                            new HeaderLayout(Resources.ValidationValidationTargetsHeader),
                            new HeaderLayout(Resources.ValidationRulesHeader),
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null
                        )

                        {
                            Contained = new ElementListLayout(validationTypes.ChildElements)
                        };
        }

        protected override void Dispose(bool disposing)
        {
            if (changeScope != null)
            {
                changeScope.Dispose();
                changeScope = null;
            }

            base.Dispose(disposing);
        }

        public override IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            if (typeof(ValidationRulesetData).IsAssignableFrom(element.ConfigurationType))
            {
                return GetRulesetRelatedElements(element);
            }
            else if (typeof(ValidatorData).IsAssignableFrom(element.ConfigurationType))
            {
                return GetRelatedValidatorElements(element);
            }

            return base.GetRelatedElements(element);
        }

        private IEnumerable<ElementViewModel> GetRelatedValidatorElements(ElementViewModel element)
        {
            var parent = element.ParentElement;
            var rulesetGrandParentElement =
                parent != null ? parent.ParentElement as ValidationRulesetDataViewModel : null;
            if (rulesetGrandParentElement == null)
            {
                // this is a regular validator element, rely on the standard related element discovery
                return base.GetRelatedElements(element);
            }

            var relatedElements = new Collection<ElementViewModel>();

            // the parent collection
            relatedElements.Add(parent);

            //elements we refer to
            AddReferredToRelatedElements(element, relatedElements);

            //elements that refer to us.
            AddReferredFromRelatedElements(element, relatedElements);

            //our children
            AddChildrenRelatedElements(element, relatedElements);

            //for children that are collection, their children as well.
            AddCollectionChildrenRelatedElement(element, relatedElements);

            return relatedElements;
        }

        private static IEnumerable<ElementViewModel> GetRulesetRelatedElements(ElementViewModel element)
        {
            var relatedElements = new Collection<ElementViewModel>();

            //elements we refer to - no references

            //elements that refer to us - no references

            //our parent
            AddNonCollectionParentRelatedElement(element, relatedElements);

            //our children
            AddChildrenRelatedElements(element, relatedElements);

            //for children that are collection, their children as well
            // except for the children of the top-level validator collection
            relatedElements.AddRange(
                element.ChildElements.OfType<ElementCollectionViewModel>()
                    .Where(x => x.ConfigurationType != typeof(ValidatorDataCollection))
                    .SelectMany(x => x.ChildElements)
                    .OfType<CollectionElementViewModel>()
                    .Cast<ElementViewModel>());

            //that should be it
            return relatedElements;
        }
    }
#pragma warning restore 1591
}
