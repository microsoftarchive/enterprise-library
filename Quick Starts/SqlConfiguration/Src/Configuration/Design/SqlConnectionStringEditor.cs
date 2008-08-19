//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
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
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Properties;


namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design
{
    /// <summary>
    /// <para>Provides a user interface for saving a file name.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class SqlConnectionStringEditor : UITypeEditor
    {
        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by GetEditStyle.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para>
        /// </param>
        /// <param name="provider">
        /// <para>An <see cref="IServiceProvider"/> that this editor can use to obtain services.</para>
        /// </param>
        /// <param name="value">
        /// <para>The object to edit.</para>
        /// </param>
        /// <returns>
        /// <para>The new value of the object.</para>
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    value = PromptForConnectionString(value as string);
                }
            }

            return value;
        }

        /// <summary>
        /// <para>Gets the editor style used by the <see cref="EditValue"/> method.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para>
        /// </param>
        /// <returns>
        /// <para><see cref="UITypeEditorEditStyle.Modal"/></para>
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        private string PromptForConnectionString(string connectionString) 
        {

            string newConnectionString = string.Empty;

            try 
            {
                ADODB.Connection connection = null;
                MSDASC.DataLinks links = new MSDASC.DataLinksClass();

                if ((connectionString == null) || (connectionString.Length == 0)) 
                {
                    connectionString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Data Source=localhost";
                }
                else
                {
                    if (connectionString.IndexOf("Provider=SQLOLEDB.1") < 0)
                    {
                        connectionString = string.Concat("Provider=SQLOLEDB.1;", connectionString);
                    }
                }
                connection = new ADODB.ConnectionClass();
                connection.ConnectionString = connectionString;
                
                System.Object connectionObject = connection;
                bool success = links.PromptEdit(ref connectionObject); 
                if (success) 
                {           
                    connection = (ADODB.Connection)connectionObject;
                    newConnectionString = connection.ConnectionString;
                    // need to rip off the provider
                    string provider = string.Concat("Provider=", ((ADODB.Connection)connectionObject).Provider);
                    int index = newConnectionString.IndexOf(provider);
                    newConnectionString = newConnectionString.Remove(index, provider.Length + 1);
                }          
                else
                {
                    newConnectionString = connectionString;
                }
            }
            catch 
            {
                newConnectionString = string.Empty;
            }
            return newConnectionString;
        }
    }        
}