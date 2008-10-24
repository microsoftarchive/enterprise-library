'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Configuration QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Public Class QuickStartForm
    Private Const HelpViewerExecutable As String = "dexplore.exe"
    Private Const HelpTopicNamespace As String = "ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct"

    Private viewerProcess As Process = Nothing
    Private WithEvents watcher As FileSystemWatcher

    Private Sub QuickStartForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Initialize image to embedded logo
        logoPictureBox.Image = GetEmbeddedImage("ConfigurationReadXmlQuickStart.logo.gif")

        ' Initialize the text box with the configuration settings
        Dim configData As EditorFontData = CType(ConfigurationManager.GetSection("EditorSettings"), EditorFontData)

        ' Initialize file system watcher
        watcher = New FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory)
        watcher.EnableRaisingEvents = False

        DisplayResults("Application configuration loaded.", readResultsTextBox)

        UpdateFont(configData, readSampleTextBox)
    End Sub

    Private Function GetEmbeddedImage(ByVal resourceName As String) As System.Drawing.Image
        Dim resourceStream As Stream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If resourceStream Is Nothing Then Return Nothing
            
        Return System.Drawing.Image.FromStream(resourceStream)

    End Function


    Delegate Sub ShowResultsDelegate(ByVal results As String, ByVal targetTextBox As TextBox)

    Private Sub DisplayResults(ByVal results As String, ByVal targetTextBox As TextBox)
        If targetTextBox.InvokeRequired Then
            targetTextBox.Invoke(New ShowResultsDelegate(AddressOf ShowResults), New Object() {results, targetTextBox})
        Else
            ShowResults(results, targetTextBox)
        End If
    End Sub

    Private Sub ShowResults(ByVal results As String, ByVal targetTextBox As TextBox)
        targetTextBox.Text = results & Environment.NewLine
    End Sub

    Delegate Sub ShowFontDataDelegate(ByVal fontData As EditorFontData, ByVal sampleTextBox As RichTextBox)

    Private Sub UpdateFont(ByVal fontData As EditorFontData, ByVal sampleTextBox As RichTextBox)
        If (sampleTextBox.InvokeRequired) Then
            sampleTextBox.Invoke(New ShowFontDataDelegate(AddressOf ShowFontData), New Object() {fontData, sampleTextBox})
        Else
            ShowFontData(fontData, sampleTextBox)
        End If
    End Sub

    Private Sub ShowFontData(ByVal fontData As EditorFontData, ByVal sampleTextBox As RichTextBox)
        Dim newFont As Font = New Font(fontData.Name, CSng(fontData.Size), _
            CType(fontData.Style, System.Drawing.FontStyle))
        sampleTextBox.Font = newFont
    End Sub


    Private Sub watcher_Changed(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles watcher.Changed
        If e.FullPath.ToLower().Contains(".config") Then

            For i As Integer = 0 To 2
                Try
                    ConfigurationManager.RefreshSection("EditorSettings")
                    Exit For
                Catch ex As ConfigurationErrorsException
                    If i = 2 Then
                        Throw
                    Else
                        Threading.Thread.Sleep(100)
                    End If
                End Try
            Next i

            Dim configData As EditorFontData = CType(ConfigurationManager.GetSection("EditorSettings"), EditorFontData)

            Dim results As New StringBuilder()
            results.Append("Configuration changes in storage were detected. Updating configuration.")
            results.Append(Environment.NewLine)
            results.Append("New configuration settings:")
            results.Append(Environment.NewLine)
            results.Append(vbTab)
            results.Append(configData.ToString())
            results.Append(Environment.NewLine)

            DisplayResults(results.ToString(), readResultsTextBox)

            UpdateFont(configData, readSampleTextBox)
        End If
    End Sub

    Private Sub readXmlConfigDataButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readXmlConfigDataButton.Click
        Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim configData As EditorFontData = CType(ConfigurationManager.GetSection("EditorSettings"), EditorFontData)

        Dim results As New StringBuilder()
        results.Append("Configuration settings:")
        results.Append(Environment.NewLine)
        results.Append(vbTab)
        results.Append(configData.ToString())
        results.Append(Environment.NewLine)

        DisplayResults(results.ToString(), readResultsTextBox)

        UpdateFont(configData, readSampleTextBox)

        Cursor = System.Windows.Forms.Cursors.Arrow
    End Sub

    Private Sub clearCacheButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearCacheButton.Click
        ConfigurationManager.RefreshSection("EditorSettings")
        DisplayResults("The cache of configuration data has been cleared.", readResultsTextBox)
    End Sub

    Private Sub automaticRefreshCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles automaticRefreshCheckBox.CheckedChanged
        If (automaticRefreshCheckBox.Checked) Then
            watcher.EnableRaisingEvents = True
            DisplayResults("Configuration changes in storage will be detected.", readResultsTextBox)
        Else
            watcher.EnableRaisingEvents = False
            DisplayResults("Configuration changes in storage will *not* be detected.", readResultsTextBox)
        End If
    End Sub

    Private Sub writeXmlConfigDataButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeXmlConfigDataButton.Click
        Dim configData As New EditorFontData()

        If fontDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            configData.Name = fontDialog.Font.Name
            configData.Size = fontDialog.Font.Size
            configData.Style = Convert.ToInt32(fontDialog.Font.Style)
            ' Write the new configuration data to the XML file
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            config.Sections.Remove("EditorSettings")
            config.Sections.Add("EditorSettings", configData)
            config.Save()

            Dim results As New StringBuilder()
            results.Append("Configuration Data Updated:")
            results.Append(Environment.NewLine)
            results.Append(vbTab)
            results.Append(configData.ToString())

            DisplayResults(results.ToString(), writeResultsTextBox)

            UpdateFont(configData, writeSampleTextBox)
        End If
    End Sub

    Private Sub viewWalkthroughButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Process has never been started. Initialize and launch the viewer.
        If (Me.viewerProcess Is Nothing) Then
            ' Initialize the Process information for the help viewer
            Me.viewerProcess = New Process

            Me.viewerProcess.StartInfo.FileName = HelpViewerExecutable
            Me.viewerProcess.StartInfo.Arguments = "/helpcol " & HelpTopicNamespace
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

 
End Class
