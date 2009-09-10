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

namespace Console.Wpf.ViewModel
{
    public class HierarchicalSectionViewModel : SectionViewModel
    {
        private HeaderViewModel[] headers;

        public HierarchicalSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section) : base(serviceProvider, section)
        {
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();

            int currentColumn = 0;
            int currentRow = 1;

            var collection = ChildElements.OfType<ElementCollectionViewModel>().FirstOrDefault();
            if (collection != null)
                UpdateLayout(collection, currentRow, currentColumn + 1);
        }

        protected int UpdateLayout(ElementCollectionViewModel collection, int row, int col)
        {
            int currentRow = row;
            foreach(var item in collection.ChildElements.OfType<CollectionElementViewModel>())
            {
                int startingRow = currentRow;
                currentRow = UpdateLayout(item, currentRow, col);
                item.RowSpan = currentRow - startingRow;
            }

            return currentRow;
        }
        
        protected int UpdateLayout(CollectionElementViewModel collectionElement, int row, int col)
        {
            collectionElement.Row = row;
            collectionElement.Column = col;

            var innerCollection = collectionElement.ChildElements.OfType<ElementCollectionViewModel>().FirstOrDefault();
            if (innerCollection == null)
            {
                return row + 1;
            }
            else
            {
                return UpdateLayout(innerCollection, row, col + 1);
            }
        }

        private void EnsureHeaders()
        {
            if (headers == null)
            {
                var newHeaders = new List<HeaderViewModel>();
                var collectionGroups =
                 DescendentElements().OfType<ElementCollectionViewModel>().GroupBy(x => x.CollectionElementType);

                //todo: Move to 0 zero based
                int i = 1;
                foreach (var group in collectionGroups)
                {
                    newHeaders.Add(new StringHeaderViewModel(group.First().Name) {Row = 0, Column = i++});
                }

                headers = newHeaders.ToArray();
            }
        }

        public override System.Collections.Generic.IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            var firstNonCollectionParent = element.AncesterElements().FirstOrDefault(x => !typeof(ElementCollectionViewModel).IsAssignableFrom(x.GetType()));
            if (firstNonCollectionParent != null)
                yield return firstNonCollectionParent;

//todo:  Reorganize logic
            foreach(var child in element.ChildElements)
            {
				if (typeof(ElementCollectionViewModel).IsAssignableFrom(child.GetType()))
				{
					foreach (var grandchild in child.ChildElements)
					{
						yield return grandchild;
					}
				}
				else
				{
					yield return child;
				}
            }
        }

        public override System.Collections.Generic.IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            EnsureHeaders();
            return headers;
        }
    }
}
