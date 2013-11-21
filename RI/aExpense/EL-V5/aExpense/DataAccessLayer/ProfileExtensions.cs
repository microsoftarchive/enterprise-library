// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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