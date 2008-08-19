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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Specifies the contract for a provider of MSMQ interfaces.
	/// </summary>
	public interface IMsmqSendInterfaceFactory
	{
		/// <summary>
		/// Returns a new MSMQ interface.
		/// </summary>
		/// <param name="queuePath">The MSMQ queue path.</param>
		/// <returns>The new MSMQ interface.</returns>
		IMsmqSendInterface CreateMsmqInterface(string queuePath);
	}
}
