//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    internal static class TreeAssert
    {
        public static void IsMatch(object match, Node node)
        {
            foreach (var matchProperty in match.GetType().GetProperties())
            {
                var nodeProperty = node.GetType().GetProperty(matchProperty.Name);

                if (nodeProperty == null)
                {
                    Assert.Fail("Non-matching property for " + matchProperty.Name);
                }

                var matchValue = matchProperty.GetValue(match, null);
                var nodeValue = nodeProperty.GetValue(node, null);

                if (matchValue is IEnumerable && !(matchValue is string))
                {
                    if (!(nodeValue is IEnumerable))
                    {
                        Assert.Fail("Node property no enumerable: " + nodeProperty.Name);
                    }

                    var matchValueEnumerable = ((IEnumerable)matchValue).Cast<object>();
                    var nodeValueEnumerable = ((IEnumerable)nodeValue).Cast<Node>();

                    if (matchValueEnumerable.Count() != nodeValueEnumerable.Count())
                    {
                        Assert.Fail("Lengths differ on " + matchProperty.Name);
                    }

                    for (int i = 0; i < matchValueEnumerable.Count(); i++)
                    {
                        IsMatch(matchValueEnumerable.ElementAt(i), nodeValueEnumerable.ElementAt(i));
                    }

                }
                else
                {
                    Assert.AreEqual(
                        matchValue,
                        nodeValue,
                        string.Format("Property {0} on match {1}", matchProperty.Name, match));
                }
            }
        }
    }
}
