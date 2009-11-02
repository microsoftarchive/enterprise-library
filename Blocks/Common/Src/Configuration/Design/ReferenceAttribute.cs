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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class ReferenceAttribute : Attribute
    {
        private readonly string scopeTypeName;
        private readonly string targetTypeName;
        private readonly string propertyToMatch = "Name";


        /// <summary/>
        public ReferenceAttribute(string targetTypeName)
        {
            this.targetTypeName = targetTypeName;
        }

        /// <summary/>
        public ReferenceAttribute(string scopeTypeName, string targetTypeName)
        {
            this.scopeTypeName = scopeTypeName;
            this.targetTypeName = targetTypeName;
        }

        /// <summary/>
        public ReferenceAttribute(Type targetType)
        {
            this.targetTypeName = targetType.AssemblyQualifiedName;
        }

        /// <summary/>
        public ReferenceAttribute(Type scopeType, Type targetType)
        {
            this.scopeTypeName = scopeType.AssemblyQualifiedName;
            this.targetTypeName = targetType.AssemblyQualifiedName;
        }

        /// <summary/>
        public Type ScopeType
        {
            get { return string.IsNullOrEmpty(scopeTypeName) ? null : Type.GetType(scopeTypeName); }
        }

        /// <summary/>
        public bool ScopeIsDeclaringElement
        {
            get;
            set;
        }

        /// <summary/>
        public Type TargetType
        {
            get { return Type.GetType(targetTypeName); }
        }

        /// <summary/>
        public string PropertyToMatch
        {
            get { return propertyToMatch; }
        }
    }
}
