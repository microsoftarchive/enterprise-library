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

using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="ExceptionPolicy"/>.
    /// </summary> 
    [ResourceDescription(typeof(DesignResources), "ExceptionPolicyDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionPolicyDataDisplayName")]
    [ViewModel(ExceptionHandlingDesignTime.ViewModelTypeNames.ExceptionPolicyDataViewModel)]
    public class ExceptionPolicyData : NamedConfigurationElement
    {
        private const string exceptionTypesProperty = "exceptionTypes";

        /// <summary>
        /// Creates a new instance of ExceptionPolicyData.
        /// </summary>
        public ExceptionPolicyData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ExceptionPolicyData"/>.
        /// </param>
        public ExceptionPolicyData(string name)
            : base(name)
        {
            this[exceptionTypesProperty] = new NamedElementCollection<ExceptionTypeData>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ExceptionTypeData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ExceptionTypeData"/> objects.
        /// </value>
        [ConfigurationProperty(exceptionTypesProperty)]
        [ResourceDescription(typeof(DesignResources), "ExceptionPolicyDataExceptionTypesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionPolicyDataExceptionTypesDisplayName")]
        [ConfigurationCollection(typeof(ExceptionTypeData))]
        [PromoteCommands]
        public NamedElementCollection<ExceptionTypeData> ExceptionTypes
        {
            get
            {
                return (NamedElementCollection<ExceptionTypeData>)this[exceptionTypesProperty];
            }
        }

        /// <summary>
        /// Builds an <see cref="ExceptionPolicyDefinition"/> based on the configuration.
        /// </summary>
        /// <returns>The policy instance.</returns>
        public ExceptionPolicyDefinition BuildExceptionPolicy()
        {
            var exceptionTypes = this.ExceptionTypes.Select(et => et.BuildExceptionType());

            return new ExceptionPolicyDefinition(this.Name, exceptionTypes);
        }
    }
}
