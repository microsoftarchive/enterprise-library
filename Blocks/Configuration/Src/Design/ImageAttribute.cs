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
    /// Defines the image to be associated with the <see cref="ConfigurationNode"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Class)]
    public sealed class ImageAttribute : NodeImageAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeImageAttribute"/> class based on a 16 x 16 bitmap that is embedded as a resource in a specified assembly.
        /// </summary>
        /// <param name="t">
        /// A Type whose defining assembly is searched for the bitmap resource.
        /// </param>
        public ImageAttribute(Type t) : base(t)
        {
        }

        /// <summary>
        /// 
        /// Initializes a new instance of the <see cref="NodeImageAttribute"/> class using the specified <see cref="Type"/> and resource entry name. The type is used to locate the assembly from which to load the <see cref="System.Resources.ResourceManager"/> that contains the image.
        /// </summary>
        /// <param name="t">A <see cref="Type"/> defined in the assembly that contains the image as an embedded resource.</param> 
        /// <param name="name">The name of the embedded resource.</param>        
        /// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>        
        public ImageAttribute(Type t, string name) : base(t, name)
        {
        }
    }
}