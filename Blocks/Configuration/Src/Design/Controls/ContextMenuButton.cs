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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// Sites and opens the <see cref="ContextMenu"/> on a button when it is clicked.  This
    /// enables showing the context menu on left-click gestures.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class ContextMenuButton : ButtonBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click"/> routed event. 
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            ContextMenu menu;

            if (TargetElement != null)
            {
                menu = TargetElement.ContextMenu;   
            }
            else
            {
                menu = this.ContextMenu;
            }

            if (menu != null)
            {
                menu.PlacementTarget = this;
                menu.IsOpen = true;
            }
        }

        ///<summary>
        /// Gets or sets the target element containing the context menu to show.
        ///</summary>
        /// <remarks>
        /// If this is <see langword="null"/>, then <see cref="ContextMenuButton"/> attempts to show its context menu.</remarks>
        public FrameworkElement TargetElement
        {
            get { return (FrameworkElement)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        ///<summary>
        /// The target element containing the context menu to show.
        ///</summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement", typeof(FrameworkElement), typeof(ContextMenuButton), new UIPropertyMetadata(null));


        internal void AutomationContextMenuButtonClick()
        {
            OnClick();
        }

        /// <summary>
        /// Returns class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> implementations for the Windows Presentation Foundation (WPF) infrastructure.
        /// </summary>
        /// <returns>
        /// The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> implementation.
        /// </returns>
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new ContextMenuButtonAutomationPeer(this);
        }
    }

    ///<summary>
    /// The automation-peer for <see cref="ContextMenuButton"/>. This is not intended to be used directly from your code.
    ///</summary>
    public class ContextMenuButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ContextMenuButtonAutomationPeer"/>.
        /// </summary>
        /// <param name="owner">The owning <see cref="ContextMenuButton"/>.</param>
        public ContextMenuButtonAutomationPeer(ContextMenuButton owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Gets the name of the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="F:System.String.Empty"/> string.
        /// </returns>
        protected override string GetClassNameCore()
        {
            return typeof(ContextMenuButton).Name;
        }

        /// <summary>
        /// Gets the control type for the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom"/> enumeration value.
        /// </returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        /// <summary>
        /// Gets the control pattern for the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>.
        /// </summary>
        /// <returns>
        /// null.
        /// </returns>
        /// <param name="patternInterface">A value from the enumeration.
        ///                 </param>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return null;
        }
        #region IInvokeProvider Members

        void IInvokeProvider.Invoke()
        {
            if (!base.IsEnabled())
            {
                throw new InvalidOperationException(Resources.ExceptionCannotInvokeContextMenuButton);
            }

            base.Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action<object>
                    (delegate(object param)
                    {
                        ((ContextMenuButton)base.Owner).AutomationContextMenuButtonClick();
                    }),
                null);

        }

        #endregion
    }
}
