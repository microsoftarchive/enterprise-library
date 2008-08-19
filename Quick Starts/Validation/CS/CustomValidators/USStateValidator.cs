//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace ValidationQuickStart.CustomValidators
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class USStateValidator : DomainValidator<string>
    {
        private static List<string> stateCodes = new List<string>(new string[] {
            "AL", "AK", "AS", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FM", "FL",
            "GA", "GU", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MH",
            "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM",
            "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC",
            "SD", "TN", "TX", "UT", "VT", "VI", "VA", "WA", "WV", "WI", "WY"});

        public USStateValidator()
            : base(stateCodes)
        {
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return "The supplied code does not represent a valid US State"; }
        }

    }
}
