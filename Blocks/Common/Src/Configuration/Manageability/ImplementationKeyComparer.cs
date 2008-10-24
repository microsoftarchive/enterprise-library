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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// A comparer for an <see cref="ImplementationKey"/>.
    /// </summary>
	public class ImplementationKeyComparer : IEqualityComparer<ImplementationKey>
	{
        ///<summary>
        ///Determines whether the specified objects are equal.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified objects are equal; otherwise, false.
        ///</returns>
        ///
        ///<param name="y">The second object of type T to compare.</param>
        ///<param name="x">The first object of type T to compare.</param>
        public bool Equals(ImplementationKey x, ImplementationKey y)
		{
			if (x.FileName != y.FileName
				&& (x.FileName == null || !x.FileName.Equals(y.FileName, StringComparison.OrdinalIgnoreCase)))
				return false;
			if (x.ApplicationName != y.ApplicationName
				&& (x.ApplicationName == null || !x.ApplicationName.Equals(y.ApplicationName, StringComparison.OrdinalIgnoreCase)))
				return false;
			if (x.EnableGroupPolicies != y.EnableGroupPolicies)
				return false;

			return true;
		}

        ///<summary>
        ///Returns a hash code for the specified object.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the specified object.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
        ///<exception cref="T:System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
        public int GetHashCode(ImplementationKey obj)
		{
			return (obj.FileName == null ? 0 : obj.FileName.GetHashCode())
				^ (obj.ApplicationName == null ? 0 : obj.ApplicationName.GetHashCode())
				^ obj.EnableGroupPolicies.GetHashCode();
		}
	}
}
