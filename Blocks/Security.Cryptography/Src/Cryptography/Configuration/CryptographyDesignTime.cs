//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary/>
    public static class CryptographyDesignTime
    {
        ///<summary>
        /// ViewModel type names for Cryptography Application Block.
        ///</summary>
        public static class ViewModelTypeNames
        {
            ///<summary>
            ///</summary>
            public const string CryptographySectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CryptographySectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            ///</summary>
            public const string KeyedHashAlgorithmProviderDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.KeyedHashAlgorithmProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            ///</summary>
            public const string SymmetricAlgorithmProviderDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SymmetricAlgorithmProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddKeyedHashProviderCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.KeyedHashAlgorithmProviderAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string AddHashProviderCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.HashAlgorithmProviderAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string AddSymmetricAlgorithmProviderCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SymmetricAlgorithmProviderAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
