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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class ConnectionStringFixture
    {
        static readonly string userName = "User";
        static readonly string password = "Password";
        static readonly string userIdTokens = "user id=,uid=";
        static readonly string passwordTokens = "password=,pwd=";
        static ConnectionString connectionString;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConnectionStringIsNullThrows()
        {
            connectionString = new ConnectionString(null, userIdTokens, passwordTokens);
            string password = connectionString.Password;
            Assert.IsTrue(password != null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyConnectionStringThrows()
        {
            connectionString = new ConnectionString(string.Empty, userIdTokens, passwordTokens);
            Assert.AreEqual(0, connectionString.UserName.Length);
            Assert.AreEqual(0, connectionString.Password.Length);
        }

        [TestMethod]
        public void CanGetCredentialsFromRealSqlDataClass()
        {
            string initialConnectionString = String.Format(@"server=(localdb)\v11.0; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }

        [TestMethod]
        public void NoUserOrPasswordDefinedReturnsAnEmptyString()
        {
            string initialConnectionString = @"server=(localdb)\v11.0; database=JoeRandom; Integrated Security=true";
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(string.Empty, connectionString.UserName);
            Assert.AreEqual(string.Empty, connectionString.Password);
        }

        [TestMethod]
        public void CreateNewConnectionStringTest()
        {
            string initialConnectionString = String.Format(@"server=(localdb)\v11.0; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens).CreateNewConnectionString(initialConnectionString);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }

        [TestMethod]
        public void CanGetCredentialsUsingAlternatePatternsForUidAndPwd()
        {
            string initialConnectionString = String.Format(@"server=(localdb)\v11.0; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, connectionString.UserName);
            Assert.AreEqual(password, connectionString.Password);
        }

        [TestMethod]
        public void CanAddCredentialsToConnectionStringThatDoesNotHaveThem()
        {
            string initialConnectionString = @"server=(localdb)\v11.0; database=RandomData; ; ;";
            connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            connectionString.UserName = userName;
            connectionString.Password = password;
            string actualConnectionString = String.Format(@"server=(localdb)\v11.0; database=RandomData; ; ;user id={0};password={1};",
                                                          userName, password);
            Assert.AreEqual(actualConnectionString, connectionString.ToString());
        }

        [TestMethod]
        public void CanSetUserIdAndPasswordInConnectionStringThatAlreadyHasOne()
        {
            string initialConnectionString = String.Format(@"server=(localdb)\v11.0; database=JoeRandom; user id={0}; password={1}; ;", "Kill", "Bill");
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            newConnectionString.UserName = userName;
            newConnectionString.Password = password;
            string actualConnectionString = String.Format(@"server=(localdb)\v11.0; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            Assert.AreEqual(actualConnectionString, newConnectionString.ToString());
        }

        /// <summary>
        /// Test to see if ToStringNoCredentials works properly for a connection string 
        /// without a username and password
        /// </summary>
        [TestMethod]
        public void RemovingCredentialsFromConnectionStringWithoutThemIsOk()
        {
            string initialConnectionString = @"server=(localdb)\v11.0;database=RandomData;";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            string expectedConnectionString = @"server=(localdb)\v11.0;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }

        [TestMethod]
        public void WillRemoveCredentialsFromConnectionString()
        {
            string initialConnectionString = @"server=(localdb)\v11.0;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString,
                                                                        userIdTokens, passwordTokens);
            string expectedConnectionString = @"server=(localdb)\v11.0;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructConnectionStrigWithNullUserIdTokensThrows()
        {
            string initialConnectionString = @"server=(localdb)\v11.0;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, null, passwordTokens);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructConnectionStrigNullPasswordTokensThrows()
        {
            string initialConnectionString = @"server=(localdb)\v11.0;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, null);
        }
    }
}
