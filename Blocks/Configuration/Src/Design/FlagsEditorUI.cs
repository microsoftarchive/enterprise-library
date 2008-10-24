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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    class FlagsEditorUI : UserControl
    {
        private Button btnAccept;
        private CheckedListBox clbEnumValues;
        private Type enumType;
        private int intialValue;

        public FlagsEditorUI(Type enumType, int initialValue)
        {
            InitializeComponent();

            this.enumType = enumType;
            this.intialValue = initialValue;
            this.BorderStyle = BorderStyle.None;

            foreach(object enumValue in Enum.GetValues(enumType))
            {
                int intValue = Convert.ToInt32(enumValue);
                if (intValue != 0)
                {
                    clbEnumValues.Items.Add(enumValue, 0 < (intValue & initialValue));
                }
            }
        }

        public int Value
        {
            get 
            {
                int value = 0;
                foreach (object enumValue in clbEnumValues.CheckedItems)
                {
                    value += Convert.ToInt32(enumValue);
                }
                return value; 
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlagsEditorUI));
            this.btnAccept = new System.Windows.Forms.Button();
            this.clbEnumValues = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // clbEnumValues
            // 
            this.clbEnumValues.CheckOnClick = true;
            resources.ApplyResources(this.clbEnumValues, "clbEnumValues");
            this.clbEnumValues.FormattingEnabled = true;
            this.clbEnumValues.Name = "clbEnumValues";
            // 
            // FlagsEditorUI
            // 
            this.Controls.Add(this.clbEnumValues);
            this.Controls.Add(this.btnAccept);
            this.Name = "FlagsEditorUI";
            this.ResumeLayout(false);

        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            OnClose();
        }

        public event EventHandler Close;

        protected virtual void OnClose()
        {
            if (Close != null)
            {
                Close(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
