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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a localized <see cref="DescriptionAttribute"/>.
	/// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;
		private readonly Type resourceType;

		/// <summary>
		/// Initialize a new instance of the <see cref="SRDescriptionAttribute"/> class with the <see cref="Type"/> containing the resources and the resource name.
		/// </summary>
		/// <param name="description">The resources string name.</param>
		/// <param name="resourceType">The <see cref="Type"/> containing the resource strings.</param>
        public SRDescriptionAttribute(string description, Type resourceType) : base(description)
        {
            this.resourceType = resourceType;
        }

		/// <summary>
		/// Gets the resource type for the resources strings.
		/// </summary>
		/// <value>
		/// The resource type for the resources strings.
		/// </value>
		public Type ResourceType
		{
			get { return resourceType; }
		} 		

		/// <summary>
		/// Gets the description stored in the attribute.
		/// </summary>
		/// <value>
		/// The description stored in the attribute.
		/// </value>
        public override string Description
        {
            get
            {
                if (!replaced)
                {
                    replaced = true;
					base.DescriptionValue =  ResourceStringLoader.LoadString(resourceType.FullName, base.Description, resourceType.Assembly);
                }
                return base.Description;
            }
        }
    }
}
