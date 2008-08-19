//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Performs validation on a collection of <see cref="Match"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class MatchDataAttribute : ValidationAttribute
	{
		/// <summary>
		/// Validate the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
		/// </summary>
		/// <param name="instance">
		/// The instance to validate.
		/// </param>
		/// <param name="propertyInfo">
		/// The property containing the value to validate.
		/// </param>
		/// <param name="errors">
		/// The collection to add any errors that occur during the validation.
		/// </param>
		/// <remarks>
		/// The value of property <paramref name="propertyInfo"/> on <paramref name="instance"/> must be an <see cref="IEnumerable{T}"/> 
		/// of <see cref="Match"/>, it must contain at least one element, each element must contain a value and 
		/// the values cannot be repeated among elements.
		/// </remarks>
		protected override void ValidateCore(object instance, System.Reflection.PropertyInfo propertyInfo, IList<ValidationError> errors)
		{
			List<string> matchValues = new List<string>();
			bool containsAtleastOne = false;
			object propertyValue = propertyInfo.GetValue(instance, null);
			IEnumerable<Match> listOfMatchData = propertyValue as IEnumerable<Match>;
			if (listOfMatchData != null)
			{
				foreach (Match matchData in listOfMatchData)
				{
					containsAtleastOne = true;
					if (string.IsNullOrEmpty(matchData.Value))
					{
						errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.MatchDataContainsEmptyMatch));
						break;
					}
					else if (matchValues.Contains(matchData.Value))
					{
						errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.MatchDataContainsDuplicateMatch));
						break;
					}
					else
					{
						matchValues.Add(matchData.Value);
					}
				}
			}
			if (!containsAtleastOne)
			{
				errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.MatchDataCollectionEmpty));
			}
		}
	}
}
