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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ValidationSectionViewModel : SectionViewModel
    {
        public ValidationSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
        }

        public override IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            var typesContainer = this.DescendentElements(x => x.ConfigurationType == typeof(ValidatedTypeReferenceCollection)).FirstOrDefault();
            yield return new ElementHeaderViewModel(typesContainer, true) { Row = 0, Column = 0 };
            yield return new StringHeaderViewModel("Rule Sets") { Row = 0, Column = 1 };
            yield return new StringHeaderViewModel("Targets") { Row = 0, Column = 2 };
            yield return new StringHeaderViewModel("Validators") { Row = 0, Column = 3 };
        }

        public override void UpdateLayout()
        {
            var typeReferences = this.DescendentElements(x => x.ConfigurationType == typeof(ValidatedTypeReference));
            int row = 1;

            foreach (var typeReference in typeReferences)
            {
                typeReference.Column = 0;
                typeReference.Row = row;

                foreach (var ruleSet in typeReference.DescendentElements(x => x.ConfigurationType == typeof(ValidationRulesetData)))
                {
                    ruleSet.Column = 1;
                    ruleSet.Row = row;

                    var thisValidators = ruleSet.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatorDataCollection)).FirstOrDefault();
                    if (thisValidators != null)
                    {
                        thisValidators.Column = 2;
                        thisValidators.Row = ruleSet.Row;
                    }

                    var nThisValidator = 0;
                    foreach (var validator in thisValidators.ChildElements.Where(x => typeof(ValidatorData).IsAssignableFrom( x.ConfigurationType) ))
                    {
                        validator.Column = 3;
                        validator.Row = thisValidators.Row + nThisValidator;
                        NestChildValidators(validator);
                        nThisValidator += validator.RowSpan;
                    }

                    if (nThisValidator > 1) thisValidators.RowSpan += (nThisValidator -1);

                    var totalMemberReferencesRowspan = thisValidators.RowSpan;
                    foreach (var memberReference in ruleSet.DescendentElements(x => typeof(ValidatedMemberReference).IsAssignableFrom(x.ConfigurationType)))
                    {
                        memberReference.Column = 2;
                        memberReference.Row = row + totalMemberReferencesRowspan;

                        var nValidator = 0;
                        foreach (var validator in memberReference.ChildElements.SelectMany(x => x.ChildElements).Where(x => typeof(ValidatorData).IsAssignableFrom(x.ConfigurationType)))
                        {
                            validator.Column = 3;
                            validator.Row = memberReference.Row + nValidator;
                            NestChildValidators(validator);
                            nValidator += validator.RowSpan;
                        }

                        if (nValidator > 1) memberReference.RowSpan += (nValidator - 1);
                        totalMemberReferencesRowspan += memberReference.RowSpan;
                    }

                    ruleSet.RowSpan += totalMemberReferencesRowspan;
                    typeReference.RowSpan += ruleSet.RowSpan;
                    row += typeReference.RowSpan;
                }
            }

            OnUpdateVisualGrid();
        }

        private void NestChildValidators(ElementViewModel validator)
        {
            int nContainedValidators = 0;
            foreach (var containedValidator in validator.ChildElements.SelectMany(x => x.ChildElements))
            {
                containedValidator.Column = validator.Column + 1;
                containedValidator.Row = validator.Row + nContainedValidators;
                NestChildValidators(containedValidator);
                nContainedValidators += containedValidator.RowSpan;
            }
            if (nContainedValidators > 1) validator.RowSpan += (nContainedValidators - 1);
        }
    }
}
