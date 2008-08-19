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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <summary>
    /// Represents a menu item for configuration.
    /// </summary>
    [DesignerCategory("Code")]
    public class ConfigurationMenuItem : MenuItem
    {
        ConfigurationUICommand command;
        ConfigurationNode node;

        ConfigurationMenuItem() {}

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationMenuItem"/> class.
        /// </summary>
        /// <param name="node">The node for the menu.</param>
        /// <param name="command">Te command for the menu.</param>
        public ConfigurationMenuItem(ConfigurationNode node,
                                     ConfigurationUICommand command)
        {
            this.command = command;
            Shortcut = command.Shortcut;
            Text = command.Text;
            this.node = node;
            Enabled = (command.GetCommandState(node) == CommandState.Enabled);
        }

        /// <summary>
        /// Gets the insertion point for the menu.
        /// </summary>
        /// <value>
        /// The insertion point for the menu.
        /// </value>
        public InsertionPoint InsertionPoint
        {
            get { return command.InsertionPoint; }
        }

        /// <summary>
        /// Gets the long text for the menu.
        /// </summary>
        /// <value>
        /// The long text for the menu.
        /// </value>
        public string LongText
        {
            get { return command.LongText; }
        }

        /// <summary>
        /// Creates a copy of the current <see cref="T:System.Windows.Forms.MenuItem"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.MenuItem"></see> that represents the duplicated menu item.
        /// </returns>
        public override MenuItem CloneMenu()
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem();
            item.CloneMenu(this);
            item.command = command;
            item.node = node;
            return item;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.MenuItem"></see>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (command != null)
                {
                    command.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.MenuItem.Click"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data. </param>
        protected override void OnClick(EventArgs e)
        {
            using (new WaitCursor())
            {
                base.OnClick(e);
                command.Execute(node);
            }
        }
    }
}