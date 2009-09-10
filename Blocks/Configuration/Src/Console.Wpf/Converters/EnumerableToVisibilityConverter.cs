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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Console.Wpf.ViewModel;

namespace Console.Wpf.Converters
{
	public class EnumerableToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var collection = value as IEnumerable;
			if (collection != null)
			{
				var enumerator = collection.GetEnumerator();
				if (enumerator.MoveNext())
				{
					var propertyModel = enumerator.Current as ElementProperty;
					return propertyModel != null && AllPropertiesAreHidden(collection) ? Visibility.Collapsed : Visibility.Visible;
				}
			}
			
			return Visibility.Collapsed;
		}

		private bool AllPropertiesAreHidden(IEnumerable propertyCollection)
		{
            var properties = propertyCollection as IEnumerable<ElementProperty>;
			if (properties != null)
			{
				foreach (var property in properties)
				{
					if (!property.Hidden)
						return false;
				}
				return true;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
