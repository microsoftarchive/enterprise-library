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

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="ExceptionPolicy"/>.
    /// </summary>    		
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
        public ExceptionPolicyData(string name) : base(name)
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
		public NamedElementCollection<ExceptionTypeData> ExceptionTypes
		{
			get
			{
				return (NamedElementCollection<ExceptionTypeData>)this[exceptionTypesProperty];
			}
		}

        /// <summary>
        /// Retrieves the <see cref="TypeRegistration"/> for registering a <see cref="ExceptionPolicyImpl"/> in the container.
        /// </summary>
        /// <returns>A completed <see cref="TypeRegistration"/></returns>
        public IEnumerable<TypeRegistration> GetRegistration(IConfigurationSource configurationSource)
        {
            yield return new TypeRegistration<ExceptionPolicyImpl>(
                () => new ExceptionPolicyImpl(
                    Name, 
                    Container.ResolvedEnumerable<ExceptionPolicyEntry>(
                        from data in ExceptionTypes
                            select BuildChildName(data.Name)
                    
                    )))
                    {Name = Name};

            yield return GetInstrumentationRegistration(configurationSource);
        }

        private string BuildChildName(string childName)
        {
            return string.Format("{0}.{1}", Name, childName);
        }

        private TypeRegistration GetInstrumentationRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return new TypeRegistration<IExceptionHandlingInstrumentationProvider>(
                () => new ExceptionHandlingInstrumentationProvider(Name,
                                                                      instrumentationSection.PerformanceCountersEnabled,
                                                                      instrumentationSection.EventLoggingEnabled,
                                                                      instrumentationSection.WmiEnabled,
                                                                      instrumentationSection.ApplicationInstanceName))
                       {
                           Name = Name
                       };
        }
    }
}
