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

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Contains data for a Template.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
	public class Template : IEnvironmentalOverridesSerializable
    {
        private string text;


        /// <summary>
        /// default constructor used when serializing for envrionmental overrides.
        /// </summary>
        public Template()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Template"/> struct with initial text for the template.
        /// </summary>
        /// <param name="text">The text of the template.</param>
        public Template(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Gets or sets the text of the template.
        /// </summary>
		/// <value>
		/// The text of the template.
		/// </value>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }		

        /// <summary>
        /// Gets the template formatted for a single line.
        /// </summary>
		/// <returns>The template formatted for a single line.</returns>
        public override string ToString()
        {
            return Resources.TemplatePlaceHolder;
        }


        #region IEnvironmentalOverridesSerializable Members


        void IEnvironmentalOverridesSerializable.DesializeFromString(string serializedContents)
        {
            text = serializedContents;
        }

        string IEnvironmentalOverridesSerializable.SerializeToString()
        {
            return text;
        }

        #endregion
    }
}