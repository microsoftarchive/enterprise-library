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
using System.Windows.Media;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
	public static class VisualTreeWalker
	{

		public static FrameworkElement FindName(string name, DependencyObject reference)
		{
			return FindName<FrameworkElement>(name, reference);
		}

		public static T FindName<T>(string name, DependencyObject reference) where T : FrameworkElement
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}
			return FindNameInternal<T>(name, reference);
		}

		private static T FindNameInternal<T>(string name, DependencyObject reference) where T : FrameworkElement
		{
			foreach (DependencyObject obj in GetChildren(reference))
			{
				T element = obj as T;
				if (element != null && element.Name == name)
				{
					return element;
				}
				element = FindNameInternal<T>(name, obj);
				if (element != null)
				{
					return element;
				}
			}
			return null;
		}


		private static IEnumerable<DependencyObject> GetChildren(DependencyObject reference)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(reference);
			for (int i = 0; i < childCount; i++)
			{
				yield return VisualTreeHelper.GetChild(reference, i);
			}
		}
	}
}
