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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Implementation of the <see cref="MsmqSendInterfaceFactory"/> contract that deals with an actual MSMQ.
	/// </summary>
	public class MsmqSendInterfaceFactory : IMsmqSendInterfaceFactory
	{
		/// <summary>
		/// Returns a new instance of <see cref="MsmqSendInterface"/>
		/// </summary>
		/// <param name="queuePath">The MSMQ queue path.</param>
		/// <returns>The new MSMQ interface.</returns>
		public IMsmqSendInterface CreateMsmqInterface(string queuePath)
		{
			return new MsmqSendInterface(queuePath);
		}
	}
}
