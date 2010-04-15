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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Layout class that can be used to visualize a <see cref="ElementViewModel"/> (typically <see cref="ElementCollectionViewModel"/>) as a list of elements with a textual header.
    /// </summary>
    /// <remarks>
    /// This list layout will show the <see cref="ElementViewModel"/>'s name as the title,<br/>
    /// the <see cref="ElementViewModel"/>'s ChildElements as the list's contents and allows to invoke the
    /// <see cref="ElementViewModel"/>'s Commands from a contextual menu.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Headered")]
    public class HeaderedListLayout : TwoVerticalsLayout
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HeaderedListLayout"/>.
        /// </summary>
        /// <param name="containingElement">The <see cref="ElementViewModel"/> instance that should be displayed as a headered list.</param>
        public HeaderedListLayout(ElementViewModel containingElement)
            : base(new HeaderLayout(containingElement.Name, containingElement.Commands), new ElementListLayout(containingElement.ChildElements))
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HeaderedListLayout"/> with the specified contextual commands.
        /// </summary>
        /// <param name="containingElement">The <see cref="ElementViewModel"/> instance that should be displayed as a headered list.</param>
        /// <param name="commands">A specific list of <see cref="CommandModel"/> that should be displayed for the add (+) button in the UI.</param>
        public HeaderedListLayout(ElementViewModel containingElement, IEnumerable<CommandModel> commands)
            : base(new HeaderLayout(containingElement.Name, commands), new ElementListLayout(containingElement.ChildElements))
        {
        }
    }
}
