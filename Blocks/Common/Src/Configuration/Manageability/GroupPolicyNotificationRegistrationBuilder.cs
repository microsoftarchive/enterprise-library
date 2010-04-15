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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a builder for Group Policy notification registration.
    /// </summary>
    public class GroupPolicyNotificationRegistrationBuilder
    {
        /// <summary>
        /// Creates the registration.
        /// </summary>
        /// <returns>
        /// A <see cref="GroupPolicyNotificationRegistration"/> object.
        /// </returns>
        public virtual GroupPolicyNotificationRegistration CreateRegistration()
        {
            return new GroupPolicyNotificationRegistration();
        }
    }
}
