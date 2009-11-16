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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using System.IO;
using System.Xml;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationMerger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainConfigurationFile"></param>
        /// <param name="mergeSection"></param>
        /// <param name="mergedConfigurationFile"></param>
        public void MergeConfiguration(string mainConfigurationFile, EnvironmentMergeSection mergeSection, string mergedConfigurationFile)
        {
            string temporaryConfigurationFile = Path.GetTempFileName();
            try
            {
                File.Copy(mainConfigurationFile, temporaryConfigurationFile, true);

                // make the target file writable (it may have inherited the read-only attribute from the original file)
                FileAttributes attributes = File.GetAttributes(temporaryConfigurationFile);
                File.SetAttributes(temporaryConfigurationFile, attributes & ~FileAttributes.ReadOnly);

                XmlDocument configurationDocument = new XmlDocument();
                configurationDocument.Load(temporaryConfigurationFile);

                foreach (EnvironmentNodeMergeElement mergeElement in mergeSection.MergeElements)
                {
                    if (mergeElement.OverrideProperties)
                    {
                        XmlElement element = (XmlElement)configurationDocument.SelectSingleNode(mergeElement.ConfigurationNodePath);
                        if (element != null)
                        {
                            foreach (string property in mergeElement.OverriddenProperties.AllKeys)
                            {
                                var attribute = element.Attributes[property];
                                if (attribute != null)
                                {
                                    attribute.Value = mergeElement.OverriddenProperties[property].Value;
                                }
                            }
                        }
                    }
                }
                configurationDocument.Save(mergedConfigurationFile);
            }
            finally
            {
                File.Delete(temporaryConfigurationFile);
            }
        }
    }
}
