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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [DataContract(Name="TaxInfo", Namespace = "http://tempuri.org/TaxInfo")]
    class TaxInfo
    {
        string taxId;
        string taxingState;

        public TaxInfo()
        {
        }

        public TaxInfo(string taxId, string taxingState)
        {
            this.taxId = taxId;
            this.taxingState = taxingState;
        }

        [DataMember(IsRequired=false, Name="TaxId", Order=0)]
        [RegexValidator(@"\d+") ]
        public string TaxId
        {
            get { return taxId; }
            set { taxId = value; }
        }

        [DataMember(IsRequired = false, Name="TaxingState", Order=1)]
        [RegexValidator(@"[A-Z]{2}")]
        public string TaxingState
        {
            get { return taxingState; }
            set { taxingState = value; }
        }
    }
}
