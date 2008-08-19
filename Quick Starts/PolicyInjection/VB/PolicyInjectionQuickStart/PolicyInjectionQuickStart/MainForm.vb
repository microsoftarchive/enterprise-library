'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Policy Injection Application Block QuickStart
'===============================================================================
' Copyright ? Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System.Diagnostics
Imports System.IO
Imports System.Configuration
Imports System.Configuration.Install
Imports System.Security.Principal
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.PolicyInjection
Imports Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Installers

Public Class MainForm
    Private viewerProcess As Process
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic PolicyInjectionQS1"

    Private bankAccount As BusinessLogic.BankAccount

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PopulateUserList()
        bankAccount = PolicyInjection.Create(Of BusinessLogic.BankAccount)()
    End Sub

    Private Sub depositButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles depositButton.Click
        Dim form As New AmountEntryForm(AmountDialogType.Deposit)
        Dim result As DialogResult = form.ShowDialog(Me)

        If result = DialogResult.OK Then
            exceptionTextBox.Text = String.Empty
            Try
                bankAccount.Deposit(form.Amount)
            Catch ex As Exception
                exceptionTextBox.Text = ex.Message
            End Try
        End If
    End Sub

    Private Sub withdrawButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles withdrawButton.Click
        Dim form As New AmountEntryForm(AmountDialogType.Withdraw)
        Dim result As DialogResult = Form.ShowDialog(Me)

        If result = DialogResult.OK Then
            exceptionTextBox.Text = String.Empty
            Try
                bankAccount.Withdraw(form.Amount)
            Catch ex As Exception
                exceptionTextBox.Text = ex.Message
            End Try
            end if
    End Sub

    Private Sub balanceInquiryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles balanceInquiryButton.Click
        exceptionTextBox.Text = String.Empty
        Try
            balanceTextBox.Text = bankAccount.GetCurrentBalance().ToString()
        Catch ex As Exception
            exceptionTextBox.Text = ex.Message
        End Try
    End Sub

    Private Sub viewLogButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewLogButton.Click
        Dim p As Process = Process.Start(New ProcessStartInfo("notepad.exe", "audit.log"))
    End Sub

    Private Sub openPerfMonButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles openPerfMonButton.Click
        Dim p As Process = Process.Start(New ProcessStartInfo("perfmon.exe"))
    End Sub

    Private Sub userComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles userComboBox.SelectedIndexChanged
        ' Obviously, you wouldn't implement security like this in a real application. 
        ' It's a Quickstart, people! :-)

        ' Find the principal of the selected user. 
        Dim pair As KeyValuePair(Of String, IPrincipal) = userComboBox.SelectedItem
        Dim selectedUser As IPrincipal = pair.Value

        ' Set the current thread principal to the selected user
        System.Threading.Thread.CurrentPrincipal = selectedUser
    End Sub

    Private Sub exitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitButton.Click
        Application.Exit()
    End Sub

    Private Sub PopulateUserList()
        Const userPrefix As String = "User:"
        For Each setting As String In ConfigurationManager.AppSettings

            If setting.StartsWith(userPrefix) Then
                Dim userName As String = setting.Substring(userPrefix.Length)
                Dim role As String = ConfigurationManager.AppSettings(setting)
                Dim principal As IPrincipal = New GenericPrincipal( _
                    New GenericIdentity(userName), New String() {role})
                userComboBox.Items.Add(New KeyValuePair(Of String, IPrincipal)(userName, principal))
            End If
        Next
        userComboBox.SelectedIndex = 0
    End Sub

    Private Sub installPerfCountersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles installPerfCountersButton.Click
        Try
            Dim installer As New PerformanceCountersInstaller(New SystemConfigurationSource())
            Dim state As New System.Collections.Hashtable()
            installer.Context = New InstallContext()
            installer.Install(state)
            installer.Commit(state)
            MessageBox.Show("Performance counters have been successfully installed.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub uninstallPerfCounters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uninstallPerfCounters.Click
        Try
            Dim installer As New PerformanceCountersInstaller(New SystemConfigurationSource())
            installer.Context = New InstallContext()
            installer.Uninstall(Nothing)
            MessageBox.Show("Performance counters have been successfully uninstalled.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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
End Class
