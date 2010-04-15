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

using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_annotation_service
{
    [TestClass]
    public class when_type_is_found_with_metadata_type_attribute : ArrangeActAssert
    {
        AnnotationService annotationService;

        protected override void Arrange()
        {
            Mock<AssemblyLocator> assemblyLocator = new Mock<AssemblyLocator>();
            assemblyLocator.Setup(x => x.Assemblies).Returns(new Assembly[] { typeof(when_type_is_found_with_metadata_type_attribute).Assembly });

            annotationService = new AnnotationService(assemblyLocator.Object);
        }

        protected override void Act()
        {
            annotationService.DiscoverSubstituteTypesFromAssemblies();
        }

        [TestMethod]
        public void then_attributes_from_metadata_type_can_be_retrieved_from_original_type()
        {
            Assert.IsNotNull(TypeDescriptor.GetAttributes(typeof(Target)).OfType<TestClassAttribute>().FirstOrDefault());
        }
    }

    public class Target
    {
    }

    [RegisterAsMetadataType(typeof(Target))]
    [TestClass]
    public class StringMetadata
    {
    }
}
