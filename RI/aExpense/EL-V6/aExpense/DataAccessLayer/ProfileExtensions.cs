#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Web.Profile;

namespace AExpense.DataAccessLayer
{
    public static class ProfileExtensions
    {
        public static T GetProperty<T>(this ProfileBase profile, string property)
        {
            if (profile == null) return default(T);

            object value = profile.GetPropertyValue(property);

            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }
    }
}