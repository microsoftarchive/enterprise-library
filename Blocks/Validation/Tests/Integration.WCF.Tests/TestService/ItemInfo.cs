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
    [DataContract(Name="ItemInfo", Namespace="http://tempuri.org/ItemInfo")]
    class ItemInfo
    {
        private string itemId;
        private string itemDescription;
        private int quantity;

        public ItemInfo()
        {
        }

        public ItemInfo(string itemId, string itemDescription, int quantity)
        {
            this.itemId = itemId;
            this.itemDescription = itemDescription;
            this.quantity = quantity;
        }

        [DataMember]
        [NotNullValidator]
        [StringLengthValidator(4, 12)]
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        [DataMember]
        [NotNullValidator]
        [StringLengthValidator(200)]
        public string ItemDescription
        {
            get { return itemDescription; }
            set { itemDescription = value; }
        }

        [DataMember]
        [RangeValidator(1, RangeBoundaryType.Inclusive, 50, RangeBoundaryType.Inclusive)]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }
}
