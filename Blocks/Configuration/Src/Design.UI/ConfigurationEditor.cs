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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <summary>
    /// 
    /// </summary>
    internal partial class ConfigurationEditor : UserControl, IUIService
    {
        private List<ConfigurationNode> errorNodes = new List<ConfigurationNode>();
        private Dictionary<IConfigurationUIHierarchy, bool> dirtyHierarchies = new Dictionary<IConfigurationUIHierarchy, bool>();
        private TreeNodeFactory treeNodeFactory;
        private ConfigurationUIHierarchy mainConfigurationHierarchy;
        private SolutionConfigurationNode solutionNode;
        private ConfigurationTreeNode solutionTreeNode;
        private IServiceProvider serviceProvider;
        private IUIService uiService;
        private SingleHierarchyConfigurationUIHostAdapter adapter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="adaptor"></param>
        public ConfigurationEditor(SingleHierarchyConfigurationUIHostAdapter adaptor, IServiceProvider serviceProvider)
        {
            this.adapter = adaptor;
            this.serviceProvider = serviceProvider;

            treeNodeFactory = new TreeNodeFactory();

            InitializeComponent();

            treeNodeFactory.SetImageContainer(new ConfigurationNodeImageContainer(this.nodeImages));

            uiService = this;
        }

        private void SetNodeColor(ConfigurationNode node, Color color)
        {
            ConfigurationTreeNode treeNode = treeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeNode.ForeColor = color;
            }
        }

        private void ResetNodeColor(ConfigurationNode node)
        {
            ConfigurationTreeNode treeNode = treeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeNode.ForeColor = treeView.ForeColor;
            }
        }

        internal void SetMainHierarchy(ConfigurationUIHierarchy hierarchy)
        {
            treeView.Nodes.Clear();
            solutionNode = new SolutionConfigurationNode();
            solutionTreeNode = treeNodeFactory.Create(solutionNode);

            treeView.Nodes.Add(solutionTreeNode);

			// the hierarchy of configuration nodes will not include the solution node
			// the app and solution nodes will be related by the tree nodes poiting to them,
			// and the hierarchy will be overriden to point to the app node.
			// The stand alone tool does something similar - see Microsoft.Practices.EnterpriseLibrary.Configuration.Console.OnHierarchyAdded
            mainConfigurationHierarchy = hierarchy;
            solutionTreeNode.Nodes.Add(treeNodeFactory.Create(hierarchy.RootNode));
            solutionTreeNode.Expand();

            ServiceHelper.GetUIHierarchyService(serviceProvider).SelectedHierarchy = hierarchy;
            IErrorLogService errorService = ServiceHelper.GetErrorService(serviceProvider);
            List<Task> tasks = GetTasksFromErrorService(errorService);
            adapter.DoTasksChanged(tasks);
        }

        internal ConfigurationUIHierarchy MainHierarchy
        {
            get { return mainConfigurationHierarchy; }
        }

        #region Treeview events
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!e.CancelEdit)
            {
                ConfigurationTreeNode selectedNode = (ConfigurationTreeNode)e.Node;

                string label = e.Label;
                if (label != null)
                {
                    if (label.Trim().Length == 0)
                    {
                        e.CancelEdit = true;
                        uiService.ShowMessage(Resources.NodeLabelEditText, Resources.NodeLabelEditCaption);
                        selectedNode.BeginEdit();
                    }
                    else
                    {
                        try
                        {
                            selectedNode.ConfigurationNode.Name = label;
                            uiService.SetUIDirty(selectedNode.ConfigurationNode.Hierarchy);
                            selectedNode.EndEdit(false);
                            RefreshPropertyGrid();
                        }
                        catch (InvalidOperationException ex)
                        {
                            e.CancelEdit = true;
                            uiService.ShowMessage(Resources.NodeLabelEditFailedText + ex.Message, Resources.NodeLabelEditCaption);
                            selectedNode.BeginEdit();
                        }
                    }
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ConfigurationTreeNode selectedNode = treeView.SelectedNode as ConfigurationTreeNode;
            if (selectedNode != null)
            {
                ConfigurationNode selectedConfigurationNode = selectedNode.ConfigurationNode;

                IUICommandService commandService = ServiceHelper.GetUICommandService(serviceProvider);
                treeView.ContextMenu = CreateContextMenu(selectedConfigurationNode, commandService);
            }
            RefreshPropertyGrid();
        }

        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ConfigurationTreeNode node = e.Node as ConfigurationTreeNode;
            if (node == null)
            {
                return;
            }
            bool readOnly = NodeReadOnly(node.ConfigurationNode);
            if (readOnly)
            {
                e.CancelEdit = true;
            }
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ConfigurationTreeNode selectedNode = treeView.GetNodeAt(e.X, e.Y) as ConfigurationTreeNode;
                SetSelectedNode(selectedNode);
            }
        }

        #endregion

        private void SetSelectedNode(ConfigurationTreeNode selectedNode)
        {
            treeView.SelectedNode = null;
            treeView.SelectedNode = selectedNode;

            RefreshPropertyGrid();
        }

        private static bool NodeReadOnly(ConfigurationNode node)
        {
            object[] attributes = node.GetType().GetProperty("Name").GetCustomAttributes(typeof(ReadOnlyAttribute), true);
            return (attributes.Length > 0);
        }

        public void ClearDirtyState()
        {
            dirtyHierarchies = new Dictionary<IConfigurationUIHierarchy, bool>();
        }

        #region Context Menus
        private ContextMenu CreateContextMenu(ConfigurationNode configurationNode, IUICommandService commandService)
        {
            MenuItem newMenuItem = CreateNewMenuItem();
            MenuItem renameMenuItem = CreateRenameMenuItem(configurationNode);

            ContextMenu contextMenu = new ContextMenu(new MenuItem[] { newMenuItem, renameMenuItem });

            List<ConfigurationMenuItem> configurationMenuItems = QueryDynamicMenuItems(configurationNode, commandService);

            foreach (ConfigurationMenuItem menuItem in configurationMenuItems)
            {
                menuItem.Select += new EventHandler(OnMenuItemSelect);

                switch (menuItem.InsertionPoint)
                {
                    case InsertionPoint.Action:
                        contextMenu.MenuItems.Add(menuItem);
                        break;

                    case InsertionPoint.New:
                        newMenuItem.MenuItems.Add(menuItem);
                        break;
                }
            }

            if (newMenuItem.MenuItems.Count == 0)
            {
                newMenuItem.Enabled = false;
            }

            return contextMenu;
        }

        private static List<ConfigurationMenuItem> QueryDynamicMenuItems(ConfigurationNode configurationNode, IUICommandService commandService)
        {
            List<ConfigurationMenuItem> configurationMenuItems = new List<ConfigurationMenuItem>();
            commandService.ForEach(configurationNode.GetType(), delegate(ConfigurationUICommand command)
            {
                configurationMenuItems.Add(new ConfigurationMenuItem(configurationNode, command));
            });

            configurationMenuItems.Sort(delegate(ConfigurationMenuItem lhs, ConfigurationMenuItem rhs)
            {
                return lhs.Text.CompareTo(rhs.Text);
            });

            return configurationMenuItems;
        }

        private MenuItem CreateRenameMenuItem(ConfigurationNode configurationNode)
        {
            MenuItem renameMenuItem = new MenuItem();
            renameMenuItem.Text = Resources.MenuItemRename;
            renameMenuItem.Enabled = !NodeReadOnly(configurationNode);
            renameMenuItem.Click += new EventHandler(renameMenuItem_Click);

            return renameMenuItem;
        }

        void renameMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }

        private MenuItem CreateNewMenuItem()
        {
            MenuItem newMenuItem = new MenuItem();
            newMenuItem.Text = Resources.MenuItemNew;

            return newMenuItem;
        }

        private void OnMenuItemSelect(object sender, EventArgs args)
        {
            RefreshPropertyGrid();

            ConfigurationMenuItem item = sender as ConfigurationMenuItem;
            if (item != null)
            {
                SetStatus(item.LongText);
            }
        }
        #endregion

        #region IUIService Members

        /// <summary>
        /// 
        /// </summary>
        public void BeginUpdate()
        {
            treeView.BeginUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndUpdate()
        {
            if (!IsDisposed)
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        public DialogResult ShowSaveDialog(SaveFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        public DialogResult ShowOpenDialog(OpenFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        /// <summary>
        /// 
        /// </summary>
        public IWin32Window OwnerWindow
        {
            get { return this; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorLogService"></param>
        public void DisplayErrorLog(IErrorLogService errorLogService)
        {
            List<Task> tasks = GetTasksFromErrorService(errorLogService);
            foreach (ConfigurationNodeTask task in tasks)
            {
                if (task.ConfigurationNode != null)
                {

                    if (!IsDisposed)
                    {
                        SetNodeColor(task.ConfigurationNode, Color.Red);
                    }
                    errorNodes.Add(task.ConfigurationNode);
                }
            }
            adapter.DoTasksChanged(tasks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchy"></param>
        public void SetUIDirty(IConfigurationUIHierarchy hierarchy)
        {
            dirtyHierarchies[hierarchy] = true;
        }

        public void ActivateNode(ConfigurationNode node)
        {
            ConfigurationTreeNode treeNode = treeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeView.SelectedNode = treeNode;
            }
            RefreshPropertyGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public bool IsDirty(IConfigurationUIHierarchy hierarchy)
        {
            if (dirtyHierarchies.ContainsKey(hierarchy))
            {
                return dirtyHierarchies[hierarchy];
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(string status)
        {
            adapter.DoStatusTextChanged(status);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearErrorDisplay()
        {

            if (!IsDisposed)
            {
                foreach (ConfigurationNode errorNode in errorNodes)
                {
                    ResetNodeColor(errorNode);
                }

                errorNodes.Clear();
            }
            adapter.DoTasksChanged(new List<Task>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void ShowError(Exception e)
        {
            MessageBox.Show(this, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void ShowError(Exception e, string message)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public void ShowError(Exception e, string message, string caption)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void ShowError(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshPropertyGrid()
        {
            ConfigurationTreeNode treeNode = treeView.SelectedNode as ConfigurationTreeNode;
            if (treeNode != null)
            {
                adapter.DoSelectionChanged(treeNode.ConfigurationNode);
            }
            else
            {
                adapter.DoSelectionChanged(null);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Task> GetTasksFromErrorService(IErrorLogService errorService)
        {
            List<Task> configuationTasks = new List<Task>();
            errorService.ForEachConfigurationErrors(delegate(ConfigurationError configurationError)
            {
                configuationTasks.Add(new ConfigurationNodeTask(configurationError.ConfigurationNode, configurationError.Message));
            });

            errorService.ForEachValidationErrors(delegate(ValidationError validationError)
            {
                configuationTasks.Add(new ConfigurationNodeTask(validationError.InvalidItem, validationError.Message));
            });

            return configuationTasks;
        }

    }
}



