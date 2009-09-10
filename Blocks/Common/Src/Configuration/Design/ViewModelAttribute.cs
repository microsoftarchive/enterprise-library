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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ViewModelAttribute : Attribute
    {
        private readonly string modelTypeName;

        /// <summary/>
        public ViewModelAttribute(Type modelType)
        {
            this.modelTypeName = modelType.AssemblyQualifiedName;
        }

        /// <summary/>
        public ViewModelAttribute(string modelTypeName)
        {
            this.modelTypeName = modelTypeName;
        }

        /// <summary/>
        public Type ModelType
        {
            get { return Type.GetType(modelTypeName); }
        }
    }
}
