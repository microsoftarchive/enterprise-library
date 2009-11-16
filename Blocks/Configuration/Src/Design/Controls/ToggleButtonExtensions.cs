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
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
	public class ToggleButtonExtensions : DependencyObject
	{
		public static Dictionary<ToggleButton, String> ElementToGroupNames = new Dictionary<ToggleButton, string>();
		public static readonly DependencyProperty GroupNameProperty =
			DependencyProperty.RegisterAttached("GroupName",
			                                    typeof(String),
			                                    typeof(ToggleButtonExtensions),
			                                    new PropertyMetadata(String.Empty, OnGroupNameChanged));

		public static void SetGroupName(ToggleButton element, String value)
		{
			element.SetValue(GroupNameProperty, value);
		}

		public static String GetGroupName(ToggleButton element)
		{
			return element.GetValue(GroupNameProperty).ToString();
		}

		private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//Add an entry to the group name collection
			var button = d as ToggleButton;
			if (button != null)
			{
				ElementToGroupNames.Add(button, e.NewValue.ToString());
				button.Checked += ThreeStateButtonChecked;
			}
		}


		static void ThreeStateButtonChecked(object sender, RoutedEventArgs e)
		{
			var toggleButton = e.OriginalSource as ToggleButton;
			foreach (var item in ElementToGroupNames)
			{
				if (item.Key != toggleButton && item.Value == GetGroupName(toggleButton))
				{
					item.Key.IsChecked = false;
				}

			}
		}

	}
}
