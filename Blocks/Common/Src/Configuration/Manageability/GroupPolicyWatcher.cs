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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// The policy watcher can be started and stopped many times. To deal with this, when a watcher thread is started
    /// it is given an 'exit' event that will be signaled when the thread needs to be stopped. Once the thread is started
    /// it own the exit event, and will release it when it terminates. More than one watching thread may be active at the
    /// same time, having different exit events, if the old watching thread doesn't get processing time before the new 
    /// thread is started; when the old thread gets to run it will consume the signaled exit event and finish.
    /// </summary>
    public sealed class GroupPolicyWatcher : IGroupPolicyWatcher
    {
        AutoResetEvent currentThreadExitEvent;
        object lockObject = new object();
        GroupPolicyNotificationRegistrationBuilder registrationBuilder;

        /// <summary>
        /// Initialize a new instance of the <see cref="GroupPolicyWatcher"/> class.
        /// </summary>
        public GroupPolicyWatcher()
            : this(new GroupPolicyNotificationRegistrationBuilder()) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="GroupPolicyWatcher"/> class with a registration builder.
        /// </summary>
        /// <param name="registrationBuilder">
        /// The builder used to create the registration for Group Policy.
        /// </param>
        public GroupPolicyWatcher(GroupPolicyNotificationRegistrationBuilder registrationBuilder)
        {
            this.registrationBuilder = registrationBuilder;
        }

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopWatching();
            }
        }

        void DoWatch(object parameter)
        {
            AutoResetEvent exitEvent = (AutoResetEvent)parameter;

            try
            {
                using (GroupPolicyNotificationRegistration registration = registrationBuilder.CreateRegistration())
                {
                    AutoResetEvent[] policyEvents
                        = new AutoResetEvent[] { exitEvent, registration.MachinePolicyEvent, registration.UserPolicyEvent };

                    bool listening = true;

                    while (listening)
                    {
                        int result = WaitHandle.WaitAny(policyEvents); // 0 == exit, 1 == machine, 2 == user
                        if (result != 0)
                        {
                            // notification from policy handles, not from exit
                            // fire the change notification mechanism
                            if (GroupPolicyUpdated != null)
                            {
                                GroupPolicyUpdated(result == 1);
                            }
                        }
                        else
                        {
                            // notification from exit
                            listening = false;
                        }
                    }
                }
            }
            finally
            {
                // release the thread's exit event.
                exitEvent.Close();
            }
        }

        ///<summary>
        ///Allows an <see cref="T:System.Object"></see> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Object"></see> is reclaimed by garbage collection.
        ///</summary>\
        ~GroupPolicyWatcher()
        {
            Dispose(false);
        }

        /// <summary>
        /// The event to update the policy.
        /// </summary>
        public event GroupPolicyUpdateDelegate GroupPolicyUpdated;

        /// <summary>
        /// Starts watching Group Policy.
        /// </summary>
        public void StartWatching()
        {
            lock (lockObject)
            {
                if (currentThreadExitEvent == null)
                {
                    // this event will be released by the watcher thread on exit.
                    currentThreadExitEvent = new AutoResetEvent(false);

                    Thread watchingThread = new Thread(new ParameterizedThreadStart(DoWatch));
                    watchingThread.IsBackground = true;
                    watchingThread.Name = Resources.GroupPolicyWatcherThread;
                    watchingThread.Start(currentThreadExitEvent);
                }
            }
        }

        /// <summary>
        /// Stops watching Group Policy.
        /// </summary>
        public void StopWatching()
        {
            lock (lockObject)
            {
                if (currentThreadExitEvent != null)
                {
                    // this signal will be consumed by the current watcher thread.
                    currentThreadExitEvent.Set();

                    currentThreadExitEvent = null;
                }
            }
        }
    }
}
