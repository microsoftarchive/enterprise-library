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

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleParameterFixture
    {
        Guid referenceGuid = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            CreateStoredProcedure();
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteStoredProcedure();
        }

        [TestMethod]
        public void CanInsertNullStringParameter()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            Database db = factory.Create("OracleTest");

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlString = "insert into Customers (CustomerID, CompanyName, ContactName) Values (:id, :name, :contact)";
                    DbCommand insert = db.GetSqlStringCommand(sqlString);
                    db.AddInParameter(insert, ":id", DbType.Int32, 1);
                    db.AddInParameter(insert, ":name", DbType.String, "fee");
                    db.AddInParameter(insert, ":contact", DbType.String, null);

                    db.ExecuteNonQuery(insert, transaction);
                    transaction.Rollback();
                }
            }
        }

        [TestMethod]
        public void CanSetValueForGuidParameters()
        {
            string parameterName = "dummyParameter";
            byte[] guidBytes = new byte[16];

            DbCommand dBCommand = db.GetStoredProcCommand("IGNORED");
            db.AddInParameter(dBCommand, parameterName, DbType.Guid);
            db.SetParameterValue(dBCommand, parameterName, new Guid(guidBytes));
            object paramValue = db.GetParameterValue(dBCommand, parameterName);

            Assert.IsNotNull(paramValue);
            Assert.AreSame(typeof(Guid), paramValue.GetType());
        }

        [TestMethod]
        public void CanSetValueForGuidParametersAfterRoundtripToDatabase()
        {
            Guid guid = new Guid(new byte[16]);
            string name = "ENTLIB";
            object outputGuidValue = null;
            object outputStringValue = null;

            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 0);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid);
            db.AddOutParameter(dbCommand, "outputString", DbType.String, 20);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.SetParameterValue(dbCommand, "inputGuid", guid);
            db.SetParameterValue(dbCommand, "inputString", name);

            db.ExecuteNonQuery(dbCommand);
            outputGuidValue = db.GetParameterValue(dbCommand, "outputGuid");
            outputStringValue = db.GetParameterValue(dbCommand, "outputString");

            Assert.IsNotNull(outputGuidValue);
            Assert.IsFalse(outputGuidValue == DBNull.Value);
            Assert.AreSame(typeof(Guid), outputGuidValue.GetType());
            Assert.AreEqual(guid, outputGuidValue);
            Assert.IsNotNull(outputStringValue);
            Assert.IsFalse(outputStringValue == DBNull.Value);
            Assert.AreSame(typeof(String), outputStringValue.GetType());
            Assert.AreEqual(name, outputStringValue);
        }

        [TestMethod]
        public void CaGetValueForDiscoveredGuidParameters()
        {
            Guid guid = new Guid(new byte[16]);
            string name = "ENTLIB";
            object inputGuidValue = null;

            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetGuid", null, guid, null, name);

            inputGuidValue = db.GetParameterValue(dbCommand, "inputGuid");

            Assert.IsNotNull(inputGuidValue);
            Assert.IsFalse(inputGuidValue == DBNull.Value);
            Assert.AreSame(typeof(Guid), inputGuidValue.GetType());
            Assert.AreEqual(guid, inputGuidValue);
        }

        [TestMethod]
        public void CanUseGuidParameterMultipleTimes()
        {
            DbCommand dbCommand;
            Guid myGuid = new Guid();
            Guid outputVal;

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 100);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.AddOutParameter(dbCommand, "outputString", DbType.String, 20);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 100);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.AddOutParameter(dbCommand, "outputString", DbType.String, 20);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void SecondUseWithDifferentTypeKeepsOriginalType()
        {
            DbCommand dbCommand;
            Guid myGuid = new Guid();
            Guid outputVal;

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 100);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.AddOutParameter(dbCommand, "outputString", DbType.String, 20);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.String, 100);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.AddOutParameter(dbCommand, "outputString", DbType.String, 20);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.ExecuteNonQuery(dbCommand);
            db.GetParameterValue(dbCommand, "outputGuid");
        }

        void CreateStoredProcedure()
        {
            string storedProcedureCreation =
                "CREATE OR REPLACE PROCEDURE SetAndGetGuid(outputGuid OUT RAW, inputGuid IN RAW, outputString OUT VARCHAR2, inputString IN VARCHAR2) AS " +
                "BEGIN" +
                "	SELECT inputGuid INTO outputGuid FROM DUAL; " +
                "	SELECT inputString INTO outputString FROM DUAL; " +
                "END;";

            DbCommand command = db.GetSqlStringCommand(storedProcedureCreation);
            db.ExecuteNonQuery(command);
        }

        void DeleteStoredProcedure()
        {
            string storedProcedureDeletion = "Drop procedure SetAndGetGuid";
            DbCommand command = db.GetSqlStringCommand(storedProcedureDeletion);
            db.ExecuteNonQuery(command);
        }
    }
}
