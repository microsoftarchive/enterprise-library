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
using System.Collections;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Helper to link a nodes rename and remove events.
	/// </summary>
    public static class LinkNodeHelper 
    {
		/// <summary>
		/// Create a reference for a node's removed event.
		/// </summary>
		/// <typeparam name="T">The type of node to link.</typeparam>
		/// <param name="oldReference">The old reference.</param>
		/// <param name="newReference">The new reference</param>
		/// <param name="referenceRemovedHandler">The handler for the removed event.</param>
		/// <returns>A new node with the removed hooked to the handler.</returns>
        public static T CreateReference<T>(T oldReference, T newReference, EventHandler<ConfigurationNodeChangedEventArgs> referenceRemovedHandler)
			where T : ConfigurationNode
        {
            if (newReference == null)
            {
                if (oldReference != null)
                {
                    oldReference.Removed -= referenceRemovedHandler;
                }
            }
            else if (newReference != oldReference)
            {            
                if (oldReference != null)
                {
                    oldReference.Removed -= referenceRemovedHandler;
                }

                newReference.Removed += referenceRemovedHandler;
            }
            return newReference;
        }

		/// <summary>
		/// Create a reference for a node's removed event.
		/// </summary>
		/// <typeparam name="T">The type of node to link.</typeparam>
		/// <param name="oldReference">The old reference.</param>
		/// <param name="newReference">The new reference</param>
		/// <param name="referenceRemovedHandler">The handler for the removed event.</param>
		/// <param name="referenceRenamedHandler">The handler for the renamed event.</param>
		/// <returns>A new node with the removed and renamed events hooked to the handler.</returns>
		public static T CreateReference<T>(T oldReference, T newReference, EventHandler<ConfigurationNodeChangedEventArgs> referenceRemovedHandler, EventHandler<ConfigurationNodeChangedEventArgs> referenceRenamedHandler) 
			where T : ConfigurationNode
        {
            T node = CreateReference(oldReference, newReference, referenceRemovedHandler);
            if (node != null && node != oldReference)
            {
                if (oldReference != null)
                {
                    oldReference.Renamed -= referenceRenamedHandler;
                }
                node.Renamed += referenceRenamedHandler;
            }
            return node;
        }
    }
}