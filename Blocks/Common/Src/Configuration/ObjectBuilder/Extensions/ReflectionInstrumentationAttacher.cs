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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Binds properly attributed events in source object to properly attributed handler methods in
	/// listener object using reflection.
	/// </summary>
	public class ReflectionInstrumentationAttacher : IInstrumentationAttacher
	{
		object source;
		Type listenerType;
		object[] listenerConstructorArgs;

		/// <summary>
		/// Initializes this object as needed to allow it to explicitly bind together events and handlers
		/// from source and listener objects.
		/// </summary>
		/// <param name="source">Source object containing events to be bound.</param>
		/// <param name="listenerType">Type of listener object to instantiate. Contains handler methods to be bound to.</param>
		/// <param name="listenerConstructorArgs">Constructor arguments used to instantiate object referred to by <see paramref="listenerType"></see>.</param>
		public ReflectionInstrumentationAttacher(object source, Type listenerType, object[] listenerConstructorArgs)
		{
			this.source = source;
			this.listenerType = listenerType;
			this.listenerConstructorArgs = listenerConstructorArgs;
		}

		/// <summary>
		/// Binds events to handling methods.
		/// </summary>
		public void BindInstrumentation()
		{
			object listener = CreateListener();
			BindSourceToListener(source, listener);
		}

		private object CreateListener()
		{
			return Activator.CreateInstance(listenerType, listenerConstructorArgs);
		}

		private void BindSourceToListener(object createdObject, object listener)
		{
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(createdObject, listener);
		}
	}
}
