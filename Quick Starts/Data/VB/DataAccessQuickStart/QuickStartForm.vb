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

Public Class QuickStartForm
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button
    Friend WithEvents retrieveUsingXmlReaderButton As System.Windows.Forms.Button
    Friend WithEvents useCaseLabel As System.Windows.Forms.Label
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents retrieveUsingDatasetButton As System.Windows.Forms.Button
    Friend WithEvents updateUsingDataSetButton As System.Windows.Forms.Button
    Friend WithEvents transactionalUpdateButton As System.Windows.Forms.Button
    Friend WithEvents singleItemButton As System.Windows.Forms.Button
    Friend WithEvents retrieveUsingReaderButton As System.Windows.Forms.Button
    Friend WithEvents singleRowButton As System.Windows.Forms.Button
    Friend WithEvents resultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid

    Private viewerProcess As Process = Nothing
    Private salesData As SalesData = New SalesData
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic DataAccessQS1"

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QuickStartForm))
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.retrieveUsingXmlReaderButton = New System.Windows.Forms.Button
        Me.useCaseLabel = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.retrieveUsingDatasetButton = New System.Windows.Forms.Button
        Me.updateUsingDataSetButton = New System.Windows.Forms.Button
        Me.transactionalUpdateButton = New System.Windows.Forms.Button
        Me.singleItemButton = New System.Windows.Forms.Button
        Me.retrieveUsingReaderButton = New System.Windows.Forms.Button
        Me.singleRowButton = New System.Windows.Forms.Button
        Me.resultsTextBox = New System.Windows.Forms.TextBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        resources.ApplyResources(Me.groupBox1, "groupBox1")
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        resources.ApplyResources(Me.label2, "label2")
        Me.label2.Name = "label2"
        '
        'logoPictureBox
        '
        resources.ApplyResources(Me.logoPictureBox, "logoPictureBox")
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.TabStop = False
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        resources.ApplyResources(Me.groupBox, "groupBox")
        Me.groupBox.Name = "groupBox"
        Me.groupBox.TabStop = False
        '
        'viewWalkthroughButton
        '
        resources.ApplyResources(Me.viewWalkthroughButton, "viewWalkthroughButton")
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        '
        'quitButton
        '
        resources.ApplyResources(Me.quitButton, "quitButton")
        Me.quitButton.Name = "quitButton"
        '
        'retrieveUsingXmlReaderButton
        '
        resources.ApplyResources(Me.retrieveUsingXmlReaderButton, "retrieveUsingXmlReaderButton")
        Me.retrieveUsingXmlReaderButton.Name = "retrieveUsingXmlReaderButton"
        '
        'useCaseLabel
        '
        resources.ApplyResources(Me.useCaseLabel, "useCaseLabel")
        Me.useCaseLabel.Name = "useCaseLabel"
        '
        'label4
        '
        resources.ApplyResources(Me.label4, "label4")
        Me.label4.Name = "label4"
        '
        'retrieveUsingDatasetButton
        '
        resources.ApplyResources(Me.retrieveUsingDatasetButton, "retrieveUsingDatasetButton")
        Me.retrieveUsingDatasetButton.Name = "retrieveUsingDatasetButton"
        '
        'updateUsingDataSetButton
        '
        resources.ApplyResources(Me.updateUsingDataSetButton, "updateUsingDataSetButton")
        Me.updateUsingDataSetButton.Name = "updateUsingDataSetButton"
        '
        'transactionalUpdateButton
        '
        resources.ApplyResources(Me.transactionalUpdateButton, "transactionalUpdateButton")
        Me.transactionalUpdateButton.Name = "transactionalUpdateButton"
        '
        'singleItemButton
        '
        resources.ApplyResources(Me.singleItemButton, "singleItemButton")
        Me.singleItemButton.Name = "singleItemButton"
        '
        'retrieveUsingReaderButton
        '
        resources.ApplyResources(Me.retrieveUsingReaderButton, "retrieveUsingReaderButton")
        Me.retrieveUsingReaderButton.Name = "retrieveUsingReaderButton"
        '
        'singleRowButton
        '
        resources.ApplyResources(Me.singleRowButton, "singleRowButton")
        Me.singleRowButton.Name = "singleRowButton"
        '
        'resultsTextBox
        '
        resources.ApplyResources(Me.resultsTextBox, "resultsTextBox")
        Me.resultsTextBox.Name = "resultsTextBox"
        Me.resultsTextBox.ReadOnly = True
        Me.resultsTextBox.TabStop = False
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.resultsDataGrid, "resultsDataGrid")
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.TabStop = False
        '
        'QuickStartForm
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.retrieveUsingXmlReaderButton)
        Me.Controls.Add(Me.useCaseLabel)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.retrieveUsingDatasetButton)
        Me.Controls.Add(Me.updateUsingDataSetButton)
        Me.Controls.Add(Me.transactionalUpdateButton)
        Me.Controls.Add(Me.singleItemButton)
        Me.Controls.Add(Me.retrieveUsingReaderButton)
        Me.Controls.Add(Me.singleRowButton)
        Me.Controls.Add(Me.resultsTextBox)
        Me.Controls.Add(Me.resultsDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private Sub QuickStartForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Initialize image on the form to the embedded logo
        Me.logoPictureBox.Image = Me.GetEmbeddedImage("logo.gif")
    End Sub

    ' Displays dialog with information about exceptions that occur in the application. 
    Private Sub DisplayErrorDialog(ByVal ex As Exception)
        Dim errorMsg As String = String.Format(My.Resources.Culture, My.Resources.GeneralExceptionMessage, ex.Message)

        errorMsg += Environment.NewLine + My.Resources.DbRequirementsMessage

        Dim result As DialogResult = MessageBox.Show(errorMsg, My.Resources.ApplicationErrorMessage, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)

        ' Exits the program when the user clicks Abort.
        If (result = System.Windows.Forms.DialogResult.Abort) Then
            Application.Exit()
        End If
        Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    ' Retrieves the specified embedded image resource.
    Private Function GetEmbeddedImage(ByVal resourceName As String) As Image

        Dim resourceStream As Stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)
        If (resourceStream Is Nothing) Then
            Return Nothing
        End If
        Dim img As Image = Image.FromStream(resourceStream)
        Return img
    End Function

    ' Updates the results textbox on the form with the information for a use case.
    Private Sub DisplayResults(ByVal useCase As String, ByVal results As String)
        Me.useCaseLabel.Text = useCase
        Me.resultsTextBox.Text = results
        Me.resultsDataGrid.Hide()
        Me.resultsTextBox.Show()
    End Sub

    ' Displays the grid showing the results of a use case.
    Private Sub DisplayResults(ByVal useCase As String)
        Me.useCaseLabel.Text = useCase
        Me.resultsDataGrid.Show()
        Me.resultsTextBox.Hide()
    End Sub

    ' Demonstrates how to retrieve multiple rows of data using
    ' a DataReader.
    Private Sub retrieveUsingReaderButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles retrieveUsingReaderButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim customerList As String = salesData.GetCustomerList()

            Me.DisplayResults(Me.retrieveUsingReaderButton.Text, customerList)
        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to retrieve multiple rows of data using
    ' a DataSet.
    Private Sub retrieveUsingDatasetButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles retrieveUsingDatasetButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim customerDataSet As DataSet = salesData.GetProductsInCategory(2)

            ' Bind the DataSet to the DataGrid for display. 
            ' The Data Access Application Block generates DataSet objects with 
            ' default names for the contained DataTable objects, for example, Table, 
            ' Table1, Table2, etc. 
            Me.resultsDataGrid.SetDataBinding(customerDataSet, "Table")

            Me.DisplayResults(Me.retrieveUsingDatasetButton.Text)

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to update the database by submitting a 
    ' modified DataSet.
    Private Sub updateUsingDataSetButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles updateUsingDataSetButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim rowsAffected As Integer = salesData.UpdateProducts()

            Me.DisplayResults(Me.updateUsingDataSetButton.Text, String.Format(My.Resources.Culture, My.Resources.AffectedRowsMessage, rowsAffected))

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to retrieve a single row of data.
    Private Sub singleRowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles singleRowButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim productDetails As String = salesData.GetProductDetails(5)

            Me.DisplayResults(Me.singleRowButton.Text, productDetails)

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to retrieve a single data item from the database.
    Private Sub singleItemButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles singleItemButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim productName As String = salesData.GetProductName(4)

            Me.DisplayResults(Me.singleItemButton.Text, productName)

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to update the database multiple times in the
    ' context of a transaction. All updates will succeed or all will be 
    ' rolled back.
    Private Sub transactionalUpdateButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles transactionalUpdateButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim results As String = ""

            Dim amount As Integer = 500
            Dim sourceAccount As Integer = 1200
            Dim destinationAccount As Integer = 2235

            If (salesData.Transfer(amount, sourceAccount, destinationAccount)) Then
                results = My.Resources.TransferCompletedMessage
            Else
                results = My.Resources.TransferFailedMessage
            End If

            Me.DisplayResults(Me.transactionalUpdateButton.Text, results)

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Demonstrates how to retrieve XML data from a SQL Server database.
    Private Sub retrieveUsingXmlReaderButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles retrieveUsingXmlReaderButton.Click
        Try
            Cursor = Cursors.WaitCursor

            Dim productList As String = salesData.GetProductList()

            DisplayResults(Me.retrieveUsingXmlReaderButton.Text, productList)

        Catch ex As Exception
            DisplayErrorDialog(ex)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    ' Quits the application.
    Private Sub quitButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles quitButton.Click
        Me.Close()
    End Sub
    Private Function GetHelpViewerExecutable() As String
        Dim commonX86 As String = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)")
        If Not String.IsNullOrEmpty(commonX86) Then
            Dim pathX86 As String = Path.Combine(commonX86, "Microsoft Shared\Help 9\dexplore.exe")
            If File.Exists(pathX86) Then
                Return pathX86
            End If
        End If
        Dim common As String = Environment.GetEnvironmentVariable("CommonProgramFiles")
        Return Path.Combine(common, "Microsoft Shared\Help 9\dexplore.exe")
    End Function
    ' Displays Quick Start help topics using the Help 2 Viewer.
    Private Sub viewWalkthroughButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles viewWalkthroughButton.Click

        ' Process has never been started. Initialize and launch the viewer.
        If (Me.viewerProcess Is Nothing) Then

            ' Initialize the Process information for the help viewer
            Me.viewerProcess = New Process

            Me.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable()
            Me.viewerProcess.StartInfo.Arguments = HelpViewerArguments
            Me.viewerProcess.Start()

        ElseIf (Me.viewerProcess.HasExited) Then

            ' Process previously started, then exited. Start the process again.
            Me.viewerProcess.Start()
        Else
            ' Process was already started - bring it to the foreground
            Dim hWnd As IntPtr = Me.viewerProcess.MainWindowHandle
            If (NativeMethods.IsIconic(hWnd)) Then
                NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE)
            End If
            NativeMethods.SetForegroundWindow(hWnd)
        End If
    End Sub

End Class
