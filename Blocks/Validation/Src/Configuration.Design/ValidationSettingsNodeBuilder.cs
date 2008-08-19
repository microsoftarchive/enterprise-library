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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	sealed class ValidationSettingsNodeBuilder : NodeBuilder
	{
		private ValidationSettings blockSettings;
        private ValidationSettingsNode node;

		public ValidationSettingsNodeBuilder(IServiceProvider serviceProvider, ValidationSettings blockSettings)
			: base(serviceProvider)
		{
			this.blockSettings = blockSettings;
		}

        public ValidationSettingsNode Build()
		{
            this.node = new ValidationSettingsNode();

			foreach(ValidatedTypeReference typeReference in blockSettings.Types)
            {
                TypeNode typeNode = new TypeNode(typeReference);

                BuildTypeNode(typeNode, typeReference);
                

                this.node.AddNode(typeNode);
            }

			node.RequirePermission = blockSettings.SectionInformation.RequirePermission;

			return node;
		}

        private void BuildTypeNode(TypeNode typeNode, ValidatedTypeReference typeReference)
        {
            foreach (ValidationRulesetData rule in typeReference.Rulesets)
            {
                RuleSetNode ruleNode = new RuleSetNode(rule);

                BuildRuleNode(ruleNode, rule);

                typeNode.AddNode(ruleNode);
                if (string.Compare(typeReference.DefaultRuleset, rule.Name) == 0)
                {
                    typeNode.DefaultRule = ruleNode;
                }
            }
        }

        private void BuildRuleNode(RuleSetNode ruleNode, ValidationRulesetData rule)
        {
            SelfNode selfNode = new SelfNode();
            ruleNode.AddNode(selfNode);

            AddValidatorNodes(selfNode, rule.Validators);

            foreach (ValidatedFieldReference fieldReference in rule.Fields)
            {
                FieldNode fieldNode = new FieldNode(fieldReference);
                AddValidatorNodes(fieldNode, fieldReference.Validators);

                ruleNode.AddNode(fieldNode);
            }

            foreach (ValidatedPropertyReference proprtyReference in rule.Properties)
            {
                PropertyNode propertyNode = new PropertyNode(proprtyReference);
                AddValidatorNodes(propertyNode, proprtyReference.Validators);

                ruleNode.AddNode(propertyNode);
            }


            foreach (ValidatedMethodReference methodReference in rule.Methods)
            {
                MethodNode methodNode = new MethodNode(methodReference);
                AddValidatorNodes(methodNode, methodReference.Validators);

                ruleNode.AddNode(methodNode);
            }
        }

        private void AddValidatorNodes(ConfigurationNode parentNode, ValidatorDataCollection validatorCollection)
        {
            foreach (ValidatorData validator in validatorCollection)
            {
                ConfigurationNode validatorNode = NodeCreationService.CreateNodeByDataType(validator.GetType(), new object[] { validator });
                if (validatorNode == null)
                {
                    LogNodeMapError(parentNode, validator.GetType());
                    continue;
                }

                if (validator is OrCompositeValidatorData)
                {
                    ValidatorDataCollection childValidators = ((OrCompositeValidatorData)validator).Validators;

                    AddValidatorNodes(validatorNode, childValidators);
                }
                else if (validator is AndCompositeValidatorData)
                {
                    ValidatorDataCollection childValidators = ((AndCompositeValidatorData)validator).Validators;

                    AddValidatorNodes(validatorNode, childValidators);
                }

                parentNode.AddNode(validatorNode);
            }
        }
	}
}
