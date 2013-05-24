#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    internal static class Extensions
    {
        public static T[] Add<T>(this T[] array, T value)
        {
            T[] newArray = new T[array.Length + 1];

            Array.Copy(array, newArray, array.Length);
            newArray[array.Length] = value;

            return newArray;
        }

        public static T[] AddRange<T>(this T[] array, params T[] values)
        {
            T[] newArray = new T[array.Length + values.Length];

            Array.Copy(array, newArray, array.Length);
            Array.Copy(values, 0, newArray, array.Length, values.Length);

            return newArray;
        }

        public static bool ApproximatelyGreaterThan(this double thisValue, double otherValue, double delta)
        {
            return thisValue >= otherValue - delta;
        }
    }
}
