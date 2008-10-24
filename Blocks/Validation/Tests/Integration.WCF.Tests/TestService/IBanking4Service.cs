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

using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    [ServiceContract(Namespace = "http://tempuri.org/Banking4Service")]
    interface IBanking4Service
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [ValidationBehavior("Bank1Rules")]
        AccountSummary GetAccountSummary(CustomerInfo customer);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [ValidationBehavior("Bank2Rules")]
        AccountSummary GetAccountSummary2(CustomerInfo customer);
    }
}
