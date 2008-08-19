//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block QuickStart
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

namespace LoggingQuickStart
{
    public partial class EventInformationForm : Form
    {
        private ICollection<string> categories = new List<string>(0);
        private int eventID = -1;

        public EventInformationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event ID for event to be logged.
        /// </summary>
        public int EventId
        {
            get { return this.eventID; }
        }

        /// <summary>
        /// Message for event to be logged.
        /// </summary>
        public string Message
        {
            get { return this.messageTextbox.Text; }
        }

        /// <summary>
        /// Priority of event to be logged.
        /// </summary>
        public int Priority
        {
            get { return Decimal.ToInt32(this.priorityNumericUpDown.Value); }
        }

        /// <summary>
        /// Collection of categories for the event.
        /// </summary>
        public ICollection<string> Categories
        {
            get { return this.categories; }
        }

        private void EventInformationForm_Load(object sender, EventArgs e)
        {
            this.messageTextbox.Focus();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            RecordSelectedCategories();

            if (ValidateInput())
            {
              this.eventID = Convert.ToInt32(this.eventIdTextBox.Text);
              base.DialogResult = DialogResult.OK;
              base.Close();
            }
        }

        private void RecordSelectedCategories()
        {
            if (this.traceCheckBox.Checked)
            {
                categories.Add("Trace");
            }
            if (this.debugCheckbox.Checked)
            {
                categories.Add("Debug");
            }
            if (this.generalCheckbox.Checked)
            {
                categories.Add("General");
            }
            if (this.uiCheckbox.Checked)
            {
                categories.Add("UI Events");
            }
            if (this.dataAccessCheckbox.Checked)
            {
                categories.Add("Data Access Events");
            }
            if (this.troubleshootingCheckbox.Checked)
            {
                categories.Add("Troubleshooting");
            }
        }

        /// <summary>
        /// Validate contents for the event ID text box, the priority text box and the category.
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
          string errorMessage = Properties.Resources.InvalidDataMessage;
          bool validationError = false;

          try
          {
            int id = Convert.ToInt32(this.eventIdTextBox.Text);
          }
          catch (Exception)
          {
            errorMessage += Properties.Resources.InvalidEventIDMessage;
            validationError = true;
          }

          if (validationError)
          {
            MessageBox.Show(errorMessage, Properties.Resources.QuickStartTitleMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
          }

          return true;
        }


    }
}