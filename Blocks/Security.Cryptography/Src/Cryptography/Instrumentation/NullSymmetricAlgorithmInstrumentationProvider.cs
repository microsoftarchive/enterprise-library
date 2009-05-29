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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary/>
    public class NullSymmetricAlgorithmInstrumentationProvider : ISymmetricAlgorithmInstrumentationProvider
    {
        
        /// <summary/>
        public void FireCyptographicOperationFailed(string message, Exception exception)
        {
        }
        
        /// <summary/>
        public void FireSymmetricDecryptionPerformed()
        {
        }

        /// <summary/>
        public void FireSymmetricEncryptionPerformed()
        {
        }
    }
}
