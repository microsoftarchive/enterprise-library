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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	/// <summary>
	/// Encapsulates shared validator building behavior.
	/// </summary>
	/// <remarks>
	/// This class relies on implementations of <see cref="IValidatedType"/> to supply validation descriptors.
	/// </remarks>
	internal class ValidatorBuilderBase
	{
		private MemberAccessValidatorBuilderFactory memberAccessValidatorFactory;

		public ValidatorBuilderBase()
			: this(new MemberAccessValidatorBuilderFactory())
		{ }

		public ValidatorBuilderBase(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
		{
			this.memberAccessValidatorFactory = memberAccessValidatorFactory;
		}

		public Validator CreateValidator(IValidatedType validatedType)
		{
			List<Validator> validators = new List<Validator>();

			CollectValidatorsForType(validatedType, validators);
			CollectValidatorsForProperties(validatedType.GetValidatedProperties(), validators, validatedType.TargetType);
			CollectValidatorsForFields(validatedType.GetValidatedFields(), validators, validatedType.TargetType);
			CollectValidatorsForMethods(validatedType.GetValidatedMethods(), validators, validatedType.TargetType);
			CollectValidatorsForSelfValidationMethods(validatedType.GetSelfValidationMethods(), validators);
            
            if (validators.Count == 1)
            {
                return validators[0];
            }
            else
            {
                return new AndCompositeValidator(validators.ToArray());
            }
		}

		private void CollectValidatorsForType(IValidatedType validatedType, List<Validator> validators)
		{
			Validator validator = CreateValidatorForValidatedElement(validatedType, this.GetCompositeValidatorBuilderForType);

			if (validator != null)
			{
				validators.Add(validator);
			}
		}

		private void CollectValidatorsForProperties(IEnumerable<IValidatedElement> validatedElements,
			List<Validator> validators,
			Type ownerType)
		{
			foreach (IValidatedElement validatedElement in validatedElements)
			{
				Validator validator = CreateValidatorForValidatedElement(validatedElement,
					this.GetCompositeValidatorBuilderForProperty);

				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForFields(IEnumerable<IValidatedElement> validatedElements,
			List<Validator> validators,
			Type ownerType)
		{
			foreach (IValidatedElement validatedElement in validatedElements)
			{
				Validator validator = CreateValidatorForValidatedElement(validatedElement,
					this.GetCompositeValidatorBuilderForField);

				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForMethods(IEnumerable<IValidatedElement> validatedElements,
			List<Validator> validators,
			Type ownerType)
		{
			foreach (IValidatedElement validatedElement in validatedElements)
			{
				Validator validator = CreateValidatorForValidatedElement(validatedElement,
					this.GetCompositeValidatorBuilderForMethod);

				if (validator != null)
				{
					validators.Add(validator);
				}
			}
		}

		private void CollectValidatorsForSelfValidationMethods(IEnumerable<MethodInfo> selfValidationMethods, List<Validator> validators)
		{
			foreach (MethodInfo selfValidationMethod in selfValidationMethods)
			{
				validators.Add(new SelfValidationValidator(selfValidationMethod));
			}
		}

		protected Validator CreateValidatorForValidatedElement(IValidatedElement validatedElement,
			CompositeValidatorBuilderCreator validatorBuilderCreator)
		{
			IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator = validatedElement.GetValidatorDescriptors().GetEnumerator();

			if (!validatorDescriptorsEnumerator.MoveNext())
			{
				return null;
			}

			CompositeValidatorBuilder validatorBuilder = validatorBuilderCreator(validatedElement);

			do
			{
				Validator validator = validatorDescriptorsEnumerator.Current.CreateValidator(validatedElement.TargetType,
					validatedElement.MemberInfo.ReflectedType,
					this.memberAccessValidatorFactory.MemberValueAccessBuilder);
				validatorBuilder.AddValueValidator(validator);
			}
			while (validatorDescriptorsEnumerator.MoveNext());

			return validatorBuilder.GetValidator();
		}

		protected delegate CompositeValidatorBuilder CompositeValidatorBuilderCreator(IValidatedElement validatedElement);

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetPropertyValueAccessValidatorBuilder(validatedElement.MemberInfo as PropertyInfo,
				validatedElement);
		}

		protected CompositeValidatorBuilder GetValueCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
		{
			return new CompositeValidatorBuilder(validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForField(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetFieldValueAccessValidatorBuilder(validatedElement.MemberInfo as FieldInfo,
				validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForMethod(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetMethodValueAccessValidatorBuilder(validatedElement.MemberInfo as MethodInfo,
				validatedElement);
		}

		protected CompositeValidatorBuilder GetCompositeValidatorBuilderForType(IValidatedElement validatedElement)
		{
			return this.memberAccessValidatorFactory.GetTypeValidatorBuilder(validatedElement.MemberInfo as Type,
				validatedElement);
		}
	}
}
