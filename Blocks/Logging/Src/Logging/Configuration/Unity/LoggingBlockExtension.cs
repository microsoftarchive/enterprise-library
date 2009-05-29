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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Logging Application Block's
	/// objects described in the configuration file.
	/// </summary>
 	/// <remarks>This function is now performed directly by the <see cref="EnterpriseLibraryCoreExtension"/>.
    /// This extension is now a noop and is obsolete.</remarks>

	[Obsolete]
	public class LoggingBlockExtension : EnterpriseLibraryBlockExtension
	{
	}
}
