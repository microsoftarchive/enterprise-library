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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls
{
    /// <summary>
    /// Provides data for the DisplayModeChanged event.
    /// </summary>
    public class CalendarModeChangedEventArgs : System.Windows.RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CalendarModeChangedEventArgs class.
        /// </summary>
        /// <param name="oldMode">Previous value of the property, prior to the event being raised.</param>
        /// <param name="newMode">Current value of the property at the time of the event.</param>
        public CalendarModeChangedEventArgs(CalendarMode oldMode, CalendarMode newMode)
        {
            this.OldMode = oldMode;
            this.NewMode = newMode;
        }

        /// <summary>
        /// Gets the new mode of the Calendar.
        /// </summary>
        public CalendarMode NewMode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the previous mode of the Calendar.
        /// </summary>
        public CalendarMode OldMode
        {
            get;
            private set;
        }
    }
}
