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
    /// 
    /// </summary>
    public class MetadataValidatedParameterElement : IValidatedElement
    {
        private ParameterInfo parameterInfo;
        private IgnoreNullsAttribute ignoreNullsAttribute;
        private ValidatorCompositionAttribute validatorCompositionAttribute;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IValidatorDescriptor> GetValidatorDescriptors()
        {
            if (parameterInfo != null)
            {
                foreach (object attribute in parameterInfo.GetCustomAttributes(typeof(ValidatorAttribute), false))
                {
                    yield return (IValidatorDescriptor)attribute;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompositionType CompositionType
        {
            get
            {
                return this.validatorCompositionAttribute != null
                    ? this.validatorCompositionAttribute.CompositionType
                    : CompositionType.And;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CompositionMessageTemplate
        {
            get
            {
                return this.validatorCompositionAttribute != null
                    ? this.validatorCompositionAttribute.GetMessageTemplate()
                    : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CompositionTag
        {
            get
            {
                return this.validatorCompositionAttribute != null
                    ? this.validatorCompositionAttribute.Tag
                    : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreNulls
        {
            get
            {
                return this.ignoreNullsAttribute != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IgnoreNullsMessageTemplate
        {
            get
            {
                return this.ignoreNullsAttribute != null
                    ? this.ignoreNullsAttribute.GetMessageTemplate()
                    : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IgnoreNullsTag
        {
            get
            {
                return this.ignoreNullsAttribute != null
                    ? this.ignoreNullsAttribute.Tag
                    : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MemberInfo MemberInfo
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type TargetType
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        public void UpdateFlyweight(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo;
            this.ignoreNullsAttribute
                = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(parameterInfo, string.Empty);
            this.validatorCompositionAttribute
                = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(parameterInfo, string.Empty);
        }
    }
}
