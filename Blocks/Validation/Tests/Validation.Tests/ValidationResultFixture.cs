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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationResultFixture
    {
        [TestMethod]
        public void CreatedValidationResultHasNoChildResults()
        {
            object target = new object();
            Validator validator = new MockValidator(false);

            ValidationResult validationResult = new ValidationResult("message", target, "key", "tag", validator);

            Assert.AreEqual("message", validationResult.Message);
            Assert.AreSame(target, validationResult.Target);
            Assert.AreEqual("key", validationResult.Key);
            Assert.AreEqual("tag", validationResult.Tag);
            Assert.AreSame(validator, validationResult.Validator);
            Assert.IsFalse(validationResult.NestedValidationResults.GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void CreatedValidationResultWithNestedResultsReturnsADifferentEnumerableWithThem()
        {
            object target = new object();
            Validator validator = new MockValidator(false);
            ValidationResult nestedResult1 = new ValidationResult("nested1", target, "key", "tag", validator);
            ValidationResult nestedResult2 = new ValidationResult("nested2", target, "key", "tag", validator);
            IList<ValidationResult> nestedResults = new List<ValidationResult>(new ValidationResult[] { nestedResult1, nestedResult2 });

            ValidationResult validationResult = new ValidationResult("message", target, "key", "tag", validator, nestedResults);

            Assert.AreEqual("message", validationResult.Message);
            Assert.AreSame(target, validationResult.Target);
            Assert.AreEqual("key", validationResult.Key);
            Assert.AreEqual("tag", validationResult.Tag);
            Assert.AreSame(validator, validationResult.Validator);
            IEnumerator<ValidationResult> nestedResultsEnumerator = validationResult.NestedValidationResults.GetEnumerator();
            Assert.IsTrue(nestedResultsEnumerator.MoveNext());
            Assert.AreSame(nestedResult1, nestedResultsEnumerator.Current);
            Assert.IsTrue(nestedResultsEnumerator.MoveNext());
            Assert.AreSame(nestedResult2, nestedResultsEnumerator.Current);
            Assert.IsFalse(nestedResultsEnumerator.MoveNext());
        }
    }
}
