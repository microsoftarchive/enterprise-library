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
    ///<summary>
    /// Indicates the type of add command to apply for a configuration collection
    ///</summary>
    [AttributeUsage(AttributeTargets.Class,  AllowMultiple = true)]
    public class CollectionElementAddCommandAttribute : Attribute
    {
        private readonly string collectionElementAddCommandType;

        ///<summary>
        ///</summary>
        ///<param name="addCommandType"></param>
        public CollectionElementAddCommandAttribute(Type addCommandType)
        {
            collectionElementAddCommandType = addCommandType.AssemblyQualifiedName;
        }

        ///<summary>
        ///</summary>
        ///<param name="addCommandType"></param>
        public CollectionElementAddCommandAttribute(string addCommandType)
        {
            collectionElementAddCommandType = addCommandType;
        }

        ///<summary>
        ///</summary>
        public Type AddCommandType
        {
            get
            {
                return Type.GetType(collectionElementAddCommandType, true, true);
            }
        }

        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that is a unique identifier for the attribute.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object TypeId
        {
            get
            {
                return GetHashCode();
            }
        }
    }
}
