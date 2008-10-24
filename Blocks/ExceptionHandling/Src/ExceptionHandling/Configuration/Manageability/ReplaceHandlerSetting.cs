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
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ReplaceHandlerData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData"/>
	[ManagementEntity]
	public class ReplaceHandlerSetting : ExceptionHandlerSetting
	{
		private string exceptionMessage;
		private string replaceExceptionType;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerSetting"/> class with a configuration source element,
        /// the name of the handler, the exception message for the handler, and the handler type.
        /// </summary>
        /// <param name="sourceElement">The configuraiton source.</param>
        /// <param name="name">The name of the handler.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="replaceExceptionType">The type of the replace exception.</param>
		public ReplaceHandlerSetting(ConfigurationElement sourceElement, string name, string exceptionMessage,
		                               string replaceExceptionType)
			: base(sourceElement, name)
		{
			this.exceptionMessage = exceptionMessage;
			this.replaceExceptionType = replaceExceptionType;
		}

		/// <summary>
		/// Gets the message for the new replacing exception.
		/// </summary>
		[ManagementConfiguration]
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
			set { exceptionMessage = value; }
		}

		/// <summary>
		/// Gets the name of the type for the new replacing exception.
		/// </summary>
		[ManagementConfiguration]
		public string ReplaceExceptionType
		{
			get { return replaceExceptionType; }
			set { replaceExceptionType = value; }
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="ReplaceHandlerSetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<ReplaceHandlerSetting> GetInstances()
		{
			return GetInstances<ReplaceHandlerSetting>();
		}

		/// <summary>
		/// Returns the <see cref="ReplaceHandlerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Policy"></param>
		/// <param name="ExceptionType"></param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="ReplaceHandlerSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static ReplaceHandlerSetting BindInstance(string ApplicationName,
		                                                 string SectionName,
		                                                 string Policy,
		                                                 string ExceptionType,
		                                                 string Name)
		{
			return BindInstance<ReplaceHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}

		/// <summary>
		/// Saves the changes on the <see cref="ReplaceHandlerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return ReplaceHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
