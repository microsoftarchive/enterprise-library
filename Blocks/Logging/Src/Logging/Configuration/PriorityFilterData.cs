//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration for a priority filter.
    /// </summary>    
    [Assembler(typeof(PriorityFilterAssembler))]
    public class PriorityFilterData : LogFilterData
    {
        private const string minimumPriorityProperty = "minimumPriority";
        private const string maximumPriorityProperty = "maximumPriority";

        /// <summary>
        /// Initializes a new <see cref="PriorityFilterData"/>.
        /// </summary>
        public PriorityFilterData()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="PriorityFilterData"/> with a minimum priority.
        /// </summary>
        /// <param name="minimumPriority">The minimum priority.</param>
        public PriorityFilterData(int minimumPriority)
            : this("priority", minimumPriority)
        {
        }

        /// <summary>
        /// Initializes a new named <see cref="PriorityFilterData"/> with a minimum priority.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minimumPriority">The minimum priority.</param>
        public PriorityFilterData(string name, int minimumPriority)
            : base(name, typeof(PriorityFilter))
        {
            this.MinimumPriority = minimumPriority;
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [ConfigurationProperty(minimumPriorityProperty)]
        public int MinimumPriority
        {
            get
            {
                return (int)this[minimumPriorityProperty];
            }
            set
            {
                this[minimumPriorityProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum priority value for messages to be processed.  Messages with a priority
        /// above the maximum are dropped immediately on the client.
        /// </summary>		
        [ConfigurationProperty(maximumPriorityProperty, DefaultValue = int.MaxValue)]
        public int MaximumPriority
        {
            get
            {
                return (int)this[maximumPriorityProperty];
            }
            set
            {
                this[maximumPriorityProperty] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeRegistration GetContainerConfigurationModel()
        {
            return
                new TypeRegistration<ILogFilter>(
                    () => new PriorityFilter(this.Name, this.MinimumPriority, this.MaximumPriority))
                {
                    Name = this.Name
                };
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="PriorityFilter"/> described by a <see cref="PriorityFilterData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="PriorityFilterData"/> type and it is used by the <see cref="LogFilterCustomFactory"/> 
    /// to build the specific <see cref="ILogFilter"/> object represented by the configuration object.
    /// </remarks>
    public class PriorityFilterAssembler : IAssembler<ILogFilter, LogFilterData>
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="PriorityFilter"/> based on an instance of <see cref="PriorityFilterData"/>.
        /// </summary>
        /// <seealso cref="LogFilterCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="PriorityFilterData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="PriorityFilter"/>.</returns>
        public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            PriorityFilterData castedObjectConfiguration = (PriorityFilterData)objectConfiguration;

            ILogFilter createdObject
                = new PriorityFilter(
                    castedObjectConfiguration.Name,
                    castedObjectConfiguration.MinimumPriority,
                    castedObjectConfiguration.MaximumPriority);

            return createdObject;
        }
    }
}
