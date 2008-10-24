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
using System.Collections.Generic;
using System.Xml;

namespace CachingQuickStart
{
	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
	public sealed class DataProvider
	{
		public const string dataFileName = "CachingQuickStartData.xml";

    public DataProvider()
    {
    }

    public Product GetProductByID(string anID)
    {
			Product product = null;
			XmlTextReader reader = new XmlTextReader(dataFileName);
			reader.MoveToContent();

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(reader.ReadOuterXml());
			
			XmlNode productNode = doc.SelectSingleNode("products/product[@id=" + anID + "]");
			if (productNode != null)
			{
				product = new Product(productNode.Attributes["id"].Value,
				productNode.Attributes["name"].Value,
				Convert.ToDouble(productNode.Attributes["price"].Value));
			}

			reader.Close();

			return product;
		}

        public List<Product> GetProductList()
        {
            List<Product> list = new List<Product>();
            XmlTextReader reader = new XmlTextReader(dataFileName);
            reader.MoveToContent();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadOuterXml());
            XmlNodeList nodes = doc.SelectNodes("products/product");
            foreach (XmlNode node in nodes)
            {
                Product product = new Product(node.Attributes["id"].Value,
                                                node.Attributes["name"].Value,
                                                Convert.ToDouble(node.Attributes["price"].Value));
                list.Add(product);
            }
            reader.Close();

			return list;
        }
    }
}
