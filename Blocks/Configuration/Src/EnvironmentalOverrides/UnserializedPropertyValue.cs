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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents unseralized values in a property.
    /// </summary>
    public class UnserializedPropertyValue
    {
        readonly string serializedContents;

        /// <summary>
        /// Initialize a new instance of the <see cref="UnserializedPropertyValue"/> class.
        /// </summary>
        /// <param name="serializedContents">The serialized contents.</param>
        public UnserializedPropertyValue(string serializedContents)
        {
            this.serializedContents = serializedContents;
        }

        /// <summary>
        /// Deseralize the property values.
        /// </summary>
        /// <param name="type">The type for the property.</param>
        /// <param name="hierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns>The deserialzied value.</returns>
        public object DeserializePropertyValue(Type type,
                                               IConfigurationUIHierarchy hierarchy)
        {
            return SerializationUtility.DeserializeFromString(serializedContents, type, hierarchy);
        }
    }
}
