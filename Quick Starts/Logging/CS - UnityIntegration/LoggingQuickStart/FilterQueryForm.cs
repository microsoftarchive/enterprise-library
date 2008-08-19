//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LoggingQuickStart
{
  public partial class FilterQueryForm : Form
  {
    private ICollection<string> categories = new List<string>(0);

    /// <summary>
    /// Priority to use in priority filter query.
    /// </summary>
    public int Priority
    {
      get { return Decimal.ToInt32(this.priorityNumericUpDown.Value); }
    }

    /// <summary>
    /// Collection of categories to use for category filter query.
    /// </summary>
    public ICollection<string> Categories
    {
      get { return categories; }
    }

    public FilterQueryForm()
    {
      InitializeComponent();
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      RecordSelectedCategories();

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void RecordSelectedCategories()
    {
      if (this.traceCheckBox.Checked)
      {
        categories.Add("Trace");
      }
      if (this.debugCheckbox.Checked)
      {
        categories.Add("Debug");
      }
      if (this.generalCheckbox.Checked)
      {
        categories.Add("General");
      }
      if (this.uiCheckbox.Checked)
      {
        categories.Add("UI Events");
      }
      if (this.dataAccessCheckbox.Checked)
      {
        categories.Add("Data Access Events");
      }
      if (this.troubleshootingCheckbox.Checked)
      {
        categories.Add("Troubleshooting");
      }
    }

    private void CancelEntryButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
  }
}