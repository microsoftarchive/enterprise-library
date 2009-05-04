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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PolicyInjectionQuickStart
{
    internal partial class AmountEntryForm : Form
    {
        private const string promptString = "Please enter the amount that you wish to {0}:";
        private decimal amount;

        public AmountEntryForm(AmountDialogType dialogType)
        {
            InitializeComponent();

            if (dialogType == AmountDialogType.Deposit)
            {
                promptLabel.Text = String.Format(promptString, "deposit");
                this.Text = "Deposit";
            }
            else if (dialogType == AmountDialogType.Withdraw)
            {
                promptLabel.Text = String.Format(promptString, "withdraw");
                this.Text = "Withdraw";
            }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool success = decimal.TryParse(amountTextBox.Text, out amount);
            if (!success)
            {
                errorProvider.SetError(amountTextBox, "Please enter a valid decimal amount.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

    internal enum AmountDialogType
    {
        Deposit,
        Withdraw
    }
}
