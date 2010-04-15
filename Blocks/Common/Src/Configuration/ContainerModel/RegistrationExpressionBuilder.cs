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
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    ///<summary>
    /// Builds expression used in <see cref="TypeRegistration"/> for custom and standard Enterprise Library objects.
    ///</summary>
    public class RegistrationExpressionBuilder
    {
        ///<summary>
        /// Builds a <see cref="LambdaExpression"/> expected for custom Enterprise Library objects based on the supplied type's object
        /// and provide attributes.
        ///</summary>
        /// <remarks>
        /// The <paramref name="typeToBuild"/> must supply an accessible constructor that takes a single parameter of type <see cref="NameValueCollection"/>.
        /// </remarks>
        ///<param name="typeToBuild">The object type to build the expression around.</param>
        ///<param name="attributes">Attributes to pass to the constructor of <paramref name="typeToBuild"/></param>
        ///<returns>A <see cref="LambdaExpression"/> that defines the construction of <paramref name="typeToBuild"/> in a <see cref="NewExpression"/>.</returns>
        ///<exception cref="ArgumentException">Is thrown if the <paramref name="typeToBuild"/> does not provide a proper constructor.</exception>
        public static LambdaExpression BuildExpression(Type typeToBuild, NameValueCollection attributes)
        {
            return Expression.Lambda(BuildNewExpression(typeToBuild, attributes));
        }

        ///<summary>
        /// Builds a <see cref="NewExpression"/> expected for custom Enterprise Library objects based on the supplied type's object
        /// and provide attributes.
        ///</summary>
        /// <remarks>
        /// The <paramref name="typeToBuild"/> must supply an accessible constructor that takes a single parameter of type <see cref="NameValueCollection"/>.
        /// </remarks>
        ///<param name="typeToBuild">The object type to build the expression around.</param>
        ///<param name="attributes">Attributes to pass to the constructor of <paramref name="typeToBuild"/></param>
        ///<returns>A <see cref="NewExpression"/> that defines the construction of <paramref name="typeToBuild"/> in a <see cref="NewExpression"/>.</returns>
        ///<exception cref="ArgumentException">Is thrown if the <paramref name="typeToBuild"/> does not provide a proper constructor.</exception>
        public static NewExpression BuildNewExpression(Type typeToBuild, NameValueCollection attributes)
        {
            if (typeToBuild == null) throw new ArgumentNullException("typeToBuild");

            NameValueCollection collectionArgument = attributes ?? new NameValueCollection();
            ConstructorInfo constructor = typeToBuild.GetConstructor(new[] { typeof(NameValueCollection) });
            if (constructor == null)
            {
                throw new ArgumentException(Properties.Resources.ExceptionTypeDoesNotProvideCorrectConstructor);
            }

            return Expression.New(constructor, Expression.Constant(collectionArgument));
        }
    }
}
