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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    /// <summary>
    /// A helper class to create a common set of configuration information
    /// to ease setup in our tests.
    /// </summary>
    internal class ConfigSourceBuilder
    {
        public const string DefaultExceptionPolicyName = "default";

        public const string NorthwindConnectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=Northwind;Integrated Security=True";

        public const string PolicyName = "policy";

        private readonly DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();

        public IConfigurationSource ConfigSource
        {
            get { return configSource; }
        }

        public ConfigSourceBuilder AddExceptionHandlingSettings()
        {
            ExceptionSectionBuilder builder = new SectionBuilder().ExceptionSection();
            builder
                .AddPolicy(DefaultExceptionPolicyName)
                    .AddException<Exception>().Named("all").NotifyRethrow
                        .AddWrapHandler().Named("wrap").WithMessage("New message").WrapWith<ArgumentException>()
                .AddTo(configSource);
            return this;
        }

        public ConfigSourceBuilder AddConnectionStringSettings()
        {
            ConnectionStringsSectionBuilder builder = new SectionBuilder().ConnectionStringsSection();
            builder
                .AddConnection()
                    .Named("northwind")
                    .WithString(NorthwindConnectionString)
                    .WithProvider(DbProviderMapping.DefaultSqlProviderName)
                    .AsDefault
                .AddTo(configSource);
            return this;
        }

        public ConfigSourceBuilder AddPolicyInjectionSettings()
        {
            PiabSectionBuilder builder = new SectionBuilder().PiabSection();
            builder
                .AddPolicy(PolicyName)
                .AddTo(configSource);

            return this;
        }
    }
}
