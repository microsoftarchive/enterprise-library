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
using SlabReconfigurationWebRole.Events;

namespace SlabReconfigurationWebRole.Messaging
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
            QuickStartEventSource.Log.SendingMessage(recipient, message);

            try
            {
                if (this.randomFailure.Next(10) == 0)
                {
                    throw new InvalidOperationException("Random error sending message");
                }

                QuickStartEventSource.Log.MessageSent(recipient);
            }
            catch (InvalidOperationException e)
            {
                QuickStartEventSource.Log.MessageSendingFailed(recipient, e);
                throw;
            }
        }
    }
}