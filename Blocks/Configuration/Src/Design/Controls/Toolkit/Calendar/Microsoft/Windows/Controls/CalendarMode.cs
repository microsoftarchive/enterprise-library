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
    /// Specifies values for the different modes of operation of a Calendar. 
    /// </summary>
    public enum CalendarMode
    {
        /// <summary>
        /// The Calendar displays a month at a time.
        /// </summary>
        Month = 0,

        /// <summary>
        ///  The Calendar displays a year at a time.
        /// </summary>
        Year = 1,
        
        /// <summary>
        /// The Calendar displays a decade at a time.
        /// </summary>
        Decade = 2,
    }
}
