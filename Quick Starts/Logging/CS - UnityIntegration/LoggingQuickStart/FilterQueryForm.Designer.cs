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
  partial class FilterQueryForm
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
      this.troubleshootingCheckbox = new System.Windows.Forms.CheckBox();
      this.dataAccessCheckbox = new System.Windows.Forms.CheckBox();
      this.uiCheckbox = new System.Windows.Forms.CheckBox();
      this.generalCheckbox = new System.Windows.Forms.CheckBox();
      this.priorityNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.debugCheckbox = new System.Windows.Forms.CheckBox();
      this.traceCheckBox = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.CancelEntryButton = new System.Windows.Forms.Button();
      this.OkButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // troubleshootingCheckbox
      // 
      this.troubleshootingCheckbox.AutoSize = true;
      this.troubleshootingCheckbox.Location = new System.Drawing.Point(158, 157);
      this.troubleshootingCheckbox.Name = "troubleshootingCheckbox";
      this.troubleshootingCheckbox.Size = new System.Drawing.Size(102, 17);
      this.troubleshootingCheckbox.TabIndex = 6;
      this.troubleshootingCheckbox.Text = "Troubleshooting";
      this.troubleshootingCheckbox.UseVisualStyleBackColor = true;
      // 
      // dataAccessCheckbox
      // 
      this.dataAccessCheckbox.AutoSize = true;
      this.dataAccessCheckbox.Location = new System.Drawing.Point(158, 134);
      this.dataAccessCheckbox.Name = "dataAccessCheckbox";
      this.dataAccessCheckbox.Size = new System.Drawing.Size(123, 17);
      this.dataAccessCheckbox.TabIndex = 4;
      this.dataAccessCheckbox.Text = "Data Access Events";
      this.dataAccessCheckbox.UseVisualStyleBackColor = true;
      // 
      // uiCheckbox
      // 
      this.uiCheckbox.AutoSize = true;
      this.uiCheckbox.Location = new System.Drawing.Point(158, 111);
      this.uiCheckbox.Name = "uiCheckbox";
      this.uiCheckbox.Size = new System.Drawing.Size(73, 17);
      this.uiCheckbox.TabIndex = 2;
      this.uiCheckbox.Text = "UI Events";
      this.uiCheckbox.UseVisualStyleBackColor = true;
      // 
      // generalCheckbox
      // 
      this.generalCheckbox.AutoSize = true;
      this.generalCheckbox.Location = new System.Drawing.Point(78, 157);
      this.generalCheckbox.Name = "generalCheckbox";
      this.generalCheckbox.Size = new System.Drawing.Size(63, 17);
      this.generalCheckbox.TabIndex = 5;
      this.generalCheckbox.Text = "General";
      this.generalCheckbox.UseVisualStyleBackColor = true;
      // 
      // priorityNumericUpDown
      // 
      this.priorityNumericUpDown.Location = new System.Drawing.Point(78, 49);
      this.priorityNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
      this.priorityNumericUpDown.Name = "priorityNumericUpDown";
      this.priorityNumericUpDown.Size = new System.Drawing.Size(36, 20);
      this.priorityNumericUpDown.TabIndex = 0;
      this.priorityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
      // 
      // debugCheckbox
      // 
      this.debugCheckbox.AutoSize = true;
      this.debugCheckbox.Location = new System.Drawing.Point(78, 134);
      this.debugCheckbox.Name = "debugCheckbox";
      this.debugCheckbox.Size = new System.Drawing.Size(58, 17);
      this.debugCheckbox.TabIndex = 3;
      this.debugCheckbox.Text = "Debug";
      this.debugCheckbox.UseVisualStyleBackColor = true;
      // 
      // traceCheckBox
      // 
      this.traceCheckBox.AutoSize = true;
      this.traceCheckBox.Location = new System.Drawing.Point(78, 111);
      this.traceCheckBox.Name = "traceCheckBox";
      this.traceCheckBox.Size = new System.Drawing.Size(54, 17);
      this.traceCheckBox.TabIndex = 1;
      this.traceCheckBox.Text = "Trace";
      this.traceCheckBox.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 111);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(60, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Categories:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(31, 51);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(41, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Priority:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(4, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(240, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Select the priority to use for the priority filter query.";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(4, 83);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(248, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Select the categories to use for the category query.";
      // 
      // CancelEntryButton
      // 
      this.CancelEntryButton.Location = new System.Drawing.Point(156, 197);
      this.CancelEntryButton.Name = "CancelEntryButton";
      this.CancelEntryButton.Size = new System.Drawing.Size(75, 23);
      this.CancelEntryButton.TabIndex = 8;
      this.CancelEntryButton.Text = "Cancel";
      this.CancelEntryButton.UseVisualStyleBackColor = true;
      this.CancelEntryButton.Click += new System.EventHandler(this.CancelEntryButton_Click);
      // 
      // OkButton
      // 
      this.OkButton.Location = new System.Drawing.Point(63, 197);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 7;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
      // 
      // FilterQueryForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 239);
      this.Controls.Add(this.CancelEntryButton);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.troubleshootingCheckbox);
      this.Controls.Add(this.dataAccessCheckbox);
      this.Controls.Add(this.uiCheckbox);
      this.Controls.Add(this.generalCheckbox);
      this.Controls.Add(this.priorityNumericUpDown);
      this.Controls.Add(this.debugCheckbox);
      this.Controls.Add(this.traceCheckBox);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FilterQueryForm";
      this.Text = "FilterQueryForm";
      ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox troubleshootingCheckbox;
    private System.Windows.Forms.CheckBox dataAccessCheckbox;
    private System.Windows.Forms.CheckBox uiCheckbox;
    private System.Windows.Forms.CheckBox generalCheckbox;
    private System.Windows.Forms.NumericUpDown priorityNumericUpDown;
    private System.Windows.Forms.CheckBox debugCheckbox;
    private System.Windows.Forms.CheckBox traceCheckBox;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button CancelEntryButton;
    private System.Windows.Forms.Button OkButton;
  }
}
