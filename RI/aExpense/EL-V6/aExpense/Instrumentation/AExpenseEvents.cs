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
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense.Instrumentation
{
    [EventSource(Name = "aExpense")]
    public class AExpenseEvents : EventSource
    {
        public static class Keywords
        {
            public const EventKeywords Application = (EventKeywords)1L;
            public const EventKeywords DataAccess = (EventKeywords)2L;
            public const EventKeywords UserInterface = (EventKeywords)4L;
            public const EventKeywords General = (EventKeywords)8L;
        }

        public static class Tasks
        {
            public const EventTask LoadUser = (EventTask)1;
            public const EventTask LoadExpenses = (EventTask)2;
            public const EventTask LoadAllExpenses = (EventTask)3;
            public const EventTask SaveExpense = (EventTask)4;
            public const EventTask GetExpensesForApproval = (EventTask)5;
            public const EventTask Initialize = (EventTask)6;
            public const EventTask Tracing = (EventTask)7;
            public const EventTask ApproveExpense = (EventTask)8;
        }

        public static class Opcodes
        {
            public const EventOpcode Start = (EventOpcode)20;
            public const EventOpcode Finish = (EventOpcode)21;
            public const EventOpcode Error = (EventOpcode)22;
            public const EventOpcode Starting = (EventOpcode)23;

            public const EventOpcode QueryStart = (EventOpcode)30;
            public const EventOpcode QueryFinish = (EventOpcode)31;
            public const EventOpcode QueryNoResults = (EventOpcode)32;

            public const EventOpcode CacheQuery = (EventOpcode)40;
            public const EventOpcode CacheUpdate = (EventOpcode)41;
            public const EventOpcode CacheHit = (EventOpcode)42;
            public const EventOpcode CacheMiss = (EventOpcode)43;
        }

        public static readonly AExpenseEvents Log = new AExpenseEvents();

        #region Application

        [Event(100, Level = EventLevel.Verbose, Keywords = Keywords.Application, Task = Tasks.Initialize, Opcode = Opcodes.Starting, Version = 1)]
        public void ApplicationStarting()
        {
            if (this.IsEnabled(EventLevel.Verbose, Keywords.Application))
            {
                this.WriteEvent(100);
            }
        }

        [Event(101, Level = EventLevel.Informational, Keywords = Keywords.Application, Task = Tasks.Initialize, Opcode = Opcodes.Start, Version = 1)]
        public void ApplicationStarted()
        {
            if (this.IsEnabled(EventLevel.Informational, Keywords.Application))
            {
                this.WriteEvent(101);
            }
        }

        [Event(102, Level = EventLevel.Error, Keywords = Keywords.Application, Task = Tasks.Tracing, Opcode = Opcodes.Error, Version = 1)]
        public void ApplicationError(string exceptionMessage, string exceptionType)
        {
            if (this.IsEnabled(EventLevel.Error, Keywords.Application))
            {
                this.WriteEvent(102, exceptionMessage, exceptionType);
            }
        }
        
        #endregion 

        #region Data Access

        #region Initialization

        [Event(250, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.Initialize, Version = 1)]
        public void ExpenseRepositoryInitialized()
        {
            if (this.IsEnabled(EventLevel.Informational, Keywords.DataAccess))
            {
                this.WriteEvent(250);
            }
        }

        [Event(251, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.Initialize, Version = 1)]
        public void UserRepositoryInitialized(string applicationName)
        {
            if (this.IsEnabled(EventLevel.Informational, Keywords.DataAccess))
            {
                this.WriteEvent(251, applicationName);
            }
        }

        [Event(252, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.Initialize, Version = 1)]
        public void ProfileStoreInitialized()
        {
            if (this.IsEnabled(EventLevel.Informational, Keywords.DataAccess))
            {
                this.WriteEvent(252);
            }
        }

        #endregion

        #region SaveExpense

        [Event(290, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.Start, Version = 1)]
        public void SaveExpenseStarted(Guid expenseId, string title)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(290, expenseId, title);
            }
        }

        [Event(291, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.Finish, Version = 1)]
        public void SaveExpenseFinished(Guid expenseId, string title)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(291, expenseId, title);
            }
        }

        [Event(292, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.CacheUpdate, Version = 1)]
        public void SaveExpenseCacheUpdated(Guid expenseId)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(292, expenseId);
            }
        }

        [Event(293, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.CacheUpdate, Version = 1)]
        public void SaveExpenseCacheRemoved(Guid expenseId, string cacheKey)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(293, expenseId, cacheKey);
            }
        }

        [Event(294, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.QueryStart, Version = 1)]
        public void SaveExpenseInsertStarted(Guid expenseId, string title)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(294, expenseId, title);
            }
        }

        [Event(295, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.SaveExpense, Opcode = Opcodes.QueryFinish, Version = 1)]
        public void SaveExpenseInsertFinished(Guid expenseId, string title)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(295, expenseId, title);
            }
        }

        #endregion

        #region GetAllExpenses

        [Event(300, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.LoadAllExpenses, Opcode = Opcodes.Start, Version = 1)]
        public void GetAllExpensesStarted()
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(300);
            }
        }

        [Event(301, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.LoadAllExpenses, Opcode = Opcodes.Finish, Version = 1)]
        public void GetAllExpensesFinished(int count)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(301, count);
            }
        }

        #endregion

        #region GetExpensesForApproval

        [Event(320, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.Start, Version = 1)]
        public void GetExpensesForApprovalStarted(string approverName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(320, approverName);
            }
        }

        [Event(321, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.Finish, Version = 1)]
        public void GetExpensesForApprovalFinished(string approverName, int count)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(321, approverName, count);
            }
        }

        [Event(322, Level = EventLevel.Error, Keywords = Keywords.DataAccess, Task = Tasks.LoadAllExpenses, Opcode = Opcodes.Error, Version = 1)]
        public void GetExpensesForApprovalFailed(string approverName, string exceptionMessage, string exceptionType)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(322, approverName, exceptionMessage, exceptionType);
            }
        }

        [Event(323, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.CacheHit, Version = 1)]
        public void GetExpensesForApprovalCacheHit(string approverName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(323, approverName);
            }
        }

        [Event(324, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.QueryFinish, Version = 1)]
        public void GetExpensesForApprovalQueryFinished(string approverName, int count)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(324, approverName, count);
            }
        }

        [Event(325, Level = EventLevel.Warning, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.QueryNoResults, Version = 1)]
        public void GetExpensesForApprovalQueryNoResults(string approverName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(325, approverName);
            }
        }

        [Event(326, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.CacheUpdate, Version = 1)]
        public void GetExpensesForApprovalCacheUpdate(string approverName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(326, approverName);
            }
        }
        
        [Event(327, Level = EventLevel.Verbose, Keywords = Keywords.DataAccess, Task = Tasks.GetExpensesForApproval, Opcode = Opcodes.QueryStart, Version = 1)]
        public void GetExpensesForApprovalQueryStarted(string approverName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(327, approverName);
            }
        }

        #endregion

        #region Approve Expense

        //ExpenseApproved
        [Event(400, Level = EventLevel.Informational, Keywords = Keywords.DataAccess, Task = Tasks.ApproveExpense, Version = 1, Message = "The expense '{0}' for user '{1}' was approved by user '{2}'.")]
        public void ExpenseApproved(string title, string user, string approver)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(400, title, user, approver);
            }
        }

        #endregion

        #endregion

        #region User Interface

        [Event(330, Level = EventLevel.Informational, Keywords = Keywords.UserInterface, Task = Tasks.SaveExpense, Opcode = Opcodes.Start, Version = 1)]
        public void UiSaveExpenseStarted()
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(330);
            }
        }

        [Event(331, Level = EventLevel.Informational, Keywords = Keywords.UserInterface, Task = Tasks.SaveExpense, Opcode = Opcodes.Finish, Version = 1)]
        public void UiSaveExpenseFinished()
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(331);
            }
        }

        [Event(332, Level = EventLevel.Error, Keywords = Keywords.UserInterface, Task = Tasks.SaveExpense, Opcode = Opcodes.Error, Version = 1)]
        public void UiSaveExpenseFailed(string exceptionMessage, string exceptionType)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(332, exceptionMessage, exceptionType);
            }
        }

        #endregion

        #region General

        [Event(1000, Level = EventLevel.Error, Keywords = Keywords.General, Task = Tasks.Tracing, Opcode = Opcodes.Error, Version = 1)]
        public void ExceptionHandlerLoggedException(string message, Guid id)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(1000, message, id);
            }
        }

        [Event(1001, Level = EventLevel.Informational, Keywords = Keywords.General, Task = Tasks.Tracing, Version = 1, Message = "Method {0}.{1} executed")]
        public void TracingBehaviorVirtualMethodIntercepted(string declaringType, string methodBaseName, string arguments)
        {
            this.WriteEvent(1001, declaringType, methodBaseName, arguments);
        }

        [NonEvent]
        public void TracingBehaviorVirtualMethodIntercepted(IMethodInvocation input)
        {
            // Check before making a long calculation or time consuming operation
            // to avoid the perf penalty in case the trace is disabled.
            if (this.IsEnabled(EventLevel.Informational, Keywords.General))
            {
                // Only log methods with arguments (bypass getters)
                if (input.Arguments.Count > 0)
                {
                    string arguments = string.Join(",", input.Arguments.OfType<object>());
                    this.TracingBehaviorVirtualMethodIntercepted(input.MethodBase.DeclaringType.FullName, input.MethodBase.Name, arguments);
                }
            }
        }

        [Event(1002, Level = EventLevel.Informational, Keywords = Keywords.General, Task = Tasks.Tracing, Version = 1, Opcode = Opcodes.Start)]
        public void LogCallHandlerPreInvoke(string declaringType, string method)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(1002, declaringType, method);
            }
        }

        [Event(1003, Level = EventLevel.Informational, Keywords = Keywords.General, Task = Tasks.Tracing, Version = 1, Opcode = Opcodes.Finish)]
        public void LogCallHandlerPostInvoke(string declaringType, string method, long elapsedMilliseconds)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(1003, declaringType, method, elapsedMilliseconds);
            }
        }

        #endregion
    }
}