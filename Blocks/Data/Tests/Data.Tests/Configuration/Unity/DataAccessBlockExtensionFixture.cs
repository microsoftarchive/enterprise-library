//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration.Unity
{
	[TestClass]
	public class DataAccessBlockExtensionFixture
	{
		[TestMethod]
		public void CanCreateDatabaseFromContainer()
		{
			IUnityContainer container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddNewExtension<DataAccessBlockExtension>();

			Database createdObject = container.Resolve<Database>("Service_Dflt");

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (SqlDatabase));
			Assert.AreEqual(@"server=(local)\sqlexpress;database=northwind;integrated security=true;",
			                createdObject.ConnectionStringWithoutCredentials);
		}

		[TestMethod]
		public void CanCreateDefaultDatabaseFromContainer()
		{
			IUnityContainer container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddNewExtension<DataAccessBlockExtension>();

			Database createdObject = container.Resolve<Database>();

			Assert.IsNotNull(createdObject);
			Assert.IsInstanceOfType(createdObject, typeof (SqlDatabase));
			Assert.AreEqual(@"server=(local)\sqlexpress;database=northwind;integrated security=true;",
			                createdObject.ConnectionStringWithoutCredentials);
		}

		[TestMethod]
		public void CanCreateSqlDatabaseFromContainer()
		{
			IUnityContainer container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddNewExtension<DataAccessBlockExtension>();

			SqlDatabase createdObject = container.Resolve<SqlDatabase>("Service_Dflt");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(@"server=(local)\sqlexpress;database=northwind;integrated security=true;",
			                createdObject.ConnectionStringWithoutCredentials);
		}

		[TestMethod]
		public void CanCreateOracleDatabaseFromContainer()
		{
			IUnityContainer container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddNewExtension<DataAccessBlockExtension>();

			OracleDatabase createdObject = container.Resolve<OracleDatabase>("OracleTest");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(@"server=entlib;", createdObject.ConnectionStringWithoutCredentials);

			// can do the configured package mapping?
			Assert.AreEqual(DatabaseWithObjectBuildUperFixture.OracleTestTranslatedStoredProcedureInPackageWithTranslation,
			                createdObject.GetStoredProcCommand(
			                	DatabaseWithObjectBuildUperFixture.OracleTestStoredProcedureInPackageWithTranslation).CommandText);
			Assert.AreEqual(DatabaseWithObjectBuildUperFixture.OracleTestStoredProcedureInPackageWithoutTranslation,
			                createdObject.GetStoredProcCommand(
			                	DatabaseWithObjectBuildUperFixture.OracleTestStoredProcedureInPackageWithoutTranslation).CommandText);
		}

		[TestMethod]
		public void CanCreateGenericDatabaseFromContainer()
		{
			IUnityContainer container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
			container.AddNewExtension<DataAccessBlockExtension>();

			GenericDatabase createdObject = container.Resolve<GenericDatabase>("OdbcDatabase");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(@"some connection string;",
			                createdObject.ConnectionStringWithoutCredentials);
			Assert.AreEqual(DbProviderFactories.GetFactory("System.Data.Odbc"), createdObject.DbProviderFactory);
		}
	}
}