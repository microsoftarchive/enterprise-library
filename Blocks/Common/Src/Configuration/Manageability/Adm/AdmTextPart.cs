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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    /// <summary>
    /// Represents a TEXT part in an ADM template.
    /// </summary>
    public class AdmTextPart : AdmPart
    {
        internal const String TextTemplate = "\t\t\tTEXT";

        /// <summary>
        /// Initialize a new instance of the <see cref="AdmTextPart"/> class.
        /// </summary>
        /// <param name="partName">
        /// The name of the part.
        /// </param>
        public AdmTextPart(String partName)
            : base(partName, null, null) {}

        /// <summary>
        /// Gest the template representing the type of the part.
        /// </summary>
        protected override string PartTypeTemplate
        {
            get { return TextTemplate; }
        }
    }
}
