'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Data Access Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports Microsoft.Practices.Unity

'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Data Access Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


' Encapulsates access to the database.
Public Class SalesData

    Public Sub SalesData()
    End Sub

    Dim db As Database

    <InjectionConstructor()> _
    Public Sub New(<Dependency()> ByVal db As Database)
        Me.db = db
    End Sub


    ' Retrieves a list of customers from the database.
    ' Returns: List of customers in a string.
    ' Remarks: Demonstrates retrieving multiple rows of data using
    ' a DataReader
    Public Function GetCustomerList() As String

        ' DataReader that will hold the returned results		
        ' Create the Database object, using the default database service. 

        Dim sqlCommand As String = "Select CustomerID, Name, Address, City, Country, PostalCode " & _
            "From Customers"
        Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)

        Dim readerData As StringBuilder = New StringBuilder

        ' The ExecuteReader call will request the connection to be closed upon
        ' the closing of the DataReader. The DataReader will be closed 
        ' automatically when it is disposed.
        Using dataReader As IDataReader = db.ExecuteReader(dbCommand)

            ' Iterate through DataReader and put results to the text box.
            ' DataReaders cannot be bound to Windows Form controls (e.g. the
            ' resultsDataGrid), but may be bound to Web Form controls.
            While (dataReader.Read())
                ' Get the value of the 'Name' column in the DataReader
                readerData.Append(dataReader("Name"))
                readerData.Append(Environment.NewLine)
            End While
        End Using
        Return readerData.ToString()
    End Function

    ' Retreives all products in the specified category.
    ' Category: The category containing the products.
    ' Returns: DataSet containing the products.
    ' Remarks: Demonstrates retrieving multiple rows using a DataSet.
    Public Function GetProductsInCategory(ByRef Category As Integer) As DataSet

        ' Create the Database object, using the default database service. The
        ' default database service is determined through configuration.

        Dim sqlCommand As String = "GetProductsByCategory"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        ' Retrieve products from the specified category.
        db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, Category)

        ' DataSet that will hold the returned results		
        Dim productsDataSet As DataSet = Nothing

        productsDataSet = db.ExecuteDataSet(dbCommand)

        ' Note: connection was closed by ExecuteDataSet method call 

        Return productsDataSet
    End Function

    ' Updates the product database.
    ' Returns: The number of rows affected by the update.
    ' Remarks: Demonstrates updating a database using a DataSet.
    Public Function UpdateProducts() As Integer
        ' Create the Database object, using the default database service. The
        ' default database service is determined through configuration.

        Dim productsDataSet As DataSet = New DataSet

        Dim sqlCommand As String = "Select ProductID, ProductName, CategoryID, UnitPrice, LastUpdate " & _
            "From Products"
        Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)

        Dim productsTable As String = "Products"

        ' Retrieve the initial data
        db.LoadDataSet(dbCommand, productsDataSet, productsTable)

        ' Get the table that will be modified
        Dim table As DataTable = productsDataSet.Tables(productsTable)

        ' Add a new product to existing DataSet
        Dim addedRow As DataRow = table.Rows.Add(New Object() {DBNull.Value, "New product", 11, 25})

        ' Modify an existing product
        table.Rows(0)("ProductName") = "Modified product"

        ' Establish our Insert, Delete, and Update commands
        Dim insertCommand As DbCommand = db.GetStoredProcCommand("AddProduct")
        db.AddInParameter(insertCommand, "ProductName", DbType.String, "ProductName", DataRowVersion.Current)
        db.AddInParameter(insertCommand, "CategoryID", DbType.Int32, "CategoryID", DataRowVersion.Current)
        db.AddInParameter(insertCommand, "UnitPrice", DbType.Currency, "UnitPrice", DataRowVersion.Current)

        Dim deleteCommand As DbCommand = db.GetStoredProcCommand("DeleteProduct")
        db.AddInParameter(deleteCommand, "ProductID", DbType.Int32, "ProductID", DataRowVersion.Current)

        Dim updateCommand As DbCommand = db.GetStoredProcCommand("UpdateProduct")
        db.AddInParameter(updateCommand, "ProductID", DbType.Int32, "ProductID", DataRowVersion.Current)
        db.AddInParameter(updateCommand, "ProductName", DbType.String, "ProductName", DataRowVersion.Current)
        db.AddInParameter(updateCommand, "LastUpdate", DbType.DateTime, "LastUpdate", DataRowVersion.Current)

        ' Submit the DataSet, capturing the number of rows that were affected
        Dim rowsAffected As Integer = db.UpdateDataSet(productsDataSet, "Products", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard)

        Return rowsAffected

    End Function

    ' Retrieves details about the specified product.
    ' productID: The ID of the product used to retrieve details.
    ' Returns: The product details as a string.
    ' Remarks: Demonstrates retrieving a single row of data using output parameters.
    Public Function GetProductDetails(ByRef productID As Integer) As String

        ' Create the Database object, using the default database service. The
        ' default database service is determined through configuration.

        Dim sqlCommand As String = "GetProductDetails"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        ' Add paramters
        ' Input parameters can specify the input value
        db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productID)
        ' Output parameters specify the size of the return data
        db.AddOutParameter(dbCommand, "ProductName", DbType.String, 50)
        db.AddOutParameter(dbCommand, "UnitPrice", DbType.Currency, 8)

        db.ExecuteNonQuery(dbCommand)

        ' Row of data is captured via output parameters
        Dim results As String = String.Format(CultureInfo.CurrentCulture, "{0}, {1}, {2:C} ", _
                                       db.GetParameterValue(dbCommand, "ProductID"), _
                                       db.GetParameterValue(dbCommand, "ProductName"), _
                                       db.GetParameterValue(dbCommand, "UnitPrice"))

        Return results
    End Function

    ' Retrieves the specified product's name.
    ' productID: The ID of the product.
    ' Returns: The name of the product.
    ' Remarks: Demonstrates retrieving a single item. Parameter discovery
    ' is used for determining the properties of the productID parameter.
    Public Function GetProductName(ByRef productID As Integer) As String

        ' Create the Database object, using the default database service. The
        ' default database service is determined through configuration.

        ' Passing the productID value to the GetStoredProcCommand
        ' results in parameter discovery being used to correctly establish the parameter
        ' information for the productID. Subsequent calls to this method will
        ' cause the block to retrieve the parameter information from the 
        ' cache, and not require rediscovery.
        Dim sqlCommand As String = "GetProductName"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand, productID)

        ' Retrieve ProductName. ExecuteScalar returns an object, so
        ' we cast to the correct type (string).
        Dim productName As String = Convert.ToString(db.ExecuteScalar(dbCommand))

        Return productName
    End Function

    ' Retrieves a list of products.
    ' Returns: A list of products as an XML string.
    ' Remarks: Demonstrates retrieving multiple rows of data as XML. This
    ' method is not portable across database providers, but is 
    ' specific to the SqlDatabase.
    Public Function GetProductList() As String

        ' Use a named database instance that refers to a SQL Server database.
        Dim dbSQL As SqlDatabase = DirectCast(db, SqlDatabase)

        ' Use "FOR XML AUTO" to have SQL return XML data
        Dim sqlCommand As String = "Select ProductID, ProductName, CategoryID, UnitPrice, LastUpdate " & _
            "From Products FOR XML AUTO"

        Dim dbCommand As DbCommand = dbSQL.GetSqlStringCommand(sqlCommand)

        Dim productsReader As XmlReader = Nothing
        Dim productList As StringBuilder = New StringBuilder

        Try
            productsReader = dbSQL.ExecuteXmlReader(dbCommand)

            ' Iterate through the XmlReader and put the data into our results.
            While (Not productsReader.EOF)

                If (productsReader.IsStartElement()) Then
                    productList.Append(productsReader.ReadOuterXml())
                    productList.Append(Environment.NewLine)
                End If
            End While
        Finally
            ' Close the Reader.
            If (Not productsReader Is Nothing) Then
                productsReader.Close()
            End If

            ' Explicitly close the connection. The connection is not closed
            ' when the XmlReader is closed.
            If (Not dbCommand.Connection Is Nothing) Then
                dbCommand.Connection.Close()
            End If
        End Try
        Return productList.ToString()
    End Function

    ' Transfers an amount between two accounts.
    ' transactionAmount: Amount to transfer.
    ' sourceAccount: Account to be credited.
    ' destinationAccount: Account to be debited.
    ' Returns: true if sucessful otherwise false.
    ' Remarks: Demonstrates executing multiple updates within the 
    ' context of a transaction.
    Public Function Transfer(ByRef transactionAmount As Integer, ByRef sourceAccount As Integer, ByRef destinationAccount As Integer) As Boolean

        Dim result As Boolean = False

        ' Create the Database object, using the default database service. The
        ' default database service is determined through configuration.

        ' Two operations, one to credit an account, and one to debit another
        ' account.
        Dim sqlCommand As String = "CreditAccount"
        Dim creditCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(creditCommand, "AccountID", DbType.Int32, sourceAccount)
        db.AddInParameter(creditCommand, "Amount", DbType.Int32, transactionAmount)

        sqlCommand = "DebitAccount"
        Dim debitCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(debitCommand, "AccountID", DbType.Int32, destinationAccount)
        db.AddInParameter(debitCommand, "Amount", DbType.Int32, transactionAmount)

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            Dim transaction As DbTransaction = connection.BeginTransaction()

            Try

                ' Credit the first account
                db.ExecuteNonQuery(creditCommand, transaction)
                ' Debit the second account
                db.ExecuteNonQuery(debitCommand, transaction)
                ' Commit the transaction
                transaction.Commit()

                result = True
            Catch
                ' Rollback transaction 
                transaction.Rollback()
            End Try

            connection.Close()
            Return result
        End Using
    End Function
End Class
