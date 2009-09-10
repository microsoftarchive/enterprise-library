'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Exception Handling Application Block QuickStart
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
        _exceptionManager = EnterpriseLibraryContainer.Current.GetInstance(Of ExceptionManager)()
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
    Friend WithEvents logExceptionButton As System.Windows.Forms.Button
    Friend WithEvents resultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents notifyUserButton As System.Windows.Forms.Button
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button

    Public Shared AppForm As System.Windows.Forms.Form
    Private viewerProcess As Process = Nothing

    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic ExceptionhandlingQS2"

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.logExceptionButton = New System.Windows.Forms.Button
        Me.resultsTextBox = New System.Windows.Forms.TextBox
        Me.notifyUserButton = New System.Windows.Forms.Button
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        Me.groupBox1.Location = New System.Drawing.Point(-5, -12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(704, 72)
        Me.groupBox1.TabIndex = 30
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(16, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(431, 31)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Exception Handling With Logging"
        '
        'logoPictureBox
        '
        Me.logoPictureBox.Location = New System.Drawing.Point(608, 14)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(69, 50)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.logoPictureBox.TabIndex = 0
        Me.logoPictureBox.TabStop = False
        '
        'logExceptionButton
        '
        Me.logExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.logExceptionButton.Location = New System.Drawing.Point(11, 79)
        Me.logExceptionButton.Name = "logExceptionButton"
        Me.logExceptionButton.Size = New System.Drawing.Size(128, 40)
        Me.logExceptionButton.TabIndex = 27
        Me.logExceptionButton.Text = "Log an exception"
        '
        'resultsTextBox
        '
        Me.resultsTextBox.Location = New System.Drawing.Point(155, 72)
        Me.resultsTextBox.Multiline = True
        Me.resultsTextBox.Name = "resultsTextBox"
        Me.resultsTextBox.ReadOnly = True
        Me.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.resultsTextBox.Size = New System.Drawing.Size(512, 236)
        Me.resultsTextBox.TabIndex = 29
        Me.resultsTextBox.TabStop = False
        '
        'notifyUserButton
        '
        Me.notifyUserButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.notifyUserButton.Location = New System.Drawing.Point(8, 135)
        Me.notifyUserButton.Name = "notifyUserButton"
        Me.notifyUserButton.Size = New System.Drawing.Size(129, 40)
        Me.notifyUserButton.TabIndex = 28
        Me.notifyUserButton.Text = "Notify the user when an exception occurs"
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        Me.groupBox.Location = New System.Drawing.Point(-5, 322)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(704, 87)
        Me.groupBox.TabIndex = 31
        Me.groupBox.TabStop = False
        '
        'viewWalkthroughButton
        '
        Me.viewWalkthroughButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewWalkthroughButton.Location = New System.Drawing.Point(420, 24)
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        Me.viewWalkthroughButton.Size = New System.Drawing.Size(113, 32)
        Me.viewWalkthroughButton.TabIndex = 0
        Me.viewWalkthroughButton.Text = "View &Walkthrough"
        '
        'quitButton
        '
        Me.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.quitButton.Location = New System.Drawing.Point(553, 24)
        Me.quitButton.Name = "quitButton"
        Me.quitButton.Size = New System.Drawing.Size(114, 32)
        Me.quitButton.TabIndex = 1
        Me.quitButton.Text = "Quit"
        '
        'QuickStartForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(692, 395)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.logExceptionButton)
        Me.Controls.Add(Me.resultsTextBox)
        Me.Controls.Add(Me.notifyUserButton)
        Me.Controls.Add(Me.groupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.Text = "Exception Handling Application Block Quick Start"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private _exceptionManager As ExceptionManager

    Private Sub QuickStartForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initialize image to embedded logo
        logoPictureBox.Image = GetEmbeddedImage("logo.gif")
    End Sub

    Private Function GetEmbeddedImage(ByVal resourceName As String) As System.Drawing.Image
        Dim resourceStream As Stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If (resourceStream Is Nothing) Then
            Return Nothing
        End If

        Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(resourceStream)

        Return img
    End Function


    Private Sub DisplayScenarioStart(ByVal scenarioDescription As String)
        Me.resultsTextBox.Text = scenarioDescription + Environment.NewLine + Environment.NewLine
        Me.resultsTextBox.Update()
    End Sub

    Private Sub DisplayResults(ByVal results As String)
        Me.resultsTextBox.Text += results
    End Sub

    Private Function GetExceptionInfo(ByVal ex As Exception) As String
        Dim sb As StringBuilder = New StringBuilder
        Dim writer As StringWriter = New StringWriter(sb)

        Dim formatter As AppTextExceptionFormatter = New AppTextExceptionFormatter(writer, ex)

        ' Format the exception
        formatter.Format()

        Return sb.ToString()
    End Function

    ' Routine that causes an exception to be thrown
    Private Sub Process()
        Throw New Exception("Quick Start Generated Exception")
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
        Me.Close()
    End Sub

    Private Sub logExceptionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logExceptionButton.Click
        Try
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            DisplayScenarioStart(My.Resources.LogExceptionText)

            Try
                Process()
            Catch ex As Exception
                Dim rethrow As Boolean = _exceptionManager.HandleException(ex, "Log Only Policy")

                DisplayResults("**Exception has been logged. See the currently configured log destination (default is event log) for exception details.")

                If (rethrow) Then
                    Throw
                End If
            End Try

            Cursor = System.Windows.Forms.Cursors.Default

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub notifyUserButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles notifyUserButton.Click
        Try

            Cursor = System.Windows.Forms.Cursors.WaitCursor
            DisplayScenarioStart(My.Resources.NotifyUserText)

            Dim svc As AppService = New AppService

            svc.ProcessAndNotify()

            Cursor = System.Windows.Forms.Cursors.Arrow

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try
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
        ' An unhandled exception occured somewhere in our application. Let
        ' the 'Global Policy' handler have a try at handling it.
        Try
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Global Policy")
            If (rethrow) Then
                ' Something has gone very wrong - exit the application.
                Application.Exit()
            End If

        Catch
            Dim errorMsg As String = "An unexpected exception occured while calling HandleException with policy 'Global Policy'. "
            errorMsg += "Please check the event log for details about the exception." + Environment.NewLine + Environment.NewLine

            MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Application.Exit()
        End Try

        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub
End Class
