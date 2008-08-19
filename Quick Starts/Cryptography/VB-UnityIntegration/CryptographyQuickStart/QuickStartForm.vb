'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Cryptography Application Block QuickStart
'===============================================================================
' Copyright ? Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


Public Class QuickStartForm
    Inherits System.Windows.Forms.Form

    Private viewerProcess As Process = Nothing
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic CryptographyQS1"

    ' The following strings correspond to the names of the providers as 
    ' specified by the configuration file.
    Private Const hashProvider As String = "hashprovider"
	Private Const symmProvider As String = "symprovider"

	Private Const symmKeyFileName As String = "SymmetricKeyFile.txt"

    ' Encrypted string
    Private stringToEncrypt As String
    Private encryptedContentsBase64 As String

    ' Generated hash
    Private stringForHash As String
    Private generatedHash As Byte()

    Private cryptographyManager As CryptographyManager

    Private inputFormObject As InputForm = New InputForm

    Public Shared AppForm As System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents checkTextButton As System.Windows.Forms.Button
    Friend WithEvents getHashButton As System.Windows.Forms.Button
    Friend WithEvents decryptButton As System.Windows.Forms.Button
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents resultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents encryptButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QuickStartForm))
        Me.checkTextButton = New System.Windows.Forms.Button
        Me.getHashButton = New System.Windows.Forms.Button
        Me.decryptButton = New System.Windows.Forms.Button
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.resultsTextBox = New System.Windows.Forms.TextBox
        Me.encryptButton = New System.Windows.Forms.Button
        Me.groupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'checkTextButton
        '
        resources.ApplyResources(Me.checkTextButton, "checkTextButton")
        Me.checkTextButton.Name = "checkTextButton"
        '
        'getHashButton
        '
        resources.ApplyResources(Me.getHashButton, "getHashButton")
        Me.getHashButton.Name = "getHashButton"
        '
        'decryptButton
        '
        resources.ApplyResources(Me.decryptButton, "decryptButton")
        Me.decryptButton.Name = "decryptButton"
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
        'resultsTextBox
        '
        resources.ApplyResources(Me.resultsTextBox, "resultsTextBox")
        Me.resultsTextBox.Name = "resultsTextBox"
        Me.resultsTextBox.ReadOnly = True
        Me.resultsTextBox.TabStop = False
        '
        'encryptButton
        '
        resources.ApplyResources(Me.encryptButton, "encryptButton")
        Me.encryptButton.Name = "encryptButton"
        '
        'QuickStartForm
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.checkTextButton)
        Me.Controls.Add(Me.getHashButton)
        Me.Controls.Add(Me.decryptButton)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.resultsTextBox)
        Me.Controls.Add(Me.encryptButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.groupBox.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub New(ByVal cryptographyManager As CryptographyManager)
        Me.New()
        Me.cryptographyManager = cryptographyManager
    End Sub

    Public Shared Sub CreateKeys()
        Dim installedPath As String = ConfigurationManager.AppSettings("InstallPath")
        Dim fileName As String = Path.Combine(installedPath, symmKeyFileName)
        Dim symmetricKey As ProtectedKey = KeyManager.GenerateSymmetricKey(GetType(RijndaelManaged), DataProtectionScope.LocalMachine)
        Using keyStream As FileStream = New FileStream(fileName, FileMode.Create)
            KeyManager.Write(keyStream, symmetricKey)
        End Using
    End Sub

    ' Displays dialog with information about exceptions that occur in the application. 
    Private Shared Sub AppThreadException(ByVal source As Object, ByVal e As ThreadExceptionEventArgs)
        Dim errorMsg As String = String.Format(New CultureInfo("en-us", True), "There are some problems while trying to use the Cryptography Quick Start, please check the following error messages: " & Environment.NewLine & "{0}" & Environment.NewLine, e.Exception.Message)
        errorMsg = errorMsg & Environment.NewLine

        Dim result As DialogResult = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)

        ' Exits the program when the user clicks Abort.
        If (result = System.Windows.Forms.DialogResult.Abort) Then
            Application.Exit()
        End If

        QuickStartForm.AppForm.Cursor = Cursors.Default
    End Sub

    Private Function GetEmbeddedImage(ByVal resourceName As String) As Image
        Dim resourceStream As Stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If (resourceStream Is Nothing) Then
            Return Nothing
        End If

        Dim img As Image = Image.FromStream(resourceStream)

        Return img
    End Function

    Private Sub DisplayResults(ByVal results As String)
        Me.resultsTextBox.Text &= results & Environment.NewLine
        Me.resultsTextBox.SelectAll()
        Me.resultsTextBox.ScrollToCaret()
    End Sub

    Private Sub QuickStartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initialize image to embedded logo
        Me.logoPictureBox.Image = GetEmbeddedImage("logo.gif")

        Me.decryptButton.Enabled = False
        Me.checkTextButton.Enabled = False
    End Sub

    Private Sub quitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitButton.Click
        Me.Close()
    End Sub

    ' Encrypt a sample text using the default symmetric provider.
    Private Sub encryptButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles encryptButton.Click
        If (Not inputFormObject Is Nothing) Then
            inputFormObject.Title = My.Resources.EncryptTitleMessage
            inputFormObject.InstructionsText = My.Resources.EncryptInstructionsMessage
            If (inputFormObject.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                Try
                    Me.Cursor = Cursors.WaitCursor

                    Me.encryptedContentsBase64 = cryptographyManager.EncryptSymmetric(symmProvider, inputFormObject.Input)

                    Me.DisplayResults(String.Format(My.Resources.Culture, My.Resources.OriginalTextMessage, inputFormObject.Input))
                    Me.DisplayResults(String.Format(My.Resources.EncryptedTextMessage, Me.encryptedContentsBase64))

                    Me.decryptButton.Enabled = True

                    Me.Cursor = Cursors.Arrow
                Catch ex As Exception
                    ProcessUnhandledException(ex)
                Finally
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End If
    End Sub

    ' Decrypts a set of bytes and displayed the decrypted contents.
    Private Sub decryptButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles decryptButton.Click
        If (Not Me.encryptedContentsBase64 Is Nothing) Then

            Try
                Me.Cursor = Cursors.WaitCursor

                ' Get a string for display
                Dim readableString As String = cryptographyManager.DecryptSymmetric(symmProvider, Me.encryptedContentsBase64)

                Me.DisplayResults(String.Format(My.Resources.Culture, My.Resources.DecryptedTextMessage, readableString))
            Catch ex As Exception
                ProcessUnhandledException(ex)
            Finally
                Me.Cursor = Cursors.Default
            End Try

        Else
            Me.DisplayResults(My.Resources.DecryptErrorMessage)
        End If
    End Sub

    ' Creates a hash based on a sample text, for further comparison.
    Private Sub getHashButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles getHashButton.Click
        If (Not inputFormObject Is Nothing) Then

            inputFormObject.Title = My.Resources.HashTitleMessage
            inputFormObject.InstructionsText = My.Resources.HashInstructionsMessage
            If (inputFormObject.ShowDialog() = Windows.Forms.DialogResult.OK) Then

                Try
                    Me.Cursor = Cursors.WaitCursor

                    Me.generatedHash = Me.GetHash(inputFormObject.Input)

                    Me.DisplayResults(String.Format(My.Resources.Culture, My.Resources.HashMessage, Convert.ToBase64String(generatedHash)))

                    Me.checkTextButton.Enabled = True
                Catch ex As Exception
                    ProcessUnhandledException(ex)
                Finally
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End If
    End Sub

    Private Function GetHash(ByVal plainText As String) As Byte()

        Dim valueToHash As Byte() = Encoding.UTF8.GetBytes(plainText)

        Dim generatedHash As Byte() = cryptographyManager.CreateHash(hashProvider, valueToHash)

        ' Clear the byte array memory
        Array.Clear(valueToHash, 0, valueToHash.Length)

        Return generatedHash

    End Function

    Private Function CompareHash(ByVal plainText As String, ByVal existingHashValue As Byte()) As Boolean

        Dim valueToHash As Byte() = Encoding.UTF8.GetBytes(plainText)

        Dim matched As Boolean = cryptographyManager.CompareHash(hashProvider, valueToHash, existingHashValue)

        ' Clear the byte array memory
        Array.Clear(valueToHash, 0, valueToHash.Length)

        Return matched
    End Function

    ' Check a previously generated hash in order to determine whether the original text
    ' has been tampered or not.
    Private Sub checkTextButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkTextButton.Click
        If (Not inputFormObject Is Nothing) Then
            inputFormObject.Title = My.Resources.HashTitleMessage
            inputFormObject.InstructionsText = My.Resources.HashInstructionsMessage
            If (inputFormObject.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                If (Not Me.generatedHash Is Nothing) Then
                    Try
                        Me.Cursor = Cursors.WaitCursor

                        Dim comparisonSucceeded As Boolean = Me.CompareHash(inputFormObject.Input, Me.generatedHash)

                        If (comparisonSucceeded) Then
                            Me.DisplayResults(My.Resources.TextNotTamperedMessage)
                        Else
                            Me.DisplayResults(My.Resources.TextTamperedMessage)
                        End If

                    Catch ex As Exception
                        ProcessUnhandledException(ex)
                    Finally
                        Me.Cursor = Cursors.Default
                    End Try
                End If
            End If
        End If
    End Sub
    Private Function GetHelpViewerExecutable() As String
        Dim common As String = Environment.GetEnvironmentVariable("CommonProgramFiles")
        Return Path.Combine(common, "Microsoft Shared\Help 9\dexplore.exe")
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

    ''' <summary>
    ''' Process any unhandled exceptions that occur in the application.
    ''' This code is called by all UI entry points in the application (e.g. button click events)
    ''' when an unhandled exception occurs.
    ''' You could also achieve this by handling the Application.ThreadException event, however
    ''' the VS2005 debugger will break before this event is called.
    ''' </summary>
    ''' <param name="ex">The unhandled exception</param>
    Private Sub ProcessUnhandledException(ByVal ex As Exception)
        Dim errorMessage As StringBuilder = New StringBuilder()
        errorMessage.Append("The following error occured during execution of the Cryptography QuickStart.")
        errorMessage.Append(Environment.NewLine & Environment.NewLine)
        errorMessage.AppendFormat(New CultureInfo("en-us", True), "{0}", ex.Message)
        errorMessage.Append(Environment.NewLine & Environment.NewLine)
        errorMessage.Append("Exceptions can be caused by invalid configuration information.")
        errorMessage.Append(Environment.NewLine & Environment.NewLine)
        errorMessage.Append("Do you want to exit the application?")

        Dim result As Windows.Forms.DialogResult = MessageBox.Show(errorMessage.ToString(), "Application Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop)

        ' Exits the program when the user clicks Abort.
        If (result = Windows.Forms.DialogResult.Yes) Then
            Application.Exit()
        End If
    End Sub
End Class

