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
using System.Globalization;
using System.Windows.Data;

namespace Console.Wpf.Converters
{
	public class ZIndexToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Boolean expanded;
			Boolean result = Boolean.TryParse(value.ToString(), out expanded);

			return result && expanded ? 99 : 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Boolean expanded;
			Boolean result = Boolean.TryParse(value.ToString(), out expanded);

			return result && expanded ? 99 : 0;
		}
	}
}
