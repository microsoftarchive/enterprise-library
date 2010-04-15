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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
	/// Binds an event source to an event handler.
    /// </summary>
    public class EventBinder
    {
        object source;
        object listener;

    	/// <summary>
		/// Initializes this object with the source and listener objects to be bound together.
    	/// </summary>
    	/// <param name="source">Object owning the event that will be bound to</param>
    	/// <param name="listener">Object owning the method that will be added as a handler for specified event.</param>
        public EventBinder(object source, object listener)
        {
            this.source = source;
            this.listener = listener;
        }
        
    	/// <summary>
		/// Adds specified <paramref name="listenerMethod"></paramref> as an event handler for 
		/// the <paramref name="sourceEvent"></paramref>.
    	/// </summary>
    	/// <param name="sourceEvent">Event on source object to which <paramref name="listenerMethod"></paramref> will be added.</param>
    	/// <param name="listenerMethod">Method to be added as event handler for <paramref name="listenerMethod"></paramref>.</param>
        public virtual void Bind(EventInfo sourceEvent, MethodInfo listenerMethod)
        {
            if (sourceEvent == null) throw new ArgumentNullException("sourceEvent");

            Delegate methodToBindTo =
                Delegate.CreateDelegate(sourceEvent.EventHandlerType, listener, listenerMethod);
            sourceEvent.AddEventHandler(source, methodToBindTo);            
        }
    }
}
