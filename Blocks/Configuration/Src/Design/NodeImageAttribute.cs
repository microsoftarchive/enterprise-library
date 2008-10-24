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
using System.Drawing;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Specifies the image to be displayed when a <see cref="ConfigurationNode"/> is viewed in the user interface.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Class)]
    public class NodeImageAttribute : ToolboxBitmapAttribute
    {
        private readonly Type componentType;
        private readonly string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="NodeImageAttribute"/> class using the specified <see cref="Type"/> and resource entry name. The type is used to locate the assembly from which to load the <see cref="System.Resources.ResourceManager"/> that contains the image.
		/// </summary>
		/// <param name="componentType">A <see cref="Type"/> defined in the assembly that contains the image as an embedded resource.</param>
		/// <param name="name">The name of the embedded resource.</param>
		/// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>
        public NodeImageAttribute(Type componentType, string name) : base(componentType, name)
        {
            this.componentType = componentType;
            this.name = name;
        }

		/// <summary>
		/// Initializes a new <see cref="NodeImageAttribute"/> object based on a 16 x 16 bitmap that is embedded  as a resource in a specified assembly.
		/// </summary>
		/// <param name="componentType">
		/// A <see cref="Type"/> whose defining assembly is searched for the bitmap resource.
		/// </param>
		public NodeImageAttribute(Type componentType)
			: base(componentType)
        {
        }

		/// <summary>
		/// Gets the name of the embedded bitmap resource.
		/// </summary>
		/// <value>
		/// The name of the embedded bitmap resource.
		/// </value>
        public string Name
        {
            get { return name; }
        }

		/// <summary>
		/// Gets a <see cref="Type"/> whose defining assembly is searched for the bitmap resource.
		/// </summary>
		/// <value>
		/// A <see cref="Type"/> whose defining assembly is searched for the bitmap resource.
		/// </value>
        public Type ComponentType
        {
            get { return componentType; }
        }

		/// <summary>
		/// Gets the large (32 x 32) <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.
		/// </summary>
		/// <returns>The large <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public Image GetLargeImage()
        {
            return GetImage(true);
        }

		/// <summary>
		/// Gets the small (16 x 16) <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.
		/// </summary>
		/// <returns>The small <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</returns>
        public Image GetImage()
        {
            return GetImage(false);
        }

        private Image GetImage(bool large)
        {
            if (name != null)
            {
                return GetImage(componentType, name, large);
            }
            else
            {
                return GetImage(componentType, large);
            }
        }
    }
}
