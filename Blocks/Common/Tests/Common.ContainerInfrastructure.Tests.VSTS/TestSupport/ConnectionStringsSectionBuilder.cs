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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    /// <summary>
    /// Helper class to build up <see cref="ConnectionStringsSection"/> objects
    /// programatically using a fluent builder pattern.
    /// </summary>
    public class ConnectionStringsSectionBuilder : IAddConnectionAddTo
    {
        private readonly List<ConnectionStringBuilder> builders = new List<ConnectionStringBuilder>();
        private string defaultDatabase;

        public INamedConnectionString AddConnection()
        {
            var builder = new ConnectionStringBuilder(this);
            builders.Add(builder);
            return builder;
        }

        void IAddTo.AddTo(IConfigurationSource configurationSource)
        {
            var section = new ConnectionStringsSection();
            foreach(var builder in builders)
            {
                builder.AddTo(section);
            }

            configurationSource.Add("connectionStrings", section);

            var databaseSettings = new DatabaseSettings {DefaultDatabase = defaultDatabase};
            configurationSource.Add(DatabaseSettings.SectionName, databaseSettings);
        }

        private class ConnectionStringBuilder :
            INamedConnectionString, IWithString,
            IAsDefaultAddConnectionAddTo,
            IWithDbProviderAsDefaultAddConnectionAddTo
        {
            private readonly ConnectionStringsSectionBuilder outer;
            private readonly ConnectionStringSettings settings = new ConnectionStringSettings();

            public ConnectionStringBuilder(ConnectionStringsSectionBuilder outer)
            {
                this.outer = outer;
            }

            public void AddTo(ConnectionStringsSection section)
            {
                var setting = new ConnectionStringSettings(settings.Name, 
                    settings.ConnectionString, settings.ProviderName);

                section.ConnectionStrings.Add(setting);
            }

            IWithString INamedConnectionString.Named(string name)
            {
                settings.Name = name;
                return this;
            }

            IWithDbProviderAsDefaultAddConnectionAddTo IWithString.WithString(string connectionString)
            {
                settings.ConnectionString = connectionString;
                return this;
            }

            IAsDefaultAddConnectionAddTo IWithDbProvider.WithProvider(string providerName)
            {
                settings.ProviderName = providerName;
                return this;
            }

            IAddConnectionAddTo IAsDefault.AsDefault
            {
                get
                {
                    outer.defaultDatabase = settings.Name;
                    return outer;
                }
            }

            INamedConnectionString IAddConnection.AddConnection()
            {
                return outer.AddConnection();
            }

            void IAddTo.AddTo(IConfigurationSource configurationSource)
            {
                ((IAddTo) outer).AddTo(configurationSource);
            }
        }
    }

    #region Interfaces that define our fluent grammar

    public interface IAddConnection
    {
        INamedConnectionString AddConnection();
    }

    public interface INamedConnectionString
    {
        IWithString Named(string name);
    }

    public interface IWithString
    {
        IWithDbProviderAsDefaultAddConnectionAddTo WithString(string connectionString);
    }

    public interface IWithDbProvider
    {
        IAsDefaultAddConnectionAddTo WithProvider(string providerName);
    }

    public interface IAsDefault
    {
        IAddConnectionAddTo AsDefault { get; }
    }

    public interface IAddConnectionAddTo : IAddConnection, IAddTo {}

    public interface IAsDefaultAddConnectionAddTo : IAsDefault, IAddConnection, IAddTo {}

    public interface IWithDbProviderAsDefaultAddConnectionAddTo : IWithDbProvider, IAsDefault, IAddConnection, IAddTo { }

    #endregion
}
