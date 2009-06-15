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

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ReflectionRowMapper<TResult> : IRowMapper<TResult>
        where TResult : new()
    {
        Dictionary<PropertyInfo, PropertyMapping> propertyMappings;

        internal ReflectionRowMapper(Dictionary<PropertyInfo, PropertyMapping> propertyMappings)
        {
            this.propertyMappings = propertyMappings;
        }

        /// <summary/>
        public TResult MapRow(System.Data.IDataRecord row) 
        {
            TResult result = new TResult();
            foreach (PropertyMapping propertyMapping in GetPropertyMappings())
            {
                propertyMapping.Map(result, row);
            }
            return result;
        }

        /// <summary/>
        public IEnumerable<PropertyMapping> GetPropertyMappings()
        {
            return propertyMappings.Values;
        }
    }

    /// <summary/>
    public abstract class PropertyMapping
    {
        /// <summary/>
        protected PropertyMapping(PropertyInfo property)
        {
            Property = property;
        }

        /// <summary/>
        public PropertyInfo Property { get; private set; }

        /// <summary/>
        public abstract void Map(object instance, IDataRecord row);

        /// <summary/>
        protected void SetValue(object instance, object value)
        {
            Property.SetValue(instance, value, new object[0]);
        }

        /// <summary/>
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

    /// <summary/>
    public class ColumnNameMapping : PropertyMapping
    {
        /// <summary/>
        public ColumnNameMapping(PropertyInfo property, string columnName)
            :base(property)
        {
            ColumnName = columnName;
        }

        /// <summary/>
        public string ColumnName { get; private set; }

        /// <summary/>
        public override void Map(object instance, IDataRecord row)
        {
            object value;
            try
            {
                value = row[ColumnName];
            }
            catch (IndexOutOfRangeException)
            {
                //todo: condiser throwing specialized exception?
                throw new InvalidOperationException();
            }
            object convertedValue = ConvertValue(value, Property.PropertyType);

            base.SetValue(instance, convertedValue);
        }

    }

    /// <summary/>
    public class IgnoreMapping : PropertyMapping
    {
        /// <summary/>
        public IgnoreMapping(PropertyInfo property)
            :base(property)
        {
        }

        /// <summary/>
        public override void Map(object instance, IDataRecord row)
        {
            //nop
        }
    }

    /// <summary/>
    public class FuncMapping : PropertyMapping
    {
        /// <summary/>
        public FuncMapping(PropertyInfo property, Func<IDataRecord, object> func)
            :base(property)
        {
            Func = func;
        }

        ///  <summary/>
        public Func<IDataRecord, object> Func { get; private set; }

        /// <summary />
        public override void Map(object instance, IDataRecord row)
        {
            object value = Func(row);

            base.SetValue(instance, value);
        }
    }
    

}
