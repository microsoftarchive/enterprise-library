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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	/// <summary>
	/// Provides client-side log filtering based on category and priority.  Each filter
	/// implements the ILogFilter interface and is registered in this class.
	/// Category filtering is done using a <see cref="CategoryFilter"/> and priority filtering
	/// is done using a <see cref="PriorityFilter"/>.
	/// </summary>
	public class LogFilterHelper
	{
		private ICollection<ILogFilter> filters;
		private ILogFilterErrorHandler handler;

		/// <summary>
		/// Initialize a new instance of a <see cref="LogFilterHelper"/> class.  Registers each ILogFilter.
		/// </summary>
		/// <param name="filters">The instances of <see cref="ILogFilter"/> to aggregate.</param>
		/// <param name="handler">The handler to deal with errors during filter checking.</param>
		public LogFilterHelper(ICollection<ILogFilter> filters, ILogFilterErrorHandler handler)
		{
			this.filters = filters;
			this.handler = handler;
		}

		/// <summary>
		/// Tests the log message against the registered filters.
		/// </summary>
		/// <param name="log">Log entry message.</param>
		/// <returns>Return <b>true</b> if the message passes through all of the filters.</returns>
		public bool CheckFilters(LogEntry log)
		{
			bool passFilters = true;

			foreach (ILogFilter filter in this.filters)
			{
				try
				{
					bool passed = filter.Filter(log);
					passFilters &= passed;
					if (!passFilters)
					{
						break;
					}
				}
				catch (Exception ex)
				{
					if (!this.handler.FilterCheckingFailed(ex, log, filter))
					{
						return false;
					}
				}
			}

			return passFilters;
		}

		/// <summary>
		/// Gets the first filter of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the filter to get.</typeparam>
		/// <returns>The first filter of type <typeparamref name="T"/>, 
		/// or <see langword="null"/> if there is no such filter.</returns>
		public T GetFilter<T>()
			where T : class, ILogFilter
		{
			foreach (ILogFilter filter in this.filters)
			{
				if (filter is T)
				{
					return filter as T;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the filter of type <typeparamref name="T"/> named <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="T">The type of the filter to get.</typeparam>
		/// <param name="name">The name of the filter to get.</param>
		/// <returns>The filter of type <typeparamref name="T"/> named <paramref name="name"/>, 
		/// or <see langword="null"/> if there is no such filter</returns>
		public T GetFilter<T>(string name)
			where T : class, ILogFilter
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "name");

			foreach (ILogFilter filter in this.filters)
			{
				if (filter is T && name.Equals(filter.Name))
				{
					return filter as T;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the filter named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the filter to get.</param>
		/// <returns>The filter named <paramref name="name"/>, 
		/// or <see langword="null"/> if there is no such filter</returns>
		public ILogFilter GetFilter(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "name");

			foreach (ILogFilter filter in this.filters)
			{
				if (name.Equals(filter.Name))
				{
					return filter;
				}
			}
			return null;
		}
	}
}