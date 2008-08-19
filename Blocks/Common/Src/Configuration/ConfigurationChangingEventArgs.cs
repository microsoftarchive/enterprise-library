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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class ConfigurationChangingEventArgs : ConfigurationChangedEventArgs
    {
        private bool cancel;
        private readonly object newValue;
        private readonly object oldValue;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationChangingEventArgs"/> class with the configuration file, the section name, the old value, and the new value of the changes.</para>
        /// </summary>
        /// <param name="sectionName"><para>The section name of the changes.</para></param>
        /// <param name="oldValue"><para>The old value.</para></param>
        /// <param name="newValue"><para>The new value.</para></param>
        public ConfigurationChangingEventArgs(string sectionName, object oldValue, object newValue) : base(sectionName)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        /// <para>Gets the old value.</para>
        /// </summary>
        /// <value>
        /// <para>The old value.</para>
        /// </value>
        /// <remarks>
        /// <value>If no old value existed this value will be <see langword="null"/>.</value>
        /// </remarks>
        public object OldValue
        {
            get { return oldValue; }
        }

        /// <summary>
        /// <para>Gets the new value.</para>
        /// </summary>
        /// <value>
        /// <para>The new value.</para>
        /// </value>
        public object NewValue
        {
            get { return newValue; }
        }

        /// <summary>
        /// <para>Determines if the changes should be cancled.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the changes should be cancled; otherwise, <see langword="false"/>.</para>
        /// </value>
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    /// <summary>
	/// Event handler called before the configuration is changed. 
    /// </summary>
    /// <param name="sender">
    /// <para>The source of the event.</para>
    /// </param>
    /// <param name="e">
    /// <para>A <see cref="ConfigurationChangingEventArgs"/> that contains the event data.</para>
    /// </param>
    public delegate void ConfigurationChangingEventHandler(object sender, ConfigurationChangingEventArgs e);


}