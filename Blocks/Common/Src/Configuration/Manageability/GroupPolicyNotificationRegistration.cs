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
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a Group Policy notification registration to watch Group Policy notifications.
    /// </summary>
    public class GroupPolicyNotificationRegistration : IDisposable
    {
        readonly AutoResetEvent machinePolicyEvent;
        readonly AutoResetEvent userPolicyEvent;

        /// <summary>
        /// Initialize a new instance of the <see cref="GroupPolicyNotificationRegistration"/> object.
        /// </summary>
        public GroupPolicyNotificationRegistration()
        {
            machinePolicyEvent = new AutoResetEvent(false);
            userPolicyEvent = new AutoResetEvent(false);

            CheckReturnValue(NativeMethods.RegisterGPNotification(machinePolicyEvent.SafeWaitHandle, true));
            CheckReturnValue(NativeMethods.RegisterGPNotification(userPolicyEvent.SafeWaitHandle, false));
        }

        /// <summary>
        /// Gets the machine policy event.
        /// </summary>
        /// <value>
        /// An <see cref="AutoResetEvent"/> for the machine policy.
        /// </value>
        public AutoResetEvent MachinePolicyEvent
        {
            get { return machinePolicyEvent; }
        }

        /// <summary>
        /// Gets the user policy event.
        /// </summary>
        /// <value>
        /// An <see cref="AutoResetEvent"/> for the user policy.
        /// </value>
        public AutoResetEvent UserPolicyEvent
        {
            get { return userPolicyEvent; }
        }

        static void CheckReturnValue(bool returnValue)
        {
            if (!returnValue)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public virtual void Dispose()
        {
            try
            {
                CheckReturnValue(NativeMethods.UnregisterGPNotification(machinePolicyEvent.SafeWaitHandle));
                CheckReturnValue(NativeMethods.UnregisterGPNotification(userPolicyEvent.SafeWaitHandle));
            }
            finally
            {
                machinePolicyEvent.Close();
                userPolicyEvent.Close();
            }
        }
    }
}
