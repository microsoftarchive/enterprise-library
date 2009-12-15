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
using System.Windows.Input;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows.Automation.Peers;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    [TemplatePart(Name = "PART_Adorner", Type = typeof(Border))]
    public class ElementModelContainer : ContentControl
    {
        private Control Title;
        public static TogglePropertiesCommand ToggleProperties { get; set; }

        #region Custom Dependency Properties
        private static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(Boolean), typeof(ElementModelContainer), new PropertyMetadata(false));
        public Boolean IsExpanded
        {
            get { return (Boolean)GetValue(IsExpandedProperty); }
            set 
            { 
                SetValue(IsExpandedProperty, value);
                if (Section != null) Section.ClearAdorners();
            }
        }
        #endregion

        public static readonly RoutedEvent ShowPropertiesEvent = EventManager.RegisterRoutedEvent(
        "ShowProperties", RoutingStrategy.Bubble, typeof(PropertiesRoutedEventHandler), typeof(ElementModelContainer));
        

        static ElementModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementModelContainer),
                                                     new FrameworkPropertyMetadata(typeof(ElementModelContainer)));

        }

        private BlockVisualizer Section;
        private ElementViewModel ElementModel;
        
        
        public ElementModelContainer()
        {
            ToggleProperties = new TogglePropertiesCommand();
            DataContextChanged += new DependencyPropertyChangedEventHandler(ElementModelContainer_DataContextChanged);
            MouseLeftButtonDown += new MouseButtonEventHandler(ElementModelContainer_MouseLeftButtonDown);

            IsExpanded = true;
        }

        void ElementModelContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Section.SetSelectedElement(this);
            ElementModel.Select();
        }

        void ElementModelContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ElementModel = e.NewValue as ElementViewModel;
        }

        void ElementModelContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ToggleProperties.Execute(this);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            Section = VisualTreeWalker.TryFindParent<BlockVisualizer>(this);
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


    public class TogglePropertiesCommand : ICommand
    {
        public void Execute(object parameter)
        {
            var container = parameter as ElementModelContainer;
            if (container != null)
            {
                container.RaiseEvent(new PropertiesRoutedEventArgs(ElementModelContainer.ShowPropertiesEvent));
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
