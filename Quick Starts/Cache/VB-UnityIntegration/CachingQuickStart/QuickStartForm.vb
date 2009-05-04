'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Caching Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


Public Class QuickStartForm
    Inherits System.Windows.Forms.Form

    Private viewerProcess As Process = Nothing
    Private masterFileViewerProcess As Process = Nothing

    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic CachingQS1"
    Private Const MasterDataViewerExecutable As String = "notepad.exe"
    ' Forms
    Private enterNewItemForm As EnterNewItemForm
    Private selectItemForm As SelectItemForm
    Private selectMasterDataItemForm As SelectMasterDataItemForm

    Private primitivesCache As ICacheManager
    Private productData As ProductData

    Public Shared AppForm As System.Windows.Forms.Form


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
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents primitivesRemoveButton As System.Windows.Forms.Button
    Friend WithEvents primitivesResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents primitivesFlushCacheButton As System.Windows.Forms.Button
    Friend WithEvents primitivesReadButton As System.Windows.Forms.Button
    Friend WithEvents primitivesAddButton As System.Windows.Forms.Button
    Friend WithEvents tabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents viewFileButton As System.Windows.Forms.Button
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents proactiveButton As System.Windows.Forms.Button
    Friend WithEvents flushCacheButton As System.Windows.Forms.Button
    Friend WithEvents reactiveButton As System.Windows.Forms.Button
    Friend WithEvents loadingResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.tabPage1 = New System.Windows.Forms.TabPage
        Me.primitivesRemoveButton = New System.Windows.Forms.Button
        Me.primitivesResultsTextBox = New System.Windows.Forms.TextBox
        Me.primitivesFlushCacheButton = New System.Windows.Forms.Button
        Me.primitivesReadButton = New System.Windows.Forms.Button
        Me.primitivesAddButton = New System.Windows.Forms.Button
        Me.tabPage2 = New System.Windows.Forms.TabPage
        Me.viewFileButton = New System.Windows.Forms.Button
        Me.label4 = New System.Windows.Forms.Label
        Me.proactiveButton = New System.Windows.Forms.Button
        Me.flushCacheButton = New System.Windows.Forms.Button
        Me.reactiveButton = New System.Windows.Forms.Button
        Me.loadingResultsTextBox = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.tabControl1.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.groupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.ItemSize = New System.Drawing.Size(98, 24)
        Me.tabControl1.Location = New System.Drawing.Point(8, 64)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(584, 368)
        Me.tabControl1.TabIndex = 43
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.primitivesRemoveButton)
        Me.tabPage1.Controls.Add(Me.primitivesResultsTextBox)
        Me.tabPage1.Controls.Add(Me.primitivesFlushCacheButton)
        Me.tabPage1.Controls.Add(Me.primitivesReadButton)
        Me.tabPage1.Controls.Add(Me.primitivesAddButton)
        Me.tabPage1.Location = New System.Drawing.Point(4, 28)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Size = New System.Drawing.Size(576, 336)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "Caching Operations"
        '
        'primitivesRemoveButton
        '
        Me.primitivesRemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.primitivesRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.primitivesRemoveButton.Location = New System.Drawing.Point(16, 132)
        Me.primitivesRemoveButton.Name = "primitivesRemoveButton"
        Me.primitivesRemoveButton.Size = New System.Drawing.Size(128, 40)
        Me.primitivesRemoveButton.TabIndex = 2
        Me.primitivesRemoveButton.Text = "Remove item from the cache"
        '
        'primitivesResultsTextBox
        '
        Me.primitivesResultsTextBox.Location = New System.Drawing.Point(160, 21)
        Me.primitivesResultsTextBox.Multiline = True
        Me.primitivesResultsTextBox.Name = "primitivesResultsTextBox"
        Me.primitivesResultsTextBox.ReadOnly = True
        Me.primitivesResultsTextBox.Size = New System.Drawing.Size(400, 305)
        Me.primitivesResultsTextBox.TabIndex = 50
        Me.primitivesResultsTextBox.TabStop = False
        '
        'primitivesFlushCacheButton
        '
        Me.primitivesFlushCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.primitivesFlushCacheButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.primitivesFlushCacheButton.Location = New System.Drawing.Point(16, 180)
        Me.primitivesFlushCacheButton.Name = "primitivesFlushCacheButton"
        Me.primitivesFlushCacheButton.Size = New System.Drawing.Size(128, 41)
        Me.primitivesFlushCacheButton.TabIndex = 3
        Me.primitivesFlushCacheButton.Text = "Flush the cache"
        '
        'primitivesReadButton
        '
        Me.primitivesReadButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.primitivesReadButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.primitivesReadButton.Location = New System.Drawing.Point(16, 83)
        Me.primitivesReadButton.Name = "primitivesReadButton"
        Me.primitivesReadButton.Size = New System.Drawing.Size(128, 41)
        Me.primitivesReadButton.TabIndex = 1
        Me.primitivesReadButton.Text = "Read item from cache"
        '
        'primitivesAddButton
        '
        Me.primitivesAddButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.primitivesAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.primitivesAddButton.Location = New System.Drawing.Point(16, 35)
        Me.primitivesAddButton.Name = "primitivesAddButton"
        Me.primitivesAddButton.Size = New System.Drawing.Size(128, 40)
        Me.primitivesAddButton.TabIndex = 0
        Me.primitivesAddButton.Text = "Add item to cache"
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.viewFileButton)
        Me.tabPage2.Controls.Add(Me.label4)
        Me.tabPage2.Controls.Add(Me.proactiveButton)
        Me.tabPage2.Controls.Add(Me.flushCacheButton)
        Me.tabPage2.Controls.Add(Me.reactiveButton)
        Me.tabPage2.Controls.Add(Me.loadingResultsTextBox)
        Me.tabPage2.Controls.Add(Me.label1)
        Me.tabPage2.Location = New System.Drawing.Point(4, 28)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Size = New System.Drawing.Size(576, 336)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "Loading the Cache"
        Me.tabPage2.Visible = False
        '
        'viewFileButton
        '
        Me.viewFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.viewFileButton.Location = New System.Drawing.Point(427, 291)
        Me.viewFileButton.Name = "viewFileButton"
        Me.viewFileButton.Size = New System.Drawing.Size(128, 40)
        Me.viewFileButton.TabIndex = 3
        Me.viewFileButton.Text = "Edit master data"
        '
        'label4
        '
        Me.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label4.Location = New System.Drawing.Point(8, 49)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(560, 27)
        Me.label4.TabIndex = 48
        Me.label4.Text = "Reactive caching retrieves data from the master source when the application reque" & _
            "sts it,  and retains it in the cache for future requests."
        '
        'proactiveButton
        '
        Me.proactiveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.proactiveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.proactiveButton.Location = New System.Drawing.Point(20, 96)
        Me.proactiveButton.Name = "proactiveButton"
        Me.proactiveButton.Size = New System.Drawing.Size(128, 40)
        Me.proactiveButton.TabIndex = 0
        Me.proactiveButton.Text = "Proactively load cache"
        '
        'flushCacheButton
        '
        Me.flushCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.flushCacheButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.flushCacheButton.Location = New System.Drawing.Point(20, 192)
        Me.flushCacheButton.Name = "flushCacheButton"
        Me.flushCacheButton.Size = New System.Drawing.Size(128, 40)
        Me.flushCacheButton.TabIndex = 2
        Me.flushCacheButton.Text = "Flush Cache"
        '
        'reactiveButton
        '
        Me.reactiveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.reactiveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.reactiveButton.Location = New System.Drawing.Point(20, 144)
        Me.reactiveButton.Name = "reactiveButton"
        Me.reactiveButton.Size = New System.Drawing.Size(128, 40)
        Me.reactiveButton.TabIndex = 1
        Me.reactiveButton.Text = "Reactively load data"
        '
        'loadingResultsTextBox
        '
        Me.loadingResultsTextBox.Location = New System.Drawing.Point(160, 90)
        Me.loadingResultsTextBox.Multiline = True
        Me.loadingResultsTextBox.Name = "loadingResultsTextBox"
        Me.loadingResultsTextBox.ReadOnly = True
        Me.loadingResultsTextBox.Size = New System.Drawing.Size(400, 187)
        Me.loadingResultsTextBox.TabIndex = 50
        Me.loadingResultsTextBox.TabStop = False
        '
        'label1
        '
        Me.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label1.Location = New System.Drawing.Point(8, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(560, 33)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Proactive caching retrieves all of the required state from the master source, usu" & _
            "ally when the application starts, and retains it in the cache for the lifetime o" & _
            "f that application."
        '
        'groupBox
        '
        Me.groupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        Me.groupBox.Location = New System.Drawing.Point(-8, 433)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(640, 128)
        Me.groupBox.TabIndex = 41
        Me.groupBox.TabStop = False
        '
        'viewWalkthroughButton
        '
        Me.viewWalkthroughButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.viewWalkthroughButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewWalkthroughButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.viewWalkthroughButton.Location = New System.Drawing.Point(361, 24)
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        Me.viewWalkthroughButton.Size = New System.Drawing.Size(114, 32)
        Me.viewWalkthroughButton.TabIndex = 0
        Me.viewWalkthroughButton.Text = "View &Walkthrough"
        '
        'quitButton
        '
        Me.quitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.quitButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.quitButton.Location = New System.Drawing.Point(488, 24)
        Me.quitButton.Name = "quitButton"
        Me.quitButton.Size = New System.Drawing.Size(113, 32)
        Me.quitButton.TabIndex = 1
        Me.quitButton.Text = "Quit"
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        Me.groupBox1.Location = New System.Drawing.Point(-8, -8)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(704, 72)
        Me.groupBox1.TabIndex = 42
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)
        Me.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label2.Location = New System.Drawing.Point(16, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(296, 31)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Caching"
        '
        'logoPictureBox
        '
        Me.logoPictureBox.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.logoPictureBox.Location = New System.Drawing.Point(520, 16)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(69, 50)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.logoPictureBox.TabIndex = 0
        Me.logoPictureBox.TabStop = False
        '
        'QuickStartForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(605, 500)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.Text = "Caching Application Block Quick Start"
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage1.PerformLayout()
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage2.PerformLayout()
        Me.groupBox.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New(ByVal cacheManager As ICacheManager, ByVal productData As ProductData)
        Me.New()
        primitivesCache = cacheManager
        Me.productData = productData
    End Sub


    ' Displays dialog with information about exceptions that occur in the application. 
    Private Shared Sub AppThreadException(ByVal source As Object, ByVal e As ThreadExceptionEventArgs)
        Dim errorMsg As String = String.Format(New CultureInfo("en-us", True), "There are some problems while trying to use the Caching Quick Start, please check the following error messages: " & Environment.NewLine & "{0}" & Environment.NewLine, e.Exception.Message)
        errorMsg = errorMsg & Environment.NewLine

        Dim result As DialogResult = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)

        ' Exits the program when the user clicks Abort.
        If (result = System.Windows.Forms.DialogResult.Abort) Then
            Application.Exit()
        End If
        QuickStartForm.AppForm.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub QuickStartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initialize image to embedded logo
        Me.logoPictureBox.Image = GetEmbeddedImage("logo.gif")

        ' Initialize primitive operations forms
        Me.enterNewItemForm = New EnterNewItemForm
        Me.selectItemForm = New SelectItemForm
        Me.selectMasterDataItemForm = New SelectMasterDataItemForm
    End Sub

    Private Function GetEmbeddedImage(ByVal resourceName As String) As Image
        Dim resourceStream As Stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If (resourceStream Is Nothing) Then
            Return Nothing
        End If

        Dim img As Image = Image.FromStream(resourceStream)

        Return img
    End Function

    Private Sub viewWalkthroughButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewWalkthroughButton.Click
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

    Private Sub quitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitButton.Click
        Application.Exit()
    End Sub

    Private Sub primitivesAddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles primitivesAddButton.Click
        ' Prompt the user to enter information about the product,
        ' as well as properties to use when adding it to the cache
        If (Me.enterNewItemForm.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Dim newProduct As Product = New Product( _
                            Me.enterNewItemForm.ProductID, _
                            Me.enterNewItemForm.ProductShortName, _
                            Me.enterNewItemForm.ProductPrice)

            ' Add the product to the cache, using the expiration and
            ' priority settings according to what the user entered.
            Select Case Me.enterNewItemForm.Expiration

                Case ExpirationType.AbsoluteTime
                    primitivesCache.Add(newProduct.ProductID, newProduct, enterNewItemForm.Priority, New ProductCacheRefreshAction, _
                        New AbsoluteTime(Me.enterNewItemForm.AbsoluteTime))
                    Exit Select
                Case ExpirationType.ExtendedFormat
                    primitivesCache.Add(newProduct.ProductID, newProduct, enterNewItemForm.Priority, New ProductCacheRefreshAction, _
                        New ExtendedFormatTime("0 0 * * *"))
                    Exit Select
                Case ExpirationType.FileDependency
                    primitivesCache.Add(newProduct.ProductID, newProduct, enterNewItemForm.Priority, New ProductCacheRefreshAction, _
                        New FileDependency("DependencyFile.txt"))
                    Exit Select
                Case ExpirationType.SlidingTime
                    primitivesCache.Add(newProduct.ProductID, newProduct, enterNewItemForm.Priority, New ProductCacheRefreshAction, _
                    New SlidingTime(TimeSpan.FromMinutes(1)))
            End Select

            ' Update the results text box to display information about the item just added
            Me.primitivesResultsTextBox.Text &= _
                String.Format(My.Resources.Culture, My.Resources.AddItemToCacheMessage, newProduct.ProductID, newProduct.ProductName, newProduct.ProductPrice, _
                Me.enterNewItemForm.Expiration.ToString(), _
                Me.enterNewItemForm.Priority.ToString()) & Environment.NewLine

            AddScenarioSeparator(Me.primitivesResultsTextBox)
        End If
    End Sub

    Private Sub primitivesReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles primitivesReadButton.Click
        Me.selectItemForm.ClearInputTextBox()
        Me.selectItemForm.Text = My.Resources.ReadItemTitleMessage
        Me.selectItemForm.SetInstructionLabelText(My.Resources.ReadItemMessage)

        If (Me.selectItemForm.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Dim requestedProduct As Product = DirectCast(Me.primitivesCache.GetData(selectItemForm.ItemKey), Product)
            If (Not requestedProduct Is Nothing) Then
                Me.primitivesResultsTextBox.Text &= String.Format(My.Resources.Culture, My.Resources.ReadItemFromCacheMessage, requestedProduct.ProductID, requestedProduct.ProductName, requestedProduct.ProductPrice) & Environment.NewLine
            Else
                Me.primitivesResultsTextBox.Text &= String.Format(My.Resources.Culture, My.Resources.ItemNotFoundMessage, selectItemForm.ItemKey) & Environment.NewLine
            End If
            AddScenarioSeparator(Me.primitivesResultsTextBox)
        End If
    End Sub

    Private Sub primitivesRemoveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles primitivesRemoveButton.Click
        Me.selectItemForm.ClearInputTextBox()
        Me.selectItemForm.ClearInputTextBox()
        Me.selectItemForm.Text = My.Resources.RemoveItemTitleMessage
        Me.selectItemForm.SetInstructionLabelText(My.Resources.RemoveItemMessage)

        If (Me.selectItemForm.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Me.primitivesCache.Remove(selectItemForm.ItemKey)

            Me.primitivesResultsTextBox.Text &= My.Resources.RemoveItemFromCacheMessage & Environment.NewLine

            AddScenarioSeparator(Me.primitivesResultsTextBox)
        End If
    End Sub

    Private Sub primitivesFlushCacheButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles primitivesFlushCacheButton.Click
        Me.primitivesCache.Flush()
        Me.primitivesResultsTextBox.Clear()
        Me.primitivesResultsTextBox.Text &= My.Resources.FlushCacheMessage & Environment.NewLine
        AddScenarioSeparator(Me.primitivesResultsTextBox)
    End Sub

    ' Reads an item and updates the results display.
    ' This method should be used for both proactive and reactive readings of products.
    Private Function ReadSingleProduct(ByVal key As String) As Boolean
        Dim requestedProduct As Product = Me.productData.ReadProductByID(key)
        Me.loadingResultsTextBox.Text &= Me.productData.dataSourceMessage
        Return (Not requestedProduct Is Nothing)
    End Function

    Private Sub proactiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles proactiveButton.Click
        Me.productData.LoadAllProducts()

        Me.loadingResultsTextBox.Text &= My.Resources.ProactiveLoadMessage & Environment.NewLine
        AddScenarioSeparator(Me.loadingResultsTextBox)
    End Sub

    Private Sub reactiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reactiveButton.Click
        ' Prompt the user to see which product should be retrieved
        If (Me.selectMasterDataItemForm.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Dim requestedProduct As Product = Me.productData.ReadProductByID(Me.selectMasterDataItemForm.ItemToRead)

            Me.loadingResultsTextBox.Text &= Me.productData.dataSourceMessage
            AddScenarioSeparator(Me.loadingResultsTextBox)
        End If
    End Sub

    Private Sub flushCacheButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flushCacheButton.Click
        Me.productData.FlushCache()
        Me.loadingResultsTextBox.Clear()
        Me.loadingResultsTextBox.Text &= My.Resources.FlushCacheMessage & Environment.NewLine
        AddScenarioSeparator(Me.loadingResultsTextBox)
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

    Private Sub viewFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewFileButton.Click
        ' Process has never been started. Initialize and launch notepad.
        If (Me.masterFileViewerProcess Is Nothing) Then
            ' Initialize the Process information for notepad
            Me.masterFileViewerProcess = New Process

            Me.masterFileViewerProcess.StartInfo.FileName = MasterDataViewerExecutable
            Me.masterFileViewerProcess.StartInfo.Arguments = DataProvider.dataFileName
            Me.masterFileViewerProcess.Start()
        ElseIf (Me.masterFileViewerProcess.HasExited) Then
            ' Process previously started, then exited. Start the process again.
            Me.masterFileViewerProcess.Start()
        Else
            ' Process was already started - bring it to the foreground
            Dim hWnd As IntPtr = Me.masterFileViewerProcess.MainWindowHandle
            If (NativeMethods.IsIconic(hWnd)) Then
                NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE)
            End If

            NativeMethods.SetForegroundWindow(hWnd)
        End If
    End Sub

    Private Sub AddScenarioSeparator(ByVal textBox As TextBox)
        textBox.Text &= "----------------------------------------------------------" & Environment.NewLine
        textBox.SelectAll()
        textBox.ScrollToCaret()
    End Sub

    Public Function GetProduct(ByVal cache As CacheManager, ByVal key As String) As Product
        Return DirectCast(Cache.GetData(key), Product)
    End Function
End Class


