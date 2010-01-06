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
using System.Windows.Input;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows.Automation.Peers;
using System.ComponentModel;
using System.Windows.Threading;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    [TemplatePart(Name = "PART_Adorner", Type = typeof(Border))]
    public class ElementModelContainer : ContentControl
    {
        private Control Title;
        private readonly SelectionHelper selectionHelper;

        #region Custom Dependency Properties

        private static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof (bool),
            typeof (ElementModelContainer),
            new PropertyMetadata(false));

        public Boolean IsExpanded
        {
            get { return (Boolean)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        #endregion

        static ElementModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementModelContainer),
                                                     new FrameworkPropertyMetadata(typeof(ElementModelContainer)));
        }

        private ElementViewModel ElementModel;

        public ElementModelContainer()
        {
            DataContextChanged += ElementModelContainer_DataContextChanged;
            
            selectionHelper = new SelectionHelper(this);

            CreateElementBindings();

        }

        void ElementModelContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ElementModel != null)
            {
                BindingOperations.ClearBinding(this, IsExpandedProperty);
            }

            ElementModel = e.NewValue as ElementViewModel;

            if (ElementModel == null) return;
            selectionHelper.Attach(ElementModel);

            CreateElementBindings();
        }

        void ElementModelContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Title = (Control)Template.FindName("Title", this);
            Title.MouseDoubleClick += new MouseButtonEventHandler(ElementModelContainer_MouseDoubleClick);
        }

        public ElementViewModel Element
        {
            get { return ElementModel; }
        }

        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new ElementModelContainerAutomationPeer(this);
        }

        private void CreateElementBindings()
        {
           var propertyExpansionBinding = new Binding("PropertiesShown")
           {
               Source = ElementModel,
               Mode = BindingMode.TwoWay,
           };

           BindingOperations.SetBinding(this, IsExpandedProperty, propertyExpansionBinding);
        }
    }

    public class ElementModelContainerAutomationPeer : FrameworkElementAutomationPeer
    {
        private readonly ElementModelContainer control;

        public ElementModelContainerAutomationPeer(ElementModelContainer control)
            : base(control)
        {
            AutomationProperties.SetHelpText(control, control.Name);
            this.control = control;
        }

        protected override Point GetClickablePointCore()
        {
            Point p = base.GetClickablePointCore();
            p.X -= control.ActualWidth / 2 - 30;
            return p;
        }
        protected override string GetAutomationIdCore()
        {
            return control.Element != null ? control.Element.Name : control.Name;
        }
        protected override string GetClassNameCore()
        {
            return typeof(ElementModelContainer).Name;
        }
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Header;
        }
    }
}
