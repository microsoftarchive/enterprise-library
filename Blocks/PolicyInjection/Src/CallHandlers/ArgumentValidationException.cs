//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Properties;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// Exception thrown by the <see cref="ValidationCallHandler"/> if validation fails.
    /// </summary>
    [Serializable]
    public class ArgumentValidationException : ArgumentException, ISerializable
    {
        readonly private ValidationResults validationResults;

        /// <summary>
        /// Creates a new <see cref="ArgumentValidationException"/>, storing the validation
        /// results and the name of the parameter that failed.
        /// </summary>
        /// <param name="validationResults"><see cref="ValidationResults"/> returned from the Validation block.</param>
        /// <param name="paramName">Parameter that failed validation.</param>
        public ArgumentValidationException(ValidationResults validationResults, string paramName)
            : base(Resources.ValidationFailedMessage, paramName)
        {
            this.validationResults = validationResults;
        }

        /// <summary>
        /// Supporting constructor for cross-appdomain exception handling.
        /// </summary>
        /// <param name="info">serialization info.</param>
        /// <param name="context">serialization context.</param>
        protected ArgumentValidationException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.validationResults = (ValidationResults)info.GetValue("ArgumentValidationException.validationResults", typeof(ValidationResults));
        }

        /// <summary>
        /// The validation results for the failure.
        /// </summary>
        /// <value>ValidationResults for the failure.</value>
        public ValidationResults ValidationResults
        {
            get { return validationResults; }
        }

        /// <summary>
        /// Supporting method for serialization.
        /// </summary>
        /// <param name="info">serialization info.</param>
        /// <param name="context">serialization context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ArgumentValidationException.validationResults", validationResults);
        }
    }
}
