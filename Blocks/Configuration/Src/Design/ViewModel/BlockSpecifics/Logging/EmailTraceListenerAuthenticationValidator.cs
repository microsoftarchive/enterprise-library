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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging
{
    ///<summary>
    /// Validates the athentication and user-name, password combinations.
    ///</summary>
    public class EmailTraceListenerAuthenticationValidator : Validator
    {
        /// <summary>
        /// When implemented in a derived class, validates <paramref name="value"/> as part of the <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            ElementViewModel element = instance as ElementViewModel;
            if (element == null) return;

            object authenticationMode = element.Property("AuthenticationMode").Value;
            string userName = element.Property("UserName").Value.ToString();

            if (authenticationMode.Equals(EmailAuthenticationMode.UserNameAndPassword)
                && string.IsNullOrEmpty(userName))
            {
                results.Add(new ElementValidationResult(element, Resources.EmailTraceListenerValidationSupplyUserNameAndPassword));
            }

            if (authenticationMode.Equals(EmailAuthenticationMode.UserNameAndPassword)
              && !string.IsNullOrEmpty(userName)
              && !element.ContainingSection.ProtectionProviderProperty.NeedsProtectionProvider)
            {
                results.Add(new ElementValidationResult(element, Resources.EmailTraceListenerEncryptWarning, true));
            }

            if (!authenticationMode.Equals(EmailAuthenticationMode.UserNameAndPassword)
                && !string.IsNullOrEmpty(userName))
            {
                results.Add(new ElementValidationResult(element, Resources.EmailTraceListenerUserNameAndPasswordNotNeeded, true));
            }
        }
    }
}
