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
    /// <summary>
    /// Attribute class used to specify a specific View Model derivement or visual representation to be used on the target element.
    /// 
    /// TODO: add more information here, possibly a reference to other documentation.
    /// </summary>
    /// <remarks>
    /// 
    /// <para>The View Model Type should derive from the ElementViewModel or Property class in the Configuration.Design assembly. <br/>
    /// As this attribute can be applied to the configuration directly and we dont want to force a dependency on the Configuration.Design assembly <br/>
    /// You can specify the View Model Type in a loosy coupled fashion, passing a qualified name of the type.</para>
    ///
    /// <para>TODO: The Model Visual Type should derive from the xXXX? add more info here</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ViewModelAttribute : Attribute
    {
        private readonly string modelTypeName;
        private readonly string modelVisualTypeName;

        ///<summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        ///</summary>
        ///<param name="modelType">The type of the View Model that should be used for the annotated element.</param>
        public ViewModelAttribute(Type modelType)
            : this(modelType.AssemblyQualifiedName)
        { }


        ///<summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        ///</summary>
        ///<param name="modelType">The type of the View Model that should be used for the annotated element.</param>
        ///<param name="modelVisualType">The type of the Model Visual that should be used to display the annotated element.</param>
        public ViewModelAttribute(Type modelType, Type modelVisualType)
            : this(modelType.AssemblyQualifiedName, modelVisualType.AssemblyQualifiedName)
        { }

        ///<summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        ///</summary>
        ///<param name="modelTypeName">The type name of the View Model that should be used for the annotated element.</param>
        public ViewModelAttribute(string modelTypeName)
            : this(modelTypeName, string.Empty)
        { }


        ///<summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        ///</summary>
        ///<param name="modelTypeName">The type name of the View Model Type that should be used for the annotated element.</param>
        ///<param name="modelVisualTypeName">The name type of the Model Visual Type that should be used to display the annotated element.</param>
        public ViewModelAttribute(string modelTypeName, string modelVisualTypeName)
        {
            this.modelTypeName = modelTypeName;
            this.modelVisualTypeName = modelVisualTypeName;
        }

        ///<summary>
        /// Gets the Model Visual Type that should be used to display the annotated element.
        ///</summary>
        public Type ModelVisualType
        {
            get { return string.IsNullOrEmpty(modelVisualTypeName) ? null : Type.GetType(modelVisualTypeName, true, true); }
        }

        ///<summary>
        /// Gets the View Model Type that should be used to bind the annotated element to its view.
        ///</summary>
        public Type ModelType
        {
            get { return Type.GetType(modelTypeName, true, true); }
        }


    }
}
