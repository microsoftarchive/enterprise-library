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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class TypeNodeFixture
    {
        [TestMethod]
        public void BuildingNonSetTypeThrows()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            try
            {
                node.BuildType();
                Assert.Fail("should have thrown");
            }
            catch (TypeBuildException)
            {
            }
        }

        [TestMethod]
        public void CanBuildSimpleType()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(int));

            Assert.AreSame(typeof(int), node.BuildType());
        }

        [TestMethod]
        public void ChangingFromSimpleTypeToSimpleTypeDoesNotChangeChildren()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.AreEqual(0, node.GenericParameterNodes.Count());

            bool childrenChanged = node.SetNodeType(typeof(int));

            Assert.IsFalse(childrenChanged);
            Assert.AreEqual(0, node.GenericParameterNodes.Count());

            childrenChanged = node.SetNodeType(typeof(string));

            Assert.IsFalse(childrenChanged);
            Assert.AreEqual(0, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void SettingAClosedGenericTypeThrows()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            try
            {
                node.SetNodeType(typeof(Dictionary<int, string>));
                Assert.Fail("should have thrown");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void SettingAGenericTypeChangesChildren()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            bool childrenChanged = node.SetNodeType(typeof(Dictionary<,>));

            Assert.IsTrue(childrenChanged);

            TypeBuildNode[] childNodes = node.GenericParameterNodes.ToArray();

            Assert.AreEqual(2, childNodes.Length);
            Assert.AreEqual("TKey", childNodes[0].ParameterType.Name);
            Assert.AreEqual("TValue", childNodes[1].ParameterType.Name);
        }

        [TestMethod]
        public void ChangingFromGenericTypeToNonGenericTypeChangesChildNodes()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            bool childrenChanged = node.SetNodeType(typeof(Dictionary<,>));
            Assert.IsTrue(childrenChanged);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());

            childrenChanged = node.SetNodeType(typeof(int));
            Assert.IsTrue(childrenChanged);
            Assert.AreEqual(0, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void ChangingFromGenericTypeToDifferentGenericTypeChangesChildNodes()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            bool childrenChanged = node.SetNodeType(typeof(Dictionary<,>));
            Assert.IsTrue(childrenChanged);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());

            childrenChanged = node.SetNodeType(typeof(List<>));
            Assert.IsTrue(childrenChanged);
            Assert.AreEqual(1, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void SettingCurrentGenericTypeDoesNotChangeChildNodes()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            bool childrenChanged = node.SetNodeType(typeof(Dictionary<,>));
            Assert.IsTrue(childrenChanged);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());

            childrenChanged = node.SetNodeType(typeof(Dictionary<,>));
            Assert.IsFalse(childrenChanged);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void BuildingNonClosedGenericTypeThrows()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(Dictionary<,>));

            try
            {
                node.BuildType();
                Assert.Fail("should have thrown");
            }
            catch (TypeBuildException)
            {
            }
        }

        [TestMethod]
        public void BuildingPartiallyClosedGenericTypeThrows()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(Dictionary<,>));
            node.GenericParameterNodes.ToArray()[0].SetNodeType(typeof(int));

            try
            {
                node.BuildType();
                Assert.Fail("should have thrown");
            }
            catch (TypeBuildException)
            {
            }
        }

        [TestMethod]
        public void CanBuildClosedGenericType()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(Dictionary<,>));
            node.GenericParameterNodes.ElementAt(0).SetNodeType(typeof(int));
            node.GenericParameterNodes.ElementAt(1).SetNodeType(typeof(string));

            Type builtType = node.BuildType();

            Assert.AreSame(typeof(Dictionary<int, string>), builtType);
        }

        [TestMethod]
        public void CanBuildClosedGenericTypeWithClosedGenericTypeParameters()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(Dictionary<,>));
            node.GenericParameterNodes.ElementAt(0).SetNodeType(typeof(int));
            node.GenericParameterNodes.ElementAt(1).SetNodeType(typeof(List<>));
            node.GenericParameterNodes.ElementAt(1).GenericParameterNodes.ElementAt(0).SetNodeType(typeof(string));

            Type builtType = node.BuildType();

            Assert.AreSame(typeof(Dictionary<int, List<string>>), builtType);
        }

        [TestMethod]
        public void BuildingAClosedGenericTypeWithNonSatisfiedConstraintsThrowsOnBuild()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericType<>));
            node.GenericParameterNodes.ElementAt(0).SetNodeType(typeof(string));

            try
            {
                node.BuildType();
                Assert.Fail("should have thrown");
            }
            catch (TypeBuildException)
            {
            }
        }

        [TestMethod]
        public void CanCreateInitializedNodeForNull()
        {
            TypeBuildNode node
                = TypeBuildNode.CreateTreeForType(
                    null,
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.AreSame(null, node.NodeType);
            Assert.AreEqual(0, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void CanCreateInitializedNodeForNonGenericType()
        {
            TypeBuildNode node
                = TypeBuildNode.CreateTreeForType(
                    typeof(int),
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.AreSame(typeof(int), node.NodeType);
            Assert.AreEqual(0, node.GenericParameterNodes.Count());
        }

        [TestMethod]
        public void CanCreateInitializedNodeForClosedGenericType()
        {
            TypeBuildNode node
                = TypeBuildNode.CreateTreeForType(
                    typeof(Dictionary<int, string>),
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.AreSame(typeof(Dictionary<,>), node.NodeType);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());
            Assert.AreSame(typeof(int), node.GenericParameterNodes.ElementAt(0).NodeType);
            Assert.AreSame(typeof(string), node.GenericParameterNodes.ElementAt(1).NodeType);
            Assert.AreSame(typeof(Dictionary<int, string>), node.BuildType());
        }

        [TestMethod]
        public void CanCreateInitializedNodeForMultiLevelClosedGenericType()
        {
            TypeBuildNode node
                = TypeBuildNode.CreateTreeForType(
                    typeof(Dictionary<int, List<string>>),
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.AreSame(typeof(Dictionary<,>), node.NodeType);
            Assert.AreEqual(2, node.GenericParameterNodes.Count());
            Assert.AreSame(typeof(int), node.GenericParameterNodes.ElementAt(0).NodeType);
            Assert.AreSame(typeof(List<>), node.GenericParameterNodes.ElementAt(1).NodeType);
            Assert.AreEqual(1, node.GenericParameterNodes.ElementAt(1).GenericParameterNodes.Count());
            Assert.AreSame(typeof(string), node.GenericParameterNodes.ElementAt(1).GenericParameterNodes.ElementAt(0).NodeType);
            Assert.AreSame(typeof(Dictionary<int, List<string>>), node.BuildType());
        }

        [TestMethod]
        public void SettingAGenericTypeWithABaseClassConstraintConfiguresTheChildNodeConstraint()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericTypeWithBaseClassConstraint<>));
            Assert.AreSame(
                typeof(Exception),
                node.GenericParameterNodes.ElementAt(0).Constraint.BaseType);
            Assert.IsFalse(
                (TypeSelectorIncludes.Interfaces
                    & node.GenericParameterNodes.ElementAt(0).Constraint.TypeSelectorIncludes)
                == TypeSelectorIncludes.Interfaces);
        }

        [TestMethod]
        public void SettingAGenericTypeWithAGenericBaseClassConstraintIgnoresTheConstraint()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericTypeWithGenericBaseClassConstraint<>));
            Assert.AreSame(
                typeof(object),
                node.GenericParameterNodes.ElementAt(0).Constraint.BaseType);
            Assert.IsTrue(
                (TypeSelectorIncludes.Interfaces
                    & node.GenericParameterNodes.ElementAt(0).Constraint.TypeSelectorIncludes)
                == TypeSelectorIncludes.Interfaces);
        }

        [TestMethod]
        public void SettingAGenericTypeWithAGenericParameterBaseClassConstraintIgnoresTheConstraint()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericTypeWithGenericParameterBaseClassConstraint<,>));
            Assert.AreSame(
                typeof(object),
                node.GenericParameterNodes.ElementAt(0).Constraint.BaseType);
        }

        [TestMethod]
        public void SettingAGenericTypeWithAnInterfaceConstraintConfiguresTheChildNodeConstraint()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericTypeWithInterfaceConstraint<>));
            Assert.AreSame(
                typeof(IConvertible),
                node.GenericParameterNodes.ElementAt(0).Constraint.BaseType);
        }

        [TestMethod]
        public void SettingAGenericTypeWithMultipleInterfaceConstraintsAndNonApplicableBaseClassConstraintSetsNonGenericInterface()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(TestGenericTypeWithGenericBaseClassAndInterfaceConstraints<>));
            Type baseType = node.GenericParameterNodes.ElementAt(0).Constraint.BaseType;
            Assert.IsTrue(baseType.IsInterface);
            Assert.IsFalse(baseType.IsGenericType);
        }

        [TestMethod]
        public void CanGetDescriptionForNonSetNode()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            Assert.IsNotNull(node.Description);
        }

        [TestMethod]
        public void CanGetDescriptionForSetAndNotSetParameterNodes()
        {
            TypeBuildNode node
                = new TypeBuildNode(
                    new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None));

            node.SetNodeType(typeof(Dictionary<,>));
            node.GenericParameterNodes.ToArray()[0].SetNodeType(typeof(int));

            Assert.IsNotNull(node.Description);
            Assert.IsNotNull(node.GenericParameterNodes.ToArray()[0].Description);
            Assert.IsNotNull(node.GenericParameterNodes.ToArray()[1].Description);
        }

        public class TestGenericType<T>
            where T : new()
        {
        }

        public class TestGenericTypeWithBaseClassConstraint<T>
            where T : Exception
        {
        }

        public class TestGenericTypeWithGenericBaseClassConstraint<T>
            where T : TestGenericType<Exception>
        {
        }

        public class TestGenericTypeWithGenericBaseClassAndInterfaceConstraints<T>
            where T : List<Exception>
        {
        }

        public class TestGenericTypeWithGenericParameterBaseClassConstraint<T, U>
            where T : U
        {
        }

        public class TestGenericTypeWithInterfaceConstraint<T>
            where T : IConvertible
        {
        }
    }
}
