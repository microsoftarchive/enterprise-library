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
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile
{
    /// <summary>
    /// Represent a discovery profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Create a default instance of a discovery profile.
        /// </summary>
        public Profile()
        {
            // TODO: Use XmlDefaultValue instead
            EnvironmentCommandsEnabled = true;
        }

        /// <summary>
        /// Environment command enabled.
        /// </summary>
        [XmlAttribute("environmentCommandsEnabled")]
        public bool EnvironmentCommandsEnabled { get; set; }

        /// <summary>
        /// Targeted platform.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Application title format.
        /// </summary>
        public string ApplicationTitleFormat { get; set; }

        MatchFilter[] matchFilters = new MatchFilter[0];

        /// <summary>
        /// A collection of type filters.
        /// </summary>
        [XmlArray("TypeFilters")]
        [XmlArrayItem("Type", typeof(TypeMatchFilter))]
        [XmlArrayItem("Assembly", typeof(AssemblyMatchFilter))]
        [XmlArrayItem("Command", typeof(CommandMatchFilter))]
        [XmlArrayItem("AddBlockCommand", typeof(AddBlockCommandMatchFilter))]
        public MatchFilter[] MatchFilters 
        {
            get { return matchFilters; }
            set { matchFilters = value ?? new MatchFilter[0]; }
        }

        private AssemblyMatchFilter[] assemblyMatchFilters;

        /// <summary>
        /// Return all the AssemblyMatchFilter in the MatchFilters array.
        /// </summary>
        public IEnumerable<AssemblyMatchFilter> AssemblyMatchFilters
        {
            get { return assemblyMatchFilters ?? (assemblyMatchFilters = MatchFilters.OfType<AssemblyMatchFilter>().ToArray()); }
        }

        private TypeMatchFilter[] typeMatchFilters;

        /// <summary>
        /// Return all the TypeMatchFilter in the MatchFilters array.
        /// </summary>
        public IEnumerable<TypeMatchFilter> TypeMatchFilters
        {
            get { return typeMatchFilters ?? (typeMatchFilters = MatchFilters.OfType<TypeMatchFilter>().ToArray()); }
        }

        private CommandMatchFilter[] commandMatchFilters;

        /// <summary>
        /// Return all the CommandMatchFilter in the MatchFilters array.
        /// </summary>
        public IEnumerable<CommandMatchFilter> CommandMatchFilters
        {
            get { return commandMatchFilters ?? (commandMatchFilters = MatchFilters.OfType<CommandMatchFilter>().ToArray()); }
        }

        /// <summary>
        /// Checks the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type should be included.</returns>
        public bool Check(Type type)
        {
            if (this.MatchFilters == null)
            {
                return true;
            }

            MatchKind? match = null;

            // Check if a type filter can be found for that exact type
            foreach (var filter in this.TypeMatchFilters.Where(filter => filter.Match(type)))
            {
                match = filter.MatchKind;
            }

            // If no type match were found, look for assembly filters
            if (!match.HasValue)
            {
                foreach (var filter in this.AssemblyMatchFilters.Where(filter => filter.Match(type)))
                {
                    match = filter.MatchKind;
                }
            }

            return match.HasValue ? match.Value == MatchKind.Allow : true;
        }
        
        /// <summary>
        /// Checks a AddApplicationBlockCommand.
        /// </summary>
        /// <param name="command">The command instance.</param>
        /// <returns>True if the command should be included.</returns>
        public bool Check(CommandModel command)
        {
            var match = MatchKind.Allow;

            foreach (var filter in this.CommandMatchFilters.Where(filter => filter.Match(command)))
            {
                match = filter.MatchKind;
            }

            return match == MatchKind.Allow;
        }
    }
}
