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

using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Implementation of <see cref="MsmqSendInterface"/> that forwards messages to an actual MSMQ.
	/// </summary>
	internal class MsmqSendInterface : IMsmqSendInterface
	{
		private MessageQueue messageQueue;

		internal MsmqSendInterface(string queuePath)
		{
			messageQueue = new MessageQueue(queuePath, false, true);
		}

		/// <summary>
		/// Closes the underlying MSMQ.
		/// </summary>
		public void Close()
		{
			messageQueue.Close() ;
		}

		/// <summary>
		/// Disposes the underlying MSMQ.
		/// </summary>
		public void Dispose()
		{
			messageQueue.Dispose();
		}

		/// <summary>
		/// Sends a message to the underlying MSMQ.
		/// </summary>
		/// <param name="message">The <see cref="Message"/> to send.</param>
		/// <param name="transactionType">The <see cref="MessageQueueTransactionType"/> value that specifies the type of transaciton to use.</param>
		public void Send(Message message, MessageQueueTransactionType transactionType)
		{
			messageQueue.Send(message, transactionType);
		}

		/// <summary>
		/// Returns the transactional status of the underlying MSMQ.
		/// </summary>
		public bool Transactional
		{
			get { return messageQueue.Transactional; }
		}
	}
}
