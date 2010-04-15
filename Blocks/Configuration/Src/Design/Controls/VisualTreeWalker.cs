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
    /// <summary>
    /// Utility class for walking the visual tree and locating visual elements relative to a reference visual element.
    /// </summary>
    internal static class VisualTreeWalker
    {
        public static T FindChild<T>(Func<T, bool> predicate, DependencyObject reference) where T : FrameworkElement
        {
            foreach (DependencyObject obj in GetChildren(reference))
            {
                T element = obj as T;
                if (element != null && predicate(element))
                {
                    return element;
                }
                element = FindChild<T>(predicate, obj);
                if (element != null)
                {
                    return element;
                }
            }
            return null;
        }

        public static FrameworkElement FindChild(string name, DependencyObject reference)
        {
            return FindChild<FrameworkElement>(name, reference);
        }

        public static T FindChild<T>(string name, DependencyObject reference) where T : FrameworkElement
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

        public static T TryFindParent<T>(this DependencyObject context)
            where T : DependencyObject
        {
            DependencyObject parent = GetParentObject(context);
            if (parent == null) return null;

            T castedParent = parent as T;
            if (castedParent != null)
            {
                return castedParent;
            }
                
            return TryFindParent<T>(parent);
        }

        public static DependencyObject GetParentObject(this DependencyObject context)
        {
            if (context == null) return null;

            ContentElement contextAsContentElement = context as ContentElement;
            if (contextAsContentElement != null)
            {
                DependencyObject parentOfContentElement = ContentOperations.GetParent(contextAsContentElement);
                if (parentOfContentElement != null) return parentOfContentElement;
            }

            FrameworkElement contextAsFrameworkElement = context as FrameworkElement;
            if (contextAsFrameworkElement != null)
            {
                DependencyObject parent = contextAsFrameworkElement.Parent;
                if (parent != null) return parent;
            }

            return VisualTreeHelper.GetParent(context);
        }
    }
}
