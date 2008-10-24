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

namespace LoggingQuickStart
{
    partial class EventInformationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
          this.label1 = new System.Windows.Forms.Label();
          this.label2 = new System.Windows.Forms.Label();
          this.label3 = new System.Windows.Forms.Label();
          this.label4 = new System.Windows.Forms.Label();
          this.label5 = new System.Windows.Forms.Label();
          this.traceCheckBox = new System.Windows.Forms.CheckBox();
          this.messageTextbox = new System.Windows.Forms.TextBox();
          this.debugCheckbox = new System.Windows.Forms.CheckBox();
          this.priorityNumericUpDown = new System.Windows.Forms.NumericUpDown();
          this.generalCheckbox = new System.Windows.Forms.CheckBox();
          this.troubleshootingCheckbox = new System.Windows.Forms.CheckBox();
          this.dataAccessCheckbox = new System.Windows.Forms.CheckBox();
          this.uiCheckbox = new System.Windows.Forms.CheckBox();
          this.OkButton = new System.Windows.Forms.Button();
          this.CancelEntryButton = new System.Windows.Forms.Button();
          this.eventIdTextBox = new System.Windows.Forms.TextBox();
          ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).BeginInit();
          this.SuspendLayout();
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(9, 8);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(137, 13);
          this.label1.TabIndex = 0;
          this.label1.Text = "Enter the event information.";
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(13, 39);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(98, 13);
          this.label2.TabIndex = 1;
          this.label2.Text = "Event ID (numeric):";
          // 
          // label3
          // 
          this.label3.AutoSize = true;
          this.label3.Location = new System.Drawing.Point(12, 71);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(84, 13);
          this.label3.TabIndex = 2;
          this.label3.Text = "Event Message:";
          // 
          // label4
          // 
          this.label4.AutoSize = true;
          this.label4.Location = new System.Drawing.Point(11, 149);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(72, 13);
          this.label4.TabIndex = 3;
          this.label4.Text = "Event Priority:";
          // 
          // label5
          // 
          this.label5.AutoSize = true;
          this.label5.Location = new System.Drawing.Point(9, 180);
          this.label5.Name = "label5";
          this.label5.Size = new System.Drawing.Size(91, 13);
          this.label5.TabIndex = 4;
          this.label5.Text = "Event Categories:";
          // 
          // traceCheckBox
          // 
          this.traceCheckBox.AutoSize = true;
          this.traceCheckBox.Location = new System.Drawing.Point(109, 176);
          this.traceCheckBox.Name = "traceCheckBox";
          this.traceCheckBox.Size = new System.Drawing.Size(54, 17);
          this.traceCheckBox.TabIndex = 3;
          this.traceCheckBox.Text = "Trace";
          this.traceCheckBox.UseVisualStyleBackColor = true;
          // 
          // messageTextbox
          // 
          this.messageTextbox.Location = new System.Drawing.Point(109, 68);
          this.messageTextbox.Multiline = true;
          this.messageTextbox.Name = "messageTextbox";
          this.messageTextbox.Size = new System.Drawing.Size(181, 54);
          this.messageTextbox.TabIndex = 1;
          // 
          // debugCheckbox
          // 
          this.debugCheckbox.AutoSize = true;
          this.debugCheckbox.Location = new System.Drawing.Point(109, 199);
          this.debugCheckbox.Name = "debugCheckbox";
          this.debugCheckbox.Size = new System.Drawing.Size(58, 17);
          this.debugCheckbox.TabIndex = 5;
          this.debugCheckbox.Text = "Debug";
          this.debugCheckbox.UseVisualStyleBackColor = true;
          // 
          // priorityNumericUpDown
          // 
          this.priorityNumericUpDown.Location = new System.Drawing.Point(109, 141);
          this.priorityNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
          this.priorityNumericUpDown.Name = "priorityNumericUpDown";
          this.priorityNumericUpDown.Size = new System.Drawing.Size(44, 20);
          this.priorityNumericUpDown.TabIndex = 2;
          this.priorityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
          // 
          // generalCheckbox
          // 
          this.generalCheckbox.AutoSize = true;
          this.generalCheckbox.Location = new System.Drawing.Point(109, 222);
          this.generalCheckbox.Name = "generalCheckbox";
          this.generalCheckbox.Size = new System.Drawing.Size(63, 17);
          this.generalCheckbox.TabIndex = 7;
          this.generalCheckbox.Text = "General";
          this.generalCheckbox.UseVisualStyleBackColor = true;
          // 
          // troubleshootingCheckbox
          // 
          this.troubleshootingCheckbox.AutoSize = true;
          this.troubleshootingCheckbox.Location = new System.Drawing.Point(189, 222);
          this.troubleshootingCheckbox.Name = "troubleshootingCheckbox";
          this.troubleshootingCheckbox.Size = new System.Drawing.Size(102, 17);
          this.troubleshootingCheckbox.TabIndex = 8;
          this.troubleshootingCheckbox.Text = "Troubleshooting";
          this.troubleshootingCheckbox.UseVisualStyleBackColor = true;
          // 
          // dataAccessCheckbox
          // 
          this.dataAccessCheckbox.AutoSize = true;
          this.dataAccessCheckbox.Location = new System.Drawing.Point(189, 199);
          this.dataAccessCheckbox.Name = "dataAccessCheckbox";
          this.dataAccessCheckbox.Size = new System.Drawing.Size(123, 17);
          this.dataAccessCheckbox.TabIndex = 6;
          this.dataAccessCheckbox.Text = "Data Access Events";
          this.dataAccessCheckbox.UseVisualStyleBackColor = true;
          // 
          // uiCheckbox
          // 
          this.uiCheckbox.AutoSize = true;
          this.uiCheckbox.Location = new System.Drawing.Point(189, 176);
          this.uiCheckbox.Name = "uiCheckbox";
          this.uiCheckbox.Size = new System.Drawing.Size(73, 17);
          this.uiCheckbox.TabIndex = 4;
          this.uiCheckbox.Text = "UI Events";
          this.uiCheckbox.UseVisualStyleBackColor = true;
          // 
          // OkButton
          // 
          this.OkButton.Location = new System.Drawing.Point(78, 273);
          this.OkButton.Name = "OkButton";
          this.OkButton.Size = new System.Drawing.Size(75, 23);
          this.OkButton.TabIndex = 9;
          this.OkButton.Text = "OK";
          this.OkButton.UseVisualStyleBackColor = true;
          this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
          // 
          // CancelEntryButton
          // 
          this.CancelEntryButton.Location = new System.Drawing.Point(171, 273);
          this.CancelEntryButton.Name = "CancelEntryButton";
          this.CancelEntryButton.Size = new System.Drawing.Size(75, 23);
          this.CancelEntryButton.TabIndex = 10;
          this.CancelEntryButton.Text = "Cancel";
          this.CancelEntryButton.UseVisualStyleBackColor = true;
          this.CancelEntryButton.Click += new System.EventHandler(this.CancelButton_Click);
          // 
          // eventIdTextBox
          // 
          this.eventIdTextBox.Location = new System.Drawing.Point(109, 36);
          this.eventIdTextBox.Name = "eventIdTextBox";
          this.eventIdTextBox.Size = new System.Drawing.Size(44, 20);
          this.eventIdTextBox.TabIndex = 0;
          // 
          // EventInformationForm
          // 
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
          this.ClientSize = new System.Drawing.Size(314, 318);
          this.Controls.Add(this.eventIdTextBox);
          this.Controls.Add(this.CancelEntryButton);
          this.Controls.Add(this.OkButton);
          this.Controls.Add(this.troubleshootingCheckbox);
          this.Controls.Add(this.dataAccessCheckbox);
          this.Controls.Add(this.uiCheckbox);
          this.Controls.Add(this.generalCheckbox);
          this.Controls.Add(this.priorityNumericUpDown);
          this.Controls.Add(this.debugCheckbox);
          this.Controls.Add(this.messageTextbox);
          this.Controls.Add(this.traceCheckBox);
          this.Controls.Add(this.label5);
          this.Controls.Add(this.label4);
          this.Controls.Add(this.label3);
          this.Controls.Add(this.label2);
          this.Controls.Add(this.label1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "EventInformationForm";
          this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
          this.Text = "Event Information";
          this.Load += new System.EventHandler(this.EventInformationForm_Load);
          ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox traceCheckBox;
        private System.Windows.Forms.TextBox messageTextbox;
        private System.Windows.Forms.CheckBox debugCheckbox;
      private System.Windows.Forms.NumericUpDown priorityNumericUpDown;
        private System.Windows.Forms.CheckBox generalCheckbox;
        private System.Windows.Forms.CheckBox troubleshootingCheckbox;
        private System.Windows.Forms.CheckBox dataAccessCheckbox;
        private System.Windows.Forms.CheckBox uiCheckbox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelEntryButton;
      private System.Windows.Forms.TextBox eventIdTextBox;
    }
}
