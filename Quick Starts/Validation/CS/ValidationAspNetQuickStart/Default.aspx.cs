//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;

public partial class _Default : System.Web.UI.Page 
{

    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the ruleset for every validator control
        foreach (IValidator v in this.GetValidators(null))
        {
            if (v is PropertyProxyValidator)
            {
                PropertyProxyValidator validator = v as PropertyProxyValidator;
                validator.RulesetName = ruleSetDropDown.Text;
            }
        }
    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            validationResultsLabel.Text = "Data is valid.";
            // Now you would create the objects and process them
        }
        else
        {
            validationResultsLabel.Text = "Data is invalid.";
        }

    }
    protected void rewardsPointsValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
    {
        string value = e.ValueToConvert as string;
        int convertedValue;

        bool success = Int32.TryParse(value, out convertedValue);

        if (success)
        {
            e.ConvertedValue = convertedValue;
        }
        else
        {
            e.ConversionErrorMessage = "Rewards points is not a valid integer";
            e.ConvertedValue = null;
        }
    }

    protected void dateOfBirthValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
    {
        string value = e.ValueToConvert as string;
        DateTime convertedValue;

        bool success = DateTime.TryParse(value, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None,  out convertedValue);

        if (success)
        {
            e.ConvertedValue = convertedValue;
        }
        else
        {
            e.ConversionErrorMessage = "Date Of Birth is not in the correct format.";
            e.ConvertedValue = null;
        }
    }
    protected void ruleSetDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
}
