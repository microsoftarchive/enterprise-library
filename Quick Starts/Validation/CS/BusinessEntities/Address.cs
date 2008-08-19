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
using ValidationQuickStart.CustomValidators;

namespace ValidationQuickStart.BusinessEntities
{
    public class Address
    {
        private string line1;
        private string line2;
        private string city;
        private string state;
        private string postCode;
        private string country;

        [StringLengthValidator(5, 50, Ruleset = "RuleSetA")]
        public string Line1
        {
            get { return line1; }
            set { line1 = value; }
        }

        [IgnoreNulls]
        [ValidatorComposition(CompositionType.Or, Ruleset = "RuleSetA", MessageTemplate = "Address line 2 must be empty, or between 5 and 50 characters")]
        [StringLengthValidator(0, Ruleset = "RuleSetA")]
        [StringLengthValidator(5, 50, Ruleset = "RuleSetA")]
        public string Line2
        {
            get { return line2; }
            set { line2 = value; }
        }

        [StringLengthValidator(1, 50, Ruleset = "RuleSetA")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [USStateValidator(Ruleset = "RuleSetA")] 
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        [RegexValidator(@"\d{5}(-\d{4})?", MessageTemplate="\"{0}\" is not a valid US zip code", Ruleset = "RuleSetA")]
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        [DomainValidator("US", "USA", "United States", "United States of America", Ruleset = "RuleSetA", MessageTemplate = "Country must be USA")]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

    }
}
