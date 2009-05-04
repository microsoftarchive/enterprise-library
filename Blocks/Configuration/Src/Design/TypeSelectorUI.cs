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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents the UI for the assembly selector dialog.</para>
    /// </summary>
    public class TypeSelectorUI : Form
    {
        private const string AssemblyFileFilter = "Assemblies (*.exe;*.dll) | *.exe;*.dll";
        private ImageList typeImageList;
        private Button browseButton;
        private IContainer components = null;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private Button cancelButton;
        private Button okButton;
        private TextBox filter;
        private Label filterLabel;
        private Button loadFromGACButton;

        private TreeView typeBuilderTreeView;
        private TypeBuildNode rootTypeNode;
        private TreeNode currentTypeTreeNode;

        // true when the types tree is being updated after the selected node in the type building tree changed
        private bool updatingTreeView = false;

        private TreeView treeView;
        private TypeSelector currentTypeSelector;

        private readonly Font undefinedNodeFont;

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
        public TypeSelectorUI(Type currentType, Type baseType)
            : this(currentType, baseType, TypeSelectorIncludes.None, null)
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
        public TypeSelectorUI(Type currentType, Type baseType, TypeSelectorIncludes flags)
            : this(currentType, baseType, flags, null)
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
        /// <param name="configurationType">The base configuration type the selected type must reference.</param>
        public TypeSelectorUI(Type currentType, Type baseType, TypeSelectorIncludes flags, Type configurationType)
            : this()
        {
            this.undefinedNodeFont = new Font(this.typeBuilderTreeView.Font, FontStyle.Italic);

            this.openFileDialog.DereferenceLinks = false;
            this.Text += baseType.FullName;

            this.rootTypeNode
                = TypeBuildNode.CreateTreeForType(
                    currentType,
                    new TypeBuildNodeConstraint(baseType, configurationType, flags));
            this.currentTypeTreeNode = this.typeBuilderTreeView.Nodes.Add(rootTypeNode.Description);
            this.currentTypeTreeNode.Tag = this.rootTypeNode;
            this.UpdateTypeTreeNodeChildren(this.currentTypeTreeNode);
            this.typeBuilderTreeView.SelectedNode = this.currentTypeTreeNode;
            this.currentTypeTreeNode.EnsureVisible();

            this.treeView.Select();
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> selected for use.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> selected for use.</para>
        /// </value>
        public Type SelectedType
        {
            get;
            private set;
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
                if (this.undefinedNodeFont != null)
                {
                    this.undefinedNodeFont.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypeSelectorUI));
            this.treeView = new System.Windows.Forms.TreeView();
            this.typeImageList = new System.Windows.Forms.ImageList(this.components);
            this.browseButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filter = new System.Windows.Forms.TextBox();
            this.filterLabel = new System.Windows.Forms.Label();
            this.loadFromGACButton = new System.Windows.Forms.Button();
            this.typeBuilderTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView
            // 
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.HideSelection = false;
            this.treeView.ImageList = this.typeImageList;
            this.treeView.ItemHeight = 16;
            this.treeView.Name = "treeView";
            this.treeView.Sorted = true;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // typeImageList
            // 
            this.typeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("typeImageList.ImageStream")));
            this.typeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.typeImageList.Images.SetKeyName(0, "");
            this.typeImageList.Images.SetKeyName(1, "");
            this.typeImageList.Images.SetKeyName(2, "");
            this.typeImageList.Images.SetKeyName(3, "");
            this.typeImageList.Images.SetKeyName(4, "");
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.Click += new System.EventHandler(this.OnBrowseButtonClick);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClick);
            // 
            // filter
            // 
            resources.ApplyResources(this.filter, "filter");
            this.filter.Name = "filter";
            this.filter.TextChanged += new System.EventHandler(this.filter_TextChanged);
            // 
            // filterLabel
            // 
            resources.ApplyResources(this.filterLabel, "filterLabel");
            this.filterLabel.Name = "filterLabel";
            // 
            // loadFromGACButton
            // 
            resources.ApplyResources(this.loadFromGACButton, "loadFromGACButton");
            this.loadFromGACButton.Name = "loadFromGACButton";
            this.loadFromGACButton.Click += new System.EventHandler(this.loadFromGACButton_Click);
            // 
            // typeBuilderTreeView
            // 
            resources.ApplyResources(this.typeBuilderTreeView, "typeBuilderTreeView");
            this.typeBuilderTreeView.HideSelection = false;
            this.typeBuilderTreeView.Name = "typeBuilderTreeView";
            this.typeBuilderTreeView.ShowNodeToolTips = true;
            this.typeBuilderTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.typeBuilderTreeView_AfterSelect);
            // 
            // TypeSelectorUI
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.typeBuilderTreeView);
            this.Controls.Add(this.loadFromGACButton);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.filter);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.treeView);
            this.MinimizeBox = false;
            this.Name = "TypeSelectorUI";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            LoadAssembly(() =>
                {
                    DialogResult result = this.openFileDialog.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        string originalAssemblyFileName = this.openFileDialog.FileName;
                        string referenceDirectory = Path.GetDirectoryName(originalAssemblyFileName);

                        using (AssemblyLoader loaderHook = new AssemblyLoader(originalAssemblyFileName, referenceDirectory))
                        {
                            return Assembly.LoadFrom(loaderHook.CopiedAssemblyPath);
                        }
                    }
                    else
                    {
                        return null;
                    }
                });
        }

        private void loadFromGACButton_Click(object sender, EventArgs e)
        {
            LoadAssembly(() =>
            {
                LoadAssemblyFromCacheDialog dialog = new LoadAssemblyFromCacheDialog();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    return Assembly.Load(dialog.AssemblyName);
                }
                else
                {
                    return null;
                }
            });
        }

        private void LoadAssembly(Func<Assembly> getAssembly)
        {
            Assembly assembly = null;

            try
            {
                if ((assembly = getAssembly()) != null)
                {
                    try
                    {
                        this.treeView.BeginUpdate();

                        if (!this.currentTypeSelector.LoadFilteredTreeView(assembly))
                        {
                            DisplayMessageBox(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.NoTypesFoundInAssemblyErrorMessage,
                                    assembly.GetName().Name,
                                    this.currentTypeSelector.TypeToVerify.FullName),
                                Resources.NoTypesFoundInAssemblyCaption);
                        }
                    }
                    finally
                    {
                        this.treeView.EndUpdate();
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                DisplayMessageBox(
                    string.Format(CultureInfo.CurrentCulture, Resources.AssemblyLoadFailedErrorMessage, ex.Message),
                    string.Empty);
                return;
            }
            catch (BadImageFormatException ex)
            {
                DisplayMessageBox(
                    string.Format(CultureInfo.CurrentCulture, Resources.AssemblyLoadFailedErrorMessage, ex.Message),
                    string.Empty);
                return;
            }
            catch (ReflectionTypeLoadException ex)
            {
                DisplayMessageBox(
                    string.Format(CultureInfo.CurrentCulture, Resources.EnumTypesFailedErrorMessage, ex.Message),
                    string.Empty);
            }
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            Type theType = null;

            try
            {
                theType = this.rootTypeNode.BuildType();
            }
            catch (TypeBuildException ex)
            {
                DisplayMessageBox(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ErrorBuildingType,
                        ex.Message),
                    string.Empty,
                    MessageBoxIcon.Error);

                TreeNode treeNodeForFailingNode
                    = GetTreeNodeForTypeNode(ex.FailingNode, this.typeBuilderTreeView.Nodes[0]);
                treeNodeForFailingNode.ForeColor = Color.Red;
                treeNodeForFailingNode.ToolTipText = ex.Message;

                return;
            }

            this.SelectedType = theType;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static TreeNode GetTreeNodeForTypeNode(TypeBuildNode typeNode, TreeNode treeNode)
        {
            if (treeNode.Tag == typeNode)
            {
                return treeNode;
            }

            foreach (TreeNode childTreeNode in treeNode.Nodes)
            {
                return GetTreeNodeForTypeNode(typeNode, childTreeNode);
            }

            throw new InvalidOperationException();
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

        /// <summary>
        /// Collapses all assemlby treeview nodes in the UI.
        /// </summary>
        public void CollapseAssemlbyNodes()
        {
            foreach (TreeNode assemblyNode in currentTypeSelector.AssembliesRootNode.Nodes)
            {
                assemblyNode.Collapse();
            }
        }

        private void filter_TextChanged(object sender, EventArgs e)
        {
            if (!updatingTreeView)
            {
                try
                {
                    this.treeView.BeginUpdate();

                    this.currentTypeSelector.UpdateFilter(filter.Text);
                }
                finally
                {
                    this.treeView.EndUpdate();
                }
            }
        }

        private void typeBuilderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.updatingTreeView = true;
                this.currentTypeTreeNode = e.Node;
                this.filter.Clear();
                this.UpdateAvailableTypesTree();
            }
            finally
            {
                this.updatingTreeView = false;
            }
        }

        private void UpdateAvailableTypesTree()
        {
            TypeBuildNode currentTypeNode = (TypeBuildNode)this.currentTypeTreeNode.Tag;

            Cursor previousCursor = this.Cursor;
            try
            {
                this.treeView.BeginUpdate();
                this.Cursor = Cursors.WaitCursor;

                this.treeView.Nodes.Clear();
                this.currentTypeSelector
                    = new TypeSelector(
                        currentTypeNode.NodeType,
                        currentTypeNode.Constraint.BaseType,
                        currentTypeNode.Constraint.TypeSelectorIncludes,
                        currentTypeNode.Constraint.ConfigurationType,
                        this.treeView);
                TreeNode selectedNode = this.treeView.SelectedNode;
                this.treeView.Sort();
                if (selectedNode != null)
                {
                    this.treeView.SelectedNode = selectedNode;
                    selectedNode.EnsureVisible();
                }
            }
            finally
            {
                this.treeView.EndUpdate();
                this.Cursor = previousCursor;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Type selectedType;
            if ((selectedType = e.Node.Tag as Type) != null)
            {
                this.AvailableTypeSelected(selectedType);
            }
        }

        private void AvailableTypeSelected(Type selectedType)
        {
            if (selectedType != null)
            {
                TypeBuildNode typeNode = (TypeBuildNode)this.currentTypeTreeNode.Tag;

                if (typeNode.NodeType != selectedType)
                {
                    this.typeBuilderTreeView.BeginUpdate();

                    try
                    {
                        bool childrenChanged = typeNode.SetNodeType(selectedType);

                        // update the node
                        this.currentTypeTreeNode.Text = typeNode.Description;
                        this.currentTypeTreeNode.ToolTipText = selectedType.AssemblyQualifiedName;
                        this.currentTypeTreeNode.NodeFont = this.typeBuilderTreeView.Font;
                        this.currentTypeTreeNode.ForeColor = SystemColors.ControlText;

                        if (childrenChanged)
                        {
                            this.UpdateTypeTreeNodeChildren(this.currentTypeTreeNode);
                        }
                    }
                    finally
                    {
                        this.typeBuilderTreeView.EndUpdate();
                    }
                }
            }
            else
            {
                Debug.Fail("This shouldn't have happened.");
            }
        }

        private void UpdateTypeTreeNodeChildren(TreeNode treeNode)
        {
            TypeBuildNode typeNode = (TypeBuildNode)treeNode.Tag;

            treeNode.Nodes.Clear();
            foreach (TypeBuildNode childTypeNode in typeNode.GenericParameterNodes)
            {
                TreeNode childTreeNode = treeNode.Nodes.Add(childTypeNode.Description);
                childTreeNode.Tag = childTypeNode;

                // set the formatting
                childTreeNode.ForeColor = SystemColors.ControlText;
                if (childTypeNode.NodeType != null)
                {
                    childTreeNode.ToolTipText = childTypeNode.NodeType.AssemblyQualifiedName;
                    UpdateTypeTreeNodeChildren(childTreeNode);
                }
                else
                {
                    childTreeNode.NodeFont = undefinedNodeFont;
                }
            }
            treeNode.Expand();
        }
    }
}
