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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Console.Wpf.ViewModel
{
    public class HierarchicalSectionViewModel : SectionViewModel
    {
        private List<ElementHeaderViewModel> headers = new List<ElementHeaderViewModel>();

        [InjectionConstructor]
        public HierarchicalSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }

        public HierarchicalSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(builder, sectionName, section, additionalAttributes)
        { 
        }


        protected class LayoutContext
        {
            public int CurrentRow = 0;
            public int CurrentColumn = 0;
            public int CurrentHeaderRow = 0;
        }

        public override void UpdateLayout()
        {
            headers.Clear();

            var context = new LayoutContext() { CurrentColumn = 0, CurrentRow = 1, CurrentHeaderRow = 0 };

            var collections = ChildElements.OfType<ElementCollectionViewModel>();
            foreach (var collection in collections)
            {
                UpdateLayout(collection, context);

                // reset initial values for next collection
                context.CurrentColumn = 0;
                context.CurrentHeaderRow = context.CurrentRow + 1;
                context.CurrentRow = context.CurrentHeaderRow + 1;
            }


            OnUpdateVisualGrid();
        }

        protected void UpdateLayout(ElementCollectionViewModel collection, LayoutContext context)
        {
            int initialRow = context.CurrentRow;

            AddCollectionHeader(collection, context.CurrentHeaderRow, context.CurrentColumn);
            foreach (var item in collection.ChildElements.OfType<CollectionElementViewModel>())
            {
                int startingRow = context.CurrentRow;
                UpdateLayout(item, context);
                item.RowSpan = Math.Max(1, context.CurrentRow - startingRow);
            }

            // Increment row by at least one, in case there are no children.
            context.CurrentRow = Math.Max(context.CurrentRow, initialRow + 1);
        }

        protected void UpdateLayout(CollectionElementViewModel collectionElement, LayoutContext context)
        {
            collectionElement.Row = context.CurrentRow;
            collectionElement.Column = context.CurrentColumn;

            var innerCollections = collectionElement.ChildElements.OfType<ElementCollectionViewModel>();
            if (innerCollections.Count() == 0)
            {
                context.CurrentRow++;
            }
            else
            {
                for (int i = 0; i < innerCollections.Count(); i++)
                {
                    var innerCollection = innerCollections.ElementAt(0);

                    context.CurrentColumn++;
                    UpdateLayout(innerCollection, context);
                    context.CurrentColumn--;


                    // Only increment header row and current row if it's
                    // not the last collection
                    if (i != innerCollections.Count() - 1)
                    {
                        context.CurrentHeaderRow = context.CurrentRow + 1;
                        context.CurrentRow = context.CurrentHeaderRow + 1;
                    }
                }
            }
        }

        private void AddCollectionHeader(ElementCollectionViewModel collection, int row, int column)
        {
            if (!headers.Any(h => h.Path == collection.TypePath))
            {
                var header = new ElementHeaderViewModel(collection, IsTopLevelCollection(column));
                header.Row = row;
                header.Column = column;
                headers.Add(header);
            }
        }

        private bool IsTopLevelCollection(int column)
        {
            return column == 0;
        }

        public override System.Collections.Generic.IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            return headers.Cast<ViewModel>();
        }
    }
}
