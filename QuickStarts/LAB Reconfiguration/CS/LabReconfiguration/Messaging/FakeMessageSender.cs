#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Quick Start
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace LabReconfiguration.Messaging
{
    public class FakeMessageSender : IMessageSender
    {
        private Random randomFailure;

        public FakeMessageSender()
        {
            this.randomFailure = new Random();
        }

        public void SendMessage(string recipient, string message)
        {
            Logger.Write(string.Format(CultureInfo.CurrentCulture, "Sending message to '{0}': {1}", recipient, message), "Messaging", 0, 1, TraceEventType.Verbose);

            try
            {
                if (this.randomFailure.Next(5) == 0)
                {
                    throw new InvalidOperationException("Random error sending message");
                }

                Logger.Write(string.Format(CultureInfo.CurrentCulture, "Message sent to '{0}'", recipient), "Messaging", 0, 2, TraceEventType.Information);
            }
            catch (InvalidOperationException e)
            {
                Logger.Write(string.Format(CultureInfo.CurrentCulture, "Sending message to '{0}' failed: {1}", recipient, e), "Messaging", 0, 3, TraceEventType.Error);
                throw;
            }
        }
    }
}