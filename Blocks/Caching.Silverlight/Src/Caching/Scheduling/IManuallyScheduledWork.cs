//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling
{
    /// <summary>
    /// This interface represents an object that can perform
    /// some sort of operation. The operation will be run at
    /// some point in the future.
    /// </summary>
    public interface IManuallyScheduledWork
    {
        /// <summary>
        /// Set the action that will be run when work is scheduled.
        /// </summary>
        /// <param name="workToDo">The <see cref="Action"/> that will
        /// be invoked when the action is scheduled.</param>
        void SetAction(Action workToDo);

        /// <summary>
        /// Requests that the object perform its work
        /// at some point in the future.
        /// </summary>
        void ScheduleWork();
    }
}
