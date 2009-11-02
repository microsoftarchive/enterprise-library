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
using Console.Wpf.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    public class ExpressionEditor : UITypeEditor
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
                ElementProperty expressionProperty = (ElementProperty)context;

                formUI.Expression = expression;
                formUI.RuleName = expressionProperty.DeclaringElement.Name;

                DialogResult result = service.ShowDialog(formUI);
                if (result == DialogResult.OK)
                {
                    expression = formUI.Expression;
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
