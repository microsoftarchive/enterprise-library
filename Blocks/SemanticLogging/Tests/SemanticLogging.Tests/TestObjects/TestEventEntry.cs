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
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects
{
    public class TestEventEntry
    {
        public int EventId { get; set; }

        public Guid ProviderId { get; set; }

        public string EventSourceName { get; set; }

        public string Message { get; set; }

        public long EventKeywords { get; set; }

        public int Level { get; set; }

        public int Opcode { get; set; }

        public int Task { get; set; }

        public int Version { get; set; }

        public Dictionary<string, object> Payload { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
