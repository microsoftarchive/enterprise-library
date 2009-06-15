//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
#pragma warning disable 618
    public class CustomToken : TokenFunction
    {
        public CustomToken() : base("[[AcmeDBLookup{", "}]]")
        {
        }

        public override string FormatToken(string tokenTemplate, LogEntry log)
        {
            return "1234";
        }
    }
#pragma warning restore 618
}

