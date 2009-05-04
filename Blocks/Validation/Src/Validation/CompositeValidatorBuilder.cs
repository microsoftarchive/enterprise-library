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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Helps building validators composed by other validators.
    /// </summary>
    public class CompositeValidatorBuilder
    {
        private IValidatedElement validatedElement;
        private List<Validator> valueValidators;
        private Validator builtValidator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedElement"></param>
        public CompositeValidatorBuilder(IValidatedElement validatedElement)
        {
            this.validatedElement = validatedElement;
            this.valueValidators = new List<Validator>();
        }

        /// <summary>
        /// Returns the validator created by the builder.
        /// </summary>
        public Validator GetValidator()
        {
            this.builtValidator = this.DoGetValidator();

            return this.builtValidator;
        }

        /// <summary>
        /// Creates the composite validator built by the builder.
        /// </summary>
        protected virtual Validator DoGetValidator()
        {
            // create the appropriate validator
            Validator validator;

            if (this.valueValidators.Count == 1)
            {
                validator = this.valueValidators[0];
            }
            else
            {

                if (CompositionType.And == this.validatedElement.CompositionType)
                {
                    validator = new AndCompositeValidator(this.valueValidators.ToArray());
                }
                else
                {
                    validator = new OrCompositeValidator(this.valueValidators.ToArray());
                    validator.MessageTemplate = this.validatedElement.CompositionMessageTemplate;
                    validator.Tag = this.validatedElement.CompositionTag;
                }
            }

            // add support for ignoring nulls
            Validator valueValidator;
            if (this.validatedElement.IgnoreNulls)
            {
                valueValidator = new OrCompositeValidator(new NotNullValidator(true), validator);
                valueValidator.MessageTemplate = this.validatedElement.IgnoreNullsMessageTemplate != null
                    ? this.validatedElement.IgnoreNullsMessageTemplate
                    : Resources.IgnoreNullsDefaultMessageTemplate;
                valueValidator.Tag = this.validatedElement.IgnoreNullsTag;
            }
            else
            {
                valueValidator = validator;
            }

            return valueValidator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueValidator"></param>
        public void AddValueValidator(Validator valueValidator)
        {
            this.valueValidators.Add(valueValidator);
        }

        #region test only properties

        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreNulls
        {
            get { return this.validatedElement.IgnoreNulls; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompositionType CompositionType
        {
            get { return this.validatedElement.CompositionType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Validator BuiltValidator
        {
            get { return this.builtValidator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Validator> ValueValidators
        {
            get { return this.valueValidators; }
        }

        #endregion
    }
}
