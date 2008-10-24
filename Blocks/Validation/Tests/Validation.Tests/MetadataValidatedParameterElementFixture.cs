//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    /// <summary>
    /// The MetadataValidatedParameterElement class provides the reflection
    /// code needed to get the validation information supplied in method
    /// parameters. This fixture tests that this is working properly.
    /// </summary>
    [TestClass]
    public class MetadataValidatedParameterElementFixture
    {
        [TestMethod]
        public void ShouldBeCleanAtCreation()
        {
            MetadataValidatedParameterElement validatedElement =
                new MetadataValidatedParameterElement();
            Assert.IsNull(validatedElement.MemberInfo);
            Assert.IsNull(validatedElement.TargetType);
            Assert.AreEqual(CompositionType.And, validatedElement.CompositionType);
            Assert.IsFalse(validatedElement.IgnoreNulls);
            List<IValidatorDescriptor> descriptors =
                new List<IValidatorDescriptor>(validatedElement.GetValidatorDescriptors());
            Assert.AreEqual(0, descriptors.Count);
        }

        [TestMethod]
        public void ShouldReturnDescriptorsWhenUpdatedWithParameterInfo()
        {
            MetadataValidatedParameterElement validatedElement = new MetadataValidatedParameterElement();
            validatedElement.UpdateFlyweight(GetMethod1NameParameterInfo());
            Assert.IsNull(validatedElement.MemberInfo);
            Assert.IsNull(validatedElement.TargetType);
            Assert.AreEqual(CompositionType.And, validatedElement.CompositionType);
            Assert.IsFalse(validatedElement.IgnoreNulls);
            List<IValidatorDescriptor> descriptors =
                new List<IValidatorDescriptor>(validatedElement.GetValidatorDescriptors());
            Assert.AreEqual(3, descriptors.Count);
            // reflection doesn't preserve ordering of attributes. Therefore,
            // it doesn't preserve ordering of validators. Is this an issue?
            Assert.IsTrue(descriptors.Exists(delegate(IValidatorDescriptor d)
                                             {
                                                 return d is NotNullValidatorAttribute;
                                             }));
            Assert.IsTrue(descriptors.Exists(delegate(IValidatorDescriptor d)
                                             {
                                                 return d is StringLengthValidatorAttribute;
                                             }));
            Assert.IsTrue(descriptors.Exists(delegate(IValidatorDescriptor d)
                                             {
                                                 return d is RegexValidatorAttribute;
                                             }));
        }

        static ParameterInfo GetMethod1NameParameterInfo()
        {
            Type t = typeof(IMethodsWithValidators);
            MemberInfo[] members = t.GetMember("MethodWithOneParamAndThreeValidators");
            MethodInfo method = (MethodInfo)members[0];
            ParameterInfo param = method.GetParameters()[0];
            return param;
        }

        // A test interface we'll be reflecting on to get various parameters and their validation
        // attributes.
        interface IMethodsWithValidators
        {
            void MethodWithOneParamAndThreeValidators(
                [NotNullValidator] [StringLengthValidator(4, RangeBoundaryType.Inclusive, 32, RangeBoundaryType.Inclusive)] [RegexValidator(@"\d{3}-\d{2}-\d{4}[A-Z]*")] string bar);
        }
    }
}
