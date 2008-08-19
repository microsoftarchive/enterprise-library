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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	/// <summary>
	/// Defines the contract to be implemented by explicit binders for individual blocks. 
	/// Explicit binders are used to define exactly how instrumentation event sources are bound to
	/// instrumentation event listeners. It is used for those instances when the implementor wishes to
	/// explicitly define how this binding happens, rather than allow the binding to occur through reflection.
	/// </summary>
	public interface IExplicitInstrumentationBinder
	{
		/// <summary>
		/// Adds specified <paramref name="listenerMethod"></paramref> as an event handler for 
		/// the <paramref name="sourceEvent"></paramref>.
		/// </summary>
		/// <param name="source">Event on source object to which <paramref name="listenerMethod"></paramref> will be added.</param>
		/// <param name="listener">Method to be added as event handler for <paramref name="listenerMethod"></paramref>.</param>
		void Bind(object source, object listener);
	}
}