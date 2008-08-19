//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    partial class TypeMemberChooserUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypeMemberChooserUI));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.memberTreeView = new System.Windows.Forms.TreeView();
            this.memberImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Controls.Add(this.okButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // memberTreeView
            // 
            this.memberTreeView.CheckBoxes = true;
            resources.ApplyResources(this.memberTreeView, "memberTreeView");
            this.memberTreeView.ImageList = this.memberImageList;
            this.memberTreeView.Name = "memberTreeView";
            this.memberTreeView.ShowLines = false;
            this.memberTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.memberTreeView_AfterCheck);
            // 
            // memberImageList
            // 
            this.memberImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("memberImageList.ImageStream")));
            this.memberImageList.TransparentColor = System.Drawing.Color.Magenta;
            this.memberImageList.Images.SetKeyName(0, "Type");
            this.memberImageList.Images.SetKeyName(1, "Field");
            this.memberImageList.Images.SetKeyName(2, "Method");
            this.memberImageList.Images.SetKeyName(3, "Property");
            this.memberImageList.Images.SetKeyName(4, "Container");
            // 
            // TypeMemberChooserUI
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.memberTreeView);
            this.Controls.Add(this.panel1);
            this.Name = "TypeMemberChooserUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView memberTreeView;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ImageList memberImageList;
    }
}