//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionManagerFixture
    {
        private string policyName;

        [TestInitialize]
        public void SetUp()
        {
            policyName = null;
            TestExceptionHandler.HandlingNames.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationWithNullPolicyDictionaryThrows()
        {
            new ExceptionManagerImpl(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleWithNullNameThrows()
        {
            new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(new Exception(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleWithNullExceptionThrows()
        {
            new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(null, "policy");
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleWithNonExistingPolicyThrows()
        {
            new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>()).HandleException(new Exception(), "policy");
        }

        [TestMethod]
        public void HandleWithNonExistingPolicyFiresInstrumentation()
        {
            ExceptionManager manager = new ExceptionManagerImpl(
                new Dictionary<string, ExceptionPolicyImpl>(),
                new TestInstrumentationProvider(this));

            try
            {
                manager.HandleException(new Exception(), "policy");
                Assert.Fail("should have thrown");
            }
            catch (ExceptionHandlingException)
            {
                Assert.IsNotNull(policyName);
                Assert.AreEqual("policy", policyName);
            }
        }

        private class TestInstrumentationProvider : IDefaultExceptionHandlingInstrumentationProvider
        {
            private readonly ExceptionManagerFixture outer;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public TestInstrumentationProvider(ExceptionManagerFixture outer)
            {
                this.outer = outer;
            }

            /// <summary>
            /// Fires the ExceptionHandlingErrorOccurred"/> event.
            /// </summary>
            /// <param name="policyName">The name of the policy involved with the error.</param>
            /// <param name="message">The message that describes the failure.</param>
            public void FireExceptionHandlingErrorOccurred(string policyName, string message)
            {
                outer.policyName = policyName;
            }
        }

        [TestMethod]
        public void HandleForwardsHandlingToConfiguredExceptionEntry()
        {
            var policies = new Dictionary<string, ExceptionPolicyImpl>();
            var policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {
                    typeof (ArithmeticException),
                    new ExceptionPolicyEntry(typeof (ArithmeticException),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[] {new TestExceptionHandler("handler11")})
                },
                {
                    typeof (ArgumentException),
                    new ExceptionPolicyEntry(typeof (ArgumentException),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[] {new TestExceptionHandler("handler12")})
                },
                {
                    typeof (ArgumentOutOfRangeException),
                    new ExceptionPolicyEntry(typeof (ArgumentOutOfRangeException),
                        PostHandlingAction.None,
                        new IExceptionHandler[] {new TestExceptionHandler("handler13")})
                }
            };

            policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

            ExceptionManager manager = new ExceptionManagerImpl(policies);

            // is the exception rethrown?
            Exception thrownException = new ArithmeticException();
            Assert.IsTrue(manager.HandleException(thrownException, "policy1"));
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);

            // is the new exception thrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentException();
            try
            {
                manager.HandleException(thrownException, "policy1");
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                Assert.AreSame(thrownException, e.InnerException);
                Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
                Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);
            }

            // is the exception swallowed? action == None
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentOutOfRangeException();
            Assert.IsFalse(manager.HandleException(thrownException, "policy1"));
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

            // is the unknwon exception rethrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new Exception();
            Assert.IsTrue(manager.HandleException(thrownException, "policy1"));
            Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
        }

        [TestMethod]
        public void HandleWithReturnForwardsHandlingToConfiguredExceptionEntry()
        {
            var policies = new Dictionary<string, ExceptionPolicyImpl>();
            var policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {
                    typeof (ArithmeticException),
                    new ExceptionPolicyEntry(typeof (ArithmeticException),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[] {new TestExceptionHandler("handler11")})
                },
                {
                    typeof (ArgumentException),
                    new ExceptionPolicyEntry(typeof (ArgumentException),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[] {new TestExceptionHandler("handler12")})
                },
                {
                    typeof (ArgumentOutOfRangeException),
                    new ExceptionPolicyEntry(typeof (ArgumentOutOfRangeException),
                        PostHandlingAction.None,
                        new IExceptionHandler[] {new TestExceptionHandler("handler13")})
                }
            };

            policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

            ExceptionManager manager = new ExceptionManagerImpl(policies);

            Exception exceptionToThrow;

            // is the exception rethrown?
            Exception thrownException = new ArithmeticException();
            Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
            Assert.IsNull(exceptionToThrow);
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);

            // is the new exception thrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentException();
            Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
            Assert.AreSame(thrownException, exceptionToThrow.InnerException);
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);

            // is the exception swallowed? action == None
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentOutOfRangeException();
            Assert.IsFalse(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
            Assert.IsNull(exceptionToThrow);
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

            // is the unknwon exception rethrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new Exception();
            Assert.IsTrue(manager.HandleException(thrownException, "policy1", out exceptionToThrow));
            Assert.IsNull(exceptionToThrow);
            Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
        }

        [TestMethod]
        public void ProcessForwardsHandlingToConfiguredExceptionEntry()
        {
            var policies = new Dictionary<string, ExceptionPolicyImpl>();
            var policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {
                    typeof (ArithmeticException),
                    new ExceptionPolicyEntry(typeof (ArithmeticException),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[] {new TestExceptionHandler("handler11")})
                },
                {
                    typeof (ArgumentException),
                    new ExceptionPolicyEntry(typeof (ArgumentException),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[] {new TestExceptionHandler("handler12")})
                },
                {
                    typeof (ArgumentOutOfRangeException),
                    new ExceptionPolicyEntry(typeof (ArgumentOutOfRangeException),
                        PostHandlingAction.None,
                        new IExceptionHandler[] {new TestExceptionHandler("handler13")})
                }
            };
            policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

            ExceptionManager manager = new ExceptionManagerImpl(policies);

            // is the exception rethrown?
            Exception thrownException = new ArithmeticException();
            try
            {
                Exception ex1 = thrownException;
                manager.Process( () => { throw ex1; }, "policy1");
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                Assert.AreSame(thrownException, e);
                Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
                Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);
            }

            // is the new exception thrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentException();
            try
            {
                Exception ex2 = thrownException;
                manager.Process(() => { throw ex2; }, "policy1");
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                Assert.AreSame(thrownException, e.InnerException);
                Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
                Assert.AreEqual("handler12", TestExceptionHandler.HandlingNames[0]);
            }

            // is the exception swallowed? action == None
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentOutOfRangeException();
            Exception ex3 = thrownException;
            manager.Process(() => { throw ex3; }, "policy1");
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);

            // is the unknwon exception rethrown?
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new Exception();
            try
            {
                manager.Process(
                    () => { throw (thrownException); },
                    "policy1");
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                Assert.AreSame(thrownException, e);
                Assert.AreEqual(0, TestExceptionHandler.HandlingNames.Count);
            }
        }

        [TestMethod]
        public void ProcessWithReturnValueReturnsCorrectValueWhenNoExceptionThrown()
        {
            var policies = new Dictionary<string, ExceptionPolicyImpl>();
            var policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {
                    typeof (ArithmeticException),
                    new ExceptionPolicyEntry(typeof (ArithmeticException),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[] {new TestExceptionHandler("handler11")})
                },
                {
                    typeof (ArgumentException),
                    new ExceptionPolicyEntry(typeof (ArgumentException),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[] {new TestExceptionHandler("handler12")})
                },
                {
                    typeof (ArgumentOutOfRangeException),
                    new ExceptionPolicyEntry(typeof (ArgumentOutOfRangeException),
                        PostHandlingAction.None,
                        new IExceptionHandler[] {new TestExceptionHandler("handler13")})
                }
            };
            policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

            ExceptionManager manager = new ExceptionManagerImpl(policies);

            int result = manager.Process(() => 42, "policy1");
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ProcessWithReturnValueProcessesExceptionsOnThrow()
        {
            var policies = new Dictionary<string, ExceptionPolicyImpl>();
            var policy1Entries = new Dictionary<Type, ExceptionPolicyEntry>
            {
                {
                    typeof (ArithmeticException),
                    new ExceptionPolicyEntry(typeof (ArithmeticException),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[] {new TestExceptionHandler("handler11")})
                },
                {
                    typeof (ArgumentException),
                    new ExceptionPolicyEntry(typeof (ArgumentException),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[] {new TestExceptionHandler("handler12")})
                },
                {
                    typeof (ArgumentOutOfRangeException),
                    new ExceptionPolicyEntry(typeof (ArgumentOutOfRangeException),
                        PostHandlingAction.None,
                        new IExceptionHandler[] {new TestExceptionHandler("handler13")})
                }
            };
            policies.Add("policy1", new ExceptionPolicyImpl("policy1", policy1Entries));

            ExceptionManager manager = new ExceptionManagerImpl(policies);

            // is the exception rethrown?
            Exception thrownException = new ArithmeticException();
            try
            {
                Exception ex1 = thrownException;
                int result = manager.Process(() => { throw ex1;
#pragma warning disable 162 // Unreachable code
                                                       return 37;
#pragma warning restore 162
                }, "policy1");
                Assert.Fail("should have thrown");
            }
            catch (Exception e)
            {
                Assert.AreSame(thrownException, e);
                Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
                Assert.AreEqual("handler11", TestExceptionHandler.HandlingNames[0]);
            }

            // is the exception swallowed? action == None
            TestExceptionHandler.HandlingNames.Clear();
            thrownException = new ArgumentOutOfRangeException();
            Exception ex3 = thrownException;
            int swallowedResult = manager.Process(() => { throw ex3;
#pragma warning disable 162 // Unreachable code
                                                   return 17; 
#pragma warning restore 162
            }, -20, "policy1");
            Assert.AreEqual(1, TestExceptionHandler.HandlingNames.Count);
            Assert.AreEqual("handler13", TestExceptionHandler.HandlingNames[0]);
            Assert.AreEqual(-20, swallowedResult);            
        }
    }

    internal class TestExceptionHandler : IExceptionHandler
    {
        static TestExceptionHandler()
        {
            HandlingNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the TestExceptionHandler class.
        /// </summary>
        public TestExceptionHandler(string name)
        {
            Name = name;
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            HandlingNames.Add(Name);

            var newException = new Exception("foo", exception);

            return newException;
        }

        public string Name { get; private set; }

        public static IList<string> HandlingNames { get; private set; }
    }
}
