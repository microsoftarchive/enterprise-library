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
using System.Windows.Controls;
using System.Windows.Input;

using Console.Wpf.ViewModel;

namespace Console.Wpf.Controls
{
	[TemplatePart(Name="PART_HelpTextDisplay",Type=typeof(TextBlock))]
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

		#region Custom Attached Properties
		public static readonly DependencyProperty IsAdderButtonProperty =
			DependencyProperty.RegisterAttached("IsAdderButton",
			                                    typeof(Boolean),
												typeof(ElementModelContainer),
			                                    new PropertyMetadata(false, OnIsAdderButtonChanged));

		public static void SetIsAdderButton(UIElement element, Boolean value)
		{
			element.SetValue(IsAdderButtonProperty, value);
		}

		public static Boolean GetIsAdderButton(UIElement element)
		{
			return (Boolean)element.GetValue(IsAdderButtonProperty);
		}

		private static void OnIsAdderButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Boolean isAdderButton;
			Boolean.TryParse(e.NewValue.ToString(), out isAdderButton);
			var button = d as RadioButton;
			if (isAdderButton)
			{
				if (button != null)
				{
					button.Checked += AdderButtonChecked;
				}
			}
			else
			{
				if (button != null)
				{
					button.Checked -= AdderButtonChecked;
				}
			}
		}

		private static void AdderButtonChecked(object sender, RoutedEventArgs e)
		{
			var button = e.OriginalSource as RadioButton;
			if (button != null && button.IsChecked == true)
			{
				button.IsChecked = false;
			}
		}

		public static readonly DependencyProperty IsHelpTextProviderProperty =
			DependencyProperty.RegisterAttached("IsHelpTextProvider",
												typeof(Boolean),
												typeof(ElementModelContainer),
												new PropertyMetadata(false, OnHelpTextProviderChanged));

		public static void SetIsHelpTextProvider(UIElement element, Boolean value)
		{
			element.SetValue(IsHelpTextProviderProperty, value);
		}

		public static Boolean GetIsHelpTextProvider(UIElement element)
		{
			return (Boolean)element.GetValue(IsHelpTextProviderProperty);
		}

		private static void OnHelpTextProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Boolean isHelpTextProvider;
			Boolean.TryParse(e.NewValue.ToString(), out isHelpTextProvider);
			var item = d as ListBoxItem;
			if (isHelpTextProvider)
			{
				if (item != null)
				{
					item.MouseEnter += ItemMouseEnter;
					item.GotFocus += ItemGotFocus;
				}
			}
			else
			{
				if (item != null)
				{
					item.MouseEnter -= ItemMouseEnter;
					item.GotFocus -= ItemGotFocus;
				}
			}
		}

		#endregion

		static void ItemGotFocus(object sender, RoutedEventArgs e)
		{
			RaiseShowHelpTextEvent(sender);
		}

		static void ItemMouseEnter(object sender, MouseEventArgs e)
		{
			RaiseShowHelpTextEvent(sender);
		}
		
		private static void RaiseShowHelpTextEvent(object sender)
		{
			var item = sender as ListBoxItem;
			if (item != null)
			{
				var adder = item.DataContext as ElementCollectionViewModelAdder;
				if (adder != null)
				{
					//Raise the ShowHelpTextEvent
					item.RaiseEvent(new AdderRoutedEventArgs(ShowHelpTextEvent)
					                	{
					                		Adder = adder
					                	});
				}
			}
		}

		public void ActivateHelpTextUI(ElementCollectionViewModelAdder provider)
		{
			//Find the help display text block and update it's text and it's visibility
			var helpText = Template.FindName("PART_HelpTextDisplay", this) as TextBlock;
			if (helpText != null)
			{
				if ((provider != null) && (!String.IsNullOrEmpty(provider.HelpText)))
				{
					helpText.Text = provider.HelpText;
					helpText.Visibility = Visibility.Visible;
				}
				else
				{
					helpText.Text = String.Empty;
					helpText.Visibility = Visibility.Collapsed;
				}
			}
		}
		
		public static readonly RoutedEvent ShowHelpTextEvent = EventManager.RegisterRoutedEvent(
		"ShowHelpText", RoutingStrategy.Bubble, typeof(AdderRoutedEventHandler), typeof(ElementModelContainer));
		internal event AdderRoutedEventHandler ShowHelpText;

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
			AddHandler(ShowHelpTextEvent, new AdderRoutedEventHandler(ElementModelContainerShowHelpText));
			ToggleProperties = new TogglePropertiesCommand();
		}

		void ElementModelContainerShowHelpText(object sender, AdderRoutedEventArgs e)
		{
			ActivateHelpTextUI(e.Adder);
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

	internal delegate void AdderRoutedEventHandler(object sender, AdderRoutedEventArgs args);
	internal class AdderRoutedEventArgs : RoutedEventArgs
	{
		public ElementCollectionViewModelAdder Adder { get; set; }

		public AdderRoutedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
		{
			
		}
	}
}
