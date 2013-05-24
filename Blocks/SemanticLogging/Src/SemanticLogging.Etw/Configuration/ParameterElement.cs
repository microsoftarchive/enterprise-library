#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    internal class ParameterElement
    {
        private static readonly XName ParametersName = XName.Get("parameters", Constants.Namespace);

        public string Name { get; set; }

        public string Type { get; set; }
        
        public string Value { get; set; }
        
        public IEnumerable<ParameterElement> Parameters { get; set; }

        internal static ParameterElement Read(XElement element)
        {
            return new ParameterElement()
            {
                Name = (string)element.Attribute("name"),
                Type = (string)element.Attribute("type"),
                Value = (string)element.Attribute("value"),
                Parameters = GetChildParameters(element)
            };
        }

        private static IEnumerable<ParameterElement> GetChildParameters(XElement element)
        {
            foreach (var e in element.Elements(ParametersName).Elements())
            {
                yield return ParameterElement.Read(e);
            }
        }
    }
}
