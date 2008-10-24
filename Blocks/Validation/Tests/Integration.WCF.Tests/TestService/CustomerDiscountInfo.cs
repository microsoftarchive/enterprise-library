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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [DataContract(Name="CustomerDiscountInfo", Namespace="http://tempuri.org/CustomerDiscountInfo")]
    internal class CustomerDiscountInfo
    {
        private string customerId;
        private double discountPercent;

        public CustomerDiscountInfo()
        {
        }

        public CustomerDiscountInfo(string customerId, double discountPercent)
        {
            this.customerId = customerId;
            this.discountPercent = discountPercent;
        }

        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public double DiscountPercent
        {
            get { return discountPercent; }
            set { discountPercent = value; }
        }
    }
}
