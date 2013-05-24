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

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System;
using System.Diagnostics.Tracing;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects
{
    public class MockColorMapper : IConsoleColorMapper
    {
        public MockColorMapper()
        {
            Instance = this;
        }

        public const ConsoleColor Error = ConsoleColor.DarkRed;

        public ConsoleColor? Color { get; private set; }

        public ConsoleColor? Map(EventLevel eventLevel)
        {
            if (eventLevel == EventLevel.Error)
            {
                this.Color = Error;
            }
            return this.Color;
        }

        public static MockColorMapper Instance { get; private set; }
    }
}
