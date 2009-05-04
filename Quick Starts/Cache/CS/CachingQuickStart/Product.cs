//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Xml.Serialization;

namespace CachingQuickStart
{
	[Serializable()]
    [XmlRoot("product")]
    public class Product
    {
        private string productId;
        private double productPrice;
        private string productName;

        /// <summary>
        /// Creates a new instance of ExceptionPolicyData.
        /// </summary>
        public Product()
        {
        }

        public Product(string id, string name, double price)
        {
            this.ProductID = id;
            this.ProductName = name;
            this.ProductPrice = price;
        }

        /// <summary>
        /// Gets or sets the Policy name.
        /// </summary>
        [XmlAttribute("name")]
        public string ProductName
        {
            get { return this.productName; }
            set { this.productName = value; }
        }

        [XmlAttribute("id")]
        public string ProductID
        {
            get { return this.productId; }
            set { this.productId = value; }
        }

        [XmlAttribute("price")]
        public double ProductPrice
        {
            get { return this.productPrice; }
            set { this.productPrice = value; }
        }

        public override string ToString()
        {
            string res = "Product: ID=" + this.productId +
                ", Name=" + this.productName +
                ", Price=" + Convert.ToString(this.productPrice);
            return res;
        }
    }
}
