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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class ValidationResultsFixture
    {
        [TestMethod]
        public void ValidationResultsWithFailureResultIsFail()
        {
            ValidationResults validationResults = new ValidationResults();

            validationResults.AddResult(new ValidationResult("message1", null, null, null, null));

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ValidationResultsWithMultipleFailureResultsIsFail()
        {
            ValidationResults validationResults = new ValidationResults();

            validationResults.AddResult(new ValidationResult("message1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("message2", null, null, null, null));

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ValidationResultsWithNoFailureResultIsSuccess()
        {
            ValidationResults validationResults = new ValidationResults();

            Assert.IsTrue(validationResults.IsValid);
        }

		[TestMethod]
		public void ReturnsActualResultsCount()
		{
			ValidationResults validationResults = new ValidationResults();
			Assert.AreEqual(0, validationResults.Count);
			validationResults.AddResult(new ValidationResult("message1", null, null, null, null));
			Assert.AreEqual(1, validationResults.Count);
			validationResults.AddResult(new ValidationResult("message2", null, null, null, null));
			Assert.AreEqual(2, validationResults.Count);
		}

        [TestMethod]
        public void CanEnumerateThroughGenericEnumeratorInterface()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("message1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("message2", null, null, null, null));

            IEnumerator<ValidationResult> enumerator = (validationResults as IEnumerable<ValidationResult>).GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("message1", enumerator.Current.Message);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("message2", enumerator.Current.Message);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void CanEnumerateThroughNonGenericEnumeratorInterface()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("message1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("message2", null, null, null, null));

            IEnumerator enumerator = (validationResults as IEnumerable).GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("message1", ((ValidationResult)enumerator.Current).Message);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("message2", ((ValidationResult)enumerator.Current).Message);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void FindAllReturnsNewInstance()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, "tag1");

            Assert.AreNotSame(validationResults, filteredValidationResults);
        }

        [TestMethod]
        public void CanFilterWithIncludeWithSingleTag()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, "tag1");

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(2, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 2"));
        }

        [TestMethod]
        public void CanFilterWithIncludeWithMultipleTags()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, "tag1", "tag3");

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(3, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 2"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag3 - 1"));
        }

        [TestMethod]
        public void CanFilterWithIncludeWithSingleNullTag()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, null);

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(2, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("without tag - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("without tag - 2"));
        }

        [TestMethod]
        public void CanFilterWithIncludeWithMultipleTagsIncludingNull()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, null, "tag2");

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(3, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag2 - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("without tag - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("without tag - 2"));
        }

        [TestMethod]
        public void RepeatedTagsDoNotResultInRepeatedResults()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Include, "tag2", "tag2");

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(1, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag2 - 1"));
        }

        [TestMethod]
        public void CanFilterWithIgnoreWithMultipleTagsIncludingNull()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("with tag1 - 1", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag1 - 2", null, null, "tag1", null));
            validationResults.AddResult(new ValidationResult("with tag2 - 1", null, null, "tag2", null));
            validationResults.AddResult(new ValidationResult("with tag3 - 1", null, null, "tag3", null));
            validationResults.AddResult(new ValidationResult("without tag - 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("without tag - 2", null, null, null, null));

            ValidationResults filteredValidationResults = validationResults.FindAll(TagFilter.Ignore, null, "tag2");

            IDictionary<string, ValidationResult> filteredResultsMapping = ValidationTestHelper.GetResultsMapping(filteredValidationResults);
            Assert.AreEqual(3, filteredResultsMapping.Count);
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 1"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag1 - 2"));
            Assert.IsTrue(filteredResultsMapping.ContainsKey("with tag3 - 1"));
        }

        [TestMethod]
        public void CanAddValidationResultsFromCollection()
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.AddResult(new ValidationResult("existing 1", null, null, null, null));
            validationResults.AddResult(new ValidationResult("existing 2", null, null, null, null));
            ValidationResult[] newValidationResults = new ValidationResult[]
                {
                    new ValidationResult("new 1", null, null, null, null),
                    new ValidationResult("new 2", null, null, null, null),
                    new ValidationResult("new 3", null, null, null, null)
                };

            validationResults.AddAllResults(newValidationResults);

            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(5, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("existing 1"));
            Assert.IsTrue(resultsMapping.ContainsKey("existing 2"));
            Assert.IsTrue(resultsMapping.ContainsKey("new 1"));
            Assert.IsTrue(resultsMapping.ContainsKey("new 2"));
            Assert.IsTrue(resultsMapping.ContainsKey("new 3"));
        }

        [TestMethod]
        public void ResultsCanBeSerializedAndDeserialized()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            byte[] serializedResults = null;

            ValidationResults validationResults = new ValidationResults();
            ValidationResult validationResult = new ValidationResult("message", this, "key", "tag", new MockValidator(false));
            validationResults.AddResult(validationResult);

            using (MemoryStream binaryStream = new MemoryStream())
            {
                formatter.Serialize(binaryStream, validationResults);

                serializedResults = binaryStream.ToArray();
            }

            ValidationResults deserializedValidationResults = null;
            using (MemoryStream binaryStream = new MemoryStream(serializedResults))
            {
                deserializedValidationResults = (ValidationResults)formatter.Deserialize(binaryStream);
            }

            Assert.IsNotNull(deserializedValidationResults);
            Assert.AreNotSame(validationResults, deserializedValidationResults);
            IList<ValidationResult> resultsList = ValidationTestHelper.GetResultsList(deserializedValidationResults);
            Assert.AreEqual(1, resultsList.Count);
            Assert.IsNotNull(resultsList[0]);
            Assert.AreNotSame(validationResult, resultsList[0]);
            Assert.AreEqual("message", resultsList[0].Message);
            Assert.AreEqual("tag", resultsList[0].Tag);
            Assert.AreEqual("key", resultsList[0].Key);
            Assert.IsNull(resultsList[0].Target);
            Assert.IsNull(resultsList[0].Validator);
        }
    }
}
