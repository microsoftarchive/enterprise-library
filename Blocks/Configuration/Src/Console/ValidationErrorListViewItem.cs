//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    class ValidationErrorListViewItem : ConfigurationNodeListViewItem
    {
        private ValidationError error;
        private ConfigurationNode node;

        public ValidationErrorListViewItem(ValidationError error) : base()
        {
            Text = error.InvalidItem.ToString();
            this.error = error;
			node = error.InvalidItem as ConfigurationNode;
            StateImageIndex = 0;
            SubItems.Add(error.PropertyName);
            SubItems.Add(error.Message);
            SubItems.Add(node.Path);
        }

        public ValidationError Error
        {
            get { return error; }
        }

        public override ConfigurationNode ConfigurationNode
        {
            get { return node; }
        }
    }
}