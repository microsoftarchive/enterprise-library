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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class BaseTestTypeWithValidatorAttributes
	{
		public String PropertyWithNoAttributes
		{
			get { return null; }
		}

		[MockValidator(false, MessageTemplate = "PropertyWithSingleAttribute-Message1")]
		[MockValidator(false, MessageTemplate = "PropertyWithSingleAttribute-Message1-RuleA", Ruleset = "RuleA")]
		public int PropertyWithSingleAttribute
		{
			get { return 0; }
		}

		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributes-Message1")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributes-Message2")]
		public virtual object PropertyWithMultipleAttributes
		{
			get { return null; }
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationModifier-Message1")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationModifier-Message2")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationModifier-Message1-RuleA", Ruleset = "RuleA")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationModifier-Message2-RuleA", Ruleset = "RuleA")]
		public object PropertyWithMultipleAttributesAndValidationModifier
		{
			get { return null; }
		}

		[ValidatorComposition(CompositionType.And)]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd-Message1")]
		[MockValidator(false, MessageTemplate = "PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd-Message2")]
		public object PropertyWithMultipleAttributesAndValidationCompositionAttributeForAnd
		{
			get { return null; }
		}

		public String FieldWithNoAttributes = null;

		[MockValidator(false, MessageTemplate = "FieldWithSingleAttribute-Message1")]
		public int FieldWithSingleAttribute = 0;

		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributes-Message1")]
		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributes-Message2")]
		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributes-Message1-RuleA", Ruleset = "RuleA")]
		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributes-Message2-RuleA", Ruleset = "RuleA")]
		public object FieldWithMultipleAttributes = null;

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributesAndValidationModifier-Message1")]
		[MockValidator(false, MessageTemplate = "FieldWithMultipleAttributesAndValidationModifier-Message2")]
		public object FieldWithMultipleAttributesAndValidationModifier = null;

		public String MethodWithNoAttributes()
		{
			return null;
		}

		[MockValidator(false, MessageTemplate = "MethodWithSingleAttribute-Message1")]
		public int MethodWithSingleAttribute()
		{
			return 0;
		}

		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributes-Message1")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributes-Message2")]
		public virtual object MethodWithMultipleAttributes()
		{
			return null;
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or)]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesAndValidationModifier-Message1")]
		[MockValidator(false, MessageTemplate = "MethodWithMultipleAttributesAndValidationModifier-Message2")]
		public object MethodWithMultipleAttributesAndValidationModifier()
		{
			return null;
		}
	}
}
