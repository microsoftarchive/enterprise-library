//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Configuration.Install;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Installers;

namespace PolicyInjectionQuickStart
{
    public partial class MainForm : Form
    {
        private Process viewerProcess = null;
        private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic PolicyInjectionQS1";
        private BusinessLogic.BankAccount bankAccount;


        public MainForm()
        {
            InitializeComponent();
            PopulateUserList();
            bankAccount = PolicyInjection.Create<BusinessLogic.BankAccount>();
        }

        private void depositButton_Click(object sender, EventArgs e)
        {
            AmountEntryForm form = new AmountEntryForm(AmountDialogType.Deposit);
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                exceptionTextBox.Text = String.Empty;
                try
                {
                    bankAccount.Deposit(form.Amount);
                }
                catch (Exception ex)
                {
                    exceptionTextBox.Text = ex.Message;
                }
            }
        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {
            AmountEntryForm form = new AmountEntryForm(AmountDialogType.Withdraw);
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                exceptionTextBox.Text = String.Empty;
                try
                {
                    bankAccount.Withdraw(form.Amount);
                }
                catch (Exception ex)
                {
                    exceptionTextBox.Text = ex.Message;
                }
            }
        }

        private void balanceInquiryButton_Click(object sender, EventArgs e)
        {
            exceptionTextBox.Text = String.Empty;
            try
            {
                balanceTextBox.Text = bankAccount.GetCurrentBalance().ToString();
            }
            catch (Exception ex)
            {
                exceptionTextBox.Text = ex.Message;
            }
        }

        private void viewLogButton_Click(object sender, EventArgs e)
        {
            Process p = Process.Start(new ProcessStartInfo("notepad.exe", "audit.log"));
        }

        private void openPerfMonButton_Click(object sender, EventArgs e)
        {
            Process p = Process.Start(new ProcessStartInfo("perfmon.exe"));
        }

        private void PopulateUserList()
        {

            const string userPrefix = "User:";
            foreach (string setting in ConfigurationManager.AppSettings)
            {
                if (setting.StartsWith(userPrefix))
                {
                    string userName = setting.Substring(userPrefix.Length);
                    string role = ConfigurationManager.AppSettings[setting];
                    IPrincipal principal = new GenericPrincipal(
                        new GenericIdentity(userName), new string[] { role });
                    userComboBox.Items.Add(new KeyValuePair<string, IPrincipal>(userName, principal));
                }
            }
            userComboBox.SelectedIndex = 0;
        }

        private void userComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obviously, you wouldn't implement security like this in a real application. 
            // It's a Quickstart, people! :-)

            // Find the principal of the selected user. 
            KeyValuePair<string, IPrincipal> pair = (KeyValuePair<string, IPrincipal>)userComboBox.SelectedItem;
            IPrincipal selectedUser = pair.Value;

            // Set the current thread principal to the selected user
            System.Threading.Thread.CurrentPrincipal = selectedUser;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void installPerfCountersButton_Click(object sender, EventArgs e)
        {
            try
            {
                PerformanceCountersInstaller installer = new PerformanceCountersInstaller(new SystemConfigurationSource());
                IDictionary state = new System.Collections.Hashtable();
                installer.Context = new InstallContext();
                installer.Install(state);
                installer.Commit(state);
                MessageBox.Show("Performance counters have been successfully installed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void uninstallPerfCountersButton_Click(object sender, EventArgs e)
        {
            try
            {
                PerformanceCountersInstaller installer = new PerformanceCountersInstaller(new SystemConfigurationSource());
                installer.Context = new InstallContext();
                installer.Uninstall(null);
                MessageBox.Show("Performance counters have been successfully uninstalled.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                InstallException installException = ex as InstallException;
                if (installException != null && installException.InnerException != null)
                {
                    ex = installException.InnerException;
                }
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
