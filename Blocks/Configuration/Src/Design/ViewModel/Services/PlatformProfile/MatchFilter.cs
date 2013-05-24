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
    /// Represent a type filter.
    /// </summary>
    public abstract class MatchFilter
    {
        ///<summary>
        /// Returns true if the type match the current type filter.
        ///</summary>
        ///<param name="type">Type to match</param>
        ///<returns>True is match</returns>
        public abstract bool Match(Type type);

        /// <summary>
        /// The match kind (allow, deny).
        /// </summary>
        [XmlAttribute("matchKind")]
        public MatchKind MatchKind { get; set; }
    }
}
