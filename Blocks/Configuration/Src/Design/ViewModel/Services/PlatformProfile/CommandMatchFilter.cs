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
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile
{
    /// <summary>
    /// Represent a filter that match CommandModel instances by type.
    /// </summary>
    public class CommandMatchFilter : TypeMatchFilter
    {
        ///<summary>
        /// Returns true if the command instance type match the current type filter.
        ///</summary>
        ///<param name="command">Command to match</param>
        ///<returns>True is match</returns>
        public virtual bool Match(CommandModel command)
        {
            return Match(command.GetType());
        }
    }
}
