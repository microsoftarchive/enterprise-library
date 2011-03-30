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
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection
{
    /// <summary>
    /// Exception thrown by the <see cref="ValidationCallHandler"/> if validation fails.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
    public class ArgumentValidationException : ArgumentException, ISerializable
#else
    public class ArgumentValidationException : ArgumentException
#endif
    {
        readonly private ValidationResults validationResults;

        /// <summary>
        /// Creates a new <see cref="ArgumentValidationException"/>, storing the validation
        /// results and the name of the parameter that failed.
        /// </summary>
        /// <param name="validationResults"><see cref="ValidationResults"/> returned from the Validation Application Block.</param>
        /// <param name="paramName">Parameter that failed validation.</param>
        public ArgumentValidationException(ValidationResults validationResults, string paramName)
            : base(Resources.ValidationFailedMessage, paramName)
        {
            this.validationResults = validationResults;
        }

#if !SILVERLIGHT
        /// <summary>
        /// Supporting constructor for cross-appdomain exception handling.
        /// </summary>
        /// <param name="info">serialization info.</param>
        /// <param name="context">serialization context.</param>
        protected ArgumentValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null) throw new ArgumentNullException("info");
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
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ArgumentValidationException.validationResults", validationResults);
        }
#endif

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            StringBuilder resultBuilder = new StringBuilder(base.ToString());

            if (this.validationResults.Count > 0)
            {
                resultBuilder.AppendLine();
                resultBuilder.AppendLine();
                resultBuilder.AppendLine(Resources.ValidationResultsHeader);
                int i = 0;
                CultureInfo culture = CultureInfo.CurrentCulture;
                foreach (ValidationResult validationResult in this.validationResults)
                {
                    if (validationResult.Key != null)
                    {
                        resultBuilder.AppendFormat(
                            culture,
                            Resources.ValidationResultWithKeyTemplate,
                            i,
                            validationResult.Message,
                            validationResult.Key);
                    }
                    else
                    {
                        resultBuilder.AppendFormat(
                            culture,
                            Resources.ValidationResultTemplate,
                            i,
                            validationResult.Message);
                    }
                    resultBuilder.AppendLine();

                    i++;
                }
            }

            return resultBuilder.ToString();
        }
    }
}
