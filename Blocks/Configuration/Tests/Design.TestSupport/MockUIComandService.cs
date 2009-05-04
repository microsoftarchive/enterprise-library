//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport
{
	public class MockUIComandService : IUICommandService
	{

		private Dictionary<Type, List<ConfigurationUICommand>> list;

		public MockUIComandService()
		{
			list = new Dictionary<Type, List<ConfigurationUICommand>>();
		}

		public Dictionary<Type, List<ConfigurationUICommand>> List
		{
			get { return list; }
		}

		public void AddCommand(Type type, ConfigurationUICommand command)
		{
			if (!list.ContainsKey(type))
			{
				list.Add(type, new List<ConfigurationUICommand>());
			}
			List<ConfigurationUICommand> cmdList = list[type];
			cmdList.Add(command);
		}

		public void RemoveCommand(Type type, ConfigurationUICommand command)
		{
		}

		public void ForEach(Type type, Action<ConfigurationUICommand> action)
		{
		}
	}
}
