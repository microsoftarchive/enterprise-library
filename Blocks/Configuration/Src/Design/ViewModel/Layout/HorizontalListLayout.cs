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

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{ 
    /// <summary>
    /// Layout class that can be used to visualize any number of <see cref="ElementViewModel"/> instances as a horizontal list with resize controls.
    /// </summary>
    public class HorizontalListLayout : ViewModel, INotifyPropertyChanged
    {
        readonly ViewModel current;
        readonly bool hasNext;
        readonly int nestingDepth;
        private readonly IElementChangeScope changeScope;
        readonly IEnumerable<ViewModel> elements;
        readonly HorizontalListLayout rootListLayout;

        /// <summary>
        /// Initializes a new instance of <see cref="HorizontalListLayout"/>.
        /// </summary>
        /// <param name="elements">The <see cref="ViewModel"/> elements that should be displayed horizontally.</param>
        public HorizontalListLayout(params ViewModel[] elements)
            : this((IEnumerable<ViewModel>)elements, 0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HorizontalListLayout"/>.
        /// </summary>
        /// <param name="changeScope">An <see cref="IElementChangeScope"/> instance that should be used to recalculate the visibility of resize controls with.</param>
        /// <param name="elements">The <see cref="ViewModel"/> elements that should be displayed horizontally.</param>
        public HorizontalListLayout(IElementChangeScope changeScope, params ViewModel[] elements)
            : this((IEnumerable<ViewModel>)elements, 0, null, changeScope)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HorizontalListLayout"/>.
        /// </summary>
        /// <param name="elements">The <see cref="ViewModel"/> elements that should be displayed horizontally.</param>
        public HorizontalListLayout(IEnumerable<ViewModel> elements)
            : this(elements, 0)
        {
        }


        private HorizontalListLayout(IEnumerable<ViewModel> elements, int nestingDepth) :
            this(elements, nestingDepth, null, null)
        {
        }

        private HorizontalListLayout(IEnumerable<ViewModel> elements, int nestingDepth, HorizontalListLayout root, IElementChangeScope changeScope)
        {
            this.nestingDepth = nestingDepth;

            this.current = elements.First();
            this.hasNext = elements.Count() > 1;
            this.elements = elements;

            rootListLayout = root ?? this;

            OwnsResizing = true;

            this.changeScope = changeScope;
            if (this.changeScope != null)
            {
                this.changeScope.CollectionChanged += ForcePropertyRequery;
            }
        }

        private void ForcePropertyRequery(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("CanResize");
        }

        /// <summary>
        /// When iterating over the horizontal list using <see cref="hasNext"/> and <see cref="Next"/>, returns an 0-based index of the <see cref="Current"/> element.
        /// </summary>
        /// <value>
        /// An 0-based index of the <see cref="Current"/> element.
        /// </value>
        public int NestingDepth
        {
            get { return nestingDepth; }
        }

        /// <summary>
        /// Gets the column name of the <see cref="Current"/> element.
        /// </summary>
        /// <value>
        /// The column name of the <see cref="Current"/> element.
        /// </value>
        public string ColumnName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Column{0}", NestingDepth); }
        }
        /// <summary>
        /// Gets a boolean that indicates whether a next <see cref="ViewModel"/> instance is available.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a next <see cref="ViewModel"/> instance is available; Otherwise <see langword="false"/>.
        /// </value>
        public bool HasNext
        {
            get { return hasNext; }
        }

        /// <summary>
        /// Gets the current <see cref="ViewModel"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="ViewModel"/> instance.
        /// </value>
        public ViewModel Current
        {
            get { return current; }
        }

        /// <summary>
        /// Gets a <see cref="HorizontalListLayout"/> instance that can be used to access the next <see cref="ViewModel"/> instance.
        /// </summary>
        /// <value>
        /// A <see cref="HorizontalListLayout"/> instance that can be used to access the next <see cref="ViewModel"/> instance.
        /// </value>
        public HorizontalListLayout Next
        {
            get
            {
                if (hasNext)
                {
                    return new HorizontalListLayout(elements.Skip(1), nestingDepth + 1, this.rootListLayout, changeScope)
                    {
                        OwnsResizing = this.OwnsResizing,
                    };
                };

                return null;
            }
        }
        /// <summary>
        /// Gets or sets a boolean that indicates whether this <see cref="HorizontalListLayout"/> should show resize controls.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if resize controls should be shown; Otherwise <see langword="false"/>.
        /// </value>
        public bool OwnsResizing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="ViewModel"/> instance that is displayed below the <see cref="HorizontalListLayout"/>.
        /// </summary>
        /// <value>
        /// The <see cref="ViewModel"/> instance that is displayed below the <see cref="HorizontalListLayout"/>.
        /// </value>
        public ViewModel Contained
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a boolean value indicating whether the <see cref="Current"/> <see cref="ViewModel"/> instance can be resized.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Current"/> <see cref="ViewModel"/> instance can be resized; Otherwise <see langword="false"/>.
        /// </value>
        public bool CanResize
        {
            get
            {
                if (this.Current != null)
                {
                    return true;
                }

                if (this.rootListLayout.ContainedHasLayoutWithColumnName(ColumnName))
                {
                    return true;
                }

                return false;
            }
        }


        private bool ContainedHasLayoutWithColumnName(string columnName)
        {
            var walker = new ElementListLayoutWalker(Contained);

            var element = walker.LayoutElements().OfType<TwoColumnsLayout>().Where(l => l.ColumnName == columnName).FirstOrDefault();
            return (element != null);
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
