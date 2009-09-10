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
using System.Configuration;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    ///<summary>
    /// Indicates that this assembly handles the <see cref="ConfigurationSection"/>.
    ///</summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class HandlesSectionNameAttribute : Attribute
    {
        private string name;

        ///<summary>
        /// Initializes the <see cref="HandlesSectionNameAttribute"/> class.
        ///</summary>
        public HandlesSectionNameAttribute()
        {

        }

        /// <summary>
        /// Initializes the <see cref="HandlesSectionNameAttribute"/> class.
        /// </summary>
        /// <param name="sectionName">Indicates that this assembly contains <see cref="ConfigurationSection"/> sections for the named section.</param>
        public HandlesSectionNameAttribute(string sectionName)
        {
            name = sectionName;
        }

        ///<summary>
        /// Name of the section handled by this assembly.
        ///</summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
