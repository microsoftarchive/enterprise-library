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

//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls
{
    /// <summary>
    /// Provides data for the DateSelected and DisplayDateChanged events.
    /// </summary>
    public class CalendarDateChangedEventArgs : RoutedEventArgs
    {
        internal CalendarDateChangedEventArgs(DateTime? removedDate, DateTime? addedDate)
        {
            this.RemovedDate = removedDate;
            this.AddedDate = addedDate;
        }

        /// <summary>
        /// Gets the date to be newly displayed.
        /// </summary>
        public DateTime? AddedDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date that was previously displayed.
        /// </summary>
        public DateTime? RemovedDate
        {
            get;
            private set;
        }
    }
}
