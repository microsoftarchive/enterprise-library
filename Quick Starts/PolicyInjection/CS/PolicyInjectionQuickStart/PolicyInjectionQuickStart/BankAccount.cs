//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;

namespace PolicyInjectionQuickStart.BusinessLogic
{
    public class BankAccount : MarshalByRefObject
    {
        private decimal balance;

        public decimal GetCurrentBalance()
        {
            return balance;
        }

        [ValidationCallHandler]
        public void Deposit([RangeValidator(typeof(Decimal), "0.0", RangeBoundaryType.Exclusive, "0.0", RangeBoundaryType.Ignore, MessageTemplate = "Deposited amount must be more than zero." )] 
            decimal depositAmount)
        {
            balance += depositAmount;
        }

        [ValidationCallHandler]
        public void Withdraw([RangeValidator(typeof(Decimal), "0.0", RangeBoundaryType.Exclusive, "1000.0", RangeBoundaryType.Inclusive, MessageTemplate="Withdrawal amount must be between zero and 1000.")]
            decimal withdrawAmount)
        {
            if (withdrawAmount > balance)
            {
                throw new ArithmeticException();
            }
            balance -= withdrawAmount;
        }
    }
}
