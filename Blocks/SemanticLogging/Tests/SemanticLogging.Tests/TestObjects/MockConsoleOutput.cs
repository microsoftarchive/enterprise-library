#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects
{
    public class MockConsoleOutput : IDisposable
    {
        private TextWriter writer;  
        private TextWriter originalOutput;

        public MockConsoleOutput() 
        {
            writer = new StringWriter();
            originalOutput = Console.Out; 
            Console.SetOut(writer); 
        }

        public string Ouput
        {
            get { return writer.ToString(); }
        }

        public void Dispose() 
        { 
            Console.SetOut(originalOutput); 
            writer.Dispose();
        }
    }
}
