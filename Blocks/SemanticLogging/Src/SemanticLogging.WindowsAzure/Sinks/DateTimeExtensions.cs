#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks.WindowsAzure
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Generates a PartitionKey representation based on the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The DateTime.</param>
        /// <returns>A string representing the Partition Key.</returns>
        public static string GeneratePartitionKey(this DateTime dateTime)
        {
            dateTime = dateTime.AddMinutes(-1D);
            var pk = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

            return GetTicks(pk);
        }

        /// <summary>
        /// Generates a PartitionKey representation (in reverse order) based on the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The DateTime.</param>
        /// <returns>A string representing the Partition Key (in reverse order).</returns>
        public static string GeneratePartitionKeyReversed(this DateTime dateTime)
        {
            dateTime = dateTime.AddMinutes(-1D);
            var pk = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

            return GetTicksReversed(pk);
        }

        internal static string GetTicksReversed(this DateTime dateTime)
        {
            return (DateTime.MaxValue - dateTime.ToUniversalTime()).Ticks.ToString("d19", CultureInfo.InvariantCulture);
        }

        internal static string GetTicks(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().Ticks.ToString("d19", CultureInfo.InvariantCulture);
        }
    }
}
