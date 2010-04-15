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
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    public static class DatabaseSectionConfigurationSourceModelAssertions
    {
        public static IDatabaseVerifier DatabaseConfiguration(this ConfigurationSourceModel sourceModel)
        {
            return new DatabaseConfigurationVerifier(sourceModel);
        }
    }

    public class DatabaseConfigurationVerifier : IDatabaseVerifier, IConnectionVerifier
    {
        private readonly ConfigurationSourceModel model;
        private SectionViewModel currentSection;
        private ElementViewModel currentConnection;

        public DatabaseConfigurationVerifier(ConfigurationSourceModel model)
        {
            this.model = model;
        }

        public IConnectionVerifier WithConnectionString(string name)
        {
            currentSection = model.Sections.Where(s => s.ConfigurationType == typeof(ConnectionStringsSection)).First();
            Assert.IsTrue(model != null, "ConfigurationSourceModel does not contain ConnectionStringsSection");

            currentConnection = currentSection.DescendentConfigurationsOfType<ConnectionStringSettings>()
                .Where(x => x.Name == name).FirstOrDefault();

            Assert.IsNotNull(currentConnection, string.Format("Could not locate connection {0}", name));

            return this;
        }

        public IConnectionVerifier MatchesConnectionString(string s)
        {
            Assert.AreEqual(s, currentConnection.Property("ConnectionString").Value);
            return this;
        }

        public IConnectionVerifier MatchesProvider(string s)
        {
            Assert.AreEqual(s, currentConnection.Property("ProviderName").Value);
            return this;
        }
    }

    public interface IDatabaseVerifier
    {
        IConnectionVerifier WithConnectionString(string name);
    }

    public interface IConnectionVerifier
    {
        IConnectionVerifier MatchesConnectionString(string connectionString);
        IConnectionVerifier MatchesProvider(string providerName);
    }
}
