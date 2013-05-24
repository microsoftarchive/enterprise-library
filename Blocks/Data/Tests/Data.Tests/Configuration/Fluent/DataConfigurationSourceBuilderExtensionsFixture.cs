//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Configuration;
using System.Data.Common;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    public abstract class Given_ConfigureDataEntryPoint : ArrangeActAssert
    {
        private IConfigurationSourceBuilder Builder { get; set; }

        public IDataConfiguration DataConfig { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            Builder = new ConfigurationSourceBuilder();

            DataConfig = Builder.ConfigureData();
        }


        public T GetSectionFromBuilder<T>(string name) where T : ConfigurationSection
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            Builder.UpdateConfigurationWithReplace(source);

            return (T)source.GetSection(name);
        }

        public T GetSectionFromBuilder<T>() where T : ConfigurationSection
        {
            var sectionNamePropertyInfo = typeof(T).GetField("SectionName", BindingFlags.Static | BindingFlags.Public);

            var sectionName = (string)sectionNamePropertyInfo.GetValue(null);
            return GetSectionFromBuilder<T>(sectionName);
        }
    }

    public abstract class Given_NamedDatabase : Given_ConfigureDataEntryPoint
    {
        protected string DatabaseName = "TestDatabase";
        protected IDatabaseConfigurationProperties DatabaseConfiguration { get; private set; }


        protected override void Arrange()
        {
            base.Arrange();
            DatabaseConfiguration = DataConfig.ForDatabaseNamed(DatabaseName);
        }

        protected ConnectionStringSettings GetConnectionStringSettings()
        {
            var connectionStrings = GetSectionFromBuilder<ConnectionStringsSection>("connectionStrings");
            return connectionStrings.ConnectionStrings[DatabaseName];
        }

    }

    [TestClass]
    public class When_ConfiguringDatabasePassingNullForName : Given_ConfigureDataEntryPoint
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ForDatabaseNamed_ThrowsArgumentException()
        {
            DataConfig.ForDatabaseNamed(null);
        }
    }

    [TestClass]
    public class When_ConfiguringOnlyANameDatabase : Given_NamedDatabase
    {

        [TestMethod]
        public void Then_NamedConnectionIsProvided()
        {
            Assert.IsNotNull(GetConnectionStringSettings());
        }

        [TestMethod]
        public void Then_DefaultsToSqlProvider()
        {
            Assert.AreEqual(DbProviderMapping.DefaultSqlProviderName, GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_DefaultsConnectionStringToLocalSql()
        {
            Assert.AreEqual(@"Database=Database;Server=(local)\SQLEXPRESS;Integrated Security=SSPI", GetConnectionStringSettings().ConnectionString);
        }
    }

#pragma warning disable 612, 618
    [TestClass]
    public class When_PassingNullConnectionStringBuilderToOracleDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithConnectionString_ThrowsArgumentNullException()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString((OracleConnectionStringBuilder)null);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringToOracleDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithConnectionString_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString((string)null);
        }
    }

    [TestClass]
    public class When_ConfiguringDatabaseWithConnectionAndBuiltInProvider : Given_NamedDatabase
    {
        private string connectionString = "MyConnectionString";
        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString(connectionString);

        }

        [TestMethod]
        public void Then_ConnectionStringPropertyIsSet()
        {
            Assert.AreEqual(connectionString, GetConnectionStringSettings().ConnectionString);
        }

        [TestMethod]
        public void Then_ProviderNameIsSetAppropriately()
        {
            Assert.AreEqual(DbProviderMapping.DefaultOracleProviderName, GetConnectionStringSettings().ProviderName);
        }
    }
#pragma warning restore 612, 618

    [TestClass]
    public class When_ConfiguringWithSpecifiedProviderName : Given_NamedDatabase
    {
        private DbConnectionStringBuilder builder;
        private string providerName = "myTestProviderName";

        protected override void Arrange()
        {
            base.Arrange();
            builder = new DbConnectionStringBuilder();
            builder.Add("SomeConnectionKeyword", "value");
        }
        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnotherDatabaseType(providerName)
                    .WithConnectionString(builder);
        }

        [TestMethod]
        public void Then_ProviderNameIsSetToSpecifiedName()
        {
            Assert.AreEqual(providerName, GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_ConnectionStringIsSet()
        {
            Assert.AreEqual(builder.ConnectionString, GetConnectionStringSettings().ConnectionString);
        }
    }

    [TestClass]
    public class When_ConfiguringWithDefaultSqlProviderName : Given_NamedDatabase
    {
        protected override void Arrange()
        {
            base.Arrange();
            DatabaseConfiguration
                .ThatIs.AnotherDatabaseType("TestProviderName");  // Change provider name from default SqlProvider

        }
        protected override void Act()
        {
            DatabaseConfiguration.ThatIs.ASqlDatabase();
        }

        [TestMethod]
        public void Then_ProviderNameIsSetAppropriately()
        {
            Assert.AreEqual(DbProviderMapping.DefaultSqlProviderName, GetConnectionStringSettings().ProviderName);
        }
    }

#pragma warning disable 612, 618
    [TestClass]
    public class When_ConfiguringMultipleDatabase : Given_NamedDatabase
    {
        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                .WithConnectionString("someConnectionString")

            .ForDatabaseNamed("MyOtherDatabase")
                .AsDefault();

        }

        [TestMethod]
        public void Then_DefaultDatabaseIsSetAppropriately()
        {
            var settings = GetSectionFromBuilder<DatabaseSettings>();
            Assert.AreEqual("MyOtherDatabase", settings.DefaultDatabase);
        }
    }
#pragma warning restore 612, 618

    [TestClass]
    public class When_PassingNullConnectionStringBuilderToOleDbDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithConnectionString_ThrowsArgumentNullException()
        {
            DatabaseConfiguration
                .ThatIs.AnOleDbDatabase()
                    .WithConnectionString((OleDbConnectionStringBuilder)null);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringToOleDbDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithConnectionString_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnOleDbDatabase()
                    .WithConnectionString((string)null);
        }
    }

    [TestClass]
    public class When_ConfiguringForOleDb : Given_NamedDatabase
    {
        private OleDbConnectionStringBuilder builder;

        protected override void Arrange()
        {
            base.Arrange();
            builder = new OleDbConnectionStringBuilder()
                       {
                           DataSource = "someSource",
                           FileName = "SomeFile"

                       };
        }

        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOleDbDatabase()
                    .WithConnectionString(builder);
        }

        [TestMethod]
        public void Then_ConnectionStringProviderIsOleDb()
        {
            Assert.AreEqual("System.Data.OleDb", GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_ConnectionStringMatchesBuilderString()
        {
            Assert.AreEqual(builder.ConnectionString, GetConnectionStringSettings().ConnectionString);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringBuilderToOdbcDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithConnectionString_ThrowsArgumentNullException()
        {
            DatabaseConfiguration
                .ThatIs.AnOdbcDatabase()
                    .WithConnectionString((OdbcConnectionStringBuilder)null);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringToOdbcDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithConnectionString_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnOdbcDatabase()
                    .WithConnectionString((string)null);
        }
    }

    [TestClass]
    public class When_ConfiguringForOdbcProvider : Given_NamedDatabase
    {

        private OdbcConnectionStringBuilder builder;

        protected override void Arrange()
        {
            base.Arrange();
            builder = new OdbcConnectionStringBuilder()
            {
                Dsn = "OdbcDsn",
                Driver = "OdbcDriver"
            };
        }

        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOdbcDatabase()
                    .WithConnectionString(builder);
        }

        [TestMethod]
        public void Then_ConnectionStringProviderIsOleDb()
        {
            Assert.AreEqual("System.Data.Odbc", GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_ConnectionStringMatchesBuilderString()
        {
            Assert.AreEqual(builder.ConnectionString, GetConnectionStringSettings().ConnectionString);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringToSqlCeDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithConnectionString_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.ASqlCeDatabase()
                    .WithConnectionString(string.Empty);
        }
    }

    [TestClass]
    public class When_ConfiguringForSqlCeProvider : Given_NamedDatabase
    {

        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.ASqlCeDatabase()
                    .WithConnectionString("someConnection");
        }

        [TestMethod]
        public void Then_ConnectionStringProviderIsSqlServerCe()
        {
            Assert.AreEqual("System.Data.SqlServerCe", GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_ConnectionStringIsConfigured()
        {
            Assert.AreEqual("someConnection", GetConnectionStringSettings().ConnectionString);
        }
    }

    [TestClass]
    public class When_ConfiguringForOdbcProviderWithoutBuilder : Given_NamedDatabase
    {

        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOdbcDatabase()
                    .WithConnectionString("someConnectionString");
        }

        [TestMethod]
        public void Then_ConnectionStringMatchesProvidedString()
        {
            Assert.AreEqual("someConnectionString", GetConnectionStringSettings().ConnectionString);
        }
    }

#pragma warning disable 612, 618
    [TestClass]
    public class When_ConfiguringOraclePackagesPassingNullForPackageName : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithPackageNamed_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString("connectionstring")
                       .WithPackageNamed(null);
        }
    }

    [TestClass]
    public class When_ConfiguringOraclePackagesPassingNullForPackagePrefix : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AndPrefix_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString("connectionstring")
                       .WithPackageNamed("packageName")
                        .AndPrefix(null);
        }
    }
#pragma warning restore 612, 618

#pragma warning disable 612, 618
    [TestClass]
    public class When_ConfiguringOracleDatabaseWithSettings : Given_NamedDatabase
    {
        OracleConnectionStringBuilder connectionStringBuilder;

        protected override void Arrange()
        {
            base.Arrange();

            connectionStringBuilder = new OracleConnectionStringBuilder()
                {
                    UserID = "someUser",
                    DataSource = "SomeDataSource"
                };

        }
        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString(connectionStringBuilder)
                       .WithPackageNamed("somePackage")
                        .AndPrefix("thePrefix");
        }

        [TestMethod]
        public void Then_IsSetToOracleProvider()
        {
            Assert.AreEqual("System.Data.OracleClient", GetConnectionStringSettings().ProviderName);
        }

        [TestMethod]
        public void Then_ConfiguredConnectionStringMatches()
        {
            Assert.AreEqual(connectionStringBuilder.ConnectionString, GetConnectionStringSettings().ConnectionString);
        }

        [TestMethod]
        public void Then_PackageSettingsMatchDatabaseName()
        {
            var oracleSettings = GetSectionFromBuilder<OracleConnectionSettings>();
            Assert.IsTrue(oracleSettings.OracleConnectionsData.Contains(DatabaseName));
        }

        [TestMethod]
        public void Then_PackageNameIsConfigured()
        {
            var oracleSettings = GetSectionFromBuilder<OracleConnectionSettings>();
            Assert.IsTrue(oracleSettings.OracleConnectionsData.Get(DatabaseName).Packages.Contains("somePackage"));
        }

        [TestMethod]
        public void Then_PackagePrefixIsConfigured()
        {
            var oracleSettings = GetSectionFromBuilder<OracleConnectionSettings>();
            var package = oracleSettings.OracleConnectionsData.Get(DatabaseName).Packages.Get("somePackage");
            Assert.AreEqual("thePrefix", package.Prefix);
        }
    }

    [TestClass]
    public class When_ConfiguringMultipleOracleDatabaseConnections : Given_NamedDatabase
    {
        protected override void Act()
        {
            DatabaseConfiguration
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString("connectionStringA")
                    .WithPackageNamed("packageA").AndPrefix("prefixA")
            .ForDatabaseNamed("DatabaseB")
                .ThatIs.AnOracleDatabase()
                    .WithConnectionString("connectionStringB")
                    .WithPackageNamed("packageB").AndPrefix("prefixB");
        }

        private OracleConnectionData GetConnectionData(string dbName)
        {
            var oracleSettings = GetSectionFromBuilder<OracleConnectionSettings>();
            return oracleSettings.OracleConnectionsData.Get(dbName);
        }

        [TestMethod]
        public void Then_TheFirstConnectionPropertiesAreSet()
        {
            var connectionString = GetConnectionStringSettings();
            Assert.AreEqual("connectionStringA", connectionString.ConnectionString);
            Assert.AreEqual("prefixA", GetConnectionData(DatabaseName).Packages.Get("packageA").Prefix);
        }

        [TestMethod]
        public void Then_TheSecondConnectionPropertiesAreSet()
        {
            var connectionStrings = GetSectionFromBuilder<ConnectionStringsSection>("connectionStrings");

            Assert.AreEqual("connectionStringB", connectionStrings.ConnectionStrings["DatabaseB"].ConnectionString);
            Assert.AreEqual("prefixB", GetConnectionData("DatabaseB").Packages.Get("packageB").Prefix);
        }

    }
#pragma warning restore 612, 618


    [TestClass]
    public class When_PassingNullProviderNameToDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AnotherDatabaseType_ThrowsArgumentNullException()
        {
            DatabaseConfiguration
                .ThatIs.AnotherDatabaseType(null);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringBuilderToAnotherDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithConnectionString_ThrowsArgumentNullException()
        {
            DatabaseConfiguration
                .ThatIs.AnotherDatabaseType("provider name")
                    .WithConnectionString((DbConnectionStringBuilder)null);
        }
    }

    [TestClass]
    public class When_PassingNullConnectionStringToAnotherDatabase : Given_NamedDatabase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithConnectionString_ThrowsArgumentException()
        {
            DatabaseConfiguration
                .ThatIs.AnotherDatabaseType("provider name")
                    .WithConnectionString((string)null);
        }
    }



    [TestClass]
    public class When_OnlyCustomProviderSupplied : Given_ConfigureDataEntryPoint
    {
        protected override void Act()
        {
            DataConfig
                .WithProviderNamed("CustomProvider");
        }

        [TestMethod]
        public void Then_ProviderMappedToGenericDatabase()
        {
            var settings = GetSectionFromBuilder<DatabaseSettings>();
            var mapping = settings.ProviderMappings.Single(m => m.Name == "CustomProvider");
            Assert.AreEqual(typeof(Data.GenericDatabase), mapping.DatabaseType);
        }
    }

    [TestClass]
    public class When_CustomProviderAndMappingsSupplied : Given_ConfigureDataEntryPoint
    {
        protected override void Act()
        {
            DataConfig
                .WithProviderNamed("CustomProvider")
                    .MappedToDatabase<SqlDatabase>();
        }

        [TestMethod]
        public void Then_ProviderMappedToCorrectDatabase()
        {
            var settings = GetSectionFromBuilder<DatabaseSettings>();
            var mapping = settings.ProviderMappings.Single(m => m.Name == "CustomProvider");
            Assert.AreEqual(typeof(SqlDatabase), mapping.DatabaseType);
        }
    }

#pragma warning disable 612, 618
    [TestClass]
    public class When_MultipleCustomProviderAndMappingsSupplied : Given_ConfigureDataEntryPoint
    {
        protected override void Act()
        {
            DataConfig
                .WithProviderNamed("CustomProvider")
                    .MappedToDatabase<SqlDatabase>()
                .WithProviderNamed("AnotherProvider")
                    .MappedToDatabase<OracleDatabase>();
        }

        [TestMethod]
        public void Then_FirstProviderMappingConfigured()
        {
            var settings = GetSectionFromBuilder<DatabaseSettings>();
            var mapping = settings.ProviderMappings.Single(m => m.Name == "CustomProvider");
            Assert.AreEqual(typeof(SqlDatabase), mapping.DatabaseType);
        }

        [TestMethod]
        public void Then_SecondProviderConfigured()
        {
            var settings = GetSectionFromBuilder<DatabaseSettings>();
            var mapping = settings.ProviderMappings.Single(m => m.Name == "AnotherProvider");
            Assert.AreEqual(typeof(OracleDatabase), mapping.DatabaseType);

        }
    }
#pragma warning restore 612, 618

    [TestClass]
    public class When_CustomProviderMappedToIncorrectImplementorType : Given_ConfigureDataEntryPoint
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            DataConfig
                .WithProviderNamed("CustomProvider").MappedToDatabase(typeof(When_CustomProviderAndMappingsSupplied));
        }
    }
}
