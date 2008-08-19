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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents a single category filter configuration settings.
	/// </summary>
	[Assembler(typeof(CategoryFilterAssembler))]
	[ContainerPolicyCreator(typeof(CategoryFilterPolicyCreator))]
	public class CategoryFilterData : LogFilterData
	{
		private const string categoryFilterModeProperty = "categoryFilterMode";
		private const string categoryFiltersProperty = "categoryFilters";

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
		/// </summary>
		public CategoryFilterData()
		{
		}

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
		/// </summary>
		/// <param name="categoryFilters">The collection of category names to filter.</param>
		/// <param name="categoryFilterMode">The mode of filtering.</param>
		public CategoryFilterData(NamedElementCollection<CategoryFilterEntry> categoryFilters, CategoryFilterMode categoryFilterMode)
			: this("category", categoryFilters, categoryFilterMode)
		{
		}

		/// <summary>
		/// <para>Initialize a new named instance of the <see cref="CategoryFilterData"/> class.</para>
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="categoryFilters">The collection of category names to filter.</param>
		/// <param name="categoryFilterMode">The mode of filtering.</param>
		public CategoryFilterData(string name, NamedElementCollection<CategoryFilterEntry> categoryFilters, CategoryFilterMode categoryFilterMode)
			: base(name, typeof(CategoryFilter))
		{
			this.CategoryFilters = categoryFilters;
			this.CategoryFilterMode = categoryFilterMode;
		}

		/// <summary>
		/// One of <see cref="CategoryFilterMode"/> enumeration.
		/// </summary>
		[ConfigurationProperty(categoryFilterModeProperty)]
		public CategoryFilterMode CategoryFilterMode
		{
			get
			{
				return (CategoryFilterMode)this[categoryFilterModeProperty];
			}
			set
			{
				this[categoryFilterModeProperty] = value;
			}
		}

		/// <summary>
		/// Collection of <see cref="CategoryFilterData"/>.
		/// </summary>
		[ConfigurationProperty(categoryFiltersProperty)]
		public NamedElementCollection<CategoryFilterEntry> CategoryFilters
		{
			get
			{
				return (NamedElementCollection<CategoryFilterEntry>)base[categoryFiltersProperty];
			}

			private set
			{
				base[categoryFiltersProperty] = value;
			}
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="CategoryFilter"/> described by a <see cref="CategoryFilterData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="CategoryFilterData"/> type and it is used by the <see cref="LogFilterCustomFactory"/> 
	/// to build the specific <see cref="ILogFilter"/> object represented by the configuration object.
	/// </remarks>
	public class CategoryFilterAssembler : IAssembler<ILogFilter, LogFilterData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="CategoryFilter"/> based on an instance of <see cref="CategoryFilterData"/>.
		/// </summary>
		/// <seealso cref="LogFilterCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="CategoryFilterData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="CategoryFilter"/>.</returns>
		public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CategoryFilterData castedObjectConfiguration = (CategoryFilterData)objectConfiguration;

			ICollection<string> categoryFilters = new List<string>();
			foreach (CategoryFilterEntry entry in castedObjectConfiguration.CategoryFilters)
			{
				categoryFilters.Add(entry.Name);
			}

			ILogFilter createdObject
				= new CategoryFilter(
					castedObjectConfiguration.Name,
					categoryFilters,
					castedObjectConfiguration.CategoryFilterMode);

			return createdObject;
		}
	}

}