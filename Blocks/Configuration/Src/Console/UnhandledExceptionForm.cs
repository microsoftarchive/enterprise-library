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
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <summary>
    /// A custom dialog for displaying information about unhandled exceptions.
    /// </summary>
    internal class UnhandledExceptionForm : Form
    {
        private Button exitButton;
        private Button continueButton;
        private Button detailsButton;
        private TextBox detailsTextBox;
        private Label messageText;
        private ImageList imageList;
        private IContainer components;
        private Bitmap upImage;
        private Bitmap downImage;
        private PictureBox pictureBox;
        private bool detailsVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionForm"/> class.
        /// </summary>
        /// <param name="e">An unhandled exception.</param>
        public UnhandledExceptionForm(Exception e)
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();

            this.upImage = (Bitmap) this.imageList.Images[0];
            this.upImage.MakeTransparent();
            this.downImage = (Bitmap) this.imageList.Images[1];
            this.downImage.MakeTransparent();
            this.detailsButton.Image = this.downImage;
            this.pictureBox.Image = SystemIcons.Error.ToBitmap();

            if (e != null)
            {
                string message = e.Message;
                if (message.Length == 0)
                {
                    message = e.GetType().Name;
                }
                this.messageText.Text =
                    this.messageText.Text +
                        Environment.NewLine +
                        Environment.NewLine +
                        message;
                this.detailsTextBox.Text = e.ToString();
            }
        }

        private UnhandledExceptionForm() : this(null)
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UnhandledExceptionForm));
            this.exitButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.detailsButton = new System.Windows.Forms.Button();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.messageText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // exitButton
            // 
            this.exitButton.AccessibleDescription = resources.GetString("exitButton.AccessibleDescription");
            this.exitButton.AccessibleName = resources.GetString("exitButton.AccessibleName");
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("exitButton.Anchor")));
            this.exitButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exitButton.BackgroundImage")));
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.exitButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("exitButton.Dock")));
            this.exitButton.Enabled = ((bool)(resources.GetObject("exitButton.Enabled")));
            this.exitButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("exitButton.FlatStyle")));
            this.exitButton.Font = ((System.Drawing.Font)(resources.GetObject("exitButton.Font")));
            this.exitButton.Image = ((System.Drawing.Image)(resources.GetObject("exitButton.Image")));
            this.exitButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("exitButton.ImageAlign")));
            this.exitButton.ImageIndex = ((int)(resources.GetObject("exitButton.ImageIndex")));
            this.exitButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("exitButton.ImeMode")));
            this.exitButton.Location = ((System.Drawing.Point)(resources.GetObject("exitButton.Location")));
            this.exitButton.Name = "exitButton";
            this.exitButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("exitButton.RightToLeft")));
            this.exitButton.Size = ((System.Drawing.Size)(resources.GetObject("exitButton.Size")));
            this.exitButton.TabIndex = ((int)(resources.GetObject("exitButton.TabIndex")));
            this.exitButton.Text = resources.GetString("exitButton.Text");
            this.exitButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("exitButton.TextAlign")));
            this.exitButton.Visible = ((bool)(resources.GetObject("exitButton.Visible")));
            // 
            // continueButton
            // 
            this.continueButton.AccessibleDescription = resources.GetString("continueButton.AccessibleDescription");
            this.continueButton.AccessibleName = resources.GetString("continueButton.AccessibleName");
            this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("continueButton.Anchor")));
            this.continueButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("continueButton.BackgroundImage")));
            this.continueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.continueButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("continueButton.Dock")));
            this.continueButton.Enabled = ((bool)(resources.GetObject("continueButton.Enabled")));
            this.continueButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("continueButton.FlatStyle")));
            this.continueButton.Font = ((System.Drawing.Font)(resources.GetObject("continueButton.Font")));
            this.continueButton.Image = ((System.Drawing.Image)(resources.GetObject("continueButton.Image")));
            this.continueButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("continueButton.ImageAlign")));
            this.continueButton.ImageIndex = ((int)(resources.GetObject("continueButton.ImageIndex")));
            this.continueButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("continueButton.ImeMode")));
            this.continueButton.Location = ((System.Drawing.Point)(resources.GetObject("continueButton.Location")));
            this.continueButton.Name = "continueButton";
            this.continueButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("continueButton.RightToLeft")));
            this.continueButton.Size = ((System.Drawing.Size)(resources.GetObject("continueButton.Size")));
            this.continueButton.TabIndex = ((int)(resources.GetObject("continueButton.TabIndex")));
            this.continueButton.Text = resources.GetString("continueButton.Text");
            this.continueButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("continueButton.TextAlign")));
            this.continueButton.Visible = ((bool)(resources.GetObject("continueButton.Visible")));
            // 
            // detailsButton
            // 
            this.detailsButton.AccessibleDescription = resources.GetString("detailsButton.AccessibleDescription");
            this.detailsButton.AccessibleName = resources.GetString("detailsButton.AccessibleName");
            this.detailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("detailsButton.Anchor")));
            this.detailsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("detailsButton.BackgroundImage")));
            this.detailsButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("detailsButton.Dock")));
            this.detailsButton.Enabled = ((bool)(resources.GetObject("detailsButton.Enabled")));
            this.detailsButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("detailsButton.FlatStyle")));
            this.detailsButton.Font = ((System.Drawing.Font)(resources.GetObject("detailsButton.Font")));
            this.detailsButton.Image = ((System.Drawing.Image)(resources.GetObject("detailsButton.Image")));
            this.detailsButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("detailsButton.ImageAlign")));
            this.detailsButton.ImageIndex = ((int)(resources.GetObject("detailsButton.ImageIndex")));
            this.detailsButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("detailsButton.ImeMode")));
            this.detailsButton.Location = ((System.Drawing.Point)(resources.GetObject("detailsButton.Location")));
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("detailsButton.RightToLeft")));
            this.detailsButton.Size = ((System.Drawing.Size)(resources.GetObject("detailsButton.Size")));
            this.detailsButton.TabIndex = ((int)(resources.GetObject("detailsButton.TabIndex")));
            this.detailsButton.Text = resources.GetString("detailsButton.Text");
            this.detailsButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("detailsButton.TextAlign")));
            this.detailsButton.Visible = ((bool)(resources.GetObject("detailsButton.Visible")));
            this.detailsButton.Click += new System.EventHandler(this.OnDetailsButtonClick);
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.AccessibleDescription = resources.GetString("detailsTextBox.AccessibleDescription");
            this.detailsTextBox.AccessibleName = resources.GetString("detailsTextBox.AccessibleName");
            this.detailsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("detailsTextBox.Anchor")));
            this.detailsTextBox.AutoSize = ((bool)(resources.GetObject("detailsTextBox.AutoSize")));
            this.detailsTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("detailsTextBox.BackgroundImage")));
            this.detailsTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("detailsTextBox.Dock")));
            this.detailsTextBox.Enabled = ((bool)(resources.GetObject("detailsTextBox.Enabled")));
            this.detailsTextBox.Font = ((System.Drawing.Font)(resources.GetObject("detailsTextBox.Font")));
            this.detailsTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("detailsTextBox.ImeMode")));
            this.detailsTextBox.Location = ((System.Drawing.Point)(resources.GetObject("detailsTextBox.Location")));
            this.detailsTextBox.MaxLength = ((int)(resources.GetObject("detailsTextBox.MaxLength")));
            this.detailsTextBox.Multiline = ((bool)(resources.GetObject("detailsTextBox.Multiline")));
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.PasswordChar = ((char)(resources.GetObject("detailsTextBox.PasswordChar")));
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("detailsTextBox.RightToLeft")));
            this.detailsTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("detailsTextBox.ScrollBars")));
            this.detailsTextBox.Size = ((System.Drawing.Size)(resources.GetObject("detailsTextBox.Size")));
            this.detailsTextBox.TabIndex = ((int)(resources.GetObject("detailsTextBox.TabIndex")));
            this.detailsTextBox.Text = resources.GetString("detailsTextBox.Text");
            this.detailsTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("detailsTextBox.TextAlign")));
            this.detailsTextBox.Visible = ((bool)(resources.GetObject("detailsTextBox.Visible")));
            this.detailsTextBox.WordWrap = ((bool)(resources.GetObject("detailsTextBox.WordWrap")));
            // 
            // pictureBox
            // 
            this.pictureBox.AccessibleDescription = resources.GetString("pictureBox.AccessibleDescription");
            this.pictureBox.AccessibleName = resources.GetString("pictureBox.AccessibleName");
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pictureBox.Anchor")));
            this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
            this.pictureBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pictureBox.Dock")));
            this.pictureBox.Enabled = ((bool)(resources.GetObject("pictureBox.Enabled")));
            this.pictureBox.Font = ((System.Drawing.Font)(resources.GetObject("pictureBox.Font")));
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pictureBox.ImeMode")));
            this.pictureBox.Location = ((System.Drawing.Point)(resources.GetObject("pictureBox.Location")));
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pictureBox.RightToLeft")));
            this.pictureBox.Size = ((System.Drawing.Size)(resources.GetObject("pictureBox.Size")));
            this.pictureBox.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pictureBox.SizeMode")));
            this.pictureBox.TabIndex = ((int)(resources.GetObject("pictureBox.TabIndex")));
            this.pictureBox.TabStop = false;
            this.pictureBox.Text = resources.GetString("pictureBox.Text");
            this.pictureBox.Visible = ((bool)(resources.GetObject("pictureBox.Visible")));
            // 
            // imageList
            // 
            this.imageList.ImageSize = ((System.Drawing.Size)(resources.GetObject("imageList.ImageSize")));
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.SystemColors.ActiveCaption;
            // 
            // messageText
            // 
            this.messageText.AccessibleDescription = resources.GetString("messageText.AccessibleDescription");
            this.messageText.AccessibleName = resources.GetString("messageText.AccessibleName");
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("messageText.Anchor")));
            this.messageText.AutoSize = ((bool)(resources.GetObject("messageText.AutoSize")));
            this.messageText.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("messageText.Dock")));
            this.messageText.Enabled = ((bool)(resources.GetObject("messageText.Enabled")));
            this.messageText.Font = ((System.Drawing.Font)(resources.GetObject("messageText.Font")));
            this.messageText.Image = ((System.Drawing.Image)(resources.GetObject("messageText.Image")));
            this.messageText.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("messageText.ImageAlign")));
            this.messageText.ImageIndex = ((int)(resources.GetObject("messageText.ImageIndex")));
            this.messageText.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("messageText.ImeMode")));
            this.messageText.Location = ((System.Drawing.Point)(resources.GetObject("messageText.Location")));
            this.messageText.Name = "messageText";
            this.messageText.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("messageText.RightToLeft")));
            this.messageText.Size = ((System.Drawing.Size)(resources.GetObject("messageText.Size")));
            this.messageText.TabIndex = ((int)(resources.GetObject("messageText.TabIndex")));
            this.messageText.Text = resources.GetString("messageText.Text");
            this.messageText.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("messageText.TextAlign")));
            this.messageText.Visible = ((bool)(resources.GetObject("messageText.Visible")));
            // 
            // UnhandledExceptionForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.continueButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.exitButton);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "UnhandledExceptionForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private void OnDetailsButtonClick(object sender, EventArgs e)
        {
            this.detailsVisible = !this.detailsVisible;

            if (this.detailsVisible)
            {
                this.detailsButton.Image = this.upImage;
                this.Height = this.Height + this.detailsTextBox.Height;
            }
            else
            {
                this.detailsButton.Image = this.downImage;
                this.Height = this.Height - this.detailsTextBox.Height;
            }
        }
    }
}
