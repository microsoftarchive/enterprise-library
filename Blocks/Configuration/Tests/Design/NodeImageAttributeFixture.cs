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

#if UNIT_TESTS
using System.Drawing;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class NodeImageAttributeFixture
    {
        [Test]
        public void GetImageByTypeTest()
        {
            NodeImageAttribute bitmapAttribute = new SelectedImageAttribute(this.GetType());
            using (Image actualImage = bitmapAttribute.GetImage())
            {
                Image defaultImage = ToolboxBitmapAttribute.Default.GetImage(typeof(ToolboxBitmapAttribute));
                Assert.IsNotNull(actualImage);
                Assert.IsFalse(defaultImage == actualImage);
            }
        }

        [Test]
        public void GetImageByNameTest()
        {
            NodeImageAttribute bitmapAttribute = new SelectedImageAttribute(this.GetType(), "BALLOON.BMP");
            using (Image actualImage = bitmapAttribute.GetImage())
            {
                Image defaultImage = ToolboxBitmapAttribute.Default.GetImage(typeof(ToolboxBitmapAttribute));
                Assert.IsNotNull(actualImage);
                Assert.IsFalse(defaultImage == actualImage);
            }
        }
    }
}

#endif