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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    /// <summary>
    /// <see cref="TypeConverter"/> implementation that returns a list of known ADO provider names as standard values.<br/>
    /// </summary>
    public class SystemDataProviderConverter : StringConverter
    {        
        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>Always returns <see langword="false"/>.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="System.ComponentModel.TypeConverter.GetStandardValues()"/> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>Always returns <see langword="false"/>.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        private static string[] DoGetStandardValues()
        {
            DataTable table = DbProviderFactories.GetFactoryClasses();
            List<string> providers = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                providers.Add((string)row.ItemArray[2]);
            }

            return providers.ToArray();
        }


        /// <summary>
        /// Returns a collection of standard values from the default context for the data type this type converter is designed for.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>A collection of known ADO provider names.</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(DoGetStandardValues());
        }
    }
}
