//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// An implementation of <see cref="IRowMapper{TResult}"/> that uses reflection to convert data rows to <typeparamref name="TResult"/>.
    /// Instances of this class can be build using the <see cref="MapBuilder{TResult}"/> API.
    /// </summary>
    /// <typeparam name="TResult">The type this <see cref="IRowMapper{TResult}"/> applies to</typeparam>
    /// <seealso cref="MapBuilder{TResult}"/>
    public class ReflectionRowMapper<TResult> : IRowMapper<TResult>
        where TResult : new()
    {
        readonly Dictionary<PropertyInfo, PropertyMapping> propertyMappings;
        
        /// <summary>
        /// Creates a new instance of <see cref="ReflectionRowMapper{TResult}"/>.
        /// </summary>
        /// <param name="propertyMappings">The <see cref="PropertyMapping"/>'s that specify how each property should be mapped.</param>
        public ReflectionRowMapper(Dictionary<PropertyInfo, PropertyMapping> propertyMappings)
        {
            if (propertyMappings == null) throw new ArgumentNullException("propertyMappings");

            this.propertyMappings = propertyMappings;
        }

        /// <summary>Given a record from a data reader, map the contents to a common language runtime object.</summary>
        /// <param name="row">The input data from the database.</param>
        /// <returns>The mapped object.</returns>
        public TResult MapRow(IDataRecord row) 
        {
            TResult result = new TResult();
            foreach (PropertyMapping propertyMapping in GetPropertyMappings())
            {
                propertyMapping.Map(result, row);
            }
            return result;
        }

        /// <summary>
        /// Returns the list of <see cref="PropertyMapping"/>s that this <see cref="ReflectionRowMapper{TResult}"/> was initialized with.
        /// </summary>
        /// <returns>The list of </returns>
        public IEnumerable<PropertyMapping> GetPropertyMappings()
        {
            return propertyMappings.Values;
        }
    }

    /// <summary>
    /// Base class for mapping values to properties by the <see cref="ReflectionRowMapper{TResult}"/>.
    /// </summary>
    /// <seealso cref="ColumnNameMapping"/>
    /// <seealso cref="IgnoreMapping"/>
    /// <seealso cref="FuncMapping"/>
    public abstract class PropertyMapping
    {
        /// <summary>
        /// Initializes a new <see cref="PropertyMapping"/>.
        /// </summary>
        protected PropertyMapping(PropertyInfo property)
        {
            Property = property;
        }

        /// <summary>
        /// Gets the property that will be mapped to.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// When implemented by a class, performs the actual mapping from <paramref name="row"/> to <paramref name="instance"/>.
        /// </summary>
        public abstract void Map(object instance, IDataRecord row);

        /// <summary>
        /// Sets the <paramref name="value"/> to <paramref name="instance"/> using <see cref="PropertyMapping.Property"/>.
        /// </summary>
        /// <param name="instance">The object <paramref name="value"/> will be assigned to.</param>
        /// <param name="value">The value that will be assigned to <paramref name="instance"/>.</param>
        protected void SetValue(object instance, object value)
        {
            Property.SetValue(instance, value, new object[0]);
        }

        /// <summary>
        /// Converts the database value <paramref name="value"/> to <paramref name="conversionType"/>.
        /// </summary>
        protected static object ConvertValue(object value, Type conversionType)
        {
            object convertedValue = null;

            if (value != DBNull.Value)
            {
                convertedValue = Convert.ChangeType(value, conversionType);
            }
            return convertedValue;
        }
    }

    /// <summary>
    /// Represents the mapping from a database column to a <see cref="PropertyInfo"/>.
    /// </summary>
    public class ColumnNameMapping : PropertyMapping
    {
        /// <summary>
        /// Creates a new instance of <see cref="ColumnNameMapping"/>
        /// </summary>
        /// <param name="columnName">The name of the column that will be used for mapping.</param>
        /// <param name="property">The property that will be used to map to.</param>
        public ColumnNameMapping(PropertyInfo property, string columnName)
            :base(property)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Gets the name of the column that is used for mapping.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Performs the actual mapping from column to property.
        /// </summary>
        /// <param name="instance">The object that contains the <see cref="PropertyMapping.Property"/>.</param>
        /// <param name="row">The row that contains the <see cref="ColumnNameMapping.ColumnName"/>.</param>
        public override void Map(object instance, IDataRecord row)
        {
            object value;
            try
            {
                value = row[ColumnName];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ExceptionColumnNotFoundWhileMapping, ColumnName));
            }

            object convertedValue;
            try
            {
                convertedValue = ConvertValue(value, Property.PropertyType);
            }
            catch (InvalidCastException castException)
            {
                string exceptionMessage = string.Format(Resources.Culture, Resources.ExceptionConvertionFailedWhenMappingPropertyToColumn, ColumnName, Property.Name, Property.PropertyType);
                throw new InvalidCastException(exceptionMessage, castException);
            }
            catch (FormatException formatException)
            {
                string exceptionMessage = string.Format(Resources.Culture, Resources.ExceptionConvertionFailedWhenMappingPropertyToColumn, ColumnName, Property.Name, Property.PropertyType);
                throw new InvalidCastException(exceptionMessage, formatException);
            }

            SetValue(instance, convertedValue);
        }

    }

    /// <summary>
    /// Represents a property that will be ignored when mapping.
    /// </summary>
    public class IgnoreMapping : PropertyMapping
    {
        /// <summary>
        /// Creates a new instance of <see cref="IgnoreMapping"/>.
        /// </summary>
        /// <param name="property">The property that will be ignored.</param>
        public IgnoreMapping(PropertyInfo property)
            :base(property)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Map(object instance, IDataRecord row)
        {
            //nop
        }
    }

    /// <summary>
    /// Represents a property that will be assigned the value of a user specified function when mapping.
    /// </summary>
    public class FuncMapping : PropertyMapping
    {
        /// <summary>
        /// Creates a new instance of <see cref="FuncMapping"/>
        /// </summary>
        /// <param name="func">The func that will be used to map the property.</param>
        /// <param name="property">The property that will be used to map to.</param>
        public FuncMapping(PropertyInfo property, Func<IDataRecord, object> func)
            :base(property)
        {
            Func = func;
        }

        /// <summary>
        /// Gets the function that will be used to map the properties value.
        /// </summary>
        public Func<IDataRecord, object> Func { get; private set; }

        /// <summary>
        /// Performs the actual mapping from <see cref="IDataRecord"/> to property.
        /// </summary>
        /// <param name="instance">The object that contains the <see cref="PropertyMapping.Property"/>.</param>
        /// <param name="row">The row that will be used as input for <see cref="FuncMapping.Func"/>.</param>
        public override void Map(object instance, IDataRecord row)
        {
            object value = Func(row);

            SetValue(instance, value);
        }
    }
    

}
