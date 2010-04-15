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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Commands
{
    /// <summary>
    /// A command that is not <see cref="CommandModel.Browsable"/> in the deisnger and can be used
    /// to replace a visible command.
    /// </summary>
    /// <remarks>
    /// This command can be used to hide an existing command by replacing it with a <see cref="CommandAttribute"/>
    /// and setting the <see cref="CommandAttribute.Replace"/> value.
    /// </remarks>
    public class HiddenCommand : CommandModel
    {
        ///<summary>
        /// Initializes a new instance of <see cref="HiddenCommand"/>.
        ///</summary>
        ///<param name="attribute">The command attribute providing context for this command.</param>
        ///<param name="uiService">The user-interface service used to display messages and windows to the user.</param>
        public HiddenCommand(CommandAttribute attribute, IUIServiceWpf uiService) :
            base(attribute, uiService)
        {
            this.Browsable = false;
        }
    }
}
