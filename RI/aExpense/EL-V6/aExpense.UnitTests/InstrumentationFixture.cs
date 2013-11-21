// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace aExpense.UnitTests
{
    [TestClass]
    public class InstrumentationFixture
    {
        [TestMethod]
        public void AnalyzeAExpenseEvents()
        {
            EventSourceAnalyzer.InspectAll(AExpenseEvents.Log);
        }
    }
}
