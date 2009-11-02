using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using Console.Wpf.ViewModel.BlockSpecifics;

namespace Console.Wpf.ViewModel
{
    public class GridPositioning
    {
        List<GridPositioning> children = new List<GridPositioning>();

        public PositioningInstructions PositioningInstructions
        {
            get;
            set;
        }

        public virtual int EndRow
        {
            get { return children.Max(x => x.StartRow + x.EndRow); }
        }

        public virtual int EndColumn
        {
            get { return children.Max(x => x.StartColumn + x.EndColumn); }
        }

        public int StartRow
        {
            get { return PositioningInstructions.Row; }
        }

        public int StartColumn
        {
            get { return PositioningInstructions.Column; }
        }

        public CollectionGridCollectionPositioning PositionCollection(string header, Type configurationElementType, PositioningInstructions instructions)
        {
            var position = new CollectionGridCollectionPositioning(header, configurationElementType) { PositioningInstructions = instructions };
            children.Add(position);

            return position;
        }

        public CollectionGridCollectionPositioning PositionCollection(string header, Type configurationCollectionType, Type configurationElementType, PositioningInstructions instructions)
        {
            var position = new CollectionInstanceCollectionPositioning(header, configurationCollectionType, configurationElementType) { PositioningInstructions = instructions };
            children.Add(position);

            return position;
        }

        public void PositionHeader(string header, PositioningInstructions positioningInstructions)
        {
            children.Add(new HeaderPositioning(header) { PositioningInstructions = positioningInstructions });
        }

        public virtual void Update(ElementViewModel root)
        {
            foreach (var child in children)
            {
                child.Update(root);
            }
        }

        public virtual IEnumerable<ViewModel> GetGridVisuals()
        {
            return children.SelectMany(x => x.GetGridVisuals());
        }

        private class HeaderPositioning : GridPositioning
        {
            string header;

            public HeaderPositioning(string header)
            {
                this.header = header;
            }

            public override int EndColumn
            {
                get
                {
                    return 1;
                }
            }

            public override int EndRow
            {
                get
                {
                    return 1;
                }
            }

            public override IEnumerable<ViewModel> GetGridVisuals()
            {
                yield return new StringHeaderViewModel(header) { Row = this.StartRow, Column = this.StartColumn };
            }
        }

        public class CollectionGridCollectionPositioning : GridPositioning
        {
            string header;
            Type configurationElementType;
            ViewModel[] collectionElements = new ViewModel[0];
            CollectionGridCollectionPositioning innerCollectionPositionTemplate = null;
            List<ViewModel> nestedCollectionElements = new List<ViewModel>();

            public CollectionGridCollectionPositioning(string header, Type configurationElementType)
            {
                this.header = header;
                this.configurationElementType = configurationElementType;
            }

            public override void Update(ElementViewModel root)
            {
                UpdateCollection(root.DescendentElements());
            }

            protected void UpdateCollection(IEnumerable<ElementViewModel> children)
            {
                nestedCollectionElements.Clear();
                collectionElements = children.Where(x => configurationElementType.IsAssignableFrom(x.ConfigurationType)).Cast<ViewModel>().ToArray();
                int nElement = 0;
                foreach (var element in collectionElements.OfType<ElementViewModel>())
                {
                    element.Row = this.StartRow + nElement + (string.IsNullOrEmpty(header) ? 0 : 1);
                    element.Column = StartColumn;

                    if (innerCollectionPositionTemplate != null)
                    {
                        innerCollectionPositionTemplate.PositioningInstructions.OffSet = element;
                        innerCollectionPositionTemplate.Update(element);

                        if (innerCollectionPositionTemplate.EndRow > 0)
                        {
                            element.RowSpan = innerCollectionPositionTemplate.EndRow;
                        }

                        nestedCollectionElements.AddRange(innerCollectionPositionTemplate.GetGridVisuals());
                    }
                    nElement += element.RowSpan;
                }
            }

            public CollectionGridCollectionPositioning PositionNestedCollection(Type collectionElementType)
            {
                innerCollectionPositionTemplate = new CollectionGridCollectionPositioning("", collectionElementType);
                innerCollectionPositionTemplate.PositioningInstructions = new PositioningInstructions { FixedColumn = 1, FixedRow = 0 };
                return innerCollectionPositionTemplate;
            }

            public override int EndRow
            {
                get
                {
                    var visuals = GetGridVisuals();
                    if (!visuals.Any()) return 0;
                    return visuals.Max(x => x.Row + x.RowSpan) - ((PositioningInstructions.OffSet == null) ? 0 : PositioningInstructions.OffSet.Row);
                }
            }

            public override int EndColumn
            {
                get
                {
                    var visuals = GetGridVisuals();
                    if (!visuals.Any()) return 0;
                    return visuals.Max(x => x.Column + x.ColumnSpan) - ((PositioningInstructions.OffSet == null) ? 0 : PositioningInstructions.OffSet.Column);
                }
            }

            protected virtual ViewModel GetHeaderVisual()
            {
                return new StringHeaderViewModel(header) { Row = this.StartRow, Column = this.StartColumn };
            }

            public override IEnumerable<ViewModel> GetGridVisuals()
            {
                foreach (var collectionElement in collectionElements)
                {
                    yield return collectionElement;
                }

                if (!string.IsNullOrEmpty(header))
                {
                    yield return GetHeaderVisual();
                }

                foreach (var innerCollectionElement in nestedCollectionElements)
                {
                    yield return innerCollectionElement;
                }

            }

        }

        public class CollectionInstanceCollectionPositioning : CollectionGridCollectionPositioning
        {
            Type configurationCollectionType;
            Type configurationElementType;
            ElementViewModel headerElement;

            public CollectionInstanceCollectionPositioning(string header, Type configurationCollectionType, Type configurationElementType)
                :base(header, configurationElementType)
            {
                this.configurationCollectionType = configurationCollectionType;
                this.configurationElementType = configurationElementType;
            }

            public override void Update(ElementViewModel root)
            {
                headerElement = root.DescendentElements(x => configurationCollectionType.IsAssignableFrom(x.ConfigurationType)).FirstOrDefault();
                if (headerElement != null)
                {
                    UpdateCollection(headerElement.ChildElements);
                }
                else
                {
                    UpdateCollection(Enumerable.Empty<ElementViewModel>());
                }
            }

            protected override ViewModel GetHeaderVisual()
            {
                if (headerElement != null) return new ElementHeaderViewModel(headerElement, true) { Row = this.StartRow, Column = this.StartColumn };
                return base.GetHeaderVisual();
            }
        }
    }

    public class PositioningInstructions
    {
        public PositioningInstructions()
        {
            FixedColumn = -1;
            FixedRow = -1;
        }

        public int FixedColumn { get; set; }
        public int FixedRow { get; set; }
        public GridPositioning RowAfter { get; set; }
        public GridPositioning ColAfter { get; set; }
        public ViewModel OffSet { get; set; }

        public int Row
        {
            get 
            { 
                if (FixedRow != -1) return FixedRow + ((OffSet == null) ? 0 : OffSet.Row);
                return RowAfter.StartRow + RowAfter.EndRow;
            }
        }

        public int Column
        {
            get { return FixedColumn + ((OffSet == null) ? 0 : OffSet.Column); }
        }
    }

}
