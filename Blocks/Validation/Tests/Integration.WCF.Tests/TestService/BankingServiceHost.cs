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
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS.TestService
{
    class Banking1ServiceHost : TestServiceHost<BankingServiceImplementation, IBankingService>
    {
        public Banking1ServiceHost() : base(Settings.Default.Bank1ServiceAddress)
        {
        }
    }

    class Banking2ServiceHost : TestServiceHost<BankingServiceImplementation, IBanking2Service>
    {
        public Banking2ServiceHost() : base(Settings.Default.Bank2ServiceAddress)
        {
        }
    }

    class Banking3ServiceHost : TestServiceHost<BankingServiceImplementation, IBanking3Service>
    {
        public Banking3ServiceHost() : base(Settings.Default.Bank3ServiceAddress)
        {
        }
    }

    class Banking4ServiceHost : TestServiceHost<BankingServiceImplementation, IBanking4Service>
    {
        public Banking4ServiceHost()
            : base(Settings.Default.Bank4ServiceAddress)
        {
        }
    }
}
