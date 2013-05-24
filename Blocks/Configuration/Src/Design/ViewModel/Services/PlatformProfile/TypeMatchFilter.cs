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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile
{
    /// <summary>
    /// Represent a filter that match a specific type.
    /// </summary>
    public class TypeMatchFilter : MatchFilter
    {
        private string name;
        private Type type;

        /// <summary>
        /// Type name to be filtered.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.type = string.IsNullOrEmpty(value) ? null : Type.GetType(value, false);
            }
        }

        /// <summary>
        /// Assembly to be filtered
        /// </summary>
        [XmlAttribute("matchSubclasses")]
        public bool MatchSubclasses { get; set; }

        /// <summary>
        /// Returns true if the type match the current type filter.
        /// </summary>
        /// <param name="type">Type to match</param>
        /// <returns>True if match.</returns>
        public override bool Match(Type type)
        {
            if (this.type == null) return false;

            if (this.MatchSubclasses)
            {
                return this.type.IsAssignableFrom(type);
            }
            else
            {
                return this.type == type;
            }
        }
    }
}
