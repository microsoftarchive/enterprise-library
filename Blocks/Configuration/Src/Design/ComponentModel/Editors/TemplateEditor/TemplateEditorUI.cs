//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Represents an editor for a template.
    /// </summary>
    public class TemplateEditorUI : Form
    {
        private string[] tokens = new string[]
            {
                "{message}",
                "{category}",
                "{priority}",
                "{eventid}",
                "{severity}",
                "{title}",
                "{timestamp}",
                "{timestamp()}",
                "{timestamp(local)}",
                "{timestamp(FixedFormatUSDate)}",
                "{timestamp(FixedFormatISOInternationalDate)}",
                "{timestamp(FixedFormatTime)}",
                "{machine}",
                "{localMachine}",
                "{appDomain}",
                "{localAppDomain}",
                "{processId}",
                "{localProcessId}",
                "{processName}",
                "{localProcessName}",
                "{threadName}",
                "{win32ThreadId}",
                "{dictionary()}",
                "{keyvalue()}",
                "{newline}",
                "{tab}",
                "{property()}"
            };

        private TextBox templateTextBox;
        private ComboBox tokenDropdown;
        private Button okButton;
        private Button insertTokenButton;
        private Button cancelButton;

        /// <summary>
        /// The text of the template as defined by the user at designtime.
        /// </summary>
        public string UserText
        {
            get { return string.Join("\n", templateTextBox.Lines); }
            set
            {
                templateTextBox.Lines = (value ?? "").Split('\n');
                templateTextBox.Select(templateTextBox.Text.Length, 0);
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Initializes the components.
        /// </summary>
        public TemplateEditorUI()
        {
            InitializeComponent();

            tokenDropdown.DataSource = tokens;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateEditorUI));
            this.templateTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.tokenDropdown = new System.Windows.Forms.ComboBox();
            this.insertTokenButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // templateTextBox
            // 
            this.templateTextBox.AcceptsReturn = true;
            this.templateTextBox.AcceptsTab = true;
            resources.ApplyResources(this.templateTextBox, "templateTextBox");
            this.templateTextBox.Name = "templateTextBox";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            // 
            // tokenDropdown
            // 
            resources.ApplyResources(this.tokenDropdown, "tokenDropdown");
            this.tokenDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tokenDropdown.Name = "tokenDropdown";
            // 
            // insertTokenButton
            // 
            resources.ApplyResources(this.insertTokenButton, "insertTokenButton");
            this.insertTokenButton.Name = "insertTokenButton";
            this.insertTokenButton.Click += new System.EventHandler(this.insertTokenButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // TemplateEditorUI
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.insertTokenButton);
            this.Controls.Add(this.tokenDropdown);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.templateTextBox);
            this.Name = "TemplateEditorUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void insertTokenButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(templateTextBox.Text);
            if (templateTextBox.SelectionLength > 0)
            {
                sb.Remove(templateTextBox.SelectionStart, templateTextBox.SelectionLength);
            }

            sb.Insert(templateTextBox.SelectionStart, tokenDropdown.SelectedValue);

            int parentIdx = tokenDropdown.SelectedValue.ToString().IndexOf("()", StringComparison.OrdinalIgnoreCase);
            int selectionStart = 0;

            selectionStart = templateTextBox.SelectionStart + tokenDropdown.SelectedValue.ToString().Length;
            if (parentIdx > 0)
            {
                selectionStart -= 2;
            }

            templateTextBox.Text = sb.ToString();
            tokenDropdown.SelectedIndex = 0;

            templateTextBox.Focus();
            templateTextBox.Select(selectionStart, 0);
            templateTextBox.ScrollToCaret();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
