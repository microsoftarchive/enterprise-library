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
using System.IO;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.ConfigFiles;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_merge_configuration
{
    public abstract class merging_protected_sections_context : ArrangeActAssert
    {
        protected string mainConfigurationFile;
        protected string mergedConfigurationFile;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            mainConfigurationFile = resources.DumpResourceFileToDisk("ehab_lab_and_daab.config");
            mergedConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       "ehab_lab_and_daab_merged.config");
        }

        protected void ProtectSection(FileConfigurationSource source, string sectionName, string protectionProvider)
        {
            ConfigurationSectionCloner cloner = new ConfigurationSectionCloner();
            var section = source.GetSection(sectionName);
            section = cloner.Clone(section);

            source.Remove(sectionName);
            source.Add(sectionName, section, protectionProvider);
        }
    }
}
