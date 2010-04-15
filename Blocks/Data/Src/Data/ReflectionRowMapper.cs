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
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.Unity.Utility;

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
        private static readonly MethodInfo ConvertValue =
            StaticReflection.GetMethodInfo<PropertyMapping>(pm => pm.GetPropertyValue(null));

        // The constructor with no parameters is guaranteed to exist because of the "new()" constraint on the 
        // TResult type parameter.
        private static readonly NewExpression CreationExpression = Expression.New(typeof(TResult));

        private readonly Func<IDataRecord, TResult> mapping;

        /// <summary>
        /// Creates a new instance of <see cref="ReflectionRowMapper{TResult}"/>.
        /// </summary>
        /// <param name="propertyMappings">The <see cref="PropertyMapping"/>'s that specify how each property should be mapped.</param>
        public ReflectionRowMapper(IDictionary<PropertyInfo, PropertyMapping> propertyMappings)
        {
            if (propertyMappings == null) throw new ArgumentNullException("propertyMappings");

            try
            {
                var parameter = Expression.Parameter(typeof(IDataRecord), "reader");
                var bindings =
                    propertyMappings.Select(kvp => (MemberBinding)
                        Expression.Bind(
                            kvp.Key,
                            Expression.Convert(
                                Expression.Call(Expression.Constant(kvp.Value), ConvertValue, new Expression[] { parameter }),
                                kvp.Key.PropertyType)));
                Expression<Func<IDataRecord, TResult>> expr =
                    Expression.Lambda<Func<IDataRecord, TResult>>(
                        Expression.MemberInit(
                            CreationExpression,
                            bindings),
                        new ParameterExpression[] { parameter });

                this.mapping = expr.Compile();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionCannotCreateRowMapping,
                        typeof(TResult).Name),
                    e);
            }
        }

        /// <summary>Given a record from a data reader, map the contents to a common language runtime object.</summary>
        /// <param name="row">The input data from the database.</param>
        /// <returns>The mapped object.</returns>
        public TResult MapRow(IDataRecord row)
        {
            return this.mapping(row);
        }
    }

    /// <summary>
    /// Base class for mapping values to properties by the <see cref="ReflectionRowMapper{TResult}"/>.
    /// </summary>
    /// <seealso cref="ColumnNameMapping"/>
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
        /// When implemented by a class, extracts the value for the mapped property from <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The data record.</param>
        /// <returns>The properly converted value.</returns>
        public abstract object GetPropertyValue(IDataRecord row);

        /// <summary>
        /// Performs the actual mapping from column to property.
        /// </summary>
        /// <param name="instance">The object that contains the <see cref="PropertyMapping.Property"/>.</param>
        /// <param name="row">The row that contains the <see cref="ColumnNameMapping.ColumnName"/>.</param>
        public void Map(object instance, IDataRecord row)
        {
            object convertedValue = GetPropertyValue(row);

            SetValue(instance, convertedValue);
        }

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
            if (IsNullableType(conversionType))
            {
                return ConvertNullableValue(value, conversionType);
            }
            return ConvertNonNullableValue(value, conversionType);
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Converts the database value <paramref name="value"/> to <paramref name="conversionType"/>,
        /// where <paramref name="conversionType"/> is a nullable value.
        /// </summary>
        /// <param name="value">Value from the database.</param>
        /// <param name="conversionType">Type to convert to.</param>
        /// <returns>The converted value.</returns>
        protected static object ConvertNullableValue(object value, Type conversionType)
        {
            if (value != DBNull.Value)
            {
                var converter = new NullableConverter(conversionType);
                return converter.ConvertFrom(value);
            }
            return null;
        }

        /// <summary>
        /// Converts the database value <paramref name="value"/> to <paramref name="conversionType"/>.
        /// Will throw an exception if <paramref name="conversionType"/> is a nullable value.
        /// </summary>
        /// <param name="value">Value from the database.</param>
        /// <param name="conversionType">Type to convert to.</param>
        /// <returns>The converted value.</returns>
        protected static object ConvertNonNullableValue(object value, Type conversionType)
        {
            object convertedValue = null;

            if (value != DBNull.Value)
            {
                convertedValue = Convert.ChangeType(value, conversionType);
            }
            else if (conversionType.IsValueType)
            {
                convertedValue = Activator.CreateInstance(conversionType);
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
            : base(property)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Gets the name of the column that is used for mapping.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Converts the value for the column in the <paramref name="row"/> with a name matching that of the 
        /// mapped property to the type of the property.
        /// </summary>
        /// <param name="row">The data record.</param>
        /// <returns>The value for the corresponding column converted to the type of the mapped property.</returns>
        public override object GetPropertyValue(IDataRecord row)
        {
            if (row == null) throw new ArgumentNullException("row");

            object value;
            try
            {
                value = row[ColumnName];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ExceptionColumnNotFoundWhileMapping, ColumnName),
                    ex);
            }

            object convertedValue;
            try
            {
                convertedValue = ConvertValue(value, Property.PropertyType);
            }
            catch (InvalidCastException castException)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionConvertionFailedWhenMappingPropertyToColumn,
                        ColumnName,
                        Property.Name,
                        Property.PropertyType);
                throw new InvalidCastException(exceptionMessage, castException);
            }
            catch (FormatException formatException)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionConvertionFailedWhenMappingPropertyToColumn,
                        ColumnName,
                        Property.Name,
                        Property.PropertyType);
                throw new InvalidCastException(exceptionMessage, formatException);
            }

            return convertedValue;
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
            : base(property)
        {
            Guard.ArgumentNotNull(func, "func");
            Func = func;
        }

        /// <summary>
        /// Gets the function that will be used to map the properties value.
        /// </summary>
        public Func<IDataRecord, object> Func { get; private set; }

        /// <summary>
        /// Gets the value for the mapped property from the <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The data record.</param>
        /// <returns>The value for the corresponding column converted to the type of the mapped property.</returns>
        public override object GetPropertyValue(IDataRecord row)
        {
            return Func(row);
        }
    }
}
