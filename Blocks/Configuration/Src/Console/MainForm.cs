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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Properties;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    internal delegate void ExceptionHandler(Exception e);

    /// <devdoc>
    /// Represents a form that contains a ConfigurationControl, PropertyGrid and ValidationErrorListView.
    /// </devdoc>
    internal class MainForm : Form, IUIService
    {
        private TreeNodeFactory treeNodeFactory;
        private IContainer components;
        private ImageList toolbarImageList;
        private ImageList treeViewImageList;
        private ToolBar toolBar;
        private ToolBarButton newButton;
        private ToolBarButton openButton;
        private ToolBarButton saveAppButton;
        private ToolBarButton saveAllButton;
        private MenuItem fileMenuItem;
        private MenuItem helpMenuItem;
        private MainMenu mainMenu;
        private MenuItem exitMenuItem;
        private MenuItem aboutMenuItem;
        private MenuItem newAppMenuItem;
        private MenuItem openAppmenuItem;
        private MenuItem saveAppMenuItem;
        private MenuItem saveAllMenuItem;
        private MenuItem actionMenuItem;
        private StatusBar statusBar;
        private StatusBarPanel statusBarPanel;
        private Panel bottomPanel;
        private Panel validationErrorsPanel;
        private ListView errorsListView;
        private Panel validationTitlePanel;
        private Label validationTitleLabel;
        private ColumnHeader nameHeader;
        private ColumnHeader propertyHeader;
        private ColumnHeader descriptionHeader;
        private ColumnHeader pathHeader;
        private Splitter bottomSplitter;
        private Panel treePanel;
        private CustomTreeView treeView;
        private Splitter middleSplitter;
        private Panel objectPanel;
        private PropertyGrid propertyGrid;
        private ServiceContainer designHost;
        private MenuItem fileSplitMenuItem;
        private MenuItem newMenuItem;
        private MenuItem renameMenuItem;
        private ConfigurationTreeNode solutionTreeNode;
        private string defaultText;
        private Dictionary<Guid, bool> hierarchyDirtyMap;
		private StringCollection hierarchyNames;
		private SolutionConfigurationNode solutionNode;
        private MenuItem saveAppAsMenuItem;
		private FormSerializer formSerializer;

        private enum MenuItemPosition
        {
            New = 0,
            Action = 1,
            Rename = 2,
            Help = 3
        }

        public MainForm(string [] files)
        {
            treeNodeFactory = new TreeNodeFactory();
			designHost = Build();
            InitializeComponent();
            InitializeHierarchy();            
            SetupExceptionHandlers();
            defaultText = this.Text;
			hierarchyDirtyMap = new Dictionary<Guid, bool>();
            hierarchyNames = new StringCollection();
			if (files.Length > 0)
			{
				OpenFiles(files);
			}
			formSerializer = new FormSerializer(this);
        }

		public ServiceContainer Build()
		{
			ServiceContainer container = new ServiceContainer();
			NodeNameCreationService nodeNameCreationService = new NodeNameCreationService();
            ConfigurationUIHierarchyService configurationUIHierarchy = new ConfigurationUIHierarchyService();

			container.AddService(typeof(INodeNameCreationService), nodeNameCreationService);
            container.AddService(typeof(IConfigurationUIHierarchyService), configurationUIHierarchy);
			container.AddService(typeof(IUIService), this);
			container.AddService(typeof(IErrorLogService), new ErrorLogService());
			container.AddService(typeof(INodeCreationService), new NodeCreationService());
            container.AddService(typeof(IUICommandService), new UICommandService(configurationUIHierarchy));
            container.AddService(typeof(IPluginDirectoryProvider), new AppDomainBasePluginDirectoryProvider());

			return container;
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (designHost != null)
                {
                    designHost.RemoveService(typeof(IUIService));
                    designHost.Dispose();
                }
            }
            base.Dispose(disposing);
        }

		public IConfigurationUIHierarchy CurrentHierarchy
		{

			get { return ServiceHelper.GetCurrentHierarchy(designHost); }
		}

		public IConfigurationUIHierarchyService HierarchyService
		{
			get { return ServiceHelper.GetUIHierarchyService(designHost); }
		}

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        public void ShowError(Exception e)
        {
            MessageBox.Show(this, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        public void ShowError(Exception e, string message)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowError(Exception e, string message, string caption)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        public void ShowError(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>Displays the specified message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// <para>Displays the specified message with the specified caption.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// <para>Displays the specified message in a message box with the specified caption and buttons to place on the dialog box.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        /// <param name="buttons">
        /// <para>One of the <see cref="MessageBoxButtons"/> values.</para>
        /// </param>
        /// <returns>
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </returns>
        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Information);
        }

        public void BeginUpdate()
        {
            this.treeView.BeginUpdate();
        }

        public void EndUpdate()
        {
            this.treeView.EndUpdate();
        }

        public DialogResult ShowSaveDialog(SaveFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        public DialogResult ShowOpenDialog(OpenFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        /// <summary>
        /// <para>When implemented by a class, gets the owner window.</para>
        /// </summary>
        /// <value>
        /// <para>The owner window.</para>
        /// </value>
        public IWin32Window OwnerWindow
        {
            get { return this; }
        }

        /// <summary>
        /// <para>Activates a node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to activate.</para>
        /// </param>
        public void ActivateNode(ConfigurationNode node)
        {
            ConfigurationTreeNode treeNode = treeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeView.SelectedNode = treeNode;
            }
        }

        public void DisplayErrorLog(IErrorLogService errorLogService)
        {
            ResetErrorList();
			errorLogService.ForEachValidationErrors(new Action<ValidationError>(DisplayValidationError));
            errorLogService.ForEachConfigurationErrors(new Action<ConfigurationError>(DisplayConfigurationError));
        }

		private void DisplayConfigurationError(ConfigurationError configurationError)
		{
			UpdateTreeNode(configurationError.ConfigurationNode);
			errorsListView.Items.Add(new ConfigurationErrorListViewItem(configurationError));
		}

		private void DisplayValidationError(ValidationError validationError)
		{
			UpdateTreeNode((ConfigurationNode)validationError.InvalidItem);
			errorsListView.Items.Add(new ValidationErrorListViewItem(validationError));
		}

        public void SetUIDirty(IConfigurationUIHierarchy hierarchy)
        {
            hierarchyDirtyMap[hierarchy.Id] = true;
            this.Text = String.Concat(defaultText, "*");
        }

        public bool IsDirty(IConfigurationUIHierarchy hierarchy)
        {
            if (hierarchyDirtyMap.ContainsKey(hierarchy.Id))
            {
                return (bool)hierarchyDirtyMap[hierarchy.Id];
            }
            return false;
        }

        public void SetStatus(string status)
        {
            this.statusBarPanel.Text = status;
        }

        public void ClearErrorDisplay()
        {
            ResetErrorList();
        }

        private void UpdateTreeNode(ConfigurationNode node)
        {
            if (node == null)
            {
                return;
            }
            TreeNode treeNode = treeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeNode.ForeColor = Color.Red;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
			propertyGrid.Refresh();

			base.OnClosing(e);
            ArrayList hierarchiesToBeSaved = GetHierarchiesToBeSaved();
            if (hierarchiesToBeSaved.Count == 0)
            {
                return;
            }

            SaveChangesDialog dialog = new SaveChangesDialog(hierarchiesToBeSaved);
            DialogResult result = dialog.ShowDialog(this);
            if (DialogResult.Cancel == result)
            {
                e.Cancel = true;
                return;
            }

            if (DialogResult.No == result)
            {
                return;
            }

			IConfigurationUIHierarchyService hierarchyService = designHost.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;
            hierarchiesToBeSaved = dialog.SelectedHieraries;
            ArrayList invalidHierarchies = new ArrayList(hierarchiesToBeSaved.Count);
            foreach (IConfigurationUIHierarchy saved in hierarchiesToBeSaved)
            {
                hierarchyService.SelectedHierarchy = saved;
                if (!SaveApplication(saved))
                {
                    invalidHierarchies.Add(saved);
                }
            }
            if (invalidHierarchies.Count > 0)
            {
                MessageBox.Show(this, Resources.SaveApplicationsMessage, Resources.SaveApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (IConfigurationUIHierarchy invalidHierarchy in invalidHierarchies)
                {
                    errorsListView.Items.Add(new ConfigurationErrorListViewItem(new ConfigurationError(invalidHierarchy.RootNode, string.Format(Resources.InvalidSaveHierarchyMessage, invalidHierarchy.RootNode.Name))));
                }
                e.Cancel = true;
            }
        }

        private void SaveAllApplications()
        {
			propertyGrid.Refresh();

			using (new WaitCursor())
            {
				IConfigurationUIHierarchyService service = (IConfigurationUIHierarchyService)designHost.GetService(typeof(IConfigurationUIHierarchyService));
				IConfigurationUIHierarchy[] hierarchies = service.GetAllHierarchies();
				IConfigurationUIHierarchy currentHierachy = GetSelectedHierarchy();
                foreach (IConfigurationUIHierarchy hierarchy in hierarchies)
                {
                    if (Object.ReferenceEquals(hierarchy.RootNode, solutionTreeNode.ConfigurationNode))
                    {
                        continue;
                    }
                    service.SelectedHierarchy = hierarchy;
                    SaveApplication(hierarchy);
                }
                service.SelectedHierarchy = currentHierachy;
            }
        }

        private ArrayList GetHierarchiesToBeSaved()
        {
			IConfigurationUIHierarchyService service = designHost.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;
            IConfigurationUIHierarchy[] hierarchies = service.GetAllHierarchies();
            ArrayList hierarchiesToBeSaved = new ArrayList(hierarchies.Length);
            foreach (IConfigurationUIHierarchy hierarchy in hierarchies)
            {
                if (IsDirty(hierarchy))
                {
                    hierarchiesToBeSaved.Add(hierarchy);
                }
            }
            return hierarchiesToBeSaved;
        }

		//private void AddServices()
		//{
		//    designHost.AddService(typeof(IUIService), this);
		//}

        private void InitializeHierarchy()
        {
            treeNodeFactory.SetImageContainer(new ConfigurationNodeImageContainer(treeViewImageList));
            solutionNode = new SolutionConfigurationNode();
			//IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(solutionNode, designHost);
			IConfigurationUIHierarchyService service = designHost.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;
			//service.AddHierarchy(hierarchy);
			service.HierarchyAdded += new EventHandler<HierarchyAddedEventArgs>(OnHierarchyAdded);
			service.HierarchyRemoved += new EventHandler<HierarchyRemovedEventArgs>(OnHierarchyRemoved);
            solutionTreeNode = treeNodeFactory.Create(solutionNode);
            treeView.Nodes.Add(solutionTreeNode);
            SetSelectedNode(solutionTreeNode);
        }

        private void SetupExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledExceptionForm form = new UnhandledExceptionForm(e.Exception);
            form.Text = Resources.UnhandledExceptionFormText;
            form.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.Abort)
            {
                this.Close();
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Invoke(new ThreadExceptionEventHandler(OnThreadException), new Object[] {sender, new ThreadExceptionEventArgs((Exception)e.ExceptionObject)});
        }

        private void ResetErrorList()
        {
            foreach (ConfigurationNodeListViewItem item in errorsListView.Items)
            {
				if (item.ConfigurationNode != null)
				{
                    TreeNode treeNode = treeNodeFactory.GetTreeNode(item.ConfigurationNode.Id);


					if (treeNode != null)
					{
						treeNode.ForeColor = treeView.ForeColor;
					}
				}
            }
            errorsListView.Items.Clear();
        }

        private void OnHierarchyAdded(object sender, HierarchyAddedEventArgs args)
        {
            ConfigurationTreeNode node = treeNodeFactory.Create(args.UIHierarchy.RootNode);
            solutionTreeNode.Nodes.Add(node);
            hierarchyNames.Add(node.Text);
            solutionTreeNode.Expand();
            hierarchyDirtyMap[args.UIHierarchy.Id] = false;			
            SetSelectedNode(node);
			SetSelectedHierarchy(node);
            EnableSaveOptions(true);
			args.UIHierarchy.Saved += new EventHandler<HierarchySavedEventArgs>(UIHierarchySaved);
        }

        private void OnHierarchyRemoved(object sender, HierarchyRemovedEventArgs args)
        {
            ConfigurationTreeNode node = treeNodeFactory.GetTreeNode(args.UIHierarchy.RootNode.Id);
            node.Remove();
			args.UIHierarchy.Saved -= new EventHandler<HierarchySavedEventArgs>(UIHierarchySaved);
            hierarchyDirtyMap.Remove(args.UIHierarchy.Id);
            hierarchyNames.Remove(node.Text);
            UpdateSaveStatus(args.UIHierarchy);
            if (solutionTreeNode.Nodes.Count == 0)
            {
                EnableSaveOptions(false);
            }
            ResetErrorList();
        }

        private void UIHierarchySaved(object sender, HierarchySavedEventArgs args)
        {
            UpdateSaveStatus(args.UIHierarchy);
        }

        private void UpdateSaveStatus(IConfigurationUIHierarchy hierarchy)
        {
            hierarchyDirtyMap[hierarchy.Id] = false;
            foreach (bool entry in hierarchyDirtyMap.Values)
            {
                if (true == entry)
                {
                    return;
                }
            }
            this.Text = defaultText;
        }

        private void OnTreeViewBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
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

        private static bool NodeReadOnly(ConfigurationNode node)
        {
            object[] attributes = node.GetType().GetProperty("Name").GetCustomAttributes(typeof(ReadOnlyAttribute), true);
            return (attributes.Length > 0);
        }

        private void OnTreeViewAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
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
                        ShowMessage(Resources.NodeLabelEditText, Resources.NodeLabelEditCaption);
                        selectedNode.BeginEdit();
                    }
                    else
                    {
                        try
                        {
                            selectedNode.ConfigurationNode.Name = label;
                            SetUIDirty(selectedNode.ConfigurationNode.Hierarchy);
                            selectedNode.EndEdit(false);
                            UpdateCurrentSelection();
                        }
                        catch (InvalidOperationException ex)
                        {
                            e.CancelEdit = true;
                            ShowMessage(Resources.NodeLabelEditFailedText + ex.Message, Resources.NodeLabelEditCaption);
                            selectedNode.BeginEdit();
                        }
                    }
                }
            }
        }

        private void OnToolbarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == newButton)
            {
                CreateNewApplication();
            }
            else if (e.Button == openButton)
            {
                OpenApplication();
            }
            else if (e.Button == saveAppButton)
            {
                SaveApplication(GetSelectedHierarchy());
            }
            else if (e.Button == this.saveAllButton)
            {
                SaveAllApplications();
            }
        }

        protected override void OnMenuComplete(EventArgs e)
        {
            base.OnMenuComplete(e);
            this.statusBarPanel.Text = Resources.DefaultStatusBarText;
        }

        private void OnTreeViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ConfigurationTreeNode selectedNode = (ConfigurationTreeNode)treeView.GetNodeAt(e.X, e.Y);
                SetSelectedNode(selectedNode);
            }
        }

        private void SetSelectedNode(ConfigurationTreeNode selectedNode)
        {
            treeView.SelectedNode = null;
            treeView.SelectedNode = selectedNode;
        }

        private void CreateContextMenu()
        {
            MenuItem cloneActionMenuItem = actionMenuItem.CloneMenu();
            MenuItem cloneNewMenuItem = newMenuItem.CloneMenu();
            MenuItem cloneHelpMenuItem = helpMenuItem.CloneMenu();
            MenuItem cloneRenameMenuItem = renameMenuItem.CloneMenu();
            LoadMenuItemsFromNode((ConfigurationTreeNode)treeView.SelectedNode, new MenuItem[] {cloneNewMenuItem, cloneActionMenuItem, cloneRenameMenuItem, cloneHelpMenuItem});
            ContextMenu menu = new ContextMenu();
            foreach (MenuItem menuItem in cloneActionMenuItem.MenuItems)
            {
                menu.MenuItems.Add(menuItem.CloneMenu());
            }
            if (menu.MenuItems.Count > 0)
            {
                menu.MenuItems.Add("-");
            }
            //menu.Popup += new EventHandler(OnContextMenuItemPopup);
            menu.MenuItems.Add(newAppMenuItem.CloneMenu());
            menu.MenuItems.Add(saveAppMenuItem.CloneMenu());
            UpdateNewMenuItem(cloneNewMenuItem);
            treeView.ContextMenu = menu;
            FindAndUpdateRenameMenuItem(menu);
        }

        private void FindAndUpdateRenameMenuItem(ContextMenu menu)
        {
            foreach (MenuItem item in menu.MenuItems)
            {
                if (string.Compare(item.Text, renameMenuItem.Text, true, CultureInfo.CurrentUICulture) == 0)
                {
                    UpdateRenameMenuItem(item);
                }
            }
        }

        private void UpdateRenameMenuItem(MenuItem item)
        {
            if (treeView.SelectedNode != null)
            {
                ConfigurationTreeNode treeNode = (ConfigurationTreeNode)treeView.SelectedNode;
                item.Enabled = !NodeReadOnly(treeNode.ConfigurationNode);
            }
            else
            {
                item.Enabled = false;
            }
        }

        private void UpdateCurrentSelection()
        {
            ConfigurationTreeNode selectedNode = treeView.SelectedNode as ConfigurationTreeNode;
            UpdateSelection(selectedNode);
        }

        private void UpdateSelection(ConfigurationTreeNode treeNode)
        {
            using (new WaitCursor())
            {
                object[] objects = new object[] {treeNode.ConfigurationNode};
                propertyGrid.SelectedObjects = objects;
            }
        }

        private void OnTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            ConfigurationTreeNode node = e.Node as ConfigurationTreeNode;
            Debug.WriteLine("Current selected node is " + treeView.SelectedNode.Text);
            Debug.WriteLine("Node to be selected from event " + e.Node.Text);
            if (node == null)
            {
                return;
            }
            UpdateSelection(node);
            if (!(node.ConfigurationNode.Id == solutionTreeNode.ConfigurationNode.Id))
            {
                SetSelectedHierarchy(node);
            }
            LoadMenuItemsFromNode(node, new MenuItem[] {newMenuItem, actionMenuItem, renameMenuItem, helpMenuItem});
            CreateContextMenu();
        }

        private void LoadMenuItemsFromNode(ConfigurationTreeNode node, MenuItem[] menuItems)
        {			
            AddMenus(node.ConfigurationNode, menuItems);
        }

        private void SetSelectedHierarchy(ConfigurationTreeNode node)
        {
			IConfigurationUIHierarchyService hierarchyService = (IConfigurationUIHierarchyService)designHost.GetService(typeof(IConfigurationUIHierarchyService));
            hierarchyService.SelectedHierarchy = node.ConfigurationNode.Hierarchy;
        }

        private void OnExitMenItemClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (errorsListView.SelectedItems.Count == 1)
            {
                ConfigurationNodeListViewItem item = (ConfigurationNodeListViewItem)errorsListView.SelectedItems[0];

                if (item.ConfigurationNode != null)
                {
                    treeView.SelectedNode = treeNodeFactory.GetTreeNode(item.ConfigurationNode.Id);
                }
            }
        }

        private void AddMenus(ConfigurationNode node, MenuItem[] menuItems)
        {
			Debug.Assert(node != null);
            ClearMenuItems(menuItems);
			IUICommandService service = (IUICommandService)designHost.GetService(typeof(IUICommandService));
			List<ConfigurationMenuItem> configMenuItems = new List<ConfigurationMenuItem>();
			service.ForEach(node.GetType(), delegate(ConfigurationUICommand command)
			{
				configMenuItems.Add(new ConfigurationMenuItem(node, command));
			});

			configMenuItems.Sort(delegate(ConfigurationMenuItem lhs, ConfigurationMenuItem rhs)
			{
				return lhs.Text.CompareTo(rhs.Text);
			});

			foreach (ConfigurationMenuItem menuItem in configMenuItems)
            {
                menuItem.Select += new EventHandler(OnMenuItemSelect);
                switch (menuItem.InsertionPoint)
                {
                    case InsertionPoint.Action:
                        menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItem);
                        break;
                    case InsertionPoint.Help:
                        menuItems[(int)MenuItemPosition.Help].MenuItems.Add(menuItem);
                        break;
                    case InsertionPoint.New:
                        menuItems[(int)MenuItemPosition.New].MenuItems.Add(menuItem);
                        break;
                }
            }
            AddDefaultMenuItems(menuItems);            
        }

        private void OnMenuItemSelect(object sender, EventArgs e)
        {
			propertyGrid.Refresh();

			ConfigurationMenuItem item = sender as ConfigurationMenuItem;
            if (item != null)
            {
                this.statusBarPanel.Text = item.LongText;
            }
            else
            {
                this.statusBarPanel.Text = Resources.DefaultStatusBarText;
            }
        }

        private void OnRenameMenuItemClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }

        private void OnNewMenuItemPopup(object sender, EventArgs e)
        {
            UpdateNewMenuItem((MenuItem)sender);
        }

        private void OnActionMenuItemPopup(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            foreach (MenuItem menuItem in item.MenuItems)
            {
                if (menuItem.Text.Equals(newMenuItem.Text))
                {
                    UpdateNewMenuItem(menuItem);
                }
                if (menuItem.Text.Equals(renameMenuItem.Text))
                {
                    UpdateRenameMenuItem(menuItem);
                }
            }
        }

        private void UpdateNewMenuItem(MenuItem newMenuItem)
        {
            if (newMenuItem.MenuItems.Count == 0)
            {
                newMenuItem.Enabled = false;
            }
            else
            {
                newMenuItem.Enabled = true;
            }
        }

        private void OnSaveAllMenuItemClick(object sender, EventArgs e)
        {
            SaveAllApplications();
        }

        private void EnableSaveOptions(bool enable)
        {
            saveAllButton.Enabled = enable;
            saveAllMenuItem.Enabled = enable;
            saveAppMenuItem.Enabled = enable;
            saveAppButton.Enabled = enable;
            saveAppAsMenuItem.Enabled = enable;
        }

        private void OnNewAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = Resources.NewApplicationStatusBarText;
        }


        private void OnNewAppMenuItemClick(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                CreateNewApplication();
            }
        }

        private void CreateNewApplication()
        {
			using (AddConfigurationApplicationNodeCommand cmd = new AddConfigurationApplicationNodeCommand(designHost))
            {
                cmd.Execute(solutionNode);
            }
        }

        private void OnOpenAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = Resources.OpenApplicationStatusBarText;
        }

        private void OnSaveAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = Resources.SaveApplicationStatusBarText;
        }

        private void OnSaveAppAsMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = Resources.SaveAsApplicationStatusBarText;
        }

        private void OnSaveAllMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = Resources.SaveAllStatusBarText;
        }

        private void AddDefaultMenuItems(MenuItem[] menuItems)
        {
            if (menuItems[(int)MenuItemPosition.Action].MenuItems.Count == 0)
            {
                menuItems[(int)MenuItemPosition.Action].Enabled = false;
            }
            else
            {
                menuItems[(int)MenuItemPosition.Action].Enabled = true;
            }
            if (menuItems[(int)MenuItemPosition.Help].MenuItems.Count > 0)
            {
                menuItems[(int)MenuItemPosition.Help].MenuItems.Add(new MenuItem("-"));
            }
            helpMenuItem.MenuItems.Add(aboutMenuItem);
            UpdateNewMenuItem(menuItems[(int)MenuItemPosition.New]);
        }

        private void ClearMenuItems(MenuItem[] menuItems)
        {
            menuItems[(int)MenuItemPosition.Action].MenuItems.Clear();
            menuItems[(int)MenuItemPosition.New].MenuItems.Clear();
            menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItems[(int)MenuItemPosition.New]);
            menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItems[(int)MenuItemPosition.Rename]);
            helpMenuItem.MenuItems.Clear();
        }

        private void OnSaveApplicationClick(object sender, EventArgs e)
        {
			propertyGrid.Refresh();

            using (new WaitCursor())
            {
                SaveApplication(GetSelectedHierarchy());
            }
        }


        private void OnSaveApplicationAsClick(object sender, EventArgs e)
        {
            propertyGrid.Refresh();

            using(SaveFileDialog saveAsFileDialog = new SaveFileDialog())
            {

                saveAsFileDialog.Filter =  Resources.ConfigurationFileFilter;
                saveAsFileDialog.Title = Resources.SaveAsDialogTitle;

                if (DialogResult.OK == ShowSaveDialog(saveAsFileDialog))
                {

                    using (new WaitCursor())
                    {
                        IConfigurationUIHierarchy currentHierarchy = ServiceHelper.GetCurrentHierarchy(designHost);

                        SaveAsConfigurationApplicationNodeCommand cmd = new SaveAsConfigurationApplicationNodeCommand(designHost, saveAsFileDialog.FileName);
                        cmd.Execute(currentHierarchy.RootNode);
                    }
                }
            }
        }
        

        private void OnOpenAppMenuItemClick(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                OpenApplication();
            }
        }

        private void OnPropertyChanged(object s, PropertyValueChangedEventArgs e)
        {
			IConfigurationUIHierarchyService service = designHost.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;
            if (service.SelectedHierarchy == null)
            {
                return;
            }
            SetUIDirty(service.SelectedHierarchy);
        }

        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            AboutForm frm = new AboutForm();
            frm.ShowDialog(this);
        }

        private void OpenApplication()
        {
            using (new WaitCursor())
            {
				using (OpenConfigurationApplicationNodeCommand cmd = new OpenConfigurationApplicationNodeCommand(designHost))
                {                    
                    cmd.Execute(solutionNode);
                }
            }
        }

		private void OpenFiles(string [] files)
		{
			using (new WaitCursor())
			{
				foreach (string file in files)
				{
					using (OpenFileConfigurationApplicationNodeCommand cmd = new OpenFileConfigurationApplicationNodeCommand(designHost, file))
					{						
						cmd.Execute(solutionNode);
					}
				}				
			}
		}

        private bool SaveApplication(IConfigurationUIHierarchy hierarchy)
        {
            using (new WaitCursor())
            {
				SaveConfigurationApplicationNodeCommand cmd = new SaveConfigurationApplicationNodeCommand(designHost);
                cmd.Execute(hierarchy.RootNode);
                return cmd.SaveSucceeded;
            }
        }

        private IConfigurationUIHierarchy GetSelectedHierarchy()
        {
			IConfigurationUIHierarchyService uiHierarchyService = designHost.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;
            Debug.Assert(uiHierarchyService != null, "Could not find IUIHierarchyService");
			IConfigurationUIHierarchy hierarchy = uiHierarchyService.SelectedHierarchy;
            Debug.Assert(hierarchy != null, "Could not find IUIHierarchy");
            return hierarchy;
        }

        public void RefreshPropertyGrid()
        {
            propertyGrid.SelectedObjects = propertyGrid.SelectedObjects;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolbarImageList = new System.Windows.Forms.ImageList(this.components);
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.newButton = new System.Windows.Forms.ToolBarButton();
            this.openButton = new System.Windows.Forms.ToolBarButton();
            this.saveAppButton = new System.Windows.Forms.ToolBarButton();
            this.saveAllButton = new System.Windows.Forms.ToolBarButton();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.newAppMenuItem = new System.Windows.Forms.MenuItem();
            this.openAppmenuItem = new System.Windows.Forms.MenuItem();
            this.saveAppMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAppAsMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAllMenuItem = new System.Windows.Forms.MenuItem();
            this.fileSplitMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.actionMenuItem = new System.Windows.Forms.MenuItem();
            this.newMenuItem = new System.Windows.Forms.MenuItem();
            this.renameMenuItem = new System.Windows.Forms.MenuItem();
            this.helpMenuItem = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.validationErrorsPanel = new System.Windows.Forms.Panel();
            this.errorsListView = new System.Windows.Forms.ListView();
            this.nameHeader = new System.Windows.Forms.ColumnHeader();
            this.propertyHeader = new System.Windows.Forms.ColumnHeader();
            this.descriptionHeader = new System.Windows.Forms.ColumnHeader();
            this.pathHeader = new System.Windows.Forms.ColumnHeader();
            this.validationTitlePanel = new System.Windows.Forms.Panel();
            this.validationTitleLabel = new System.Windows.Forms.Label();
            this.bottomSplitter = new System.Windows.Forms.Splitter();
            this.treePanel = new System.Windows.Forms.Panel();
            this.treeView = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI.CustomTreeView();
            this.middleSplitter = new System.Windows.Forms.Splitter();
            this.objectPanel = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.validationErrorsPanel.SuspendLayout();
            this.validationTitlePanel.SuspendLayout();
            this.treePanel.SuspendLayout();
            this.objectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarImageList
            // 
            this.toolbarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImageList.ImageStream")));
            this.toolbarImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImageList.Images.SetKeyName(0, "");
            this.toolbarImageList.Images.SetKeyName(1, "");
            this.toolbarImageList.Images.SetKeyName(2, "");
            this.toolbarImageList.Images.SetKeyName(3, "");
            this.toolbarImageList.Images.SetKeyName(4, "");
            this.toolbarImageList.Images.SetKeyName(5, "");
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            resources.ApplyResources(this.treeViewImageList, "treeViewImageList");
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolBar
            // 
            resources.ApplyResources(this.toolBar, "toolBar");
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.newButton,
            this.openButton,
            this.saveAppButton,
            this.saveAllButton});
            this.toolBar.ImageList = this.toolbarImageList;
            this.toolBar.Name = "toolBar";
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.OnToolbarButtonClick);
            // 
            // newButton
            // 
            resources.ApplyResources(this.newButton, "newButton");
            this.newButton.Name = "newButton";
            // 
            // openButton
            // 
            resources.ApplyResources(this.openButton, "openButton");
            this.openButton.Name = "openButton";
            // 
            // saveAppButton
            // 
            resources.ApplyResources(this.saveAppButton, "saveAppButton");
            this.saveAppButton.Name = "saveAppButton";
            // 
            // saveAllButton
            // 
            resources.ApplyResources(this.saveAllButton, "saveAllButton");
            this.saveAllButton.Name = "saveAllButton";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.actionMenuItem,
            this.helpMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newAppMenuItem,
            this.openAppmenuItem,
            this.saveAppMenuItem,
            this.saveAppAsMenuItem,
            this.saveAllMenuItem,
            this.fileSplitMenuItem,
            this.exitMenuItem});
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            this.fileMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // newAppMenuItem
            // 
            this.newAppMenuItem.Index = 0;
            resources.ApplyResources(this.newAppMenuItem, "newAppMenuItem");
            this.newAppMenuItem.Select += new System.EventHandler(this.OnNewAppMenuItemSelect);
            this.newAppMenuItem.Click += new System.EventHandler(this.OnNewAppMenuItemClick);
            // 
            // openAppmenuItem
            // 
            this.openAppmenuItem.Index = 1;
            resources.ApplyResources(this.openAppmenuItem, "openAppmenuItem");
            this.openAppmenuItem.Select += new System.EventHandler(this.OnOpenAppMenuItemSelect);
            this.openAppmenuItem.Click += new System.EventHandler(this.OnOpenAppMenuItemClick);
            // 
            // saveAppMenuItem
            // 
            resources.ApplyResources(this.saveAppMenuItem, "saveAppMenuItem");
            this.saveAppMenuItem.Index = 2;
            this.saveAppMenuItem.Select += new System.EventHandler(this.OnSaveAppMenuItemSelect);
            this.saveAppMenuItem.Click += new System.EventHandler(this.OnSaveApplicationClick);
            // 
            // saveAppAsMenuItem
            // 
            resources.ApplyResources(this.saveAppAsMenuItem, "saveAppAsMenuItem");
            this.saveAppAsMenuItem.Index = 3;
            this.saveAppAsMenuItem.Select += new System.EventHandler(this.OnSaveAppAsMenuItemSelect);
            this.saveAppAsMenuItem.Click += new System.EventHandler(this.OnSaveApplicationAsClick);
            // 
            // saveAllMenuItem
            // 
            resources.ApplyResources(this.saveAllMenuItem, "saveAllMenuItem");
            this.saveAllMenuItem.Index = 4;
            this.saveAllMenuItem.Select += new System.EventHandler(this.OnSaveAllMenuItemSelect);
            this.saveAllMenuItem.Click += new System.EventHandler(this.OnSaveAllMenuItemClick);
            // 
            // fileSplitMenuItem
            // 
            this.fileSplitMenuItem.Index = 5;
            resources.ApplyResources(this.fileSplitMenuItem, "fileSplitMenuItem");
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 6;
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            this.exitMenuItem.Click += new System.EventHandler(this.OnExitMenItemClick);
            // 
            // actionMenuItem
            // 
            this.actionMenuItem.Index = 1;
            this.actionMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newMenuItem,
            this.renameMenuItem});
            resources.ApplyResources(this.actionMenuItem, "actionMenuItem");
            this.actionMenuItem.Popup += new System.EventHandler(this.OnActionMenuItemPopup);
            // 
            // newMenuItem
            // 
            resources.ApplyResources(this.newMenuItem, "newMenuItem");
            this.newMenuItem.Index = 0;
            this.newMenuItem.Popup += new System.EventHandler(this.OnNewMenuItemPopup);
            // 
            // renameMenuItem
            // 
            resources.ApplyResources(this.renameMenuItem, "renameMenuItem");
            this.renameMenuItem.Index = 1;
            this.renameMenuItem.Click += new System.EventHandler(this.OnRenameMenuItemClick);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Index = 2;
            this.helpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutMenuItem});
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            this.helpMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 0;
            resources.ApplyResources(this.aboutMenuItem, "aboutMenuItem");
            this.aboutMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            this.aboutMenuItem.Click += new System.EventHandler(this.OnAboutMenuItemClick);
            // 
            // statusBar
            // 
            resources.ApplyResources(this.statusBar, "statusBar");
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel});
            this.statusBar.ShowPanels = true;
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            resources.ApplyResources(this.statusBarPanel, "statusBarPanel");
            // 
            // bottomPanel
            // 
            resources.ApplyResources(this.bottomPanel, "bottomPanel");
            this.bottomPanel.Controls.Add(this.validationErrorsPanel);
            this.bottomPanel.Controls.Add(this.validationTitlePanel);
            this.bottomPanel.Name = "bottomPanel";
            // 
            // validationErrorsPanel
            // 
            this.validationErrorsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.validationErrorsPanel.Controls.Add(this.errorsListView);
            resources.ApplyResources(this.validationErrorsPanel, "validationErrorsPanel");
            this.validationErrorsPanel.Name = "validationErrorsPanel";
            // 
            // errorsListView
            // 
            this.errorsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.propertyHeader,
            this.descriptionHeader,
            this.pathHeader});
            resources.ApplyResources(this.errorsListView, "errorsListView");
            this.errorsListView.FullRowSelect = true;
            this.errorsListView.GridLines = true;
            this.errorsListView.HideSelection = false;
            this.errorsListView.Name = "errorsListView";
            this.errorsListView.UseCompatibleStateImageBehavior = false;
            this.errorsListView.View = System.Windows.Forms.View.Details;
            this.errorsListView.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // nameHeader
            // 
            resources.ApplyResources(this.nameHeader, "nameHeader");
            // 
            // propertyHeader
            // 
            resources.ApplyResources(this.propertyHeader, "propertyHeader");
            // 
            // descriptionHeader
            // 
            resources.ApplyResources(this.descriptionHeader, "descriptionHeader");
            // 
            // pathHeader
            // 
            resources.ApplyResources(this.pathHeader, "pathHeader");
            // 
            // validationTitlePanel
            // 
            this.validationTitlePanel.Controls.Add(this.validationTitleLabel);
            resources.ApplyResources(this.validationTitlePanel, "validationTitlePanel");
            this.validationTitlePanel.Name = "validationTitlePanel";
            // 
            // validationTitleLabel
            // 
            resources.ApplyResources(this.validationTitleLabel, "validationTitleLabel");
            this.validationTitleLabel.Name = "validationTitleLabel";
            // 
            // bottomSplitter
            // 
            resources.ApplyResources(this.bottomSplitter, "bottomSplitter");
            this.bottomSplitter.Name = "bottomSplitter";
            this.bottomSplitter.TabStop = false;
            // 
            // treePanel
            // 
            this.treePanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.treePanel.Controls.Add(this.treeView);
            resources.ApplyResources(this.treePanel, "treePanel");
            this.treePanel.Name = "treePanel";
            // 
            // treeView
            // 
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.HideSelection = false;
            this.treeView.ImageList = this.treeViewImageList;
            this.treeView.ItemHeight = 16;
            this.treeView.LabelEdit = true;
            this.treeView.Name = "treeView";
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnTreeViewAfterLabelEdit);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeViewAfterSelect);
            this.treeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnTreeViewBeforeLabelEdit);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTreeViewMouseDown);
            // 
            // middleSplitter
            // 
            resources.ApplyResources(this.middleSplitter, "middleSplitter");
            this.middleSplitter.Name = "middleSplitter";
            this.middleSplitter.TabStop = false;
            // 
            // objectPanel
            // 
            this.objectPanel.Controls.Add(this.propertyGrid);
            resources.ApplyResources(this.objectPanel, "objectPanel");
            this.objectPanel.Name = "objectPanel";
            // 
            // propertyGrid
            // 
            resources.ApplyResources(this.propertyGrid, "propertyGrid");
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.objectPanel);
            this.Controls.Add(this.middleSplitter);
            this.Controls.Add(this.treePanel);
            this.Controls.Add(this.bottomSplitter);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.validationErrorsPanel.ResumeLayout(false);
            this.validationTitlePanel.ResumeLayout(false);
            this.treePanel.ResumeLayout(false);
            this.objectPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}