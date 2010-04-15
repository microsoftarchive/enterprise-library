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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]        
    public sealed class ExpressionEditor : UITypeEditor, IDisposable
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
                var bindableProperty = EditorUtility.GetBindableProperty(context);
                var expressionProperty = bindableProperty.Property;

                if (expressionProperty != null)
                {
                    formUI.Expression = expression;
                    formUI.RuleName = ((ILogicalPropertyContainerElement)expressionProperty).ContainingElementDisplayName;

                    DialogResult result = service.ShowDialog(formUI);
                    if (result == DialogResult.OK)
                    {
                        expression = formUI.Expression;
                    }
                    return expression;
                }
            }
            
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (formUI != null)
            {
                formUI.Dispose();
            }
        }

        #endregion
    }

#pragma warning restore 1591
}
