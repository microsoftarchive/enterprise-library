using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TypeLoadTestAssembly;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    [TestClass]
    public class TypeLoadingFixture
    {
        [TestMethod]
        public void WhenIncludingAssemblyWithMissingReferences_ThenLoadsTypesWithAvailableReferences()
        {
            var nodeCreatorMock = new Mock<INodeCreator>(MockBehavior.Strict);

            nodeCreatorMock
                .Setup(nc => nc.CreateNamespaceNode("TypeLoadTestAssembly"))
                .Returns<string>(ns => new NamespaceNode(ns))
                .Verifiable();

            nodeCreatorMock
                .Setup(nc => nc.CreateTypeNode(typeof(DerivedFromObject)))
                .Returns<Type>(t => new TypeNode(t))
                .Verifiable();

            var namespaces = new AssemblyNode(typeof(DerivedFromObject).Assembly, nodeCreatorMock.Object, t => true).Namespaces;

            nodeCreatorMock.VerifyAll();
        }
    }
}
