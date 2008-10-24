//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
    /// <summary>
    /// Represents a parameter type.
    /// </summary>
    public class ParameterType
    {
        private string name;
        private string parameterType;

        /// <summary>
		/// Initializes a new instance of the <see cref="ParameterType"/> class with default values.
        /// </summary>
        public ParameterType()
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="ParameterType"/> class with the supplied type.
        /// </summary>
        /// <param name="name">A unique name that identifies this parameter in case there are repeated types.</param>
        /// <param name="parameterType">The parameter <see cref="Type"/>.</param>
        public ParameterType(string name, string parameterType)
        {
            this.name = name;
            this.parameterType = parameterType;
        }


        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the represented type.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return parameterType; }
            set { parameterType = value; }
        }

        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <returns>The name of the represented type.</returns>
        public override string ToString()
        {
            return parameterType;
        }
    }
}
