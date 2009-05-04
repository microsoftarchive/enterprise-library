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
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    // Helper class to build up config sources with various sections in them.
    class ConfigSourceBuilder
    {
        private readonly DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();

        public IConfigurationSource ConfigSource
        {
            get { return configSource; }
        }

        public ConfigSourceBuilder AddCryptoSettings()
        {
            var cryptoSettings = new CryptographySettings();

            var md5Data = new HashAlgorithmProviderData("md5",
                                                        typeof(MD5CryptoServiceProvider), true);


            cryptoSettings.HashProviders.Add(md5Data);

            var sha512Data = new HashAlgorithmProviderData("sha512", typeof(SHA512CryptoServiceProvider), true);
            cryptoSettings.HashProviders.Add(sha512Data);

            cryptoSettings.DefaultHashProviderName = "md5";

            configSource.Add(CryptographyConfigurationView.SectionName, cryptoSettings);

            return this;
        }

        public const string DefaultExceptionPolicyName = "default";

        public ConfigSourceBuilder AddExceptionHandlingSettings()
        {
            var ehSettings = new ExceptionHandlingSettings();

            var policy = new ExceptionPolicyData(DefaultExceptionPolicyName);
            var typeData = new ExceptionTypeData("all", typeof(Exception), PostHandlingAction.NotifyRethrow);
            typeData.ExceptionHandlers.Add(new WrapHandlerData("wrap", "New message", "System.ArgumentException"));
            policy.ExceptionTypes.Add(typeData);
            ehSettings.ExceptionPolicies.Add(policy);

            configSource.Add(ExceptionHandlingSettings.SectionName, ehSettings);
            return this;
        }

        public const string NorthwindConnectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=Northwind;Integrated Security=True";

        public ConfigSourceBuilder AddConnectionStringSettings()
        {
            var connectionStrings = new ConnectionStringsSection();

            connectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("northwind", 
                NorthwindConnectionString,
                DbProviderMapping.DefaultSqlProviderName));
            configSource.Add("connectionStrings", connectionStrings);

            var databaseSettings = new DatabaseSettings() {DefaultDatabase = "northwind"};
            configSource.Add(DatabaseSettings.SectionName, databaseSettings);

            return this;
        }
    }
}
