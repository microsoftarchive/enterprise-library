//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	/// <summary>
	/// Represents a client-side log filter based on message category.  
	/// Either specific categories can be explicitly allowed, or specific categories can be denied.
	/// </summary>
	[ConfigurationElementType(typeof(CategoryFilterData))]
	public class CategoryFilter : LogFilter
	{
		private ICollection<string> categoryFilters;
		private CategoryFilterMode categoryFilterMode;

		/// <summary>
		/// Initializes a new instance with configuration data.
		/// </summary>
		/// <param name="name">Name of this category filter.</param>
		/// <param name="categoryFilters">Categories to be included in the filter.</param>
		/// <param name="categoryFilterMode"><see cref="CategoryFilterMode"/> used to include or exclude category filters.</param>
		public CategoryFilter(string name, ICollection<string> categoryFilters, CategoryFilterMode categoryFilterMode)
			: base(name)
		{
			this.categoryFilters = categoryFilters;
			this.categoryFilterMode = categoryFilterMode;
		}

		/// <summary>
		/// Tests a log entry against the category filters.
		/// </summary>
		/// <param name="log">Log entry to test.</param>
		/// <returns><b>true</b> if the message passes through the filter and should be logged, <b>false</b> otherwise.</returns>
		public override bool Filter(LogEntry log)
		{
			return ShouldLog(log.Categories);
		}

		/// <summary>
		/// Tests a set of categories against the category filters.
		/// </summary>
		/// <param name="categories">The set of categories.</param>
		/// <returns><b>true</b> if the message passes through the filter and should be logged, <b>false</b> otherwise.</returns>
		public bool ShouldLog(IEnumerable<string> categories)
		{
			bool matchDetected = false;

			foreach (string category in categories)
			{
				if (this.CategoryFilters.Contains(category))
				{
					matchDetected = true;
					break;
				}
			}

			return ((CategoryFilterMode.AllowAllExceptDenied == this.CategoryFilterMode) && !matchDetected)
				|| ((CategoryFilterMode.DenyAllExceptAllowed == this.CategoryFilterMode) && matchDetected);
		}

		/// <summary>
		/// Test a category against the category filters.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns><b>true</b> if the category passes through the category filter, <b>false</b> otherwise.</returns>
		/// <remarks>A log entry for an allowed category may be rejected if the log entry has other denied categories
		/// in its categories liset.</remarks>
		public bool ShouldLog(string category)
		{
			return ShouldLog(new string[] { category });
		}

		/// <summary>
		/// Gets the category names to filter.
		/// </summary>
		public ICollection<string> CategoryFilters
		{
			get { return categoryFilters; }
		}

		/// <summary>
		/// Gets the <see cref="CategoryFilterMode"/> to use for filtering.
		/// </summary>
		public CategoryFilterMode CategoryFilterMode
		{
			get { return categoryFilterMode; }
			set { categoryFilterMode = value; }
		}
	}
}