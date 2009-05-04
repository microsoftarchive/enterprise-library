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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Exception Handling Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class ExceptionHandlingBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Adds the policies describing the Exception Handling Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			ExceptionHandlingSettings settings
				= (ExceptionHandlingSettings)ConfigurationSource.GetSection(ExceptionHandlingSettings.SectionName);

			if (settings == null)
			{
				return;
			}

            UnityContainerConfigurator unityContainerConfigurator = new UnityContainerConfigurator(Container);
		    unityContainerConfigurator.RegisterAll(settings.CreateRegistrations());
		}

	}
}
