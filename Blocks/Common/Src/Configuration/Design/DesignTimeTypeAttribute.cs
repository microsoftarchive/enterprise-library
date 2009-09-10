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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class DesignTimeTypeAttribute : Attribute
    {
        private readonly string designTimeTypeName;
        private readonly string designTimeTypeConverterName;

        /// <summary/>
        public DesignTimeTypeAttribute(Type designtimeType, Type designtimeTypeConverter)
            :this(designtimeType.AssemblyQualifiedName, designtimeTypeConverter.AssemblyQualifiedName)
        {
        }

        /// <summary/>
        public DesignTimeTypeAttribute(string designTimeTypeName, string designTimeTypeConverterName)
        {
            this.designTimeTypeConverterName = designTimeTypeConverterName;
            this.designTimeTypeName = designTimeTypeName;
        }

        /// <summary/>
        public Type DesignTimeType
        {
            get { return Type.GetType(designTimeTypeName, true); }
        }

        /// <summary/>
        public Type DesignTimeTypeConverter
        {
            get { return Type.GetType(designTimeTypeConverterName, true); }
        }
    }
}
