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
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// The UI for the reference type selector.
    /// </devdoc>    
    internal class ReferenceEditorUI : UserControl
    {
        private Container components = null;
        private ConfigurationNode rootNode;
        private Type referenceType;
        private ConfigurationNode currentNode;
        private IWindowsFormsEditorService service;
        private ListBox referencesListBox;
        private string noneText;
		private bool isRequired;

        public ReferenceEditorUI()
        {
            InitializeComponent();
			noneText = Resources.None;
            referencesListBox.Click += new EventHandler(OnListBoxClick);
        }

		public ReferenceEditorUI(ConfigurationNode rootNode, Type referenceType, ConfigurationNode currentReference, IWindowsFormsEditorService service, bool isRequired)
			: this()
        {
            this.rootNode = rootNode;
            this.referenceType = referenceType;
            this.currentNode = currentReference;
            this.service = service;
			this.isRequired = isRequired;
            DisplayList();
        }

        public ConfigurationNode SelectedNode
        {
            get
            {
				ConfigurationNode selectedNode = referencesListBox.SelectedItem as ConfigurationNode;
                if (selectedNode == null)
                {
                    return null;
                }
                return selectedNode;
            }
        }
        
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

        private void OnListBoxClick(object sender, EventArgs args)
        {
            service.CloseDropDown();
        }

        private void DisplayList()
        {			
            referencesListBox.Items.Clear();
			if (!isRequired)
			{
				referencesListBox.Items.Add(noneText);
			}		
			List<ConfigurationNode> nodes = new List<ConfigurationNode>(rootNode.Hierarchy.FindNodesByType(rootNode, referenceType));
            if (nodes != null)
            {
				nodes.Sort();                
                foreach (ConfigurationNode node in nodes)
                {
                    referencesListBox.Items.Add(node);
                    if ((currentNode != null) && (currentNode == node))
                    {
                        referencesListBox.SelectedItem = node;
                    }
                }
            }

        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferenceEditorUI));
            this.referencesListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // referencesListBox
            // 
            resources.ApplyResources(this.referencesListBox, "referencesListBox");
            this.referencesListBox.Name = "referencesListBox";
            // 
            // ReferenceEditorUI
            // 
            this.Controls.Add(this.referencesListBox);
            this.Name = "ReferenceEditorUI";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }
        #endregion
    }
}