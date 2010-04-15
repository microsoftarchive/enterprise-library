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
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows.Automation.Peers;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// Contains an <see cref="ElementViewModel"/> providing hookup for keyboard acceleration
    /// and default layout and visualization.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    [TemplatePart(Name = "PART_Adorner", Type = typeof(Border))]
    public class ElementModelContainer : ContentControl
    {
        private Control title;
        private readonly SelectionHelper selectionHelper;
        private ElementViewModel elementModel;

        #region Custom Dependency Properties

        private static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(ElementModelContainer),
            new PropertyMetadata(false, ElementIsExpandedChangeHandler)
            );

        private static void ElementIsExpandedChangeHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ElementModelContainer;
            if (element != null)
            {
                element.Focus();
            }
        }

        ///<summary>
        /// Gets or sets the <see cref="IsExpandedProperty"/> value.
        ///</summary>
        public Boolean IsExpanded
        {
            get { return (Boolean)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ElementModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementModelContainer),
                                                     new FrameworkPropertyMetadata(typeof(ElementModelContainer)));          
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementModelContainer"/>.
        /// </summary>
        public ElementModelContainer()
        {
            DataContextChanged += DataContextChangedHandler;
            Unloaded += UnloadedHandler;

            selectionHelper = new SelectionHelper(this);

            CreateElementBindings();
        }

        void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (elementModel != null)
            {
                elementModel = null; // have to set to null before clearing bindings
                selectionHelper.Clear();
                BindingOperations.ClearBinding(this, IsExpandedProperty);
            }
        }

        void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (elementModel != null)
            {
                BindingOperations.ClearBinding(this, IsExpandedProperty);
            }

            elementModel = e.NewValue as ElementViewModel;

            if (elementModel == null) return;
            selectionHelper.Attach(elementModel);

            CreateElementBindings();
        }

        /// <summary>
        /// Applies the control's template.
        /// </summary>
        /// <remarks>
        /// Locates the Title control in the template and monitors the <see cref="Control.MouseDoubleClick"/> event
        /// to toggle the contained <see cref="IsExpanded"/> property.
        /// </remarks>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            title = (Control)Template.FindName("Title", this);
            title.MouseDoubleClick += (o, e) => IsExpanded = !IsExpanded;
        }

        ///<summary>
        /// The contained <see cref="ElementViewModel"/>.
        ///</summary>
        public ElementViewModel Element
        {
            get { return elementModel; }
        }

        /// <summary>
        /// Returns class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> implementations for the Windows Presentation Foundation (WPF) infrastructure.
        /// </summary>
        /// <returns>
        /// Returns a new instance of <see cref="ElementModelContainerAutomationPeer"/>.
        /// </returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ElementModelContainerAutomationPeer(this);
        }

        private void CreateElementBindings()
        {
            var propertyExpansionBinding = new Binding("PropertiesShown")
            {
                Source = elementModel,
                Mode = BindingMode.TwoWay,
            };

            BindingOperations.SetBinding(this, IsExpandedProperty, propertyExpansionBinding);
        }
    }

    /// <summary>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// <br/>
    /// The automation peer for <see cref="ElementModelContainer"/>.
    /// </summary>
    public class ElementModelContainerAutomationPeer : FrameworkElementAutomationPeer
    {
        private readonly ElementModelContainer control;

        ///<summary>
        /// Initializes a new instance of <see cref="ElementModelContainerAutomationPeer"/>.
        ///</summary>
        ///<param name="control">The owning <see cref="ElementModelContainer"/></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated by parent class")]
        public ElementModelContainerAutomationPeer(ElementModelContainer control)
            : base(control)
        {
            AutomationProperties.SetHelpText(control, control.Name);
            this.control = control;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Windows.Point"/> that represents the clickable space that is on the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Windows.Point"/> on the element that allows a click. The point values are (<see cref="F:System.Double.NaN"/>, <see cref="F:System.Double.NaN"/>) if the element is not both a <see cref="T:System.Windows.Interop.HwndSource"/> and a <see cref="T:System.Windows.PresentationSource"/>.
        /// </returns>
        protected override Point GetClickablePointCore()
        {
            Point p = base.GetClickablePointCore();
            p.X -= control.ActualWidth / 2 - 30;
            p.Y -= control.ActualHeight / 2 - 35;
            return p;
        }

        /// <summary>
        /// Gets the string that uniquely identifies the <see cref="T:System.Windows.FrameworkElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationId"/>.
        /// </summary>
        /// <returns>
        /// The automation identifier for the element associated with the <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer"/>, or <see cref="F:System.String.Empty"/> if there isn't an automation identifier.
        /// </returns>
        protected override string GetAutomationIdCore()
        {
            return control.Element != null ? control.Element.Name : control.Name;
        }

        /// <summary>
        /// Gets the name of the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="F:System.String.Empty"/> string.
        /// </returns>
        protected override string GetClassNameCore()
        {
            return typeof(ElementModelContainer).Name;
        }

        /// <summary>
        /// Gets the control type for the <see cref="T:System.Windows.UIElement"/> that is associated with this <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom"/> enumeration value.
        /// </returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Header;
        }
    }
}
