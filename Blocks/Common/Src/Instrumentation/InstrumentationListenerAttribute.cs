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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	/// <summary>
	/// Defines a class that will listen for instrumentation events broadcast by other classes
	/// and report them to system services. This attribute is placed on the class that is to be
	/// listened to, in order to define the class that will listen to it.
	/// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=true)]
	public sealed class InstrumentationListenerAttribute : Attribute
    {
        Type listenerType;
		Type listenerBinderType;

		/// <summary>
		/// Gets type of class to instantiate to listen for events.
		/// </summary>
        public Type ListenerType
        {
            get { return listenerType; }
        }
		
		/// <summary>
		/// Gets type of class to use to bind an instance of the attributed class to 
		/// an instance of the listener class
		/// </summary>
		public Type ListenerBinderType
		{
			get { return listenerBinderType; }
		}

		/// <overloads>
		/// Initializes attribute with given <paramref name="listenerType"></paramref>. 
		/// </overloads>
		/// <summary>
		/// Initializes attribute with given <paramref name="listenerType"></paramref>. 
		/// </summary>
		/// <param name="listenerType">Instrumentation listener type to instantiate.</param>
        public InstrumentationListenerAttribute(Type listenerType)
        {
            this.listenerType = listenerType;  
			this.listenerBinderType = null;
        }
		
		/// <summary>
		/// Initializes attribute with given <paramref name="listenerType"></paramref>. Use when
		/// you need to specify an explicit binder class.
		/// </summary>
		/// <param name="listenerType">Instrumentation listener type to instantiate.</param>
		/// <param name="listenerBinderType">Instrumentation binder listener type to instantiate.</param>
		public InstrumentationListenerAttribute(Type listenerType, Type listenerBinderType)
		{
			this.listenerType = listenerType;
			this.listenerBinderType = listenerBinderType;
		}
    }
}
