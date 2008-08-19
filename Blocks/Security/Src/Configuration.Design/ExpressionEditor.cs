//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    internal class ExpressionEditor : UITypeEditor
    {
        private ExpressionEditorFormUI formUI;

        public ExpressionEditor()
        {
            formUI = new ExpressionEditorFormUI();
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			if (null == context) return value;
			if (null == provider) return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
            if (edSvc != null)
            {
                IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                string expression = (string)value;
                ConfigurationNode node = (ConfigurationNode)context.Instance;

                formUI.Expression = expression;
                formUI.RuleName = node.Name;

                DialogResult result = service.ShowDialog(formUI);
                if (result == DialogResult.OK)
                {
                    expression = formUI.Expression;
                    if (node.Name != formUI.RuleName)
                    {
                        INodeNameCreationService nameCreationService = node.Site.GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
                        node.Name = nameCreationService.GetUniqueName(formUI.RuleName, node, node.Parent);
                    }
                }
                return expression;
            }
            
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}