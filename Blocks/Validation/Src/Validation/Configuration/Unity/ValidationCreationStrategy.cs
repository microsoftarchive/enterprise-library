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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    ///<summary>
    /// A <see cref="BuilderStrategy"/> for Unity that lets the container
    /// resolve <see cref="Validator{T}"/> objects directly.
    ///</summary>
    public class ValidationCreationStrategy : BuilderStrategy
    {
        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        ///             PreBuildUp method is called when the chain is being executed in the
        ///             forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (!(context.BuildKey is NamedTypeBuildKey)) return;
            var key = (NamedTypeBuildKey) context.BuildKey;

            if(!RequestIsForValidatorOfT(key)) return;

            var typeToValidate = TypeToValidate(key.Type);
            var rulesetName = key.Name;

            var validatorFactory = context.NewBuildUp<ValidatorFactory>();

            Validator validator;

            if(string.IsNullOrEmpty(rulesetName))
            {
                validator = validatorFactory.CreateValidator(typeToValidate);
            }
            else
            {
                validator = validatorFactory.CreateValidator(typeToValidate, rulesetName);
            }

            context.Existing = validator;
            context.BuildComplete = true;
        }

        private static bool RequestIsForValidatorOfT(NamedTypeBuildKey key)
        {
            var typeToBuild = key.Type;
            if(!typeToBuild.IsGenericType) return false;

            if(typeToBuild.GetGenericTypeDefinition() != typeof(Validator<>)) return false;

            return true;

        }

        private static Type TypeToValidate(Type validatorType)
        {
            return validatorType.GetGenericArguments()[0];
        }
    }
}
