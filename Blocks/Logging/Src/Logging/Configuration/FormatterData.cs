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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings for a log formatter.  This class is abstract.
	/// </summary>
	public class FormatterData : NameTypeConfigurationElement
	{
		/// <summary>
		/// Create a new instance of a <see cref="FormatterData"/>.
		/// </summary>
		public FormatterData()
		{
		}

		/// <summary>
		/// Create a new instance of a <see cref="FormatterData"/>.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="formatterType">The <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.ILogFormatter"/> type.</param>
		public FormatterData(string name, Type formatterType)
			: base(name, formatterType)
		{
		}
	}
}