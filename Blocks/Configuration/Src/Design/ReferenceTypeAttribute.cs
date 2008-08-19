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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Specifies the <see cref="Type"/> that a node references.
    /// </summary>	
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class ReferenceTypeAttribute : Attribute
    {

        private Type referenceType;
        private bool localOnly;

        /// <summary>
        /// Initialzie a new instance of the <see cref="ReferenceTypeAttribute"/> class with type to reference.
        /// </summary>
        /// <param name="referenceType">
        /// The <see cref="Type"/> of the reference.
        /// </param>
        public ReferenceTypeAttribute(Type referenceType) 
            : this(referenceType, false)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceType"></param>
        /// <param name="localOnly"></param>
        public ReferenceTypeAttribute(Type referenceType, bool localOnly)
            : base()
        {
            this.referenceType = referenceType;
            this.localOnly = localOnly;
        }
        /// <summary>
        /// Gets the <see cref="Type"/> to reference.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> to reference.
        /// </value>
        public Type ReferenceType
        {
            get { return referenceType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LocalOnly
        {
            get { return localOnly; }
        }
    }
}