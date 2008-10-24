//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Represents the logic to access values from a method.
	/// </summary>
	/// <seealso cref="ValueAccess"/>
	internal sealed class MethodValueAccess : ValueAccess
	{
		private MethodInfo methodInfo;

		public MethodValueAccess(MethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;
		}

		public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
		{
			value = null;
			valueAccessFailureMessage = null;

			if (null == source)
			{
				valueAccessFailureMessage 
					= string.Format(
						CultureInfo.CurrentCulture,
						Resources.ErrorValueAccessNull,
						this.Key);
				return false;
			}
			if (!this.methodInfo.DeclaringType.IsAssignableFrom(source.GetType()))
			{
				valueAccessFailureMessage 
					= string.Format(
						CultureInfo.CurrentCulture,
						Resources.ErrorValueAccessInvalidType,
						this.Key,
						source.GetType().FullName);
				return false;
			}
			
			value = this.methodInfo.Invoke(source, null);
			return true;
		}

		public override string Key
		{
			get { return this.methodInfo.Name; }
		}

		#region test only properties

		internal MethodInfo MethodInfo
		{
			get { return this.methodInfo; }
		}

		#endregion
	}
}
