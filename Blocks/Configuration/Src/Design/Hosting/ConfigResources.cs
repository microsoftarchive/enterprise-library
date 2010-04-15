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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// Provides depenency properties to merge dictionaries into <see cref="FrameworkElement"/> from
    /// the <see cref="GlobalResources"/> dictionary.
    /// </summary>
    public static class ConfigurationResources
    {
        ///<summary>
        /// Manages a semi-colon separated list of merged dictionaries to merge into a <see cref="FrameworkElement"/>
        ///</summary>
        public static readonly DependencyProperty MergedDictionariesProperty =
            DependencyProperty.RegisterAttached("MergedDictionaries",
                                                typeof(string),
                                                typeof(ConfigurationResources),
                                                new FrameworkPropertyMetadata(null,
                                                                              new PropertyChangedCallback(
                                                                                  MergedDictionaryPropertyChanged)));



        /// <summary>
        /// Retrieves the semi-colon separated list of resource keys that are
        /// merged into the <paramref name="targetObject"/>.  Each entry separated
        /// by a semi-colon maps to an entry in the <see cref="GlobalResources"/>
        /// dictionary.
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static string GetMergedDictionaries(FrameworkElement targetObject)
        {
            Guard.ArgumentNotNull(targetObject, "targetObject");

            return (string)targetObject.GetValue(MergedDictionariesProperty);
        }

        ///<summary>
        /// Sets the semi-colon separated list of resource keys used to 
        /// locate <see cref="ResourceDictionary"/> items from the <see cref="GlobalResources"/>
        /// to merge into the <paramref name="targetObject"/>s resources.
        ///</summary>
        ///<param name="targetObject"></param>
        ///<param name="dictionaryNames"></param>
        ///<example>
        ///<![CDATA[
        /// FrameworkElement frameworkElement = new FrameworkElement();
        /// ConfigurationResources.SetMergedDictionaries(frameworkElement,"DictionaryKeyOne;DictionaryKeyTwo");
        /// ]]>
        ///</example>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetMergedDictionaries(FrameworkElement targetObject, string dictionaryNames)
        {
            Guard.ArgumentNotNull(targetObject, "targetObject");

            targetObject.SetValue(MergedDictionariesProperty, dictionaryNames);
        }

        private static void MergedDictionaryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = d as FrameworkElement;
            if (frameworkElement == null) throw new ArgumentException(Resources.MergedDictionariesTargetTypeError, "d");

            RemoveOldValues(frameworkElement, e.OldValue);
            AddNewValues(frameworkElement, e.NewValue);
        }

        private static void AddNewValues(FrameworkElement frameworkElement, object newValue)
        {
            if (newValue == null) return;

            var newDictionaries = newValue.ToString().Split(';');
            foreach (var dictionaryName in newDictionaries)
            {
                var dictionary = GlobalResources.Get(dictionaryName.Trim());
                frameworkElement.Resources.MergedDictionaries.Add(dictionary);
            }
        }

        private static void RemoveOldValues(FrameworkElement frameworkElement, object oldValue)
        {
            if (oldValue == null) return;

            var oldDictionaries = oldValue.ToString().Split(';');
            foreach (var dictionaryName in oldDictionaries)
            {
                var dictionary = GlobalResources.Get(dictionaryName.Trim());
                frameworkElement.Resources.MergedDictionaries.Remove(dictionary);
            }
        }
    }
}
