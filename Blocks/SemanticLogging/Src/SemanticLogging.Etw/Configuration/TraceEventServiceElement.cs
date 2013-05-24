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

using System.Xml.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    internal class TraceEventServiceElement
    {
        internal TraceEventServiceElement()
        {
            this.SessionNamePrefix = Constants.DefaultSessionNamePrefix;
        }

        internal string SessionNamePrefix { get; set; }

        internal static TraceEventServiceElement Read(XElement element)
        {
            var instance = new TraceEventServiceElement();

            var snpAttr = (string)element.Attribute("sessionNamePrefix");
            if (!string.IsNullOrWhiteSpace(snpAttr))
            {
                instance.SessionNamePrefix = snpAttr;
            }

            return instance;
        }
    }
}
