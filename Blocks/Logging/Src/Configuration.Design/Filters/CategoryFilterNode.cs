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

using System;
using System.Drawing.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a <see cref="CategoryFilterData"/> configuration element.
    /// </summary>
    public sealed class CategoryFilterNode : LogFilterNode
    {
		private CategoryFilterSettings categoryFilterSettings;		

        /// <summary>
        /// Initialize a new instance of the <see cref="CategoryFilterNode"/> class.
        /// </summary>
        public CategoryFilterNode()
            : this(new CategoryFilterData(Resources.CategoryLogFilterNode, new NamedElementCollection<CategoryFilterEntry>(), CategoryFilterMode.DenyAllExceptAllowed))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CategoryFilterNode"/> class with a <see cref="CategoryFilterData"/> instance.
		/// </summary>
		/// <param name="categoryLogFilterData">A <see cref="CategoryFilterData"/> instance</param>
        public CategoryFilterNode(CategoryFilterData categoryLogFilterData)
            : base(null == categoryLogFilterData ? string.Empty : categoryLogFilterData.Name)
        {
			if (null == categoryLogFilterData) throw new ArgumentNullException("categoryLogFilterData");

			categoryFilterSettings = new CategoryFilterSettings(categoryLogFilterData.CategoryFilterMode, new NamedElementCollection<CategoryFilterEntry>());
			foreach (CategoryFilterEntry filterEntry in categoryLogFilterData.CategoryFilters)
			{
				this.categoryFilterSettings.CategoryFilters.Add(filterEntry);
			}            
        }
    
        /// <summary>
        /// Gets the category filter expression.
        /// </summary>
		/// <value>
		/// The category filter expression.
		/// </value>
        [Editor(typeof(CategoryFilterEditor), typeof(UITypeEditor))]
        [SRDescription("CategoryFilterExpressionDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public CategoryFilterSettings CategoryFilterExpression
        {
            get { return new CategoryFilterSettings(categoryFilterSettings.CategoryFilterMode, categoryFilterSettings.CategoryFilters); }
            set
            {
                categoryFilterSettings.CategoryFilters.Clear();
                foreach (CategoryFilterEntry filterEntry in value.CategoryFilters)
                {
                    categoryFilterSettings.CategoryFilters.Add(filterEntry);
                }
                categoryFilterSettings.CategoryFilterMode = value.CategoryFilterMode;
            }
        }

		/// <summary>
		/// Gets the <see cref="CategoryFilterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="CategoryFilterData"/> this node represents.
		/// </value>
		public override LogFilterData LogFilterData
		{
			get 
			{ 
				NamedElementCollection<CategoryFilterEntry> entries = new NamedElementCollection<CategoryFilterEntry>();
				foreach (CategoryFilterEntry filterEntry in categoryFilterSettings.CategoryFilters)
				{
					entries.Add(filterEntry);
				}
				return new CategoryFilterData(Name, entries ,categoryFilterSettings.CategoryFilterMode); 
			}
		}

    }
}
