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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Represents the description of how validation must be performed on a language element as defined by configuration.
    /// </summary>
    /// <remarks>
    /// This class is a flyweight, so instances should not be kept for later use.
    /// </remarks>
    public class ConfigurationValidatedElement : IValidatedElement
    {
        private ValidatedMemberReference validatedMemberReference;
        private MemberInfo memberInfo;
        private Type targetType;

        /// <summary>
        /// 
        /// </summary>
        public ConfigurationValidatedElement()
        { }

        #region erase when posible

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedFieldReference"></param>
        /// <param name="fieldInfo"></param>
        public ConfigurationValidatedElement(ValidatedFieldReference validatedFieldReference, FieldInfo fieldInfo)
        {
            UpdateFlyweight(validatedFieldReference, fieldInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedMethodReference"></param>
        /// <param name="methodInfo"></param>
        public ConfigurationValidatedElement(ValidatedMethodReference validatedMethodReference, MethodInfo methodInfo)
        {
            UpdateFlyweight(validatedMethodReference, methodInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedPropertyReference"></param>
        /// <param name="propertyInfo"></param>
        public ConfigurationValidatedElement(ValidatedPropertyReference validatedPropertyReference, PropertyInfo propertyInfo)
        {
            UpdateFlyweight(validatedPropertyReference, propertyInfo);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedFieldReference"></param>
        /// <param name="fieldInfo"></param>
        public void UpdateFlyweight(ValidatedFieldReference validatedFieldReference, FieldInfo fieldInfo)
        {
            this.UpdateFlyweight(validatedFieldReference, fieldInfo, fieldInfo.FieldType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedMethodReference"></param>
        /// <param name="methodInfo"></param>
        public void UpdateFlyweight(ValidatedMethodReference validatedMethodReference, MethodInfo methodInfo)
        {
            UpdateFlyweight(validatedMethodReference, methodInfo, methodInfo.ReturnType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedPropertyReference"></param>
        /// <param name="propertyInfo"></param>
        public void UpdateFlyweight(ValidatedPropertyReference validatedPropertyReference, PropertyInfo propertyInfo)
        {
            UpdateFlyweight(validatedPropertyReference, propertyInfo, propertyInfo.PropertyType);
        }

        private void UpdateFlyweight(ValidatedMemberReference validatedMemberReference, MemberInfo memberInfo, Type targetType)
        {
            this.validatedMemberReference = validatedMemberReference;
            this.memberInfo = memberInfo;
            this.targetType = targetType;
        }

        IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
        {
            if (this.validatedMemberReference == null)
            {
                yield break;
            }

            foreach (IValidatorDescriptor validatorData in this.validatedMemberReference.Validators)
            {
                yield return validatorData;
            }
        }

        CompositionType IValidatedElement.CompositionType
        {
            get { return CompositionType.And; }
        }

        string IValidatedElement.CompositionMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.CompositionTag
        {
            get { return null; }
        }

        bool IValidatedElement.IgnoreNulls
        {
            get { return false; }
        }

        string IValidatedElement.IgnoreNullsMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.IgnoreNullsTag
        {
            get { return null; }
        }

        MemberInfo IValidatedElement.MemberInfo
        {
            get { return this.memberInfo; }
        }

        Type IValidatedElement.TargetType
        {
            get { return this.targetType; }
        }
    }
}
