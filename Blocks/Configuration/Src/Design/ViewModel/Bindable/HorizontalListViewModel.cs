using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class HorizontalListViewModel : ViewModel
    {
        readonly ViewModel current;
        readonly bool hasNext;
        readonly int nestingDept;
        readonly IEnumerable<ViewModel> elements;

        public HorizontalListViewModel(params ViewModel[] elements)
            : this((IEnumerable<ViewModel>)elements)
        {
        }

        public HorizontalListViewModel(IEnumerable<ViewModel> elements)
            : this(elements, 0)
        {
        }

        public HorizontalListViewModel(IEnumerable<ViewModel> elements, int nestingDept)
        {
            this.nestingDept = nestingDept;

            this.current = elements.First();
            this.hasNext = elements.Count() > 1;
            this.elements = elements;

            Root = true;
        }

        public int NestingDept
        {
            get { return nestingDept; }
        }

        //todo, move to xaml
        public string ColumnName
        {
            get { return string.Format("Column{0}", NestingDept); }
        }

        public bool HasNext
        {
            get { return hasNext; }
        }

        public ViewModel Current
        {
            get { return current; }
        }

        public HorizontalListViewModel Next
        {
            get
            {
                if (hasNext)
                {
                    return new HorizontalListViewModel(elements.Skip(1), nestingDept + 1)
                    {
                        Root = this.Root
                    };
                };

                return null;
            }
        }

        public bool Root
        {
            get;
            set;
        }

        public ViewModel Contained
        {
            get;
            set;
        }
    }
}
