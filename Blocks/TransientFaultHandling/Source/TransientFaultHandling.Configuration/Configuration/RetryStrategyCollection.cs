#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

    /// <summary>
    /// Represents a collection RetryStrategyData instances.
    /// </summary>
    public class RetryStrategyCollection : NameTypeConfigurationElementCollection<RetryStrategyData, CustomRetryStrategyData>
    {
        /// <summary>
        /// Called when an unknown element is encountered while deserializing the System.Configuration.ConfigurationElement object.
        /// </summary>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="reader">The reader used to deserialize the element.</param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            Type configurationElementType;

            if (WellKnownRetryStrategies.AllKnownRetryStrategies.TryGetValue(elementName, out configurationElementType))
            {
                var currentElement = (RetryStrategyData)Activator.CreateInstance(configurationElementType);
                currentElement.DeserializeElement(reader);
                this.Add(currentElement);
                return true;
            }

            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }
    }
}
