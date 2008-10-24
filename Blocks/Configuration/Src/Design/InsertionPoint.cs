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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Points on the main menu where <see cref="ConfigurationUICommand"/> objets can be sited.
    /// </summary>
    public enum InsertionPoint
    {
        /// <summary>
        /// The item will become a menu item of the main task menu.
        /// </summary>
        Action = 0,
        /// <summary>
        /// The item will become a menu item of the main help menu.
        /// </summary>
        Help = 1,
        /// <summary>
        /// Will create a new menu item in the action menu item and add to it's submenu.
        /// </summary>
        New = 2
    }
}
