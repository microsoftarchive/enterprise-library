//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using ExceptionHandlingQuickStart.BusinessLayer;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ExceptionHandlingQuickStart
{
    /// <summary>
    /// Enterprise Library Exception Handling Application Block Quick Start.
    /// </summary>
    public class QuickStartForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox;

        private Process viewerProcess = null;
        private System.Windows.Forms.Button logExceptionButton;
        private System.Windows.Forms.Button notifyUserButton;
        private System.Windows.Forms.Button viewWalkthroughButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TextBox resultsTextBox;

        private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic ExceptionhandlingQS2";

        public static System.Windows.Forms.Form AppForm;

        public QuickStartForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
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
            this.logExceptionButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.notifyUserButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.Location = new System.Drawing.Point(192, 88);
            this.resultsTextBox.Multiline = true;
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.ReadOnly = true;
            this.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultsTextBox.Size = new System.Drawing.Size(614, 272);
            this.resultsTextBox.TabIndex = 3;
            this.resultsTextBox.TabStop = false;
            // 
            // logExceptionButton
            // 
            this.logExceptionButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.logExceptionButton.Location = new System.Drawing.Point(19, 96);
            this.logExceptionButton.Name = "logExceptionButton";
            this.logExceptionButton.Size = new System.Drawing.Size(154, 46);
            this.logExceptionButton.TabIndex = 1;
            this.logExceptionButton.Text = "&Log an exception";
            this.logExceptionButton.Click += new System.EventHandler(this.logExceptionButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.logoPictureBox);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(0, -9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(845, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(517, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Exception Handling With Logging";
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Location = new System.Drawing.Point(730, 16);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(69, 50);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.viewWalkthroughButton);
            this.groupBox.Controls.Add(this.quitButton);
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Location = new System.Drawing.Point(0, 376);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(845, 101);
            this.groupBox.TabIndex = 4;
            this.groupBox.TabStop = false;
            // 
            // viewWalkthroughButton
            // 
            this.viewWalkthroughButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.viewWalkthroughButton.Location = new System.Drawing.Point(528, 28);
            this.viewWalkthroughButton.Name = "viewWalkthroughButton";
            this.viewWalkthroughButton.Size = new System.Drawing.Size(125, 37);
            this.viewWalkthroughButton.TabIndex = 0;
            this.viewWalkthroughButton.Text = "View &Walkthrough";
            this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.quitButton.Location = new System.Drawing.Point(672, 28);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(125, 37);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "&Quit";
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // notifyUserButton
            // 
            this.notifyUserButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.notifyUserButton.Location = new System.Drawing.Point(19, 159);
            this.notifyUserButton.Name = "notifyUserButton";
            this.notifyUserButton.Size = new System.Drawing.Size(154, 47);
            this.notifyUserButton.TabIndex = 2;
            this.notifyUserButton.Text = "&Notify the user when an exception occurs";
            this.notifyUserButton.Click += new System.EventHandler(this.notifyUserButton_Click);
            // 
            // QuickStartForm
            // 
            this.CancelButton = this.quitButton;
            this.ClientSize = new System.Drawing.Size(834, 458);
            this.Controls.Add(this.notifyUserButton);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.logExceptionButton);
            this.Controls.Add(this.resultsTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "QuickStartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exception Handling Application Block Quick Start";
            this.Load += new System.EventHandler(this.QuickStartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.groupBox.ResumeLayout(false);
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
            AppForm = new QuickStartForm();
            Application.Run(AppForm);
        }

        private void QuickStartForm_Load(object sender, System.EventArgs e)
        {
            // Initialize image to embedded logo
            logoPictureBox.Image = GetEmbeddedImage("ExceptionHandlingQuickStart.logo.gif");
        }

        private System.Drawing.Image GetEmbeddedImage(string resourceName)
        {
            Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                return null;
            }

            System.Drawing.Image img = System.Drawing.Image.FromStream(resourceStream);

            return img;
        }


        private void DisplayScenarioStart(string scenarioDescription)
        {
            this.resultsTextBox.Text = scenarioDescription + Environment.NewLine + Environment.NewLine;
            this.resultsTextBox.Update();
        }

        private void DisplayResults(string results)
        {
            this.resultsTextBox.Text += results;
        }

        private string GetExceptionInfo(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            AppTextExceptionFormatter formatter = new AppTextExceptionFormatter(writer, ex);

            // Format the exception
            formatter.Format();

            return sb.ToString();
        }

        private void quitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Routine that causes an exception to be thrown
        /// </summary>
        private void Process()
        {
            throw new Exception("Quick Start Generated Exception");
        }

        private void notifyUserButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                StringBuilder sb = new StringBuilder();

                sb.Append("Scenario: Notify the user when an exception occurs");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("1. An exception occurs and is detected in the Business layer.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("2. The Business layer specifies the \"Notify Policy\" as the exception handling policy.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("3. The \"Notify Policy\" is configured to first log the exception, then replace the exception with a new one, and finally return to the application by recommending a rethrow.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("4. The exception is propagated to and caught by the UI layer.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("5. The UI layer catches the exception and calls the \"Global Policy\", which displays the exception in a message box.");
                sb.Append(Environment.NewLine);
                DisplayScenarioStart(sb.ToString());

                AppService svc = new AppService();

                svc.ProcessAndNotify();

                Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }
        }

        private void logExceptionButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                StringBuilder sb = new StringBuilder();

                sb.Append("Scenario: Log exception");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("1. An exception occurs and is detected in the UI layer.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("2. The UI layer specifies the \"Log Only Policy\" as the exception handling policy.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("3. The \"Log Only Policy\" is configured to log the exception and return to the application without recommending a rethrow.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("4. Control is returned to the UI layer.");
                sb.Append(Environment.NewLine);

                DisplayScenarioStart(sb.ToString());

                try
                {
                    Process();
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Log Only Policy");

                    DisplayResults("**Exception has been logged. See the currently configured log destination (default is event log) for exception details.");

                    if (rethrow)
                    {
                        throw;
                    }
                }

                Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                ProcessUnhandledException(ex);
            }

        }

		/// <summary>
		/// Returns the path and executable name for the help viewer.
		/// </summary>
		private string GetHelpViewerExecutable()
		{
            string commonX86 = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
            if (!string.IsNullOrEmpty(commonX86))
            {
                string pathX86 = Path.Combine(commonX86, @"Microsoft Shared\Help 9\dexplore.exe");
                if (File.Exists(pathX86))
                {
                    return pathX86;
                }
            }
            string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
            return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
        }

        /// <summary>
        /// Displays Quick Start help topics using the Help 2 Viewer.
        /// </summary>
        private void viewWalkthroughButton_Click(object sender, System.EventArgs e)
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

        /// <summary>
        /// Process any unhandled exceptions that occur in the application.
        /// This code is called by all UI entry points in the application (e.g. button click events)
        /// when an unhandled exception occurs.
        /// You could also achieve this by handling the Application.ThreadException event, however
        /// the VS2005 debugger will break before this event is called.
        /// </summary>
        /// <param name="ex">The unhandled exception</param>
        private void ProcessUnhandledException(Exception ex)
        {
            // An unhandled exception occured somewhere in our application. Let
            // the 'Global Policy' handler have a try at handling it.
            try
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Global Policy");
                if (rethrow)
                {
                    // Something has gone very wrong - exit the application.
                    Application.Exit();
                }
            }
            catch
            {
                // Something has gone wrong during HandleException (e.g. incorrect configuration of the block).
                // Exit the application
                string errorMsg = "An unexpected exception occured while calling HandleException with policy 'Global Policy'. ";
                errorMsg += "Please check the event log for details about the exception." + Environment.NewLine + Environment.NewLine;

                MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Application.Exit();
            }
            QuickStartForm.AppForm.Cursor = System.Windows.Forms.Cursors.Default;
        }
    }
}
