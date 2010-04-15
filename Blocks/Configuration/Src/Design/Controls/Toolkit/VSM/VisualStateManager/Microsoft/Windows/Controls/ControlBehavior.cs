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
using System.Windows;
using System.Windows.Controls;

#pragma warning disable 1591

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls
{
    /// <summary>
    ///     Provides VisualStateManager base behavior for controls.
    /// </summary>
    /// <remarks>
    ///     Provides focus states.
    ///     Forwards the Loaded event to UpdateState.
    /// </remarks>
    public class ControlBehavior : VisualStateBehavior
    {
        /// <summary>
        ///     This behavior targets Control derived Controls.
        /// </summary>
        protected override internal Type TargetType
        {
            get { return typeof(Control); }
        }

        /// <summary>
        ///     Attaches to property changes and events.
        /// </summary>
        /// <param name="control">An instance of the control.</param>
        protected override void OnAttach(Control control)
        {
            control.Loaded += delegate(object sender, RoutedEventArgs e) { UpdateState(control, false);};
            AddValueChanged(UIElement.IsKeyboardFocusWithinProperty, typeof(Control), control, UpdateStateHandler);
        }

        /// <summary>
        /// Detaches property changes and events.
        /// </summary>
        /// <param name="control">The control</param>
        protected override void OnDetach(Control control)
        {
            RemoveValueChanged(UIElement.IsKeyboardFocusWithinProperty, typeof(Control), control, UpdateStateHandler);
        }

        protected override void UpdateStateHandler(Object o, EventArgs e)
        {
            Control cont = o as Control;
            if (cont == null)
            {
                throw new InvalidOperationException("This should never be used on anything other than a control.");
            }
            UpdateState(cont, true);
        }

        /// <summary>
        ///     Called to update the control's visual state.
        /// </summary>
        /// <param name="control">The instance of the control being updated.</param>
        /// <param name="useTransitions">Whether to use transitions or not.</param>
        protected override void UpdateState(Control control, bool useTransitions)
        {
            if (control.IsKeyboardFocusWithin)
            {
                VisualStateManager.GoToState(control, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(control, "Unfocused", useTransitions);
            }
        }
    }
}

#pragma warning restore 1591
