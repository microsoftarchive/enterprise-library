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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{

    /// <summary>
    /// Component used to create an environment specific configuration file, using a Delta Configuration (*.dconfig) file and a base configuration file.
    /// </summary>
    public class ConfigurationMerger
    {
        readonly string mainConfigurationFile;
        readonly EnvironmentalOverridesSection mergeSection;
        string mergedConfigurationFile;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationMerger"/>.
        /// </summary>
        /// <param name="mainConfigurationFile">The path to the main configuration file, which should be used to create the environment specific configuration file.</param>
        /// <param name="deltaConfigurationFile">The path to the delta configuration file (*.dconfig) that contains the environment specific configuration.</param>
        public ConfigurationMerger(string mainConfigurationFile, string deltaConfigurationFile)
        {
            this.mainConfigurationFile = mainConfigurationFile;

            using (var configurationSource = new FileConfigurationSource(deltaConfigurationFile))
            {
                mergeSection =
                    (EnvironmentalOverridesSection)configurationSource.GetSection(EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationMerger"/>.
        /// </summary>
        /// <param name="mainConfigurationFile">The path to the main configuration file, which should be used to create the environment specific configuration file.</param>
        /// <param name="mergeSection">The <see cref="EnvironmentalOverridesSection"/> instance that contains the environment specific configuration.</param>
        public ConfigurationMerger(string mainConfigurationFile, EnvironmentalOverridesSection mergeSection)
        {
            this.mainConfigurationFile = mainConfigurationFile;
            this.mergeSection = mergeSection;
        }

        /// <summary>
        /// Gets the path to the environment specific configuration file.
        /// </summary>
        /// <value>
        /// The path to the environment specific configuration file.
        /// </value>
        public string MergedConfigurationFile
        {
            get { return mergedConfigurationFile; }
        }

        /// <summary>
        /// Creates the environment specific configuration file.
        /// </summary>
        /// <param name="mergedConfigurationFile">The path to the file which should be used to store the environment specific configuration.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "mergedConfigurationFile")]
        public void MergeConfiguration(string mergedConfigurationFile)
        {
            this.mergedConfigurationFile = mergeSection.EnvironmentConfigurationFile;
            if (!string.IsNullOrEmpty(mergedConfigurationFile))
            {
                this.mergedConfigurationFile = mergedConfigurationFile;
            }

            string temporaryConfigurationFile = Path.GetTempFileName();
            try
            {
                File.Copy(mainConfigurationFile, temporaryConfigurationFile, true);

                // make the target file writable (it may have inherited the read-only attribute from the original file)
                FileAttributes attributes = File.GetAttributes(temporaryConfigurationFile);
                File.SetAttributes(temporaryConfigurationFile, attributes & ~FileAttributes.ReadOnly);

                XmlDocument configurationDocument = new XmlDocument();
                configurationDocument.Load(temporaryConfigurationFile);

                using(var unencryptedContents = new UnEncryptedConfigurationSections(temporaryConfigurationFile, configurationDocument))
                {
                    if (mergeSection.MergeElements != null)
                    {
                        foreach (EnvironmentalOverridesElement mergeElement in mergeSection.MergeElements)
                        {
                            foreach (
                                EnvironmentOverriddenPropertyElement overriddenProperty in
                                    mergeElement.OverriddenProperties)
                            {
                                unencryptedContents.EnsureUnencryptedConfigurationSection(
                                    overriddenProperty.ConfigurationSectionName,
                                    overriddenProperty.ContainingSectionXPath);

                                XmlElement element =
                                    (XmlElement)
                                    configurationDocument.SelectSingleNode(overriddenProperty.ContainingElementXPath);
                                if (element != null)
                                {
                                    var attribute = element.Attributes[overriddenProperty.Attribute];
                                    if (attribute != null)
                                    {
                                        attribute.Value = overriddenProperty.OverriddenValue;
                                    }
                                    else
                                    {
                                        var createdAttribute =
                                            element.OwnerDocument.CreateAttribute(overriddenProperty.Attribute);
                                        createdAttribute.Value = overriddenProperty.OverriddenValue;
                                        element.Attributes.Append(createdAttribute);
                                    }

                                }
                            }
                        }
                    }

                    foreach(EnvironmentOverriddenProtectionProviderElement overridenProtectionProvider in mergeSection.OverriddenProtectionProviders)
                    {
                        unencryptedContents.OverrideProtectionProvider(
                            overridenProtectionProvider.ContainingSectionXPath,
                            overridenProtectionProvider.ProtectionProvider);
                    }
                }

                configurationDocument.Save(this.mergedConfigurationFile);
            }
            finally
            {
                File.Delete(temporaryConfigurationFile);
            }
        }

        private class UnEncryptedConfigurationSections : IDisposable
        {
            readonly XmlDocument configurationFileContents;
            readonly System.Configuration.Configuration configuration;
            readonly Dictionary<string, ProtectedConfigurationProvider> sectionXPathsWithProtectionProviders;

            public UnEncryptedConfigurationSections(string configurationFile, XmlDocument configurationFileContents)
            {
                this.sectionXPathsWithProtectionProviders = new Dictionary<string, ProtectedConfigurationProvider>();
                this.configurationFileContents = configurationFileContents;

                this.configuration =
                    ConfigurationManager.OpenMappedExeConfiguration(
                        new ExeConfigurationFileMap {ExeConfigFilename = configurationFile}, ConfigurationUserLevel.None);
            }

            public void EnsureUnencryptedConfigurationSection(string configurationSectionName, string sectionXPath)
            {
                if (sectionXPathsWithProtectionProviders.ContainsKey(sectionXPath)) return;

                var section = this.configuration.GetSection(configurationSectionName);
                if (section != null && section.SectionInformation.IsProtected && section.SectionInformation.ProtectionProvider != null)
                {
                    var protectionProvider = section.SectionInformation.ProtectionProvider;
                    sectionXPathsWithProtectionProviders.Add(sectionXPath, protectionProvider);

                    var sectionNode = configurationFileContents.SelectSingleNode(sectionXPath);
                    if (sectionNode != null)
                    {
                        DecryptNode(sectionNode, protectionProvider);
                    }
                }
            }

            public void OverrideProtectionProvider(string sectionXpath, string protectedConfigurationProviderName)
            {
                if (string.IsNullOrEmpty(protectedConfigurationProviderName))
                {
                    sectionXPathsWithProtectionProviders.Remove(sectionXpath);
                    return;
                }

                var provider = ProtectedConfiguration.Providers.Cast<ProtectedConfigurationProvider>().Where(x => x.Name == protectedConfigurationProviderName).FirstOrDefault();
                sectionXPathsWithProtectionProviders[sectionXpath] = provider;
            }

            private void EncryptNode(XmlNode node, ProtectedConfigurationProvider protectionProvider)
            {
                XmlDocument document = node.OwnerDocument;
                XmlNode parentNode = node.ParentNode;

                var encryptedNode = protectionProvider.Encrypt(node);
                encryptedNode = document.ImportNode(encryptedNode, true);
                var encyptedNodeContainer = document.CreateElement(node.Name, node.NamespaceURI);
                encyptedNodeContainer.AppendChild(encryptedNode);

                var protectionProviderAttribute = document.CreateAttribute("configProtectionProvider");
                protectionProviderAttribute.Value = protectionProvider.Name;
                encyptedNodeContainer.Attributes.Append(protectionProviderAttribute);

                parentNode.ReplaceChild(encyptedNodeContainer, node);
            }

            private void DecryptNode(XmlNode node, ProtectedConfigurationProvider protectionProvider)
            {
                XmlDocument document = node.OwnerDocument;
                XmlNode parentNode = node.ParentNode;

                node.Attributes.RemoveNamedItem("configProtectionProvider", string.Empty);

                var unencryptedNode = protectionProvider.Decrypt(node.ChildNodes.OfType<XmlElement>().Where(x => x.Name == "EncryptedData").First());
                unencryptedNode = document.ImportNode(unencryptedNode, true);
                parentNode.ReplaceChild(unencryptedNode, node);
            }

            #region IDisposable Members

            public void Dispose()
            {
                foreach(var sectionAndProtectionProvider in sectionXPathsWithProtectionProviders)
                {
                    var unencryptedNode = configurationFileContents.SelectSingleNode(sectionAndProtectionProvider.Key);
                    if (unencryptedNode != null)
                    {
                        EncryptNode(unencryptedNode, sectionAndProtectionProvider.Value);
                    }
                }
            }

            #endregion
        }
    }
}
