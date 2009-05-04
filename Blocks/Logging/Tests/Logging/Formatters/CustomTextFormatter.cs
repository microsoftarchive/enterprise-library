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

using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    public class CustomTextFormatter : TextFormatter
    {
        public CustomTextFormatter(string template)
            : base(template, GetExtraTokenHandlersDictionary())
        { }

        private static IDictionary<string, TokenHandler<LogEntry>> GetExtraTokenHandlersDictionary()
        {
            Dictionary<string, TokenHandler<LogEntry>> tokenHandlers = new Dictionary<string, TokenHandler<LogEntry>>();
            tokenHandlers["field1"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField1);
            tokenHandlers["field2"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField2);
            tokenHandlers["field3"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((CustomLogEntry)le).AcmeCoField3);

            return tokenHandlers;
        }

        /// <summary>
        /// This implementation does not represent the recommended approach for handling additional tokens.
        /// </summary>
        public override string Format(LogEntry log)
        {
            StringBuilder templateBuilder = new StringBuilder(base.Format(log));

            CustomToken custom = new CustomToken();
            custom.Format(templateBuilder, log);

            return templateBuilder.ToString();
        }
    }
}

