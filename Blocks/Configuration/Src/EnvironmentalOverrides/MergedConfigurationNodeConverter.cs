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
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
	/// <summary>
	/// Represents a TypeConverter that be used to modify a <see cref="MergedConfigurationNode"/> instance from within the property grid.
	/// </summary>
	public class MergedConfigurationNodeConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to. </param>
		/// <returns>Always <see langword="true"/>.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		/// <summary>
		/// Converts an instance of <see cref="MergedConfigurationNode"/> to the type specified in <paramref name="destinationType"/>.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="System.Globalization.CultureInfo"/>. If <see langword="null"/> is passed, the current culture is assumed.</param>
		/// <param name="value">The instance of <see cref="MergedConfigurationNode"/> that should be converted.</param>
		/// <param name="destinationType">The type to which the <paramref name="value"/> should be converted.</param>
		/// <returns>An instance of <paramref name="destinationType"/> if converstion succeeds, otherwise <see langword="null"/>.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value is string)
				{
					return value;
				}
				MergedConfigurationNode mergedConfigurationNode = value as MergedConfigurationNode;
				if (mergedConfigurationNode != null)
				{
					if (mergedConfigurationNode.MergeData.OverrideProperties)
					{
						return Resources.OverrideProperties;
					}
					else
					{
						return Resources.DontOverrideProperties;
					}
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
		/// <returns>Always <see langword="true"/>.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
		}

		/// <summary>
		/// Converts an arbitrary instance of <see cref="System.Object"/> to an instance of <see cref="MergedConfigurationNode"/>.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="System.Globalization.CultureInfo"/>. If <see langword="null"/> is passed, the current culture is assumed.</param>
		/// <param name="value">The instance of <see cref="System.Object"/> that should be used for conversion.</param>
		/// <returns>If conversion succeeds, an instance of <see cref="MergedConfigurationNode"/>, otherwise <see langword="null"/>.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value != null)
			{
				string overrideString = value.ToString();
				MergedConfigurationNode oldMergedConfigurationNode = context.PropertyDescriptor.GetValue(context.Instance) as MergedConfigurationNode;
				ConfigurationNodeMergeData oldMergeData = oldMergedConfigurationNode.MergeData;

				bool overrideProperties = (overrideString == Resources.OverrideProperties);

				ConfigurationNodeMergeData newMergeData = new ConfigurationNodeMergeData(overrideProperties, oldMergeData);
				return new MergedConfigurationNode(newMergeData, oldMergedConfigurationNode);
			}
			return base.ConvertFrom(context, culture, value);

		}

		/// <summary>
		/// Returns whether the collection of standard values returned from <see cref="MergedConfigurationNodeConverter.GetStandardValues(ITypeDescriptorContext)"/> is an exclusive list.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>Always <see langword="true"/>.</returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>Always <see langword="true"/>.</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		/// <summary>
		/// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>A <see cref="TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ArrayList configurationNodeValues = new ArrayList();

			configurationNodeValues.Add(ConvertFromInvariantString(context, Resources.OverrideProperties));
			configurationNodeValues.Add(ConvertFromInvariantString(context, Resources.DontOverrideProperties));

			return new StandardValuesCollection(configurationNodeValues);
		}

		/// <summary>
		/// Returns whether this object supports properties, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>Always <see langword="true"/>.</returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="value">An <see cref="Object"/> that specifies the type of array for which to get properties.</param>
		/// <param name="attributes">An array of type <see cref="Attribute"/> that is used as a filter.</param>
		/// <returns>A <see cref="PropertyDescriptorCollection"/> with the properties that are exposed for this data type, or <see langword="null"/> if there are no properties.</returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			MergedConfigurationNode mergedConfigurationNode = value as MergedConfigurationNode;
			if (mergedConfigurationNode != null)
			{
				return TypeDescriptor.GetProperties(mergedConfigurationNode, attributes);
			}

			return base.GetProperties(context, value, attributes);
		}
	}
}
