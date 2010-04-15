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
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions
{
    /// <summary>
    /// Provides extensions for selecting and filtering from <see cref="IEnumerable{T}"/> while
    /// catching a fixed set of exception and logs those exceptions using <see cref="ConfigurationLogWriter"/>.
    /// </summary>
    /// <remarks>
    /// This monitors for a fixed set of exceptions and logs those exceptions using <see cref="ConfigurationLogWriter.LogWarning"/>.
    /// 
    /// The set of exceptions currently monitore are:
    /// <list>
    /// <item><see cref="FileLoadException"/></item>
    /// <item><see cref="FileNotFoundException"/></item>
    /// <item><see cref="TypeLoadException"/></item>
    /// <item><see cref="ReflectionTypeLoadException"/></item>
    /// </list>
    /// </remarks>
    public static class FilteredSafeExtensions
    {
        private static readonly Type[] FilterTypeList = new[] {typeof (FileLoadException), typeof (FileNotFoundException), typeof(TypeLoadException), typeof(ReflectionTypeLoadException)};

        /// <summary>
        /// Safely selects items from <see cref="IEnumerable{T}"/> and runs exceptions through the <see cref="FilterException"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> FilterSelectSafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.SelectSafe(selector, FilterException);
        }

        /// <summary>
        /// Safely selects many items from <see cref="IEnumerable{T}"/> and runs exceptions through the <see cref="FilterException"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> FilterSelectManySafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.SelectManySafe(selector, FilterException);
        }


        /// <summary>
        /// Filters an exception by comparing it to an internal type list.  If the exception type matches one in the list, it logs a warning.
        /// Exceptions not matching the list are rethrown.
        /// </summary>
        /// <param name="ex"></param>
        /// <seealso cref="ConfigurationLogWriter.LogWarning"/>
        public static void FilterException(Exception ex)
        {
            if (FilterTypeList.Contains(ex.GetType()))
            {
                ConfigurationLogWriter.LogWarning(
                    string.Format(CultureInfo.CurrentCulture, Resources.WarningAssemblyLoadNonFatal, ex.ToString()));
            }
            else
            {
                throw ex;
            }
        }

    }
}
