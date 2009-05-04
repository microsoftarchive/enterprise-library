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
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ConfigurationModel
{
    [TestClass]
    public class GivenATypeToBuild
    {
        private Type typeToBuild;

        [TestInitialize]
        public void Given()
        {
            typeToBuild = typeof(MockCustomData);
        }


        [TestMethod]
        public void WhenNullAttributes_ThenProvidesNewLambdaExpression()
        {
            LambdaExpression expression = RegistrationExpressionBuilder.BuildExpression(typeToBuild, null);
            Assert.AreEqual(ExpressionType.New, expression.Body.NodeType);
        }

        [TestMethod]
        public void WhenNullAttributes_ThenUsesConstructorWithNameValueCollectionParameter()
        {
            LambdaExpression expression = RegistrationExpressionBuilder.BuildExpression(typeToBuild, null);
            var newExpression = ((NewExpression) expression.Body);
            Assert.AreEqual(1, newExpression.Constructor.GetParameters().Count());
            Assert.AreEqual(typeof(NameValueCollection), newExpression.Constructor.GetParameters()[0].ParameterType);
        }

        [TestMethod]
        public void WhenNullAttributes_ThenCreatesEmptyNameValueCollectionForConstructorParameter()
        {
            LambdaExpression expression = RegistrationExpressionBuilder.BuildExpression(typeToBuild, null);
            var newExpression = ((NewExpression)expression.Body);
            var collection =
                ((ConstantExpression) newExpression.Arguments[0]).Value as NameValueCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void WhenNonNullAttributes_ThenAttributesCollectionIsProvidedAsParameterValue()
        {
            NameValueCollection inputCollection = new NameValueCollection() {{"checkone", "one"}, {"checktwo", "two"}};

            LambdaExpression expression = RegistrationExpressionBuilder.BuildExpression(typeToBuild, inputCollection);
            var newExpression = ((NewExpression)expression.Body);
            var collection =
                ((ConstantExpression)newExpression.Arguments[0]).Value as NameValueCollection;

            Assert.AreSame(inputCollection, collection);
        }
    }

    [TestClass]
    public class GivenCustomDataWithoutSingleParameterConstructorOfNameValue
    {

        private Type typeToBuild;

        [TestInitialize]
        public void Given()
        {
            typeToBuild = typeof (MockBadCustomData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenTheExpressionIsBuilt_ThenExpressionBuilderThrowsArgumentException()
        {
            LambdaExpression expression = RegistrationExpressionBuilder.BuildExpression(typeToBuild, null);
        }
    }



 

    internal class MockBadCustomData
    {
        public MockBadCustomData(NameValueCollection attributes, string somethingElse)
        {
        }
    }
    internal class MockCustomData
    {
        public MockCustomData(NameValueCollection attributes)
        {
        }
    }

}
