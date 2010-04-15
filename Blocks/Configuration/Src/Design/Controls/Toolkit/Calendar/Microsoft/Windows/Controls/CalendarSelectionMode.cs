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
    /// Specifies values for the different selection modes of a Calendar. 
    /// </summary>
    public enum CalendarSelectionMode
    {
        /// <summary>
        /// One date can be selected at a time.
        /// </summary>
        SingleDate = 0,
        
        /// <summary>
        /// One range of dates can be selected at a time.
        /// </summary>
        SingleRange = 1,
        
        /// <summary>
        /// Multiple dates or ranges can be selected at a time.
        /// </summary>
        MultipleRange = 2,
        
        /// <summary>
        /// No dates can be selected.
        /// </summary>
        None = 3,
    }
}
