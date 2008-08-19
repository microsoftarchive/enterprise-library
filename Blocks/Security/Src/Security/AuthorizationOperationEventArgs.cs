//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Principal;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Provides data for authorization events.
    /// </summary>
	public class AuthorizationOperationEventArgs : EventArgs
	{
		private string identity;
        private string ruleName;

		/// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationOperationEventArgs"/> class.
		/// </summary>
		/// <param name="identity">The name of the identity this event applies to.</param>
        /// <param name="ruleName">The name of the rule this event applies to.</param>
        public AuthorizationOperationEventArgs(string identity, string ruleName)
		{
			this.identity = identity;
            this.ruleName = ruleName;
		}

		/// <summary>
        /// Gets the name of the identity this event applies to.
		/// </summary>
		public string Identity
		{
			get { return this.identity; }
		}

		/// <summary>
        /// Gets the name of the authorization rule this event applies to.
		/// </summary>
		public string RuleName
		{
            get { return this.ruleName; }
		}
	}
}
