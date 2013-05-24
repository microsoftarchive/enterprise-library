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
    /// Represent a filter that match AddApplicationBlockCommand instances by type and/or section name.
    /// </summary>
    public class AddBlockCommandMatchFilter : CommandMatchFilter
    {
        /// <summary>
        /// Gets or sets the section name to match.
        /// </summary>
        [XmlAttribute("sectionName")]
        public string SectionName 
        {
            get; 
            set;
        }

        ///<summary>
        /// Returns true if the type match the current type filter and section name.
        ///</summary>
        ///<param name="command">Command to match</param>
        ///<returns>True is match</returns>
        public override bool Match(CommandModel command)
        {
            var blockCommand = command as AddApplicationBlockCommand;
            
            if (blockCommand == null)
            {
                return base.Match(command);
            }

            // If only a type filter is specified
            if (!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SectionName))
            {
                return base.Match(command);
            }

            // If only a section filter is specified
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(SectionName))
            {
                return SectionName.Equals(blockCommand.SectionName);
            }

            // If both a section and type filters are specified
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(SectionName))
            {
                return base.Match(blockCommand) && SectionName.Equals(blockCommand.SectionName); 
            }

            return false;
        }
    }
}
