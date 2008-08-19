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
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.WrapHandlerData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData"/>
	[ManagementEntity]
	public class WrapHandlerSetting : ExceptionHandlerSetting
	{
		private string exceptionMessage;
		private string wrapExceptionType;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="exceptionMessage"></param>
		/// <param name="wrapExceptionType"></param>
		/// <param name="sourceElement"></param>
		public WrapHandlerSetting(ConfigurationElement sourceElement, string name, string exceptionMessage,
		                          string wrapExceptionType)
			: base(sourceElement, name)
		{
			this.exceptionMessage = exceptionMessage;
			this.wrapExceptionType = wrapExceptionType;
		}

		/// <summary>
		/// Gets the message for the wrapping exception.
		/// </summary>
		[ManagementConfiguration]
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
			set { exceptionMessage = value; }
		}

		/// <summary>
		/// Gets the name of the type for the wrapping exception.
		/// </summary>
		[ManagementConfiguration]
		public string WrapExceptionType
		{
			get { return wrapExceptionType; }
			set { wrapExceptionType = value; }
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="WrapHandlerSetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<WrapHandlerSetting> GetInstances()
		{
			return GetInstances<WrapHandlerSetting>();
		}

		/// <summary>
		/// Returns the <see cref="WrapHandlerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Policy"></param>
		/// <param name="ExceptionType"></param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="WrapHandlerSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static WrapHandlerSetting BindInstance(string ApplicationName,
		                                              string SectionName,
		                                              string Policy,
		                                              string ExceptionType,
		                                              string Name)
		{
			return BindInstance<WrapHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}

		/// <summary>
		/// Saves the changes on the <see cref="WrapHandlerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return WrapHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}