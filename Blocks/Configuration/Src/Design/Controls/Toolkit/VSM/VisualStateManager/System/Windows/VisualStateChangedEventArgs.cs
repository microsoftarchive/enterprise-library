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
using System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit
{
    /// <summary>
    ///     EventArgs for VisualStateGroup.CurrentStateChanging and CurrentStateChanged events.
    /// </summary>
    public sealed class VisualStateChangedEventArgs : EventArgs
    {
        internal VisualStateChangedEventArgs(VisualState oldState, VisualState newState, Control control)
        {
            _oldState = oldState;
            _newState = newState;
            _control = control;
        }

        /// <summary>
        ///     The old state the control is transitioning from
        /// </summary>
        public VisualState OldState
        {
            get
            {
                return _oldState;
            }
        }

        /// <summary>
        ///     The new state the control is transitioning to
        /// </summary>
        public VisualState NewState
        {
            get
            {
                return _newState;
            }
        }

        /// <summary>
        ///     The control involved in the state change
        /// </summary>
        public Control Control
        {
            get
            {
                return _control;
            }
        }

        private VisualState _oldState;
        private VisualState _newState;
        private Control _control;
    }
}
