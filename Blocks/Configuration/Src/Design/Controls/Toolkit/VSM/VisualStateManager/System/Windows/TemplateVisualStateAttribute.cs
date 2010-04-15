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

// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit
{
    /// <summary>
    ///     Define an expected VisualState in the contract between a Control and its
    ///     ControlTemplate for use with the VisualStateManager.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TemplateVisualStateAttribute : Attribute
    {
        /// <summary>
        ///     Name of the VisualState.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        ///     Name of the VisualStateGroup containing this state.
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
    }
}
