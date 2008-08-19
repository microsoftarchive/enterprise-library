//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
//===============================================================================
// Copyright ? Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using ValidationQuickStart.BusinessEntities;

namespace ValidationQuickStart
{
    public partial class MainForm : Form
    {
        private Process viewerProcess = null;
        private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic ValidationQS1";

        public MainForm()
        {
            InitializeComponent();
            customerRuleSetCombo.SelectedIndex = 0;
        }

        private Customer CreateCustomer()
        {
            // Build the customer object
            Customer customer = new Customer();
            customer.FirstName = firstNameTextBox.Text;
            customer.LastName = lastNameTextBox.Text;
            customer.DateOfBirth = birthDatePicker.Value;
            customer.Email = emailTextBox.Text;
            customer.Address = CreateAddress();
            int rewardPoints;
            bool success = Int32.TryParse(rewardPointTextBox.Text, out rewardPoints);
            if (success)
            {
                customer.RewardPoints = rewardPoints;
            }
            else
            {
                MessageBox.Show("Rewards Points must be a valid integer.\r\nNote that the Validation Application Block cannot validate data if it is incompatible with the object's type system.",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return customer;
        }

        private Address CreateAddress()
        {
            Address address = new Address();
            address.Line1 = line1TextBox.Text;
            address.Line2 = line2TextBox.Text;
            address.City = cityTextBox.Text;
            address.PostCode = postCodeTextBox.Text;
            address.State = stateTextBox.Text;
            address.Country = countryTextBox.Text;
            return address;
        }

        private void validateCustomerButton_Click(object sender, EventArgs e)
        {
            Customer customer = CreateCustomer();
            if (customer != null)
            {
                Validator<Customer> validator = ValidationFactory.CreateValidator<Customer>(customerRuleSetCombo.Text);
                ValidationResults results = validator.Validate(customer);

                DisplayValidationResults(results, customerResultsTreeView);
            }


        }

        private void DisplayValidationResults(ValidationResults results, TreeView resultsTreeView)
        {
            resultsTreeView.Nodes.Clear();
            foreach (ValidationResult result in results)
            {
                TreeNode node = new TreeNode(result.Message);
                node.Nodes.Add(String.Format("Key = \"{0}\"", result.Key.ToString()));
                node.Nodes.Add(String.Format("Tag = {0}", result.Tag == null ? "null" : "\"" + result.Tag.ToString() + "\""));
                node.Nodes.Add(String.Format("Target = <{0}>", result.Target.ToString()));
                node.Nodes.Add(String.Format("Validator = <{0}>", result.Validator.ToString()));
                resultsTreeView.Nodes.Add(node);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void customerRuleSetCombo_TextChanged(object sender, EventArgs e)
        {
            // Assume that both classes use the same ruleset, for the purposes of the Winforms integration
            customerValidationProvider.RulesetName = customerRuleSetCombo.Text;
            addressValidationProvider.RulesetName = customerRuleSetCombo.Text;

            if (enableWinFormsValidationCheckBox.Checked)
            {
                // Force validation of all controls
                this.ValidateChildren();
            }

        }

        private void enableWinFormsValidationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customerValidationProvider.Enabled = enableWinFormsValidationCheckBox.Checked;
            addressValidationProvider.Enabled = enableWinFormsValidationCheckBox.Checked;
            if (enableWinFormsValidationCheckBox.Checked)
            {
                // Force validation of all controls
                this.ValidateChildren();
            }
        }



        private void customerValidationProvider_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            // Get the value of the RewardPoints control and convert to an integer
            if (e.SourcePropertyName == "RewardPoints")
            {
                string originalValue = e.ValueToConvert as string;
                int convertedValue;
                bool success = Int32.TryParse(originalValue, out convertedValue);
                if (success)
                {
                    e.ConvertedValue = convertedValue;
                }
                else
                {
                    e.ConversionErrorMessage = "Reward Points must be a valid integer";
                }
            }
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

    }
}
