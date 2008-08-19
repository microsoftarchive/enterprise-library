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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Used to create a friendly name for configurations nodes that represent types.
    /// </summary>
    public class TypeNodeNameFormatter
    {
        /// <summary>
        /// Creates a friendly name based on a typeName, which can be used as a displayname within a graphical tool.
        /// </summary>
        /// <param name="typeName">The typeName that should be used.</param>
        /// <returns>A friendly name that can be used as a display name.</returns>
        public string CreateName(string typeName)
        {
            string nodeName = typeName;
            if (!string.IsNullOrEmpty(nodeName))
            {
                string[] qualifiedNameParts = nodeName.Split(',');
                if (qualifiedNameParts.Length > 0)
                {
                    //first segment is namespace + type
                    nodeName = qualifiedNameParts[0].Trim();
                    string[] typeNameParts = nodeName.Split('.');

                    //name of the type is found after the last '.'
                    if (typeNameParts.Length > 0)
                    {
                        nodeName = typeNameParts[typeNameParts.Length - 1].Trim();
                    }
                }
            }
            else
            {
                nodeName = string.Empty;
            }

            return nodeName;
        }
    }
}
