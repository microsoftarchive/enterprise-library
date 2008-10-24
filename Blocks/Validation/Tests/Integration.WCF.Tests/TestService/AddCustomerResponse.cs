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

using System.Runtime.Serialization;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [MessageContract]
    internal class AddCustomerResponse
    {
        private string fullName;
        private bool added;

        public AddCustomerResponse()
        {
        }

        public AddCustomerResponse(string fullName, bool added)
        {
            this.fullName = fullName;
            this.added = added;
        }

        [DataMember(IsRequired = false, Name="FullName")]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        [DataMember(IsRequired=false, Name="Added")]
        public bool Added
        {
            get { return added; }
            set { added = value; }
        }
    }
}
