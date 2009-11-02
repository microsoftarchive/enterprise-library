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

using Console.Wpf.ViewModel;
using System.Windows.Automation.Peers;

namespace Console.Wpf.Controls
{
	[TemplatePart(Name="PART_Adorner",Type=typeof(Border))]
	public class ElementModelContainer : ContentControl
	{

		public static TogglePropertiesCommand ToggleProperties { get;set; }
		
		#region Custom Dependency Properties
		private static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(Boolean), typeof(ElementModelContainer), new PropertyMetadata(false));
		public Boolean IsExpanded
		{
			get { return (Boolean)GetValue(IsExpandedProperty); }
			set { SetValue(IsExpandedProperty, value); }
		}
		#endregion

		public static readonly RoutedEvent ShowPropertiesEvent = EventManager.RegisterRoutedEvent(
		"ShowProperties", RoutingStrategy.Bubble, typeof(PropertiesRoutedEventHandler), typeof(ElementModelContainer));
		internal event PropertiesRoutedEventHandler ShowProperties;
        
		static ElementModelContainer()
		{
				DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementModelContainer),
				                                         new FrameworkPropertyMetadata(typeof(ElementModelContainer)));

		}

		public ElementModelContainer()
		{
			ToggleProperties = new TogglePropertiesCommand();
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

        protected override string GetNameCore()
        {
            var element = control.Content as ElementViewModel;
            return element != null ? element.Name : control.Name;
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
