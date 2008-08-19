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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides data for a <see cref="ConfigurationNodeCommand.Executing"/> event.
    /// </summary>
    [Serializable]
    public class CommandExecutingEventArgs : EventArgs
    {
        private bool cancel;

        /// <summary>
        /// Initializes a new instance of  the <see cref="CommandExecutingEventArgs"/> class.
        /// </summary>
        public CommandExecutingEventArgs()
        {
        }        

        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the event should be canceled; otherwise, <see langword="false"/>.
        /// </value>
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }
}