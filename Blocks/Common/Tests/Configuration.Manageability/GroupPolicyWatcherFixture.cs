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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class GroupPolicyWatcherFixture
    {
        int notifications;
        bool lastNotifiedValue;
        MockGroupPolicyNotificationRegistrationBuilder builder;
        GroupPolicyWatcher watcher;

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            notifications = 0;
            lastNotifiedValue = false;
            builder = new MockGroupPolicyNotificationRegistrationBuilder();
            watcher = new GroupPolicyWatcher(builder);
            watcher.GroupPolicyUpdated += GroupPolicyUpdated;
        }

        [TestMethod]
        public void RegistrationsAreRequestedOnStart()
        {
            Assert.AreEqual(0, builder.issuedRegistrations.Count);

            try
            {
                watcher.StartWatching();
                Thread.Sleep(300);

                Assert.AreEqual(1, builder.issuedRegistrations.Count);
            }
            finally
            {
                watcher.StopWatching();
            }
        }

        // might give false negatives if a real group policy update event is handled
        [TestMethod]
        public void NoEventsAreFiredIfGPNotificationsAreNotReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(300);

                Assert.AreEqual(0, notifications);
            }
            finally
            {
                watcher.StopWatching();
            }
        }

        // might give false negatives if a real group policy update event is handled
        [TestMethod]
        public void EventIsFiredIfMachineGPNotificationIsReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(100);
                builder.LastRegistration.MachinePolicyEvent.Set();
                Thread.Sleep(300);

                Assert.AreEqual(1, notifications);
                Assert.AreEqual(true, lastNotifiedValue);
            }
            finally
            {
                watcher.StopWatching();
            }
        }

        // might give false negatives if a real group policy update event is handled
        [TestMethod]
        public void EventIsFiredIfUserGPNotificationIsReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(100);
                builder.LastRegistration.MachinePolicyEvent.Set();
                Thread.Sleep(300);

                Assert.AreEqual(1, notifications);
                Assert.AreEqual(true, lastNotifiedValue);
            }
            finally
            {
                watcher.StopWatching();
            }
        }

        void GroupPolicyUpdated(bool machine)
        {
            notifications++;
            lastNotifiedValue = machine;
        }
    }

    class MockGroupPolicyNotificationRegistrationBuilder : GroupPolicyNotificationRegistrationBuilder
    {
        public List<GroupPolicyNotificationRegistration> issuedRegistrations
            = new List<GroupPolicyNotificationRegistration>();

        public GroupPolicyNotificationRegistration LastRegistration
        {
            get
            {
                if (issuedRegistrations.Count > 0)
                {
                    return issuedRegistrations[issuedRegistrations.Count - 1];
                }
                return null;
            }
        }

        public override GroupPolicyNotificationRegistration CreateRegistration()
        {
            GroupPolicyNotificationRegistration registration = base.CreateRegistration();

            issuedRegistrations.Add(registration);

            return registration;
        }
    }
}