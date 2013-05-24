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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection
{
    /// <summary>
    /// The exception that is thrown by the <see cref="ValidationCallHandler"/> if validation fails.
    /// </summary>
    [Serializable]
    public class ArgumentValidationException : ArgumentException, ISerializable
    {
        [NonSerialized]
        private ValidationResults validationResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentValidationException"/> class, storing the validation
        /// results and the name of the parameter that failed.
        /// </summary>
        /// <param name="validationResults">The <see cref="ValidationResults"/> returned from the Validation Application Block.</param>
        /// <param name="paramName">The parameter that failed validation.</param>
        public ArgumentValidationException(ValidationResults validationResults, string paramName)
            : base(Resources.ValidationFailedMessage, paramName)
        {
            this.validationResults = validationResults;

            SerializeObjectState += (s, e) => e.AddSerializedState(new ValidationResultsSerializationData(this.validationResults));
        }

        /// <summary>
        /// Gets the validation results for the failure.
        /// </summary>
        /// <value>The validation results for the failure.</value>
        public ValidationResults ValidationResults
        {
            get { return this.validationResults; }
        }

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

        [Serializable]
        private class ValidationResultsSerializationData : ISafeSerializationData
        {
            private ValidationResults validationResults;

            public ValidationResultsSerializationData(ValidationResults validationResults)
            {
                this.validationResults = validationResults;
            }

            public void CompleteDeserialization(object deserialized)
            {
                var exception = (ArgumentValidationException)deserialized;
                exception.validationResults = this.validationResults;
            }
        }
    }
}
