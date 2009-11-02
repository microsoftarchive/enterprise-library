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
                "Console.Wpf.ViewModel.BlockSpecifics.CryptographySectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            ///</summary>
            public const string KeyedHashAlgorithmProviderDataViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.KeyedHashAlgorithmProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            ///</summary>
            public const string SymmetricAlgorithmProviderDataViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.SymmetricAlgorithmProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddKeyedHashProviderCommand = "Console.Wpf.ViewModel.BlockSpecifics.KeyedHashAlgorithmProviderAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string AddSymmetricAlgorithmProviderCommand = "Console.Wpf.ViewModel.BlockSpecifics.SymmetricAlgorithmProviderAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
