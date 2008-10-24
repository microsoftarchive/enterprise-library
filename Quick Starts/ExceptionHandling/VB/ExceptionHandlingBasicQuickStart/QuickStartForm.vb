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
    Friend WithEvents suppressExceptionButton As System.Windows.Forms.Button
    Friend WithEvents replaceExceptionButton As System.Windows.Forms.Button
    Friend WithEvents resultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents wrapExceptionButton As System.Windows.Forms.Button
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button

    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic ExceptionhandlingQS1"
    Private viewerProcess As Process = Nothing
    Public Shared AppForm As System.Windows.Forms.Form
    Friend WithEvents propagateOriginalExceptionButton As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.suppressExceptionButton = New System.Windows.Forms.Button
        Me.propagateOriginalExceptionButton = New System.Windows.Forms.Button
        Me.replaceExceptionButton = New System.Windows.Forms.Button
        Me.resultsTextBox = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.wrapExceptionButton = New System.Windows.Forms.Button
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'suppressExceptionButton
        '
        Me.suppressExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.suppressExceptionButton.Location = New System.Drawing.Point(15, 218)
        Me.suppressExceptionButton.Name = "suppressExceptionButton"
        Me.suppressExceptionButton.Size = New System.Drawing.Size(128, 40)
        Me.suppressExceptionButton.TabIndex = 30
        Me.suppressExceptionButton.Text = "Process and suppress an exception"
        '
        'propagateOriginalExceptionButton
        '
        Me.propagateOriginalExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.propagateOriginalExceptionButton.Location = New System.Drawing.Point(15, 72)
        Me.propagateOriginalExceptionButton.Name = "propagateOriginalExceptionButton"
        Me.propagateOriginalExceptionButton.Size = New System.Drawing.Size(128, 41)
        Me.propagateOriginalExceptionButton.TabIndex = 27
        Me.propagateOriginalExceptionButton.Text = "Propagate the original exception"
        '
        'replaceExceptionButton
        '
        Me.replaceExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.replaceExceptionButton.Location = New System.Drawing.Point(15, 169)
        Me.replaceExceptionButton.Name = "replaceExceptionButton"
        Me.replaceExceptionButton.Size = New System.Drawing.Size(128, 40)
        Me.replaceExceptionButton.TabIndex = 29
        Me.replaceExceptionButton.Text = "Replace one exception with another"
        '
        'resultsTextBox
        '
        Me.resultsTextBox.Location = New System.Drawing.Point(155, 72)
        Me.resultsTextBox.Multiline = True
        Me.resultsTextBox.Name = "resultsTextBox"
        Me.resultsTextBox.ReadOnly = True
        Me.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.resultsTextBox.Size = New System.Drawing.Size(512, 264)
        Me.resultsTextBox.TabIndex = 31
        Me.resultsTextBox.TabStop = False
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        Me.groupBox1.Location = New System.Drawing.Point(-5, -12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(704, 72)
        Me.groupBox1.TabIndex = 32
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(16, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(371, 24)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Basic"
        '
        'logoPictureBox
        '
        Me.logoPictureBox.Location = New System.Drawing.Point(608, 21)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(69, 50)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.logoPictureBox.TabIndex = 0
        Me.logoPictureBox.TabStop = False
        '
        'wrapExceptionButton
        '
        Me.wrapExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.wrapExceptionButton.Location = New System.Drawing.Point(15, 121)
        Me.wrapExceptionButton.Name = "wrapExceptionButton"
        Me.wrapExceptionButton.Size = New System.Drawing.Size(128, 41)
        Me.wrapExceptionButton.TabIndex = 28
        Me.wrapExceptionButton.Text = "Wrap one exception with another"
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        Me.groupBox.Location = New System.Drawing.Point(-5, 350)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(704, 87)
        Me.groupBox.TabIndex = 33
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
        Me.ClientSize = New System.Drawing.Size(691, 420)
        Me.Controls.Add(Me.suppressExceptionButton)
        Me.Controls.Add(Me.propagateOriginalExceptionButton)
        Me.Controls.Add(Me.replaceExceptionButton)
        Me.Controls.Add(Me.resultsTextBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.wrapExceptionButton)
        Me.Controls.Add(Me.groupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MinimizeBox = False
        Me.Name = "QuickStartForm"
        Me.Text = "Exception Handling Quick Start"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

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

    Private Sub quitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles quitButton.Click
        Me.Close()
    End Sub


    ' Routine that causes an exception to be thrown
    Private Sub Process()
        Throw New Exception("Quick Start Generated Exception")
    End Sub

    Private Sub propagateOriginalExceptionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles propagateOriginalExceptionButton.Click
        Try

            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim sb As StringBuilder = New StringBuilder

            sb.Append("Scenario: Propagate original exception")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("1. UI layer calls into business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("2. A System.Exception occurs and is detected in the business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("3. The business layer specifies the ""Propagate Policy"" as the exception handling policy.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("4. The ""Propagate Policy"" is configured to recommend a rethrow upon return from processing the exception handlers.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("5. Control is returned to the business layer, which rethrows the original exception.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("6. The original exception is caught and displayed.")

            DisplayScenarioStart(sb.ToString())

            Dim svc As AppService = New AppService

            svc.ProcessWithPropagate()

        Catch ex As Exception
            ProcessUnhandledException(ex)

        End Try
    End Sub

    Private Sub wrapExceptionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles wrapExceptionButton.Click
        Try

            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim sb As StringBuilder = New StringBuilder

            sb.Append("Scenario: Wrap the original exception with another before propagating")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("1. UI layer calls into business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("2. A DBConcurrencyException occurs and is detected in the business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("3. The business layer specifies the ""Wrap Policy"" as the exception handling policy.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("4. The ""Wrap Policy"" is configured to use a wrap handler to wrap the original exception with a BusinessLayerException exception.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("5. The rethrowAction is set to ""Throw"", resulting in the BusinessLayerException exception being thrown by the block upon completion of the handler chain execution.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("6. The new exception, which wraps the original exception, is caught and displayed.")

            DisplayScenarioStart(sb.ToString())

            Dim svc As AppService = New AppService

            svc.ProcessWithWrap()

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub replaceExceptionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles replaceExceptionButton.Click
        Try

            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim sb As StringBuilder = New StringBuilder

            sb.Append("Scenario: Replace the original exception with another before propagating")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("1. UI layer calls into business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("2. A SecurityException exception occurs and is detected in the business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("3. The business layer specifies the ""Replace Policy"" as the exception handling policy.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("4. The ""Replace Policy"" is configured to use a replace handler to replace the original exception with an ApplicationException exception.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("5. The rethrowAction is set to ""Throw"", resulting in the new exception being thrown by the block upon completion of the handler chain execution.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("6. The new exception is caught and displayed.")

            DisplayScenarioStart(sb.ToString())
            Dim svc As AppService = New AppService

            svc.ProcessWithReplace()

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try
    End Sub

    Private Sub suppressExceptionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles suppressExceptionButton.Click
        Try
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim sb As StringBuilder = New StringBuilder

            sb.Append("Scenario: Process and suppress the original exception")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("1. UI layer calls into business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("2. A SecurityException occurs and is detected in the business layer.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("3. The business layer specifies the ""Handle and Suppress Policy"" as the exception handling policy.")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)
            sb.Append("4. The ""Handle and Suppress Policy"" is configured with a rethrowAction of ""None"", resulting in the exception being suppressed upon completion of the handler chain execution.")

            DisplayScenarioStart(sb.ToString())

            Dim svc As AppService = New AppService

            svc.ProcessAndResume()

            Me.resultsTextBox.Text += "Exception processed, execution resumed."

            Cursor = System.Windows.Forms.Cursors.Default

        Catch ex As Exception
            ProcessUnhandledException(ex)
        End Try

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
    Private Sub viewWalkthroughButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles viewWalkthroughButton.Click
        Try

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
                If (ExceptionHandlingBasicQuickStart.NativeMethods.IsIconic(hWnd)) Then
                    ExceptionHandlingBasicQuickStart.NativeMethods.ShowWindowAsync(hWnd, ExceptionHandlingBasicQuickStart.SW_RESTORE)
                End If
                ExceptionHandlingBasicQuickStart.NativeMethods.SetForegroundWindow(hWnd)
            End If

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

