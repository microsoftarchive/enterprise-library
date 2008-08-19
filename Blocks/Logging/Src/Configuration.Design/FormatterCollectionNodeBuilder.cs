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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class FormatterCollectionNodeBuilder : NodeBuilder
	{
		private NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> formatters;
		private FormatterCollectionNode node;

        public FormatterCollectionNodeBuilder(IServiceProvider serviceProvider, NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData> formatters)
			: base(serviceProvider)
		{
			this.formatters = formatters;
		}

		public FormatterCollectionNode Build()
		{
			node = new FormatterCollectionNode();
			formatters.ForEach(new Action<FormatterData>(CreateFormatterNode));
			return node;
		}

		private void CreateFormatterNode(FormatterData formatterData)
		{
			FormatterNode formatterNode = NodeCreationService.CreateNodeByDataType(formatterData.GetType(), new object[] { formatterData }) as FormatterNode;
			if (null == formatterNode)
			{
				LogNodeMapError(node, formatterData.GetType());
				return;
			}
			node.AddNode(formatterNode);
		}
	}
}
