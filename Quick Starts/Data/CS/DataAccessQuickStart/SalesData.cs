//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;

namespace DataAccessQuickStart
{
    /// <summary>
    /// Encapulsates access to the database.
    /// </summary>
    public class SalesData
    {
        public SalesData()
        {
        }

        /// <summary>
        /// Retreives a list of customers from the database.
        /// </summary>
        /// <returns>List of customers in a string.</returns>
        /// <remarks>Demonstrates retrieving multiple rows of data using
        /// a DataReader</remarks>
        public string GetCustomerList()
        {
            // DataReader that will hold the returned results		
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = "Select CustomerID, Name, Address, City, Country, PostalCode " +
				"From Customers";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            StringBuilder readerData = new StringBuilder();

            // The ExecuteReader call will request the connection to be closed upon
            // the closing of the DataReader. The DataReader will be closed 
            // automatically when it is disposed.
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                // Iterate through DataReader and put results to the text box.
                // DataReaders cannot be bound to Windows Form controls (e.g. the
                // resultsDataGrid), but may be bound to Web Form controls.
                while (dataReader.Read())
                {
                    // Get the value of the 'Name' column in the DataReader
                    readerData.Append(dataReader["Name"]);
                    readerData.Append(Environment.NewLine);
                }
            }

            return readerData.ToString();
        }

        /// <summary>
        /// Retreives all products in the specified category.
        /// </summary>
        /// <param name="Category">The category containing the products.</param>
        /// <returns>DataSet containing the products.</returns>
        /// <remarks>Demonstrates retrieving multiple rows using a DataSet.</remarks>
        public DataSet GetProductsInCategory(int Category)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = "GetProductsByCategory";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Retrieve products from the specified category.
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, Category);

            // DataSet that will hold the returned results		
            DataSet productsDataSet = null;

            productsDataSet = db.ExecuteDataSet(dbCommand);

            // Note: connection was closed by ExecuteDataSet method call 

            return productsDataSet;
        }

        /// <summary>
        /// Updates the product database.
        /// </summary>
        /// <returns>The number of rows affected by the update.</returns>
        /// <remarks>Demonstrates updating a database using a DataSet.</remarks>
        public int UpdateProducts()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            DataSet productsDataSet = new DataSet();

            string sqlCommand = "Select ProductID, ProductName, CategoryID, UnitPrice, LastUpdate " +
				"From Products";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            string productsTable = "Products";

            // Retrieve the initial data
            db.LoadDataSet(dbCommand, productsDataSet, productsTable);

            // Get the table that will be modified
            DataTable table = productsDataSet.Tables[productsTable];

            // Add a new product to existing DataSet
            DataRow addedRow = table.Rows.Add(new object[] {DBNull.Value, "New product", 11, 25});

            // Modify an existing product
            table.Rows[0]["ProductName"] = "Modified product";

            // Establish our Insert, Delete, and Update commands
            DbCommand insertCommand = db.GetStoredProcCommand("AddProduct");
            db.AddInParameter(insertCommand, "ProductName", DbType.String, "ProductName", DataRowVersion.Current);
			db.AddInParameter(insertCommand, "CategoryID", DbType.Int32, "CategoryID", DataRowVersion.Current);
			db.AddInParameter(insertCommand, "UnitPrice", DbType.Currency, "UnitPrice", DataRowVersion.Current);

            DbCommand deleteCommand = db.GetStoredProcCommand("DeleteProduct");
			db.AddInParameter(deleteCommand, "ProductID", DbType.Int32, "ProductID", DataRowVersion.Current);

            DbCommand updateCommand = db.GetStoredProcCommand("UpdateProduct");
			db.AddInParameter(updateCommand, "ProductID", DbType.Int32, "ProductID", DataRowVersion.Current);
			db.AddInParameter(updateCommand, "ProductName", DbType.String, "ProductName", DataRowVersion.Current);
			db.AddInParameter(updateCommand, "LastUpdate", DbType.DateTime, "LastUpdate", DataRowVersion.Current);

            // Submit the DataSet, capturing the number of rows that were affected
            int rowsAffected = db.UpdateDataSet(productsDataSet, "Products", insertCommand, updateCommand,
                                                deleteCommand, UpdateBehavior.Standard);

            return rowsAffected;

        }

        /// <summary>
        /// Retrieves details about the specified product.
        /// </summary>
        /// <param name="productID">The ID of the product used to retrieve details.</param>
        /// <returns>The product details as a string.</returns>
        /// <remarks>Demonstrates retrieving a single row of data using output parameters.</remarks>
        public string GetProductDetails(int productID)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = "GetProductDetails";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            // Add paramters
            // Input parameters can specify the input value
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productID);
            // Output parameters specify the size of the return data
			db.AddOutParameter(dbCommand, "ProductName", DbType.String, 50);
			db.AddOutParameter(dbCommand, "UnitPrice", DbType.Currency, 8);

            db.ExecuteNonQuery(dbCommand);

            // Row of data is captured via output parameters
            string results = string.Format(CultureInfo.CurrentCulture, "{0}, {1}, {2:C} ",
                                           db.GetParameterValue(dbCommand, "ProductID"),
										   db.GetParameterValue(dbCommand, "ProductName"),
										   db.GetParameterValue(dbCommand, "UnitPrice"));

            return results;
        }

        /// <summary>
        /// Retrieves the specified product's name.
        /// </summary>
        /// <param name="productID">The ID of the product.</param>
        /// <returns>The name of the product.</returns>
        /// <remarks>Demonstrates retrieving a single item. Parameter discovery
        /// is used for determining the properties of the productID parameter.</remarks>
        public string GetProductName(int productID)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            // Passing the productID value to the GetStoredProcCommand
            // results in parameter discovery being used to correctly establish the parameter
            // information for the productID. Subsequent calls to this method will
            // cause the block to retrieve the parameter information from the 
            // cache, and not require rediscovery.
            string sqlCommand = "GetProductName";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, productID);

            // Retrieve ProdcutName. ExecuteScalar returns an object, so
            // we cast to the correct type (string).
            string productName = (string) db.ExecuteScalar(dbCommand);

            return productName;
        }

        /// <summary>
        /// Retrieves a list of products.
        /// </summary>
        /// <returns>A list of products as an XML string.</returns>
        /// <remarks>Demonstrates retrieving multiple rows of data as XML. This
        /// method is not portable across database providers, but is 
        /// specific to the SqlDatabase.</remarks>
        public string GetProductList()
        {
            // Use a named database instance that refers to a SQL Server database.
            SqlDatabase dbSQL = DatabaseFactory.CreateDatabase() as SqlDatabase;

            // Use "FOR XML AUTO" to have SQL return XML data
			string sqlCommand = "Select ProductID, ProductName, CategoryID, UnitPrice, LastUpdate " +
				"From Products FOR XML AUTO";
			DbCommand dbCommand = dbSQL.GetSqlStringCommand(sqlCommand);

            XmlReader productsReader = null;
            StringBuilder productList = new StringBuilder();

            try
            {
                productsReader = dbSQL.ExecuteXmlReader(dbCommand);

                // Iterate through the XmlReader and put the data into our results.
                while (!productsReader.EOF)
                {
                    if (productsReader.IsStartElement())
                    {
                        productList.Append(productsReader.ReadOuterXml());
                        productList.Append(Environment.NewLine);
                    }
                }
            }
            finally
            {
              // Close the Reader.
              if (productsReader != null)
              {
                  productsReader.Close();
              }
              
              // Explicitly close the connection. The connection is not closed
              // when the XmlReader is closed.
              if (dbCommand.Connection != null)
              {
                dbCommand.Connection.Close();
              }  
            }

            return productList.ToString();
        }

        /// <summary>
        /// Transfers an amount between two accounts.
        /// </summary>
        /// <param name="transactionAmount">Amount to transfer.</param>
        /// <param name="sourceAccount">Account to be credited.</param>
        /// <param name="destinationAccount">Account to be debited.</param>
        /// <returns>true if sucessful; otherwise false.</returns>
        /// <remarks>Demonstrates executing multiple updates within the 
        /// context of a transaction.</remarks>
        public bool Transfer(int transactionAmount, int sourceAccount, int destinationAccount)
        {
            bool result = false;
            
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase();

            // Two operations, one to credit an account, and one to debit another
            // account.
            string sqlCommand = "CreditAccount";
            DbCommand creditCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(creditCommand, "AccountID", DbType.Int32, sourceAccount);
			db.AddInParameter(creditCommand, "Amount", DbType.Int32, transactionAmount);

            sqlCommand = "DebitAccount";
            DbCommand debitCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(debitCommand, "AccountID", DbType.Int32, destinationAccount);
            db.AddInParameter(debitCommand, "Amount", DbType.Int32, transactionAmount);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Credit the first account
                    db.ExecuteNonQuery(creditCommand, transaction);
                    // Debit the second account
                    db.ExecuteNonQuery(debitCommand, transaction);

                    // Commit the transaction
                    transaction.Commit();
                    
                    result = true;
                }
                catch
                {
                    // Rollback transaction 
                    transaction.Rollback();
                }
                connection.Close();
                
                return result;
            }
        }
    }
}