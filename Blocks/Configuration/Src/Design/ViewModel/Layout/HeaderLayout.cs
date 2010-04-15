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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{    
    /// <summary>
    /// Layout class that can be used to visualize a textual header and contextual commnands.
    /// </summary>
    public class HeaderLayout : ViewModel
    {
        string title;
        IEnumerable<CommandModel> commands;

        /// <summary>
        /// Initializes a new instance of <see cref="HeaderLayout"/>.
        /// </summary>
        /// <param name="title">The title that should be shown in the header.</param>
        /// <param name="commands">The commands that can be invoked from a contextual menu.</param>
        public HeaderLayout(string title, IEnumerable<CommandModel> commands)
            :this(title)
        {
            this.commands = commands;
        }
        /// <summary>
        /// Initializes a new instance of <see cref="HeaderLayout"/>.
        /// </summary>
        /// <param name="title">The title that should be shown in the header.</param>
        public HeaderLayout(string title)
        {
            this.title = title;
        }

        /// <summary>
        /// Gets the title that should be shown in the header.
        /// </summary>
        /// <value>
        /// The title that should be shown in the header.
        /// </value>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// Gets the commands that should be shown in the contextual menu.
        /// </summary>
        /// <value>
        /// The commands that should be shown in the contextual menu.
        /// </value>
        public IEnumerable<CommandModel> Commands
        {
            get { return commands; }
        }

    }
}
