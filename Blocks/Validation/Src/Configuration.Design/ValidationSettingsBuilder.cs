//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	sealed partial class ValidationSettingsBuilder
	{
        private ValidationSettingsNode blockSettingsNode;
		private IConfigurationUIHierarchy hierarchy;
		private ValidationSettings blockSettings;

        public ValidationSettingsBuilder(IServiceProvider serviceProvider, ValidationSettingsNode blockSettingsNode)
		{
			this.blockSettingsNode = blockSettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}

		public ValidationSettings Build()
		{
			blockSettings = new ValidationSettings();
			if (!blockSettingsNode.RequirePermission)	// don't set if false
				blockSettings.SectionInformation.RequirePermission = blockSettingsNode.RequirePermission;

            foreach (TypeNode typeNode in blockSettingsNode.Hierarchy.FindNodesByType(typeof(TypeNode)))
            {
                ValidatedTypeReference typeReference = new ValidatedTypeReference();

                typeReference.Name = typeNode.TypeName;
                typeReference.AssemblyName = typeNode.AssemblyName;

                if (typeNode.DefaultRule != null)
                {
                    typeReference.DefaultRuleset = typeNode.DefaultRule.Name;
                }
                BuildRules(typeReference, typeNode);

                blockSettings.Types.Add(typeReference);
            }
			return blockSettings;
		}

        private void BuildRules(ValidatedTypeReference typeReference, TypeNode typeNode)
        {
            foreach (RuleSetNode ruleNode in typeNode.Hierarchy.FindNodesByType(typeNode, typeof(RuleSetNode)))
            {
                ValidationRulesetData validationRule = new ValidationRulesetData(ruleNode.Name);

                BuildPropertyReferences(validationRule, ruleNode);
                BuildMethodReferences(validationRule, ruleNode);
                BuildFieldReferences(validationRule, ruleNode);

                SelfNode selfNode = ruleNode.Hierarchy.FindNodeByType(ruleNode, typeof(SelfNode)) as SelfNode;
                if (selfNode != null)
                {
                    foreach (ValidatorData validator in FindValidators(selfNode))
                    {
                        validationRule.Validators.Add(validator);
                    }
                }

                typeReference.Rulesets.Add(validationRule);
            }
        }

        private void BuildPropertyReferences(ValidationRulesetData validationRule, RuleSetNode ruleNode)
        {
            foreach (PropertyNode propertyNode in ruleNode.Hierarchy.FindNodesByType(ruleNode, typeof(PropertyNode)))
            {
                ValidatedPropertyReference propertyReference = new ValidatedPropertyReference();
                propertyReference.Name = propertyNode.Name;

                foreach (ValidatorData validator in FindValidators(propertyNode))
                {
                    propertyReference.Validators.Add(validator);
                }

                validationRule.Properties.Add(propertyReference);
            }
        }
        private void BuildMethodReferences(ValidationRulesetData validationRule, RuleSetNode ruleNode)
        {
            foreach (MethodNode methodNode in ruleNode.Hierarchy.FindNodesByType(ruleNode, typeof(MethodNode)))
            {
                ValidatedMethodReference methodReference = new ValidatedMethodReference();
                methodReference.Name = methodNode.Name;

                foreach (ValidatorData validator in FindValidators(methodNode))
                {
                    methodReference.Validators.Add(validator);
                }

                validationRule.Methods.Add(methodReference);
            }
        }

        private void BuildFieldReferences(ValidationRulesetData validationRule, RuleSetNode ruleNode)
        {
            foreach (FieldNode fieldNode in ruleNode.Hierarchy.FindNodesByType(ruleNode, typeof(FieldNode)))
            {
                ValidatedFieldReference fieldReference = new ValidatedFieldReference();
                fieldReference.Name = fieldNode.Name;

                foreach (ValidatorData validator in FindValidators(fieldNode))
                {
                    fieldReference.Validators.Add(validator);
                }

                validationRule.Fields.Add(fieldReference);
            }
        }

        private IEnumerable<ValidatorData> FindValidators(ConfigurationNode validatorContainer)
        {
            foreach (ConfigurationNode child in validatorContainer.Nodes)
            {
                ValidatorNodeBase validatorNode = child as ValidatorNodeBase;
                if (validatorNode != null)
                {
                    yield return validatorNode.CreateValidatorData();
                }
            }
        }

	}
}