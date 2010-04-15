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
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    [TestClass]
    public class TypeFilterFixture
    {
        [TestMethod]
        public void AcceptsPublicNonAbstractNonInterfaceTypesByDefault()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsFalse(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsFalse(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsNonPublicTypesIfSpecified()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.NonpublicTypes);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsFalse(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsTrue(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsAbstractTypesIfSpecified()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.AbstractTypes);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsTrue(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsFalse(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsAbstractAndNonPublicTypesIfSpecified()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.NonpublicTypes);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsTrue(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsTrue(constraint.Matches(typeof(InternalType)));
            Assert.IsTrue(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsBaseTypeIfSpecified()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.BaseType);

            Assert.IsTrue(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsFalse(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsFalse(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsInterfacesIfSpecified()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.Interfaces);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsFalse(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(IBase)));
            Assert.IsTrue(constraint.Matches(typeof(IDerived)));
            Assert.IsFalse(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsAllTypesIfSpecified()
        {
            var constraint =
                new TypeBuildNodeConstraint(
                    typeof(object),
                    null,
                    TypeSelectorIncludes.Interfaces | TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.BaseType | TypeSelectorIncludes.NonpublicTypes);

            Assert.IsTrue(constraint.Matches(typeof(object)));
            Assert.IsTrue(constraint.Matches(typeof(BaseType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsTrue(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(IBase)));
            Assert.IsTrue(constraint.Matches(typeof(IDerived)));
            Assert.IsTrue(constraint.Matches(typeof(InternalType)));
            Assert.IsTrue(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void AcceptsTypesFromDifferentLoadContext()
        {
            string newAssemblyName = Path.GetRandomFileName();
            File.Copy(Assembly.GetExecutingAssembly().Location, newAssemblyName);

            var testDomain = AppDomain.CreateDomain("test", null, AppDomain.CurrentDomain.SetupInformation);
            var helper = new TestHelper { AssemblyFileName = newAssemblyName };
            try
            {
                testDomain.DoCallBack(helper.DoTestAcceptsTypesFromDifferentLoadContext);
            }
            finally
            {
                AppDomain.Unload(testDomain);
                File.Delete(newAssemblyName);
            }
        }

        [Serializable]
        public class TestHelper
        {
            public string AssemblyFileName { get; set; }

            public void DoTestAcceptsTypesFromDifferentLoadContext()
            {
                var newAssembly = Assembly.LoadFrom(AssemblyFileName);

                var constraint =
                    new TypeBuildNodeConstraint(
                        typeof(IBase),
                        null,
                        TypeSelectorIncludes.Interfaces | TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.BaseType | TypeSelectorIncludes.NonpublicTypes);

                Assert.IsTrue(constraint.Matches(newAssembly.GetType(typeof(BaseType).FullName, true)));
                Assert.IsTrue(constraint.Matches(newAssembly.GetType(typeof(DerivedType).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(DerivedType.NestedType).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(DerivedType.NestedInternalType).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(AbstractType).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(DerivedFromAbstractType).FullName, true)));
                Assert.IsTrue(constraint.Matches(newAssembly.GetType(typeof(IBase).FullName, true)));
                Assert.IsTrue(constraint.Matches(newAssembly.GetType(typeof(IDerived).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(InternalType).FullName, true)));
                Assert.IsFalse(constraint.Matches(newAssembly.GetType(typeof(InternalAbstractType).FullName, true)));
            }
        }

        [TestMethod]
        public void AcceptsOnlyTypesWithTheConfigurationElementTypeIfSpecified()
        {
            var constraint =
                new TypeBuildNodeConstraint(
                    typeof(object),
                    typeof(int),
                    TypeSelectorIncludes.Interfaces | TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.BaseType | TypeSelectorIncludes.NonpublicTypes);

            Assert.IsFalse(constraint.Matches(typeof(object)));
            Assert.IsFalse(constraint.Matches(typeof(BaseType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedType)));
            Assert.IsFalse(constraint.Matches(typeof(DerivedType.NestedInternalType)));
            Assert.IsFalse(constraint.Matches(typeof(AbstractType)));
            Assert.IsTrue(constraint.Matches(typeof(DerivedFromAbstractType)));
            Assert.IsFalse(constraint.Matches(typeof(IBase)));
            Assert.IsFalse(constraint.Matches(typeof(IDerived)));
            Assert.IsFalse(constraint.Matches(typeof(InternalType)));
            Assert.IsFalse(constraint.Matches(typeof(InternalAbstractType)));
        }

        [TestMethod]
        public void ObjectIsReferredToAsAnyInDisplayString()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(object), null, TypeSelectorIncludes.None);
            Assert.IsTrue(constraint.GetDisplayString().IndexOf("any", StringComparison.OrdinalIgnoreCase) > -1);
        }

        [TestMethod]
        public void BaseTypeIsReferredToInDisplayString()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(Guid), null, TypeSelectorIncludes.None);
            Assert.IsTrue(constraint.GetDisplayString().IndexOf("Guid", StringComparison.OrdinalIgnoreCase) > -1);
        }

        [TestMethod]
        public void ConfigurationElementTypeIsReferredToInDisplayString()
        {
            var constraint = new TypeBuildNodeConstraint(typeof(Guid), typeof(double), TypeSelectorIncludes.None);
            Assert.IsTrue(constraint.GetDisplayString().IndexOf("double", StringComparison.OrdinalIgnoreCase) > -1);
        }


        public class BaseType : IBase { }
        [ConfigurationElementType(typeof(string))]
        public class DerivedType : IDerived { public class NestedType { } internal class NestedInternalType { } }
        public abstract class AbstractType { }
        [ConfigurationElementType(typeof(int))]
        public class DerivedFromAbstractType { }
        public interface IBase { }
        public interface IDerived : IBase { }
        internal class InternalType { }
        internal abstract class InternalAbstractType { }
    }
}
