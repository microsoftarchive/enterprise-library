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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    class TestServiceImplementation : ITestService
    {
        public string ToUpperCase(string input)
        {
            return input.ToUpperInvariant();
        }

        public AddCustomerResponse AddCustomer(AddCustomerRequest request)
        {
            AddCustomerResponse response = new AddCustomerResponse();
            response.FullName = string.Format("{0} {1}", request.FirstName, request.LastName);
            response.Added = true;
            return response;
        }

        public void PlaceOrder(
            string customerId, TaxInfo taxInfo, ItemInfo itemInfo, CustomerDiscountInfo discountInfo)
        {
            
        }

        public void LookupItem(string itemId, out ItemInfo info)
        {
            info = new ItemInfo(itemId, "This is a dummy", 42);
        }


        public ItemInfo LookupItem(string itemId)
        {
            return new ItemInfo(itemId, string.Format("Dummy item {0}", itemId), 44);
        }


        public void LookupById(int id, string customerName)
        {
            
        }
    }
}
