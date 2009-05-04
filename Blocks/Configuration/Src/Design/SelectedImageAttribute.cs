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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Defines the image to be displayed when a <see cref="ConfigurationNode"/> is not selected in the user interface.
	/// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SelectedImageAttribute : NodeImageAttribute
    {
		/// <summary>
		/// Initializes a new instance of the  <see cref="SelectedImageAttribute"/> class using the specified <see cref="Type"/> and resource entry name. The type is used locate the assembly from which to load the  <see cref="System.Resources.ResourceManager"/> that contains the image.
		/// </summary>
		/// <param name="componentType">
		/// A <see cref="Type"/> defined in the assembly that contains the image as an embedded resource.
		/// </param>
		/// <param name="name">The name of the embedded resource.</param>
		/// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>
		public SelectedImageAttribute(Type componentType, string name)
			: base(componentType, name)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectedImageAttribute"/> class based on a 16 x 16 bitmap that is embedded as a resource in a specified assembly.
		/// </summary>
		/// <param name="componentType">A <see cref="Type"/> whose defining assembly is  searched for the bitmap resource.</param>
		/// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>
		public SelectedImageAttribute(Type componentType)
			: base(componentType)
        {
        }
    }
}
