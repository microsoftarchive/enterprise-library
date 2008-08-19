//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Explicit binder for data access instrumentation.
	/// </summary>
	public class DataInstrumentationListenerBinder : IExplicitInstrumentationBinder
    {
		/// <summary>
		/// Binds the events exposed by the source to the handlers in the listener.
		/// </summary>
		/// <param name="source">The source of instrumentation events. Must be an instance of <see cref="DataInstrumentationProvider"/>.</param>
		/// <param name="listener">The listener for instrumentation events. Must be an instance of <see cref="DataInstrumentationListener"/>.</param>
		public void Bind(object source, object listener)
        {
            DataInstrumentationListener castedListener = (DataInstrumentationListener)listener;
            DataInstrumentationProvider castedProvider = (DataInstrumentationProvider)source;

            castedProvider.commandExecuted += castedListener.CommandExecuted;
            castedProvider.commandFailed += castedListener.CommandFailed;
            castedProvider.connectionFailed += castedListener.ConnectionFailed;
            castedProvider.connectionOpened += castedListener.ConnectionOpened;
        }
    }
}
