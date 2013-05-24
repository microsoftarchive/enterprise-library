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

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Service
{
    internal class Parameter
    {
        public Parameter(string name, string description, Action<ParameterSet> action)
        {
            this.Names = new HashSet<string>(name.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
            this.Action = action;
            this.Description = description;
        }

        public ICollection<string> Names { get; set; }

        public Action<ParameterSet> Action { get; set; }

        public string Description { get; set; }
    }
}
