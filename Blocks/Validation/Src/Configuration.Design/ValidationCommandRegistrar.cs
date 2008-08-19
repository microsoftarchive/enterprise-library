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
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	class ValidationCommandRegistrar : CommandRegistrar
	{
        public ValidationCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
            AddValidationSettingCommands();

            AddTypeCommands();

            AddRuleCommands();

            AddFieldCommands();
            AddPropertyCommands();
            AddMethodCommands();

            AddValidatorCommands(typeof(NotNullValidatorNode), Resources.AddNotNullValidatorCommandName, Resources.AddNotNullValidatorCommand);
			AddValidatorCommands(typeof(RangeValidatorNode), Resources.AddRangeValidatorCommandName, Resources.AddRangeValidatorCommand);
			AddValidatorCommands(typeof(DateRangeValidatorNode), Resources.AddDateRangeValidatorCommandName, Resources.AddDateRangeValidatorCommand);
			AddValidatorCommands(typeof(StringLengthValidatorNode), Resources.AddStringLengthValidatorCommandName, Resources.AddStringLengthValidatorCommand);
			AddValidatorCommands(typeof(RegexValidatorNode), Resources.AddRegexValidatorCommandName, Resources.AddRegexValidatorCommand);
			AddValidatorCommands(typeof(TypeConversionValidatorNode), Resources.AddTypeConversionValidatorCommandName, Resources.AddTypeConversionValidatorCommand);
			AddValidatorCommands(typeof(EnumConversionValidatorNode), Resources.AddEnumConversionValidatorCommandName, Resources.AddEnumConversionValidatorCommand);
			AddValidatorCommands(typeof(RelativeDateTimeValidatorNode), Resources.AddRelativeDateTimeValidatorCommandName, Resources.AddRelativeDateTimeValidatorCommand);
			AddValidatorCommands(typeof(ContainsCharactersValidatorNode), Resources.AddContainsCharactersValidatorCommandName, Resources.AddContainsCharactersValidatorCommand);
			AddValidatorCommands(typeof(DomainValidatorNode), Resources.AddDomainValidatorCommandName, Resources.AddDomainValidatorCommand);

            AddValidatorCommands(typeof(CustomValidatorNode), Resources.AddCustomValidatorCommandName, Resources.AddCustomValidatorCommand);
            
            AddValidatorCommands(typeof(OrCompositeValidatorNode), Resources.AddOrCompositeValidatorCommandName, Resources.AddOrCompositeValidatorCommand);
            AddValidatorCommands(typeof(AndCompositeValidatorNode), Resources.AddAndCompositeValidatorCommandName, Resources.AddAndCompositeValidatorCommand);

			AddValidatorCommands(typeof(ObjectValidatorNode), Resources.AddObjectValidatorCommandName, Resources.AddObjectValidatorCommand);
			AddValidatorCommands(typeof(ObjectCollectionValidatorNode), Resources.AddObjectCollectionValidatorCommandName, Resources.AddObjectCollectionValidatorCommand);
			AddValidatorCommands(typeof(PropertyComparisonValidatorNode), Resources.AddPropertyComparisonValidatorCommandName, Resources.AddPropertyComparisonValidatorCommand);

            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.AddMemberCommandName,
                Resources.AddMemberCommand,
                new ChooseMembersCommand(ServiceProvider), 
                typeof(ConfigurationNode));

            AddUICommand(cmd, typeof(RuleSetNode));
		}

        protected virtual void AddValidatorCommands(Type validatorNodeType, string text, string longText)
        {
            AddMoveUpDownCommands(validatorNodeType);

            AddDefaultCommands(validatorNodeType);

            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(FieldNode));

            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(MethodNode));

            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(PropertyNode));


            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(SelfNode));


            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(AndCompositeValidatorNode));


            AddMultipleChildNodeCommand(
                text,
                longText,
                validatorNodeType,
                typeof(OrCompositeValidatorNode));
        }

        private void AddMethodCommands()
        {
            AddMultipleChildNodeCommand(
                Resources.AddMethodReferenceCommandName,
                Resources.AddMethodReferenceCommand,
                typeof(MethodNode),
                typeof(RuleSetNode));

            AddDefaultCommands(typeof(MethodNode));
        }

        private void AddPropertyCommands()
        {
            AddMultipleChildNodeCommand(
                Resources.AddPropertyReferenceCommandName,
                Resources.AddPropertyReferenceCommand,
                typeof(PropertyNode),
                typeof(RuleSetNode));

            AddDefaultCommands(typeof(PropertyNode));
        }

        private void AddFieldCommands()
        {
            AddMultipleChildNodeCommand(
                Resources.AddFieldReferenceCommandName,
                Resources.AddFieldReferenceCommand,
                typeof(FieldNode),
                typeof(RuleSetNode));

            AddDefaultCommands(typeof(FieldNode));
        }

        private void AddRuleCommands()
        {
            AddDefaultCommands(typeof(RuleSetNode));

            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.AddRuleSetCommandName,
                Resources.AddRuleSetCommand,
                new AddRuleSetNodeCommand(ServiceProvider),
                typeof(RuleSetNode));

            AddUICommand(cmd, typeof(TypeNode));
        }

        private void AddTypeCommands()
        {
            AddDefaultCommands(typeof(TypeNode));

            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.AddValidationTypeCommandName,
                Resources.AddValidationTypeCommand,
                new AddTypeNodeCommand(ServiceProvider),
                typeof(TypeNode));

            AddUICommand(cmd, typeof(ValidationSettingsNode));

        }

		public void AddValidationSettingCommands()
        {
            AddDefaultCommands(typeof(ValidationSettingsNode));

            AddSingleChildNodeCommand(Resources.AddValidationSettingsCommandName, 
                Resources.AddValidationSettingsCommand, 
                typeof(ValidationSettingsNode), 
                typeof(ConfigurationApplicationNode));
		}
	}
}
