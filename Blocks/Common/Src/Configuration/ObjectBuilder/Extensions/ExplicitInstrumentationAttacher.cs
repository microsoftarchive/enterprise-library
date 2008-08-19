//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Binds properly attributed events in source object to properly attributed handler methods in
	/// listener object using an explicit binding object.
	/// </summary>
	public class ExplicitInstrumentationAttacher : IInstrumentationAttacher
	{
		object source;
		Type listenerType;
		object[] listenerConstructorArguments;
		Type explicitBinderType;

		/// <summary>
		/// Initializes this object as needed to allow it to explicitly bind together events and handlers
		/// from source and listener objects.
		/// </summary>
		/// <param name="source">Source object containing events to be bound.</param>
		/// <param name="listenerType">Type of listener object to instantiate. Contains handler methods to be bound to.</param>
		/// <param name="listenerConstructorArguments">Constructor arguments used to instantiate object referred to by <see paramref="listenerType"></see>.</param>
		/// <param name="explicitBinderType">Type to be used in explicitly binding events to handlers.</param>
		public ExplicitInstrumentationAttacher(object source, Type listenerType, object [] listenerConstructorArguments, Type explicitBinderType)
		{
			this.source = source;
			this.listenerType = listenerType;
			this.listenerConstructorArguments = listenerConstructorArguments;
			this.explicitBinderType = explicitBinderType;
		}
		
		/// <summary>
		/// Binds events to handling methods.
		/// </summary>
		public void BindInstrumentation()
		{
			IExplicitInstrumentationBinder binder = (IExplicitInstrumentationBinder) Activator.CreateInstance(explicitBinderType);
			object listener = Activator.CreateInstance(listenerType, listenerConstructorArguments);
			
			binder.Bind(source, listener);
		}
	}
}
