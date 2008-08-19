//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    /// <summary>
    /// Design time representation of a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerMappingData"/>.
    /// </summary>
    public class FaultContractPropertyMapping
    {
        string name;

        string source;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [SRDescription("FaultContractMappingNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [Required]
        [SRDescription("FaultContractMappingSourceDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
    }
}