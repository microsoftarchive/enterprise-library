//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents the UI for the assembly selector dialog.</para>
    /// </summary>
    public class TypeSelectorUI : Form
    {
        private const string AssemblyFileFilter = "Assemblies (*.exe;*.dll) | *.exe;*.dll";
        private ImageList typeImageList;
        private TreeView treeView;
        private Button browseButton;
        private IContainer components = null;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private Button cancelButton;
        private Button okButton;
        private TypeSelector selector;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TypeSelectorUI"/> class.</para>
        /// </summary>
        public TypeSelectorUI()
        {
            InitializeComponent();
            openFileDialog.Filter = AssemblyFileFilter;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TypeSelectorUI"/> class.</para>
        /// </summary>
        /// <param name="currentType">
        /// <para>The current <see cref="Type"/> selected.</para>
        /// </param>
        /// <param name="baseType">
        /// <para>The <see cref="Type"/> to filter and verify when loading.</para>
        /// </param>
        public TypeSelectorUI(Type currentType, Type baseType) : this(currentType, baseType, TypeSelectorIncludes.None, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="baseType"></param>
        /// <param name="flags"></param>
        public TypeSelectorUI(Type currentType, Type baseType, TypeSelectorIncludes flags) : this(currentType, baseType, flags, null)
        {
        }
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TypeSelectorUI"/> class.</para>
        /// </summary>
        /// <param name="currentType">
        /// <para>The current <see cref="Type"/> selected.</para>
        /// </param>
        /// <param name="baseType">
        /// <para>The <see cref="Type"/> to filter and verify when loading.</para>
        /// </param>
        /// <param name="flags">
        /// <para>The flags for the editor.</para>
        /// </param>
		/// <param name="configurationType"></param>
        public TypeSelectorUI(Type currentType, Type baseType, TypeSelectorIncludes flags, Type configurationType) : this()
        {
            this.openFileDialog.DereferenceLinks = false;
            this.selector = new TypeSelector(currentType, baseType, flags, configurationType, this.treeView);
            this.treeView.Select();
            this.Text += baseType.FullName;
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> selected for use.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> selected for use.</para>
        /// </value>
        public Type SelectedType
        {
            get { return this.treeView.SelectedNode.Tag as Type; }
        }

        /// <summary>
        /// <para>Gets the <see cref="ImageList"/> for the types.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ImageList"/> for the types.</para>
        /// </value>
        public ImageList TypeImageList
        {
            get { return this.typeImageList; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="Form"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
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

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TypeSelectorUI));
            this.treeView = new System.Windows.Forms.TreeView();
            this.typeImageList = new System.Windows.Forms.ImageList(this.components);
            this.browseButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AccessibleDescription = resources.GetString("treeView.AccessibleDescription");
            this.treeView.AccessibleName = resources.GetString("treeView.AccessibleName");
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("treeView.Anchor")));
            this.treeView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("treeView.BackgroundImage")));
            this.treeView.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("treeView.Dock")));
            this.treeView.Enabled = ((bool)(resources.GetObject("treeView.Enabled")));
            this.treeView.Font = ((System.Drawing.Font)(resources.GetObject("treeView.Font")));
            this.treeView.ImageIndex = ((int)(resources.GetObject("treeView.ImageIndex")));
            this.treeView.ImageList = this.typeImageList;
            this.treeView.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("treeView.ImeMode")));
            this.treeView.Indent = ((int)(resources.GetObject("treeView.Indent")));
            this.treeView.ItemHeight = ((int)(resources.GetObject("treeView.ItemHeight")));
            this.treeView.Location = ((System.Drawing.Point)(resources.GetObject("treeView.Location")));
            this.treeView.Name = "treeView";
            this.treeView.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("treeView.RightToLeft")));
            this.treeView.SelectedImageIndex = ((int)(resources.GetObject("treeView.SelectedImageIndex")));
            this.treeView.Size = ((System.Drawing.Size)(resources.GetObject("treeView.Size")));
            this.treeView.Sorted = true;
            this.treeView.TabIndex = ((int)(resources.GetObject("treeView.TabIndex")));
            this.treeView.Text = resources.GetString("treeView.Text");
            this.treeView.Visible = ((bool)(resources.GetObject("treeView.Visible")));
            this.treeView.DoubleClick += new System.EventHandler(this.OnTreeViewDoubleClick);
            // 
            // typeImageList
            // 
            this.typeImageList.ImageSize = ((System.Drawing.Size)(resources.GetObject("typeImageList.ImageSize")));
            this.typeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("typeImageList.ImageStream")));
            this.typeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // browseButton
            // 
            this.browseButton.AccessibleDescription = resources.GetString("browseButton.AccessibleDescription");
            this.browseButton.AccessibleName = resources.GetString("browseButton.AccessibleName");
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("browseButton.Anchor")));
            this.browseButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("browseButton.BackgroundImage")));
            this.browseButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("browseButton.Dock")));
            this.browseButton.Enabled = ((bool)(resources.GetObject("browseButton.Enabled")));
            this.browseButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("browseButton.FlatStyle")));
            this.browseButton.Font = ((System.Drawing.Font)(resources.GetObject("browseButton.Font")));
            this.browseButton.Image = ((System.Drawing.Image)(resources.GetObject("browseButton.Image")));
            this.browseButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("browseButton.ImageAlign")));
            this.browseButton.ImageIndex = ((int)(resources.GetObject("browseButton.ImageIndex")));
            this.browseButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("browseButton.ImeMode")));
            this.browseButton.Location = ((System.Drawing.Point)(resources.GetObject("browseButton.Location")));
            this.browseButton.Name = "browseButton";
            this.browseButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("browseButton.RightToLeft")));
            this.browseButton.Size = ((System.Drawing.Size)(resources.GetObject("browseButton.Size")));
            this.browseButton.TabIndex = ((int)(resources.GetObject("browseButton.TabIndex")));
            this.browseButton.Text = resources.GetString("browseButton.Text");
            this.browseButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("browseButton.TextAlign")));
            this.browseButton.Visible = ((bool)(resources.GetObject("browseButton.Visible")));
            this.browseButton.Click += new System.EventHandler(this.OnBrowseButtonClick);
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
            this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
            this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
            this.okButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("okButton.Dock")));
            this.okButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
            this.okButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("okButton.FlatStyle")));
            this.okButton.Font = ((System.Drawing.Font)(resources.GetObject("okButton.Font")));
            this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
            this.okButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
            this.okButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
            this.okButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("okButton.ImeMode")));
            this.okButton.Location = ((System.Drawing.Point)(resources.GetObject("okButton.Location")));
            this.okButton.Name = "okButton";
            this.okButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("okButton.RightToLeft")));
            this.okButton.Size = ((System.Drawing.Size)(resources.GetObject("okButton.Size")));
            this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.okButton.Text = resources.GetString("okButton.Text");
            this.okButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.TextAlign")));
            this.okButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClick);
            // 
            // TypeSelectorUI
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.treeView);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "TypeSelectorUI";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
        #endregion        

        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string originalAssemblyFileName = this.openFileDialog.FileName;
                    string referenceDirectory = Path.GetDirectoryName(originalAssemblyFileName);

                    Assembly assembly = null;
                    using (AssemblyLoader loaderHook = new AssemblyLoader(originalAssemblyFileName, referenceDirectory))
                    {
                        assembly = Assembly.LoadFrom(loaderHook.CopiedAssemblyPath);
                    
                        if (!this.selector.LoadTreeView(assembly))
                        {
                            DisplayMessageBox(string.Format(CultureInfo.CurrentUICulture, Resources.NoTypesFoundInAssemblyErrorMessage, assembly.GetName().Name, this.selector.TypeToVerify.FullName), Resources.NoTypesFoundInAssemblyCaption);
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    DisplayMessageBox(string.Format(CultureInfo.CurrentUICulture, Resources.AssemblyLoadFailedErrorMessage, ex.Message), string.Empty);
                    return;
                }
                catch (BadImageFormatException ex)
                {
                    DisplayMessageBox(string.Format(CultureInfo.CurrentUICulture, Resources.AssemblyLoadFailedErrorMessage, ex.Message), string.Empty);
                    return;
                }
                catch(ReflectionTypeLoadException ex)
                {
                    DisplayMessageBox(string.Format(CultureInfo.CurrentUICulture, Resources.EnumTypesFailedErrorMessage, ex.Message), string.Empty);
                }
            }
        }

        private void OnTreeViewDoubleClick(object sender, EventArgs e)
        {
            if (SelectedType != null)
            {
                this.OnOkButtonClick(this, EventArgs.Empty);
            }
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (this.selector.TypeToVerify != null)
            {
                if (!this.selector.IsTypeValid(SelectedType))
                {
                    DisplayMessageBox(string.Format(CultureInfo.CurrentUICulture, Resources.TypeSubclassErrorMsg, this.selector.TypeToVerify.FullName), string.Empty);
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DisplayMessageBox(string message, string caption)
        {
            DisplayMessageBox(message, caption, MessageBoxIcon.Information);
        }

        private void DisplayMessageBox(string message, string caption, MessageBoxIcon icon)
        {
			MessageBoxOptions options = (MessageBoxOptions)0;

			if (RightToLeft == RightToLeft.Yes)
			{
				options = MessageBoxOptions.RtlReading |
				   MessageBoxOptions.RightAlign;
			}


            MessageBox.Show(message,
                                caption,
                                MessageBoxButtons.OK,
                                icon,
                                MessageBoxDefaultButton.Button1,
								options);

        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Collapses all assemlby treeview nodes in the UI.
        /// </summary>
        public void CollapseAssemlbyNodes()
        {
            foreach (TreeNode assemblyNode in selector.AssembliesRootNode.Nodes)
            {
                assemblyNode.Collapse();
            }
        }
    }
}