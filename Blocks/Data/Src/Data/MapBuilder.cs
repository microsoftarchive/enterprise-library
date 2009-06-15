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
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class MapBuilder<TResult> : IMapBuilderContext<TResult>
        where TResult : new()
    {
        private Dictionary<PropertyInfo, PropertyMapping> mappings;

        /// <summary/>
        public MapBuilder()
        {
            mappings = new Dictionary<PropertyInfo, PropertyMapping>();
        }

        /// <summary/>
        public IMapBuilderContext<TResult> CreateDefault()
        {
            IMapBuilderContext<TResult> context = this;

            foreach (PropertyInfo property in new List<PropertyInfo>(typeof(TResult)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite)))
            {
                context = MapByName(property);
            }
            return context;
        }

        /// <summary/>
        public IMapBuilderContextMap<TResult, TMember> Map<TMember>(Expression<Func<TResult, TMember>> propertySelector)
        {
            PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

            return new MapBuilderContextMap<TMember>(this, property);
        }

        /// <summary/>
        public IMapBuilderContextMap<TResult, object> Map(PropertyInfo property)
        {
            return new MapBuilderContextMap<object>(this, property);
        }

        /// <summary/>
        public IMapBuilderContext<TResult> MapByName<TMember>(Expression<Func<TResult, TMember>> propertySelector)
        {
            PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

            return MapByName(property);
        }

        /// <summary/>
        public IMapBuilderContext<TResult> MapByName(PropertyInfo property)
        {
            return this.Map(property).ToColumn(property.Name);
        }

        /// <summary/>
        public IMapBuilderContext<TResult> DoNotMap<TMember>(Expression<Func<TResult, TMember>> propertySelector)
        {
            PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

            return DoNotMap(property);
        }

        /// <summary/>
        public IMapBuilderContext<TResult> DoNotMap(PropertyInfo property)
        {
            mappings[property] = new IgnoreMapping(property);

            return this;
        }

        /// <summary/>
        public IRowMapper<TResult> BuildMapper()
        {
            return new ReflectionRowMapper<TResult>(mappings);
        }

        private static PropertyInfo ExtractPropertyInfo<TMember>(Expression<Func<TResult, TMember>> propertySelector)
        {
            MemberExpression memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null) throw new InvalidOperationException();

            PropertyInfo property = (PropertyInfo)memberExpression.Member;
            return property;
        }

        private class MapBuilderContextMap<TMember> : IMapBuilderContextMap<TResult, TMember>
        {
            PropertyInfo property;
            MapBuilder<TResult> builder;

            public MapBuilderContextMap(MapBuilder<TResult> builder, PropertyInfo property)
            {
                this.property = property;
                this.builder = builder;
            }

            public IMapBuilderContext<TResult> ToColumn(string columnName)
            {
                builder.mappings[property] = new ColumnNameMapping(property, columnName);

                return builder;
            }

            public IMapBuilderContext<TResult> WithFunc(Func<IDataRecord, TMember> f)
            {
                builder.mappings[property] = new FuncMapping(property, row => f(row));

                return builder;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IMapBuilderContext<TResult> : IFluentInterface
    {
        /// <summary />
        IMapBuilderContext<TResult> MapByName(PropertyInfo property);

        /// <summary />
        IMapBuilderContext<TResult> MapByName<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary />
        IMapBuilderContext<TResult> DoNotMap(PropertyInfo property);

        /// <summary />
        IMapBuilderContext<TResult> DoNotMap<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary />
        IMapBuilderContextMap<TResult, TMember> Map<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary />
        IMapBuilderContextMap<TResult, object> Map(PropertyInfo property);

        /// <summary />
        IRowMapper<TResult> BuildMapper();
    }

    /// <summary />
    public interface IMapBuilderContextMap<TResult, TMember> : IFluentInterface
    {
        /// <summary />
        IMapBuilderContext<TResult> ToColumn(string columnName);

        /// <summary />
        IMapBuilderContext<TResult> WithFunc(Func<IDataRecord, TMember> f);

    }


}
