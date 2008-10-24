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
using System.ServiceModel;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [ServiceContract(Namespace = "http://tempuri.org/BankingService")]
    [ValidationBehavior("Bank2Rules")]
    internal interface IBanking2Service
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        AccountSummary GetAccountSummary(CustomerInfo customer);
    }
}
