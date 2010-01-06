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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation
{
    ///<summary>
    /// Defines the type of attribute to apply this configuration property or field.
    ///</summary>
    /// <remarks>
    /// This attribute is applied to create validators for use in the configuration design-time.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ValidationAttribute : Attribute
    {
        private string validatorType;

        ///<summary>
        /// Creates an instance of ValidationAttribute with the validator type specified by <see cref="string"/>.
        ///</summary>
        ///<param name="validatorType"></param>
        public ValidationAttribute(string validatorType)
        {
            this.validatorType = validatorType;
        }


        ///<summary>
        /// Creates an instance of the ValidationAttribute with the validator type specified by <see cref="Type"/>
        ///</summary>
        ///<param name="validatorType"></param>
        public ValidationAttribute(Type validatorType) 
            : this(validatorType.AssemblyQualifiedName)
        {
        }

        ///<summary>
        /// Retrieves the validator <see cref="Type"/>.
        ///</summary>
        public Type ValidatorType
        {
            get { return Type.GetType(validatorType, true, true); }
        }

        ///<summary>
        /// Creates a validator objects.   This is expected to return a Validator type from
        /// the Microsoft.Practices.EnterpriseLibrary.Configuration.Design namespace.  
        ///</summary>
        ///<returns></returns>
        public object CreateValidator()
        {
            var validatorType = ValidatorType;
            return Activator.CreateInstance(validatorType);
        }
    }
}
