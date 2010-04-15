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
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Defines a watcher for Group Policy.
    /// </summary>
	public interface IGroupPolicyWatcher : IDisposable
	{
        /// <summary>
        /// The event to update the policy.
        /// </summary>
		event GroupPolicyUpdateDelegate GroupPolicyUpdated;

        /// <summary>
        /// Starts watching Group Policy.
        /// </summary>
		void StartWatching();

        /// <summary>
        /// Stops watching Group Policy.
        /// </summary>
		void StopWatching();
	}
    
    /// <summary>
    /// The delegate used to update the Group Policy based on machine.
    /// </summary>
    /// <param name="machine">The machine where Group Policy is updated.</param>
	public delegate void GroupPolicyUpdateDelegate(bool machine);
}
