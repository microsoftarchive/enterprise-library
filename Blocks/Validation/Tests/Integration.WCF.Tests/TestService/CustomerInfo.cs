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
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [DataContract]
    public class CustomerInfo
    {
        private string firstName;
        private string lastName;
        private string ssn;


        public CustomerInfo()
        {
        }


        public CustomerInfo(string firstName, string lastName, string ssn)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.ssn = ssn;
        }

        [DataMember(Order = 0)]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [DataMember(Order = 1)]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [DataMember(Order = 2)]
        public string Ssn
        {
            get { return ssn; }
            set { ssn = value; }
        }
    }
}
