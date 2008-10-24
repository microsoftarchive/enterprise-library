//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using System.Threading;


namespace ConfigurationMigrationQuickStart
{
    /// <summary>
    /// Enterprise Library Configuration Block Quick Start Sample.
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
        private System.Windows.Forms.TextBox readResultsTextBox;
        private System.Windows.Forms.Button readXmlConfigDataButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Button clearCacheButton;
        private System.Windows.Forms.RichTextBox readSampleTextBox;
        private System.Windows.Forms.CheckBox automaticRefreshCheckBox;

        private Process viewerProcess = null;
        private const string HelpViewerExecutable = "dexplore.exe";
        private const string HelpTopicNamespace = @"ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct";
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox writeSampleTextBox;
        private Button writeXmlConfigDataButton;
        private TextBox writeResultsTextBox;
        private FontDialog fontDialog;
        private FileSystemWatcher watcher;
        private Label label1;
    
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
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickStartForm));
            this.readResultsTextBox = new System.Windows.Forms.TextBox();
            this.readXmlConfigDataButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.quitButton = new System.Windows.Forms.Button();
            this.readSampleTextBox = new System.Windows.Forms.RichTextBox();
            this.clearCacheButton = new System.Windows.Forms.Button();
            this.automaticRefreshCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.writeSampleTextBox = new System.Windows.Forms.RichTextBox();
            this.writeXmlConfigDataButton = new System.Windows.Forms.Button();
            this.writeResultsTextBox = new System.Windows.Forms.TextBox();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.groupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // readResultsTextBox
            // 
            this.readResultsTextBox.Location = new System.Drawing.Point(173, 13);
            this.readResultsTextBox.Multiline = true;
            this.readResultsTextBox.Name = "readResultsTextBox";
            this.readResultsTextBox.ReadOnly = true;
            this.readResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.readResultsTextBox.Size = new System.Drawing.Size(496, 104);
            this.readResultsTextBox.TabIndex = 3;
            this.readResultsTextBox.TabStop = false;
            // 
            // readXmlConfigDataButton
            // 
            this.readXmlConfigDataButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readXmlConfigDataButton.Location = new System.Drawing.Point(16, 20);
            this.readXmlConfigDataButton.Name = "readXmlConfigDataButton";
            this.readXmlConfigDataButton.Size = new System.Drawing.Size(136, 40);
            this.readXmlConfigDataButton.TabIndex = 1;
            this.readXmlConfigDataButton.Text = "&Read configuration data from an XML file ";
            this.readXmlConfigDataButton.Click += new System.EventHandler(this.readXmlConfigDataButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.logoPictureBox);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(0, -8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(384, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Configuration Migration QuickStart";
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Location = new System.Drawing.Point(608, 14);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(69, 50);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.quitButton);
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Location = new System.Drawing.Point(-12, 383);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(713, 89);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            // 
            // quitButton
            // 
            this.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.quitButton.Location = new System.Drawing.Point(573, 24);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(104, 32);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "&Quit";
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // readSampleTextBox
            // 
            this.readSampleTextBox.Location = new System.Drawing.Point(173, 131);
            this.readSampleTextBox.Name = "readSampleTextBox";
            this.readSampleTextBox.ReadOnly = true;
            this.readSampleTextBox.Size = new System.Drawing.Size(494, 83);
            this.readSampleTextBox.TabIndex = 4;
            this.readSampleTextBox.Text = "Sample Text";
            // 
            // clearCacheButton
            // 
            this.clearCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clearCacheButton.Location = new System.Drawing.Point(16, 76);
            this.clearCacheButton.Name = "clearCacheButton";
            this.clearCacheButton.Size = new System.Drawing.Size(136, 40);
            this.clearCacheButton.TabIndex = 2;
            this.clearCacheButton.Text = "&Clear configuration cache";
            this.clearCacheButton.Click += new System.EventHandler(this.clearCacheButton_Click);
            // 
            // automaticRefreshCheckBox
            // 
            this.automaticRefreshCheckBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.automaticRefreshCheckBox.Location = new System.Drawing.Point(20, 138);
            this.automaticRefreshCheckBox.Name = "automaticRefreshCheckBox";
            this.automaticRefreshCheckBox.Size = new System.Drawing.Size(140, 55);
            this.automaticRefreshCheckBox.TabIndex = 6;
            this.automaticRefreshCheckBox.Text = "Detect changes in configuration storage and automatically refresh";
            this.automaticRefreshCheckBox.CheckedChanged += new System.EventHandler(this.automaticRefreshCheckBox_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 133);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 253);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.readXmlConfigDataButton);
            this.tabPage1.Controls.Add(this.automaticRefreshCheckBox);
            this.tabPage1.Controls.Add(this.readResultsTextBox);
            this.tabPage1.Controls.Add(this.clearCacheButton);
            this.tabPage1.Controls.Add(this.readSampleTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(694, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Read";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.writeSampleTextBox);
            this.tabPage2.Controls.Add(this.writeXmlConfigDataButton);
            this.tabPage2.Controls.Add(this.writeResultsTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(694, 227);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Write";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // writeSampleTextBox
            // 
            this.writeSampleTextBox.Location = new System.Drawing.Point(179, 131);
            this.writeSampleTextBox.Name = "writeSampleTextBox";
            this.writeSampleTextBox.ReadOnly = true;
            this.writeSampleTextBox.Size = new System.Drawing.Size(494, 83);
            this.writeSampleTextBox.TabIndex = 6;
            this.writeSampleTextBox.Text = "Sample Text";
            // 
            // writeXmlConfigDataButton
            // 
            this.writeXmlConfigDataButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.writeXmlConfigDataButton.Location = new System.Drawing.Point(19, 20);
            this.writeXmlConfigDataButton.Name = "writeXmlConfigDataButton";
            this.writeXmlConfigDataButton.Size = new System.Drawing.Size(136, 40);
            this.writeXmlConfigDataButton.TabIndex = 4;
            this.writeXmlConfigDataButton.Text = "Write &configuration data to an XML file";
            this.writeXmlConfigDataButton.Click += new System.EventHandler(this.writeXmlConfigDataButton_Click);
            // 
            // writeResultsTextBox
            // 
            this.writeResultsTextBox.Location = new System.Drawing.Point(179, 13);
            this.writeResultsTextBox.Multiline = true;
            this.writeResultsTextBox.Name = "writeResultsTextBox";
            this.writeResultsTextBox.ReadOnly = true;
            this.writeResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.writeResultsTextBox.Size = new System.Drawing.Size(496, 104);
            this.writeResultsTextBox.TabIndex = 5;
            this.writeResultsTextBox.TabStop = false;
            // 
            // fontDialog
            // 
            this.fontDialog.AllowScriptChange = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(694, 58);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // QuickStartForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(709, 458);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "QuickStartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration Migration Quick Start";
            this.Load += new System.EventHandler(this.QuickStartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            AppForm = new QuickStartForm();

            // Unhandled exceptions will be delivered to our ThreadException handler
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(AppThreadException);  

            Application.Run(AppForm);
        }
		
        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        private static void AppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
        {
            string errorMsg = string.Format(new CultureInfo("en-us", true), "There are some problems while trying to use the Configuration Quick Start, please check the following error messages: \n{0}\n", e.Exception.Message);
            errorMsg += Environment.NewLine;

            DialogResult result = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
            QuickStartForm.AppForm.Cursor = System.Windows.Forms.Cursors.Default;
        }

        delegate void ShowFontDataDelegate(EditorFontData fontData, RichTextBox sampleTextBox);

		private void UpdateFont(EditorFontData fontData, RichTextBox sampleTextBox)
		{
            if (sampleTextBox.InvokeRequired)
			{
                readSampleTextBox.Invoke(new ShowFontDataDelegate(ShowFontData), new object[] { fontData, sampleTextBox });
			}
			else
			{
                ShowFontData(fontData, sampleTextBox);
			}
		}

        private void ShowFontData(EditorFontData fontData, RichTextBox sampleTextBox)
		{
            if (fontData != null)
            {
                Font newFont = new Font(fontData.Name, fontData.Size, (FontStyle)fontData.Style);
                sampleTextBox.Font = newFont;
            }
		}

        private void QuickStartForm_Load(object sender, System.EventArgs e)
        {
            // Initialize image to embedded logo
            this.logoPictureBox.Image = GetEmbeddedImage("ConfigurationReadXmlQuickStart.logo.gif");

            // Initialize the text box with the configuration settings
            EditorFontData configData = ConfigurationManager.GetSection("EditorSettings") as EditorFontData;
			
            // Initialize file system watcher
            watcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory);
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = false;
            

            DisplayResults("Application configuration loaded.", readResultsTextBox);

            this.UpdateFont(configData, readSampleTextBox);
        }


        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.ToLower().Contains(".config"))
            {
				for (int i = 0; i < 3; i++)
				{
					try
					{
						// Using the static method, read the cached configuration settings
						ConfigurationManager.RefreshSection("EditorSettings");
						break;
					}
					catch (ConfigurationErrorsException)
					{
						if (i == 2) throw;
						else Thread.Sleep(100);
					}
				}

                EditorFontData configData = ConfigurationManager.GetSection("EditorSettings") as EditorFontData;

                StringBuilder results = new StringBuilder();
                results.Append("Configuration changes in storage were detected. Updating configuration.");
                results.Append(Environment.NewLine);
                results.Append("New configuration settings:");
                results.Append(Environment.NewLine);
                results.Append('\t');
                results.Append(configData.ToString());
                results.Append(Environment.NewLine);

                DisplayResults(results.ToString(), readResultsTextBox);

                this.UpdateFont(configData, readSampleTextBox);
            }
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

        delegate void ShowResultsDelegate(string results, TextBox targetTextBox);

		private void DisplayResults(string results, TextBox targetTextBox)
		{
            if (targetTextBox.InvokeRequired)
			{
                targetTextBox.Invoke(new ShowResultsDelegate(ShowResults), new object[] { results, targetTextBox });
			}
			else
			{
                ShowResults(results, targetTextBox);
			}
		}

        private void ShowResults(string results, TextBox targetTextBox)
		{
            targetTextBox.Text = results + Environment.NewLine;
		}

        /// <summary>
        /// Use case: demonstrate how to retrieve configuration data
        /// </summary>
        private void readXmlConfigDataButton_Click(object sender, System.EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            // Using the static method, read the cached configuration settings
            EditorFontData configData = ConfigurationManager.GetSection("EditorSettings") as EditorFontData;

            StringBuilder results = new StringBuilder();            
            results.Append("Configuration settings:");
            results.Append(Environment.NewLine);
            results.Append('\t');
            results.Append(configData.ToString());
            results.Append(Environment.NewLine);

            DisplayResults(results.ToString(), readResultsTextBox);

            this.UpdateFont(configData, readSampleTextBox);
	
            Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void quitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void clearCacheButton_Click(object sender, System.EventArgs e)
        {
            ConfigurationManager.RefreshSection("EditorSettings");
            DisplayResults("The cache of configuration data has been cleared.", readResultsTextBox);

        }

        private void automaticRefreshCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (automaticRefreshCheckBox.Checked)
            {
                watcher.EnableRaisingEvents = true;
                DisplayResults("Configuration changes in storage will be detected.", readResultsTextBox);
            }
            else
            {
                watcher.EnableRaisingEvents = false;
                //ConfigurationManager.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnConfigurationChanged);
                DisplayResults("Configuration changes in storage will *not* be detected.", readResultsTextBox);
            }
        }

        private void viewWalkthroughButton_Click(object sender, System.EventArgs e)
        {
            // Process has never been started. Initialize and launch the viewer.
            if (this.viewerProcess == null)
            {
                // Initialize the Process information for the help viewer
                this.viewerProcess = new Process();

                this.viewerProcess.StartInfo.FileName = HelpViewerExecutable;
                this.viewerProcess.StartInfo.Arguments = @"/helpcol " + HelpTopicNamespace;
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

        private void writeXmlConfigDataButton_Click(object sender, EventArgs e)
        {
            EditorFontData configData = new EditorFontData();

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                configData.Name = fontDialog.Font.Name;
                configData.Size = fontDialog.Font.Size;
                configData.Style = Convert.ToInt32(fontDialog.Font.Style);

                // Write the new configuration data to the XML file
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.Sections.Remove("EditorSettings");
                config.Sections.Add("EditorSettings", configData);
                config.Save();


                StringBuilder results = new StringBuilder();
                results.Append("Configuration Data Updated:");
                results.Append(Environment.NewLine);
                results.Append('\t');
                results.Append(configData.ToString());

                DisplayResults(results.ToString(), writeResultsTextBox);

                this.UpdateFont(configData, writeSampleTextBox);
            }
        }

    }
}
