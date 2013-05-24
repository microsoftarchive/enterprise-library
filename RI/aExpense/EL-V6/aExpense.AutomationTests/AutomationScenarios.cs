#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using aExpense.AutomationTests.Util;
using System.Linq;


namespace aExpense.AutomationTests
{
    /// <summary>
    /// Summary description for AutomationScenarios
    /// </summary>
    [CodedUITest]
    public class AutomationScenarios
    {
        private static readonly string aExpenseSitePath = Path.GetFullPath(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["aExpenseSiteRelativePath"])); 
        private static  readonly string aExpenseSitePort = ConfigurationManager.AppSettings["aExpenseSitePort"];
        private static readonly string localDBConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ToString();
        private const string ProcessIISExpress = "iisexpresstray";

        [ClassInitialize]
        public static void AutomationInitialize(TestContext testContext)
        {
            var processes = Process.GetProcessesByName(ProcessIISExpress);
            foreach(var pr in processes)
            {
                pr.Kill();
            }


            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = string.Format("/path:\"{0}\" /port:{1}", aExpenseSitePath, aExpenseSitePort)
            };

            var programfiles = string.IsNullOrEmpty(startInfo.EnvironmentVariables["programfiles"])
                                ? startInfo.EnvironmentVariables["programfiles(x86)"]
                                : startInfo.EnvironmentVariables["programfiles"];

            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";

            Process.Start(startInfo);

            DatabaseHelper.DeleteAllNonDefaultExpenses(localDBConnectionString);
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            var processes = Process.GetProcessesByName(ProcessIISExpress);
            foreach (var pr in processes)
            {
                pr.Kill();
            }

        }

        [TestInitialize]
        public void TestInit()
        {
            BrowserWindow browser = BrowserWindow.Launch(new System.Uri("about:blank"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.UIMap.LogoutAndExit();
            DatabaseHelper.DeleteAllNonDefaultExpenses(localDBConnectionString);
        }

        [TestMethod]
        public void TitleValidationInExpense()
        {
            this.UIMap.TryCreateAnExpenseWithEmptyTitle();
            this.UIMap.AssertTitleValidation();

        }

        [TestMethod]
        public void DescriptionAndAmountValidationInExpenseItem()
        {
            this.UIMap.TryCreateAnInvalidExpenseItem();
            this.UIMap.AssertExpenseItemValidation();
        }

        [TestMethod]
        public void ExpenseIsCreatedOK()
        {
            this.UIMap.CreateAnExpense();
            this.UIMap.AssertExpenseIsOK();

            var expenses = DatabaseHelper.GetExpenses(localDBConnectionString);

            var expense = expenses.SingleOrDefault(e => e.Date.Year == DateTime.Today.Year);

            Assert.IsNotNull(expense);
            Assert.AreEqual(1000, expense.Amount);
            Assert.AreEqual("testExpense", expense.Title);
            Assert.AreEqual(false, expense.Approved);
            Assert.AreNotEqual(expense.UserName, expense.Approver);

            var details = DatabaseHelper.GetExpenseDetailsForExpense(localDBConnectionString, expense.Id);

            Assert.IsNotNull(details);
            Assert.AreEqual(1, details.Count);
            var detail = details.First();

            Assert.AreEqual(1000, detail.Amount);
            Assert.AreEqual("item1", detail.Description);

            
        }

        [TestMethod]
        public void ExpenseApprovalIsOK()
        {
            this.UIMap.CreateAnExpenseAndApproveIt();
            this.UIMap.AssertExpenseIsApproved();

            var expenses = DatabaseHelper.GetExpenses(localDBConnectionString);

            var expense = expenses.SingleOrDefault(e => e.Date.Year == DateTime.Today.Year);
            
            Assert.IsNotNull(expense);
            Assert.AreEqual(1002, expense.Amount);
            Assert.AreEqual("expenseForApproval", expense.Title);
            Assert.AreEqual(true, expense.Approved);
            Assert.AreNotEqual(expense.UserName, expense.Approver);

            var details = DatabaseHelper.GetExpenseDetailsForExpense(localDBConnectionString, expense.Id);

            Assert.IsNotNull(details);
            Assert.AreEqual(1, details.Count);
            var detail = details.First();

            Assert.AreEqual(1002, detail.Amount);
            Assert.AreEqual("testItem1", detail.Description);
        }

        [TestMethod]
        public void ValidateInvalidApprover()
        {
            this.UIMap.CreateExpenseWithWrongApprover();
            //this.UIMap.AssertErrorMessageIsDisplayed();
            this.UIMap.AssertWrongApprover();
        }


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
