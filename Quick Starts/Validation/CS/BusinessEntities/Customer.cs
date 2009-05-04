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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace ValidationQuickStart.BusinessEntities
{
    public class Customer
    {
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string email;
        private Address address;
        private int rewardPoints;

        [StringLengthValidator(1, 50, Ruleset="RuleSetA", MessageTemplate="First Name must be between 1 and 50 characters")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value;  }
        }

        [StringLengthValidator(1, 50, Ruleset = "RuleSetA", MessageTemplate = "Last Name must be between 1 and 50 characters")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [RelativeDateTimeValidator(-120, DateTimeUnit.Year, -18, DateTimeUnit.Year, Ruleset="RuleSetA", MessageTemplate="Must be 18 years or older.")]
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        [RegexValidator(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", MessageTemplate="Invalid e-mail address", Ruleset = "RuleSetA")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [ObjectValidator("RuleSetA", Ruleset = "RuleSetA")]
        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 1000000, RangeBoundaryType.Inclusive, Ruleset = "RuleSetA", MessageTemplate="Rewards points cannot exceed 1,000,000")]
        public int RewardPoints
        {
            get { return rewardPoints; }
            set { rewardPoints = value; }
        }
    }
}
