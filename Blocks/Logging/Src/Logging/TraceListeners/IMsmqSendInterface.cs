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
using System.Messaging;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Specifies the contract for a MSMQ interface object.
	/// </summary>
	public interface IMsmqSendInterface : IDisposable
	{
		/// <summary>
		/// Close the msmq.
		/// </summary>
		void Close();

		/// <summary>
		/// Send a message to the MSMQ.
		/// </summary>
		/// <param name="message">The <see cref="Message"/> to send.</param>
		/// <param name="transactionType">The <see cref="MessageQueueTransactionType"/> value that specifies the type of transaciton to use.</param>
		void Send(Message message, MessageQueueTransactionType transactionType);

		/// <summary>
		/// The transactional status of the MSMQ.
		/// </summary>
		bool Transactional { get; }
	}
}
