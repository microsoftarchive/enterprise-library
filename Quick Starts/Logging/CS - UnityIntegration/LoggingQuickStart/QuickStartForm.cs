//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block QuickStart
//===============================================================================
// Copyright ? Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace LoggingQuickStart
{
    /// <summary>
    /// Enterprise Library Logging Block Quick Start Sample.
    /// </summary>
    public class QuickStartForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private GroupBox groupBox1;
        private Label label2;
        private GroupBox groupBox;
        private TextBox resultsTextBox;
        private Button viewWalkthroughButton;
        private Button quitButton;
        private Button logEventInformationButton;
        private PictureBox logoPictureBox;

        // Names for log files.
        private const string TRACE_LOG_FILE_NAME = "trace.log";
        private Button traceButton;
        private Button customizedSinkButton;

        private Process viewerProcess = null;
        private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic LoggingQS1";
        private System.Windows.Forms.Button logExtraInformationButton;
        private Button checkLogginButton;
        private GroupBox groupBox2;
        private Button viewTraceLogButton;
        private Button viewEventLogButton;

        private LogWriter logWriter;
        private const string containerName = "loggingContainer";
        
        private TraceManager traceManager;
        public static Form AppForm;

        public QuickStartForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public QuickStartForm(LogWriter logWriter, TraceManager traceManager)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.logWriter = logWriter;
            this.traceManager = traceManager;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.resultsTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.logEventInformationButton = new System.Windows.Forms.Button();
            this.traceButton = new System.Windows.Forms.Button();
            this.customizedSinkButton = new System.Windows.Forms.Button();
            this.logExtraInformationButton = new System.Windows.Forms.Button();
            this.checkLogginButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.viewTraceLogButton = new System.Windows.Forms.Button();
            this.viewEventLogButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.Location = new System.Drawing.Point(223, 76);
            this.resultsTextBox.Multiline = true;
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.ReadOnly = true;
            this.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultsTextBox.Size = new System.Drawing.Size(473, 363);
            this.resultsTextBox.TabIndex = 20;
            this.resultsTextBox.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.logoPictureBox);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(-10, -8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(722, 72);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(244, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Logging";
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Location = new System.Drawing.Point(601, 14);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(58, 43);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.viewWalkthroughButton);
            this.groupBox.Controls.Add(this.quitButton);
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Location = new System.Drawing.Point(-25, 449);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(737, 86);
            this.groupBox.TabIndex = 7;
            this.groupBox.TabStop = false;
            // 
            // viewWalkthroughButton
            // 
            this.viewWalkthroughButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.viewWalkthroughButton.Location = new System.Drawing.Point(420, 24);
            this.viewWalkthroughButton.Name = "viewWalkthroughButton";
            this.viewWalkthroughButton.Size = new System.Drawing.Size(113, 32);
            this.viewWalkthroughButton.TabIndex = 0;
            this.viewWalkthroughButton.Text = "View &Walkthrough";
            this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.quitButton.Location = new System.Drawing.Point(553, 24);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(114, 32);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "&Quit";
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // logEventInformationButton
            // 
            this.logEventInformationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.logEventInformationButton.Location = new System.Drawing.Point(20, 76);
            this.logEventInformationButton.Name = "logEventInformationButton";
            this.logEventInformationButton.Size = new System.Drawing.Size(158, 45);
            this.logEventInformationButton.TabIndex = 0;
            this.logEventInformationButton.Text = "&Log event information";
            this.logEventInformationButton.Click += new System.EventHandler(this.logEventInformationButton_Click);
            // 
            // traceButton
            // 
            this.traceButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.traceButton.Location = new System.Drawing.Point(20, 181);
            this.traceButton.Name = "traceButton";
            this.traceButton.Size = new System.Drawing.Size(158, 45);
            this.traceButton.TabIndex = 2;
            this.traceButton.Text = "&Trace activities and propogate context information";
            this.traceButton.Click += new System.EventHandler(this.traceButton_Click);
            // 
            // customizedSinkButton
            // 
            this.customizedSinkButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.customizedSinkButton.Location = new System.Drawing.Point(20, 233);
            this.customizedSinkButton.Name = "customizedSinkButton";
            this.customizedSinkButton.Size = new System.Drawing.Size(158, 45);
            this.customizedSinkButton.TabIndex = 3;
            this.customizedSinkButton.Text = "Log an event using a customized &sink";
            this.customizedSinkButton.Click += new System.EventHandler(this.customizedSinkButton_Click);
            // 
            // logExtraInformationButton
            // 
            this.logExtraInformationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.logExtraInformationButton.Location = new System.Drawing.Point(20, 129);
            this.logExtraInformationButton.Name = "logExtraInformationButton";
            this.logExtraInformationButton.Size = new System.Drawing.Size(158, 45);
            this.logExtraInformationButton.TabIndex = 1;
            this.logExtraInformationButton.Text = "&Populate a log message with additional context information";
            this.logExtraInformationButton.Click += new System.EventHandler(this.logExtraInformationButton_Click);
            // 
            // checkLogginButton
            // 
            this.checkLogginButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkLogginButton.Location = new System.Drawing.Point(21, 284);
            this.checkLogginButton.Name = "checkLogginButton";
            this.checkLogginButton.Size = new System.Drawing.Size(158, 45);
            this.checkLogginButton.TabIndex = 4;
            this.checkLogginButton.Text = "&Determine if event will be logged";
            this.checkLogginButton.Click += new System.EventHandler(this.checkLogFilterButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.viewTraceLogButton);
            this.groupBox2.Controls.Add(this.viewEventLogButton);
            this.groupBox2.Location = new System.Drawing.Point(21, 335);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 104);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Trace listener viewers";
            // 
            // viewTraceLogButton
            // 
            this.viewTraceLogButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.viewTraceLogButton.Location = new System.Drawing.Point(14, 19);
            this.viewTraceLogButton.Name = "viewTraceLogButton";
            this.viewTraceLogButton.Size = new System.Drawing.Size(132, 32);
            this.viewTraceLogButton.TabIndex = 0;
            this.viewTraceLogButton.Text = "View T&race Log";
            this.viewTraceLogButton.Click += new System.EventHandler(this.viewTraceLogButton_Click);
            // 
            // viewEventLogButton
            // 
            this.viewEventLogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.viewEventLogButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.viewEventLogButton.Location = new System.Drawing.Point(14, 60);
            this.viewEventLogButton.Name = "viewEventLogButton";
            this.viewEventLogButton.Size = new System.Drawing.Size(132, 32);
            this.viewEventLogButton.TabIndex = 1;
            this.viewEventLogButton.Text = "View &Event Log";
            this.viewEventLogButton.Click += new System.EventHandler(this.viewEventLogButton_Click);
            // 
            // QuickStartForm
            // 
            this.CancelButton = this.quitButton;
            this.ClientSize = new System.Drawing.Size(706, 516);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkLogginButton);
            this.Controls.Add(this.logExtraInformationButton);
            this.Controls.Add(this.customizedSinkButton);
            this.Controls.Add(this.traceButton);
            this.Controls.Add(this.logEventInformationButton);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.resultsTextBox);
            this.MaximizeBox = false;
            this.Name = "QuickStartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logging Application Block Quick Start";
            this.Load += new System.EventHandler(this.QuickStartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers[containerName].Configure(container);

            AppForm = container.Resolve<QuickStartForm>();
            // Unhandled exceptions will be delivered to our ThreadException handler
            Application.ThreadException += new ThreadExceptionEventHandler(AppThreadException);
            Application.Run(AppForm);
        }

        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        private static void AppThreadException(object source, ThreadExceptionEventArgs e)
        {
            ProcessUnhandledException(e.Exception);
        }

        private void QuickStartForm_Load(object sender, EventArgs e)
        {
            // Initialize image to embedded logo
            this.logoPictureBox.Image = GetEmbeddedImage("LoggingQuickStart.logo.gif");

            DisplayConfiguration();
        }

        private Image GetEmbeddedImage(string resourceName)
        {
            Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                return null;
            }

            Image img = Image.FromStream(resourceStream);

            return img;
        }
        private void DisplayConfiguration()
        {
            try
            {
                // Get configuration settings for Logging and Instrmentation Application Block. 
                // This assumes the configuration source is the SystemConfigurationSource, which
                // is the default setting when the QuickStart ships.
                LoggingSettings settings = LoggingSettings.GetLoggingSettings(new SystemConfigurationSource());

                string defaultCategory = settings.DefaultCategory;

                StringBuilder results = new StringBuilder();

                results.Append("Current Configuration");
                results.Append(Environment.NewLine);
                results.Append("---------------------------------");
                results.Append(Environment.NewLine);
                results.Append(Environment.NewLine);
                results.Append("Default Category: " + settings.DefaultCategory + Environment.NewLine + Environment.NewLine);
                results.Append("Categories and category listeners");
                results.Append(Environment.NewLine);
                results.Append(Environment.NewLine);

                // Grab the list of categories and loop through for display.
                NamedElementCollection<TraceSourceData> sources = settings.TraceSources;

                foreach (TraceSourceData source in sources)
                {
                    results.Append("   " + source.Name);

                    // Flag any of the categories that would be denied based upon
                    // the current category filter configuration.
                    if (!logWriter.GetFilter<CategoryFilter>().ShouldLog(source.Name))
                    {
                        results.Append("*");
                    }

                    // Loop through the list of trace listeners for the category.
                    NamedElementCollection<TraceListenerReferenceData> TraceListeners = source.TraceListeners;

                    StringBuilder listener = new StringBuilder();
                    listener.Append("  -  ");
                    foreach (TraceListenerReferenceData listenerData in TraceListeners)
                    {
                        listener.Append(listenerData.Name + ", ");
                    }
                    // Remove trailing comma and space
                    listener.Remove(listener.Length - 2, 2);
                    results.Append(listener.ToString());
                    results.Append(Environment.NewLine);
                }
                results.Append(Environment.NewLine);
                results.Append("   * Events in category will not be logged");

                this.resultsTextBox.Text += results.ToString();
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void DisplayResults(string results)
        {
            this.resultsTextBox.Text += results + Environment.NewLine;
            this.resultsTextBox.SelectAll();
            this.resultsTextBox.ScrollToCaret();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayScenarioStart(string message)
        {
            this.resultsTextBox.Text = message + Environment.NewLine + Environment.NewLine;
            this.resultsTextBox.Update();
        }

        private void logEventInformationButton_Click(object sender, EventArgs e)
        {
            EventInformationForm eventForm = new EventInformationForm();

            DialogResult result = eventForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // Creates and fills the log entry with user information
                    LogEntry logEntry = new LogEntry();
                    logEntry.EventId = eventForm.EventId;
                    logEntry.Priority = eventForm.Priority;
                    logEntry.Message = eventForm.Message;
                    logEntry.Categories.Clear();

                    // Add the categories selected by the user
                    foreach (string category in eventForm.Categories)
                    {
                        logEntry.Categories.Add(category);
                    }

                    DisplayScenarioStart(String.Format(Properties.Resources.LogEventStartMessage, logEntry.ToString()));

                    // Writes the log entry.
                    logWriter.Write(logEntry);

                    this.DisplayResults(String.Format(Properties.Resources.EventProcessedMessage, logEntry.EventId));
                }
                catch (Exception ex)
                {
                    ProcessUnhandledException(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void logExtraInformationButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.DisplayScenarioStart(Properties.Resources.ExtraInformationStartMessage);

                // Use the WindowsPrincipal as the current principal. This will cause the 
                // ManagedSecurityContextInformationProvider to add the current Windows user's name
                // to the additional information to be logged.
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                // Create the dictionary to hold the extra information, and populate it
                // with managed security information.  
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
                ManagedSecurityContextInformationProvider informationHelper = new ManagedSecurityContextInformationProvider();

                informationHelper.PopulateDictionary(dictionary);

                // Add a custom property for screen resolution
                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                string resolution = String.Format("{0}x{1}", width, height);

                dictionary.Add("Screen resolution", resolution);

                // Write the log entry that contains the extra information
                LogEntry logEntry = new LogEntry();
                logEntry.Message = "Log entry with extra information";
                logEntry.ExtendedProperties = dictionary;

                logWriter.Write(logEntry);

                this.DisplayResults(Properties.Resources.ExtraInformationEndMessage);
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void traceButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DisplayScenarioStart(Properties.Resources.TraceStartMessage);

                using (traceManager.StartTrace("Trace"))
                {
                    DoDataAccess();
                }
                this.DisplayResults(String.Format(Properties.Resources.TraceDoneMessage, TRACE_LOG_FILE_NAME));
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void DoDataAccess()
        {
            using (traceManager.StartTrace("Data Access Events"))
            {
                // Peform work here

                // Assume an error condition was detected - perform some troubleshooting.
                DoTroubleShooting();
            }
        }

        private void DoTroubleShooting()
        {
            string logMessage = "Simulated troubleshooting message for Logging QuickStart. " +
              "Current activity=\"" + Trace.CorrelationManager.ActivityId + "\"";

            LogEntry logEntry = new LogEntry();

            logEntry.Categories.Clear();
            logEntry.Categories.Add("Troubleshooting");
            logEntry.Priority = 5;
            logEntry.Severity = TraceEventType.Error;
            logEntry.Message = logMessage;

            logWriter.Write(logEntry);
        }

        private void customizedSinkButton_Click(object sender, EventArgs e)
        {
            DisplayScenarioStart(Properties.Resources.CustomizedSinkStartMessage);

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LogEntry log = new LogEntry();
                log.Message = Properties.Resources.DebugSinkTestMessage;
                log.Priority = 5;
                log.EventId = 100;
                log.Categories.Clear();
                log.Categories.Add("Debug");
                
                logWriter.Write(log);

                LogEntry logEntry = new LogEntry();
                logEntry.Message = "My Message";
                logEntry.Categories.Add("Debug");

                logWriter.Write(logEntry);                

                this.DisplayResults(Properties.Resources.CustomizedSinkEndMessage);
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void checkLogFilterButton_Click(object sender, EventArgs e)
        {
            FilterQueryForm filterQueryForm = new FilterQueryForm();

            DialogResult result = filterQueryForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    DisplayScenarioStart(Properties.Resources.CheckFilterStartMessage);

                    ICollection<string> categories = filterQueryForm.Categories;
                    int priority = filterQueryForm.Priority;
                    string names = GetCategoriesString(categories);

                    this.resultsTextBox.Text += "Log enabled filter check" + Environment.NewLine;
                    this.resultsTextBox.Text += "==================" + Environment.NewLine;

                    // ----------------------------------------------------------------------
                    // Query the logging enabled filter to determine if logging is enabled.
                    // ----------------------------------------------------------------------
                    if (logWriter.GetFilter<LogEnabledFilter>().Enabled)
                    {
                        // Logging is enabled.
                        this.resultsTextBox.Text += "Logging is enabled." + Environment.NewLine;
                    }
                    else
                    {
                        // Logging is not enabled. Events will not be logged. Your application can avoid the performance
                        // penalty of collecting information for an event that will not be
                        // loggeed.
                        this.resultsTextBox.Text += "Logging is not enabled." + Environment.NewLine;
                    }

                    this.resultsTextBox.Text += Environment.NewLine;
                    this.resultsTextBox.Text += "Category filter check" + Environment.NewLine;
                    this.resultsTextBox.Text += "==================" + Environment.NewLine;
                    
                    // ----------------------------------------------------------------------
                    // Query the category filter to determine if the categories selected by the
                    // user would pass the filter check.
                    // ----------------------------------------------------------------------
                    if (logWriter.GetFilter<CategoryFilter>().ShouldLog(categories))
                    {
                        // Event will be logged according to currently configured filters.
                        // Perform operations (possibly expensive) to gather information for the 
                        // event to be logged. For this QuickStart, we simply display the
                        // result of the query.
                        this.resultsTextBox.Text += "The selected categories (" + names + ") pass filter check." + Environment.NewLine;
                    }
                    else
                    {
                        // Event will not be logged. You application can avoid the performance
                        // penalty of collecting information for an event that will not be
                        // loggeed.
                        this.resultsTextBox.Text += "The selected categories (" + names + ") do not pass filter check." + Environment.NewLine;
                    }

                    this.resultsTextBox.Text += Environment.NewLine;
                    this.resultsTextBox.Text += "Priority filter check" + Environment.NewLine;
                    this.resultsTextBox.Text += "==================" + Environment.NewLine;

                    // ----------------------------------------------------------------------
                    // Query the priority filter to determine if the priority selected by the
                    // user would pass the filter check.
                    // ----------------------------------------------------------------------
                    if (logWriter.GetFilter<PriorityFilter>().ShouldLog(priority))
                    {
                        // Perform possibly expensive operations gather information for the 
                        // event to be logged. For the QuickStart, we simply display the
                        // result of the query.
                        this.resultsTextBox.Text += "The selected priority (" + priority.ToString() + ") passes the filter check." + Environment.NewLine;
                    }
                    else
                    {
                        // Event will not be logged. You application can avoid the performance
                        // penalty of collection information for an even that will not be
                        // loggeed.
                        this.resultsTextBox.Text += "The selected priority (" + priority.ToString() + ") does not pass the filter check." + Environment.NewLine;
                    }

                    this.resultsTextBox.Text += Environment.NewLine;
                    this.resultsTextBox.Text += "Event check" + Environment.NewLine;
                    this.resultsTextBox.Text += "===========" + Environment.NewLine;

                    // Create a new log entry to demonstrate how to query if an existing log
                    // entry will be logged.
                    LogEntry logEntry = new LogEntry();
                    logEntry.Message = "Demonstrate filter check";
                    logEntry.Priority = priority;
                    logEntry.EventId = 100;

                    foreach (string category in categories)
                    {
                        logEntry.Categories.Add(category);
                    }

                    // ----------------------------------------------------------------------
                    // Query the LogWriter class to determine if an event with the
                    // specified priority and categories would pass the filter checks.
                    // ----------------------------------------------------------------------
                    if (logWriter.ShouldLog(logEntry))
                    {
                        // Perform possibly expensive operations gather information for the 
                        // event to be logged. For the QuickStart, we simply display the
                        // result of the query.
                        this.resultsTextBox.Text += "An event with the selected priority (" + priority.ToString() + ") and " +
                            "categories (" + names + ") passes the filter check." + Environment.NewLine;
                    }
                    else
                    {
                        // Event will not be logged. You application can avoid the performance
                        // penalty of collection information for an even that will not be
                        // loggeed.
                        if (!logWriter.GetFilter<LogEnabledFilter>().Enabled)
                        {
                            this.resultsTextBox.Text += "Logging is not enabled. The event will not be logged." + Environment.NewLine;
                        }
                        else
                        {
                            this.resultsTextBox.Text += "An event with the selected priority (" + priority.ToString() + ") and " +
                                "categories (" + names + ") does not pass the filter check." + Environment.NewLine;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ProcessUnhandledException(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

        }
        /// <summary>
        /// Helper method to construct a string contianing a serial list of categories in a 
        /// collection.
        /// </summary>
        /// <param name="categories">Collection of category names</param>
        /// <returns>Comma-separated list of category names</returns>
        private string GetCategoriesString(ICollection<string> categories)
        {
            StringBuilder namesBuilder = new StringBuilder();

            foreach (string category in categories)
            {
                namesBuilder.Append(category + ", ");
            }

            string names = namesBuilder.ToString();
            if (names.Length > 0)
            {
                names = names.Substring(0, names.Length - 2);
            }

            if (names.Length == 0)
            {
                names = "none selected, using default category";
            }
            return names;
        }

		/// <summary>
		/// Returns the path and executable name for the help viewer.
		/// </summary>
		private string GetHelpViewerExecutable()
		{
			string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
			return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
		}

        private void viewWalkthroughButton_Click(object sender, EventArgs e)
        {
            // Process has never been started. Initialize and launch the viewer.
            if (this.viewerProcess == null)
            {
                // Initialize the Process information for the help viewer
                this.viewerProcess = new Process();

				this.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable();
                this.viewerProcess.StartInfo.Arguments = HelpViewerArguments;
                this.viewerProcess.Start();
            }
            else if (this.viewerProcess.HasExited)
            {
                // Process previously started, then exited. Start the process again.
                this.viewerProcess.Start();
            }
            else
            {
                // Process was already started - bring it to the foreground
                IntPtr hWnd = this.viewerProcess.MainWindowHandle;
                if (NativeMethods.IsIconic(hWnd))
                {
                    NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
                }
                NativeMethods.SetForegroundWindow(hWnd);
            }
        }

        private void viewEventLogButton_Click(object sender, EventArgs e)
        {
            Process traceFileViewerProcess = new Process();

            string executable = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\eventvwr.msc");
            traceFileViewerProcess.StartInfo.FileName = executable;

            traceFileViewerProcess.StartInfo.Arguments = "/s";
            traceFileViewerProcess.Start();
        }

        private void viewTraceLogButton_Click(object sender, EventArgs e)
        {
            Process traceFileViewerProcess = new Process();

            traceFileViewerProcess.StartInfo.FileName = "Notepad.exe";
            traceFileViewerProcess.StartInfo.Arguments = "Trace.log";
            traceFileViewerProcess.Start();
        }
        /// <summary>
        /// Process any unhandled exceptions that occur in the application.
        /// This code is called by all UI entry points in the application (e.g. button click events)
        /// when an unhandled exception occurs.
        /// You could also achieve this by handling the Application.ThreadException event, however
        /// the VS2005 debugger will break before this event is called.
        /// </summary>
        /// <param name="ex">The unhandled exception</param>
        private static void ProcessUnhandledException(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendFormat(new CultureInfo("en-us", true), "The following error occured during execution of the Logging QuickStart.\n\n{0}\n\n", ex.Message);
            errorMessage.Append("Exceptions can be caused by invalid configuration information.\n");
            errorMessage.Append(Environment.NewLine);
            errorMessage.Append("Do you want to exit the application?");

            DialogResult result = MessageBox.Show(errorMessage.ToString(), "Application Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            QuickStartForm.AppForm.Cursor = Cursors.Default;
        }
    }
}
