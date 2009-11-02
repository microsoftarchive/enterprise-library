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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.TypePicker;

namespace Console.Wpf.ComponentModel.Editors
{
    /// <summary>
    /// Represents a type in the data structure used by the <see cref="TypeSelectorUI"/> to specify types.
    /// </summary>
    /// <remarks>
    /// A node holds a <see cref="Type"/>, and if the type is an open generic type the node will have children 
    /// representing the generic type parameters.
    /// <para/>
    /// A node can be asked to create the type it represents. Doing so might throw exceptions if the represented type
    /// is not valid (e.g. if the types assigned to the child nodes of a node representing a generic type do not satisfy 
    /// the parameter constraints); the node doesn't attempt to validate whether the specified type can be built, 
    /// letting the .NET framework perform the necessary validations.
    /// </remarks>
    public class TypeBuildNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildNode"/> with a type constraint.
        /// </summary>
        /// <param name="constraint">The constraint for the node.</param>
        public TypeBuildNode(TypeBuildNodeConstraint constraint)
        {
            this.Constraint = constraint;
            this.GenericParameterNodes = new TypeBuildNode[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildNode"/> representing a generic parameter node.
        /// </summary>
        /// <param name="genericTypeParameter">The type representing the generic type parameter.</param>
        public TypeBuildNode(Type genericTypeParameter)
            : this(GetConstraintForGenericTypeParameter(genericTypeParameter))
        {
            this.ParameterType = genericTypeParameter;
        }

        /// <summary>
        /// Creates a tree of <see cref="TypeBuildNode"/> instances that represents the <paramref name="nodeType"/> type.
        /// </summary>
        /// <param name="nodeType">The <see cref="Type"/> to represent.</param>
        /// <param name="typeConstraint">The type constraint that the limits what types can be specified
        /// for the root of the tree.</param>
        /// <returns>A tree of <see cref="TypeBuildNode"/> representing <paramref name="nodeType"/>.</returns>
        public static TypeBuildNode CreateTreeForType(Type nodeType, TypeBuildNodeConstraint typeConstraint)
        {
            TypeBuildNode rootTypeNode = new TypeBuildNode(typeConstraint);

            UpdateForType(rootTypeNode, nodeType);

            return rootTypeNode;
        }

        private static void UpdateForType(TypeBuildNode rootTypeNode, Type nodeType)
        {
            // what kind of type are we dealing with?
            if (nodeType == null || !nodeType.IsGenericType)
            {
                rootTypeNode.SetNodeType(nodeType);
            }
            else
            {
                // is it an open or a closed generic?
                if (!nodeType.IsGenericTypeDefinition)
                {
                    // we need to set it up as if it was created by the user,
                    // so the type to set on the node will be the generic type definition
                    // and the actual type parameters will be set to the child nodes
                    Type genericTypeDefinition = nodeType.GetGenericTypeDefinition();
                    rootTypeNode.SetNodeType(genericTypeDefinition);

                    // by now the root type node should have the appropriate children
                    // set the types for them out from the actual types in the original type
                    Type[] actualGenericParameters = nodeType.GetGenericArguments();
                    int i = 0;
                    foreach (var childNode in rootTypeNode.GenericParameterNodes)
                    {
                        UpdateForType(childNode, actualGenericParameters[i++]);
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Creates the type representing by the receiving node and its children.
        /// </summary>
        /// <returns>The represented type.</returns>
        /// <exception cref="TypeBuildException">when there is an error building the type (e.g. if the 
        /// types assigned to the child nodes of a node representing a generic type do not satisfy the parameter
        /// constraints)</exception>
        public Type BuildType()
        {
            if (this.NodeType == null)
            {
                throw new TypeBuildException(Resources.ExceptionTypeBuildTypeNotSet, this, null);
            }

            if (!this.NodeType.IsGenericTypeDefinition)
            {
                return this.NodeType;
            }

            // dealing with generic types
            try
            {
                return this.NodeType.MakeGenericType(
                    (from tn in GenericParameterNodes select tn.BuildType()).ToArray());
            }
            catch (TypeBuildException)
            {
                // bubble-up type creation exception from descendant
                throw;
            }
            catch (Exception e)
            {
                throw new TypeBuildException(
                    string.Format(Resources.ExceptionTypeBuildTypeExceptionMakingGeneric, e.Message),
                    this,
                    e);
            }
        }

        /// <summary>
        /// Sets the <see cref="Type"/> the node represents, and updates its children to represent generic type 
        /// parameters when an open generic type is involved.
        /// </summary>
        /// <param name="nodeType">The new type the node represents.</param>
        /// <returns><see langword="true"/> if the update caused the child nodes to change; otherwise 
        /// <see langword="false"/></returns>
        public bool SetNodeType(Type nodeType)
        {
            if (this.NodeType == nodeType)
            {
                return false;
            }

            if (nodeType.IsGenericType && !nodeType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(Resources.ExceptionCannotSetClosedGenerics, "nodeType");
            }

            Type previousType = this.NodeType;
            this.NodeType = nodeType;

            if ((previousType == null || !previousType.IsGenericType) && !this.NodeType.IsGenericType)
            {
                // no generic types involved, just changing the parameter is enough.
                return false;
            }

            // the sub nodes need to be updated.
            List<TypeBuildNode> newGenericParameterNodes = new List<TypeBuildNode>();
            if (this.NodeType.IsGenericTypeDefinition)
            {
                foreach (Type genericTypeParameter in this.NodeType.GetGenericArguments())
                {
                    newGenericParameterNodes.Add(new TypeBuildNode(genericTypeParameter));
                }
            }
            this.GenericParameterNodes = newGenericParameterNodes;

            return true;
        }

        private static TypeBuildNodeConstraint GetConstraintForGenericTypeParameter(Type genericTypeParameter)
        {
            if (genericTypeParameter == null)
            {
                throw new ArgumentNullException("genericTypeParameter");
            }
            if (!genericTypeParameter.IsGenericParameter)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.TypeNotGenericTypeParameter,
                        genericTypeParameter.FullName),
                    "genericTypeParameter");
            }

            Type baseType = GetBaseType(genericTypeParameter);
            TypeSelectorIncludes flags;
            if (baseType == typeof(object) || baseType.IsInterface)
            {
                flags
                    = TypeSelectorIncludes.AbstractTypes
                        | TypeSelectorIncludes.BaseType
                        | TypeSelectorIncludes.Interfaces
                        | TypeSelectorIncludes.NonpublicTypes;
            }
            else
            {
                // do now allow interfaces if the base type is a class that is not object
                flags
                    = TypeSelectorIncludes.AbstractTypes
                        | TypeSelectorIncludes.BaseType
                        | TypeSelectorIncludes.NonpublicTypes;
            }

            return new TypeBuildNodeConstraint(baseType, null, flags); ;
        }

        private static Type GetBaseType(Type genericParameter)
        {
            Type baseType = genericParameter.BaseType;
            if (baseType != typeof(object) && !baseType.IsGenericType && !baseType.IsGenericParameter)
            {
                return baseType;
            }

            foreach (var _interface in genericParameter.GetInterfaces())
            {
                if (!_interface.IsGenericType)
                {
                    return _interface;
                }
            }

            return typeof(object);
        }

        /// <summary>
        /// Gets the generic type parameter associated to the node, if any.
        /// </summary>
        public Type ParameterType { get; private set; }

        /// <summary>
        /// Gets the constraint that should be used to filter the available types this node can hold.
        /// </summary>
        /// <remarks>
        /// This constraint is less detailed than the constraints that can be specified for a generic type parameter,
        /// so satisfying this constraint does not guarantee that the type can be built.
        /// </remarks>
        public TypeBuildNodeConstraint Constraint { get; private set; }

        /// <summary>
        /// Gets the type currently set for this node.
        /// </summary>
        /// <remarks>
        /// This is not the same as <see cref="Type"/> represented by the node, which involves the current node and its
        /// children, should there be any.
        /// </remarks>
        public Type NodeType { get; private set; }

        /// <summary>
        /// Gets the children of a node.
        /// </summary>
        /// <remarks>
        /// Child nodes represent generic type parameters.
        /// </remarks>
        public IEnumerable<TypeBuildNode> GenericParameterNodes { get; private set; }

        /// <summary>
        /// Gets a description of the current node.
        /// </summary>
        /// <remarks>
        /// The description of a node includes whether its type has been set and whether it represents a generic
        /// type parameter.
        /// </remarks>
        public string Description
        {
            get
            {
                StringBuilder descriptionBuilder = new StringBuilder();
                if (this.ParameterType != null)
                {
                    descriptionBuilder.Append(this.ParameterType.Name);
                    descriptionBuilder.Append(": ");
                }

                if (this.NodeType != null)
                {
                    descriptionBuilder.Append(this.NodeType.Name);
                }
                else
                {
                    descriptionBuilder.Append(Resources.TypeParameterUndefined);
                }

                return descriptionBuilder.ToString();
            }
        }
    }
}
