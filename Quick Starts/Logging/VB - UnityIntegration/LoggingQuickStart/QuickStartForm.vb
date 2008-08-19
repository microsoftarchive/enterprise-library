'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Logging Application Block QuickStart
'===============================================================================
' Copyright ? Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System.IO


Public Class QuickStartForm

    Private viewerProcess As Process
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic LoggingQS1"
    Private Const TRACE_LOG_FILE_NAME As String = "trace.log"
    Private logWriter As LogWriter
    Private traceManager As TraceManager

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal logWriter As LogWriter, ByVal traceManager As TraceManager)
        Me.New()
        Me.traceManager = traceManager
        Me.logWriter = logWriter
    End Sub

    Private Sub QuickStartForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Initialize image to embedded logo
        Me.logoPictureBox.Image = GetEmbeddedImage("LoggingQuickStart.logo.gif")

        'DisplayConfiguration()
    End Sub

    Private Function GetEmbeddedImage(ByVal resourceName As String) As Image
        Dim resourceStream As IO.Stream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If resourceStream Is Nothing Then
            Return Nothing
        End If

        Return Image.FromStream(resourceStream)

    End Function

    Private Sub DisplayConfiguration()
        Try

            ' Get configuration settings for Logging and Instrmentation Application Block. 
            ' This assumes the configuration source is the SystemConfigurationSource, which
            ' is the default setting when the QuickStart ships.
            Dim settings As LoggingSettings = LoggingSettings.GetLoggingSettings(New SystemConfigurationSource())

            Dim defaultCategory As String = settings.DefaultCategory

            Dim results As StringBuilder = New StringBuilder()

            results.Append("Current Configuration")
            results.Append(Environment.NewLine)
            results.Append("---------------------------------")
            results.Append(Environment.NewLine)
            results.Append(Environment.NewLine)
            results.Append("Default Category: " & settings.DefaultCategory & Environment.NewLine & Environment.NewLine)
            results.Append("Categories and category listeners")
            results.Append(Environment.NewLine)
            results.Append(Environment.NewLine)

            ' Grab the list of categories and loop through for display.
            Dim sources As NamedElementCollection(Of TraceSourceData) = settings.TraceSources

            For Each source As TraceSourceData In sources

                results.Append("   " & source.Name)

                ' Flag any of the categories that would be denied based upon
                ' the current category filter configuration.
                If Not (logWriter.GetFilter(Of CategoryFilter)().ShouldLog(source.Name)) Then
                    results.Append("*")
                End If

                ' Loop through the list of trace listeners for the category.
                Dim TraceListeners As NamedElementCollection(Of TraceListenerReferenceData) = source.TraceListeners

                Dim listener As StringBuilder = New StringBuilder()
                listener.Append("  -  ")
                For Each listenerData As TraceListenerReferenceData In TraceListeners
                    listener.Append(listenerData.Name + ", ")
                Next
                ' Remove trailing comma and space
                listener.Remove(listener.Length - 2, 2)
                results.Append(listener.ToString())
                results.Append(Environment.NewLine)
            Next
            results.Append(Environment.NewLine)
            results.Append("   * Events in category will not be logged")

            resultsTextBox.Text += results.ToString()
        Catch ex As Exception
            ProcessUnhandledException(ex)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub DisplayResults(ByVal results As String)
        resultsTextBox.Text += results + Environment.NewLine
        resultsTextBox.SelectAll()
        resultsTextBox.ScrollToCaret()
    End Sub

    Private Sub DisplayScenarioStart(ByVal message As String)
        resultsTextBox.Text = message & Environment.NewLine & Environment.NewLine
        resultsTextBox.Update()
    End Sub

    Private Sub logEventInformationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logEventInformationButton.Click
        Dim eventForm As EventInformationForm = New EventInformationForm()

        Dim result As DialogResult = eventForm.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then

            Try

                Windows.Forms.Cursor.Current = Cursors.WaitCursor

                ' Creates and fills the log entry with user information
                Dim logEntry As New LogEntry()
                logEntry.EventId = eventForm.EventId
                logEntry.Priority = eventForm.Priority
                logEntry.Message = eventForm.Message
                logEntry.Categories.Clear()

                ' Add the categories selected by the user
                For Each category As String In eventForm.Categories
                    logEntry.Categories.Add(category)
                Next

                DisplayScenarioStart(String.Format(My.Resources.LogEventStartMessage, logEntry.ToString()))

                ' Writes the log entry.
                logWriter.Write(logEntry)

                DisplayResults(String.Format(My.Resources.EventProcessedMessage, logEntry.EventId))
            Catch ex As Exception
                ProcessUnhandledException(ex)
            Finally
                Windows.Forms.Cursor.Current = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub logExtraInformationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logExtraInformationButton.Click
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        DisplayScenarioStart(My.Resources.ExtraInformationStartMessage)

        Try
            '' Use the WindowsPrincipal as the current principal. This will cause the 
            '' ManagedSecurityContextInformationProvider to add the current Windows user's name
            '' to the additional information to be logged.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)

            '' Create the dictionary to hold the extra information, and populate it
            '' with managed security information.  
            Dim dictionary As New Dictionary(Of String, Object)()
            Dim informationHelper As New ManagedSecurityContextInformationProvider()

            informationHelper.PopulateDictionary(dictionary)

            '' Add a custom property for screen resolution
            Dim width As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim height As Integer = Screen.PrimaryScreen.Bounds.Height
            Dim resolution As String = String.Format("{0}x{1}", width, height)

            dictionary.Add("Screen resolution", resolution)

            '' Write the log entry that contains the extra information
            Dim logEntry As LogEntry = New LogEntry()
            logEntry.Message = "Log entry with extra information"
            logEntry.ExtendedProperties = dictionary

            logWriter.Write(logEntry)

            DisplayResults(My.Resources.ExtraInformationEndMessage)

        Catch ex As Exception
            ProcessUnhandledException(ex)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub viewTraceLogButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewTraceLogButton.Click
        Dim traceFileViewerProcess As New Process()

        traceFileViewerProcess.StartInfo.FileName = "Notepad.exe"
        traceFileViewerProcess.StartInfo.Arguments = TRACE_LOG_FILE_NAME
        traceFileViewerProcess.Start()
    End Sub

    Private Sub viewEventLogButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewEventLogButton.Click
        Dim traceFileViewerProcess As New Process()

        Dim executable As String = Environment.ExpandEnvironmentVariables("%SystemRoot%\system32\eventvwr.msc")
        traceFileViewerProcess.StartInfo.FileName = executable

        traceFileViewerProcess.StartInfo.Arguments = "/s"
        traceFileViewerProcess.Start()
    End Sub

    Private Sub quitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitButton.Click
        Me.Close()
    End Sub

    Private Sub traceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles traceButton.Click
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            DisplayScenarioStart(My.Resources.TraceStartMessage)

            Using (traceManager.StartTrace("Trace"))
                DoDataAccess()
            End Using

            DisplayResults(String.Format(My.Resources.TraceDoneMessage, TRACE_LOG_FILE_NAME))
        Catch ex As Exception
            ProcessUnhandledException(ex)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub DoDataAccess()
        Using (traceManager.StartTrace("Data Access Events"))
            ' Peform work here

            ' Assume an error condition was detected - perform some troubleshooting.
            DoTroubleShooting()
        End Using
    End Sub

    Private Sub DoTroubleShooting()
        Dim logMessage As String = "Simulated troubleshooting message for Logging QuickStart. " & _
          "Current activity=""" & Trace.CorrelationManager.ActivityId.ToString() & """"""

        Dim logEntry As New LogEntry()

        logEntry.Categories.Clear()
        logEntry.Categories.Add("Troubleshooting")
        logEntry.Priority = 5
        logEntry.Severity = TraceEventType.Error
        logEntry.Message = logMessage

        logWriter.Write(logEntry)
    End Sub

    Private Sub customizedSinkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles customizedSinkButton.Click
        DisplayScenarioStart(My.Resources.CustomizedSinkStartMessage)

        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim log As New LogEntry()
            log.Message = My.Resources.DebugSinkTestMessage
            log.Priority = 5
            log.EventId = 100
            log.Categories.Clear()
            log.Categories.Add("Debug")
            logWriter.Write(log)
            DisplayResults(My.Resources.CustomizedSinkEndMessage)

        Catch ex As Exception
            ProcessUnhandledException(ex)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub checkLogginButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkLogginButton.Click
        Dim filterQueryForm As New FilterQueryForm()

        Dim result As Windows.Forms.DialogResult = filterQueryForm.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            Try

                Windows.Forms.Cursor.Current = Cursors.WaitCursor

                DisplayScenarioStart(My.Resources.CheckFilterStartMessage)

                Dim categories As ICollection(Of String) = filterQueryForm.Categories
                Dim priority As Integer = filterQueryForm.Priority
                Dim names As String = GetCategoriesString(categories)

                resultsTextBox.Text &= "Log enabled filter check" & Environment.NewLine
                resultsTextBox.Text &= "==================" & Environment.NewLine

                ' ----------------------------------------------------------------------
                ' Query the logging enabled filter to determine if logging is enabled.
                ' ----------------------------------------------------------------------
                If logWriter.GetFilter(Of LogEnabledFilter)().Enabled Then
                    ' Logging is enabled.
                    resultsTextBox.Text &= "Logging is enabled." & Environment.NewLine
                Else
                    ' Logging is not enabled. Events will not be logged. Your application can avoid the performance
                    ' penalty of collecting information for an event that will not be
                    ' loggeed.
                    resultsTextBox.Text &= "Logging is not enabled." & Environment.NewLine
                End If

                resultsTextBox.Text &= Environment.NewLine
                resultsTextBox.Text &= "Category filter check" & Environment.NewLine
                resultsTextBox.Text &= "==================" & Environment.NewLine

                ' ----------------------------------------------------------------------
                ' Query the category filter to determine if the categories selected by the
                ' user would pass the filter check.
                ' ----------------------------------------------------------------------
                If logWriter.GetFilter(Of CategoryFilter)().ShouldLog(categories) Then
                    ' Event will be logged according to currently configured filters.
                    ' Perform operations (possibly expensive) to gather information for the 
                    ' event to be logged. For this QuickStart, we simply display the
                    ' result of the query.
                    resultsTextBox.Text &= "The selected categories (" & names & ") pass filter check." & Environment.NewLine
                Else
                    ' Event will not be logged. You application can avoid the performance
                    ' penalty of collecting information for an event that will not be
                    ' loggeed.
                    resultsTextBox.Text &= "The selected categories (" & names & ") do not pass filter check." & Environment.NewLine
                End If

                resultsTextBox.Text &= Environment.NewLine
                resultsTextBox.Text &= "Priority filter check" & Environment.NewLine
                resultsTextBox.Text &= "==================" & Environment.NewLine

                ' ----------------------------------------------------------------------
                ' Query the priority filter to determine if the priority selected by the
                ' user would pass the filter check.
                ' ----------------------------------------------------------------------
                If logWriter.GetFilter(Of PriorityFilter)().ShouldLog(priority) Then
                    ' Perform possibly expensive operations gather information for the 
                    ' event to be logged. For the QuickStart, we simply display the
                    ' result of the query.
                    resultsTextBox.Text &= "The selected priority (" & priority.ToString() & ") passes the filter check." & Environment.NewLine
                Else
                    ' Event will not be logged. You application can avoid the performance
                    ' penalty of collection information for an even that will not be
                    ' loggeed.
                    resultsTextBox.Text &= "The selected priority (" & priority.ToString() & ") does not pass the filter check." & Environment.NewLine
                End If

                resultsTextBox.Text &= Environment.NewLine
                resultsTextBox.Text &= "Event check" & Environment.NewLine
                resultsTextBox.Text &= "===========" & Environment.NewLine

                ' Create a new log entry to demonstrate how to query if an existing log
                ' entry will be logged.
                Dim logEntry As New LogEntry()
                logEntry.Message = "Demonstrate filter check"
                logEntry.Priority = priority
                logEntry.EventId = 100

                For Each category As String In categories
                    logEntry.Categories.Add(category)
                Next

                ' ----------------------------------------------------------------------
                ' Query the LogWriter class to determine if an event with the
                ' specified priority and categories would pass the filter checks.
                ' ----------------------------------------------------------------------
                If (logWriter.ShouldLog(logEntry)) Then
                    ' Perform possibly expensive operations gather information for the 
                    ' event to be logged. For the QuickStart, we simply display the
                    ' result of the query.
                    resultsTextBox.Text &= "An event with the selected priority (" & priority.ToString() & ") and " & _
                        "categories (" & names & ") passes the filter check." & Environment.NewLine
                Else
                    If Not logWriter.GetFilter(Of LogEnabledFilter)().Enabled Then
                        ' Logging is not enabled.
                        resultsTextBox.Text &= "Logging is not enabled. The event will not be logged." & Environment.NewLine
                    Else

                        ' Event will not be logged. You application can avoid the performance
                        ' penalty of collection information for an even that will not be
                        ' loggeed.
                        resultsTextBox.Text &= "An event with the selected priority (" & priority.ToString() & ") and " & _
                            "categories (" & names & ") does not pass the filter check." & Environment.NewLine
                    End If
                End If

            Catch ex As Exception
                ProcessUnhandledException(ex)
            Finally
                Windows.Forms.Cursor.Current = Cursors.Default
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Helper method to construct a string contianing a serial list of categories in a 
    ''' collection.
    ''' </summary>
    ''' <param name="categories">Collection of category names</param>
    ''' <returns>Comma-separated list of category names</returns>
    Private Function GetCategoriesString(ByVal categories As ICollection(Of String)) As String
        Dim namesBuilder = New StringBuilder()

        For Each category As String In categories
            namesBuilder.Append(category & ", ")
        Next

        Dim names As String = namesBuilder.ToString()
        If names.Length > 0 Then
            names = names.Substring(0, names.Length - 2)
        End If

        If names.Length = 0 Then
            names = "none selected, using default category"
        End If
        Return names
    End Function
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
        errorMessage.Append("The following error occured during execution of the Logging QuickStart.")
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
