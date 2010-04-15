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
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Walks the <see cref="ViewModel"/> bindables to disover and return layout items
    /// of type <see cref="ElementListLayout"/>, <see cref="ListLayout"/>,
    /// and <see cref="TwoColumnsLayout"/>.
    /// </summary>
    public class ElementListLayoutWalker
    {
        private readonly ViewModel root;

        /// <summary>
        /// Initializes a new <see cref="ElementListLayoutWalker"/>.
        /// </summary>
        /// <param name="root">Root <see cref="ViewModel"/>'s bindable is expected to be one of <br/>
        /// <list>
        /// <item><see cref="ElementListLayout"/></item>
        /// <item><see cref="ListLayout"/></item>
        /// <item><see cref="TwoColumnsLayout"/></item>
        /// </list>
        /// 
        /// If it is not one of these classes <see cref="ElementListLayoutWalker"/> will
        /// return empty at all times.
        /// </param>
        public ElementListLayoutWalker(ViewModel root)
        {
            if (root == null) throw new ArgumentNullException("root");

            this.root = root;
        }

        /// <summary>
        /// Visits all layout elements under the root specified
        /// in <see cref="ElementListLayoutWalker"/> and returns
        /// them.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViewModel> LayoutElements()
        {
            return VisitLayoutItem(root);
        }

        private IEnumerable<ViewModel> VisitLayoutItem(ViewModel element)
        {
            var elementList = element.Bindable as ElementListLayout;
            if (elementList != null) return RecurseElementList(elementList);

            var listLayout = element.Bindable as ListLayout;
            if (listLayout != null) return RecurseListLayout(listLayout);

            var twoColumn = element.Bindable as TwoColumnsLayout;
            if (twoColumn != null) return RecurseTwoColumnLayout(twoColumn);

            return Enumerable.Empty<ViewModel>();
        }

        private IEnumerable<ViewModel> RecurseListLayout(ListLayout layout)
        {
            IEnumerable<ViewModel> layoutElements = Enumerable.Empty<ViewModel>();

            foreach (var element in layout.Elements)
            {
                layoutElements = layoutElements.Concat(VisitLayoutItem(element));
            }

            return layoutElements;
        }

        private IEnumerable<ViewModel> RecurseElementList(ElementListLayout layout)
        {
            IEnumerable<ViewModel> layoutElements = Enumerable.Empty<ViewModel>();

            foreach (var element in layout.Elements)
            {
                layoutElements = layoutElements.Concat(VisitLayoutItem(element));
            }

            return layoutElements;
        }
        private IEnumerable<ViewModel> RecurseTwoColumnLayout(TwoColumnsLayout layout)
        {
            IEnumerable<ViewModel> layouts = new[] { layout };
            return layouts.Concat(VisitLayoutItem(layout.Right));
        }

    }
}
