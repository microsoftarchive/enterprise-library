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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    class BankingServiceImplementation : IBankingService, IBanking2Service, IBanking3Service, IBanking4Service
    {
        public AccountSummary GetAccountSummary(CustomerInfo customer)
        {
            return new AccountSummary();
        }

        public AccountSummary GetAccountSummary2(CustomerInfo customer)
        {
            return new AccountSummary();
        }
    }
}
