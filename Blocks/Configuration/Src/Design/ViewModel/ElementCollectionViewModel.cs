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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ElementCollectionViewModel"/> is a model for a <see cref="ConfigurationElementCollection"/>.
    /// </summary>
    public class ElementCollectionViewModel : ElementViewModel
    {
        readonly ConfigurationElementCollection thisElementCollection;
        readonly ConfigurationCollectionAttribute configurationCollectionAttribute;
        readonly ConfigurationPropertyAttribute configurationPropertyAttribute;
        readonly IMergeableConfigurationElementCollection mergeableConfigurationCollection;

        private Type[] polymorphicCollectionElementTypes;
        private Type customPolyporpicCollectionElementType;

        /// <summary>
        /// The current DiscoverDerivedConfigurationTypesService.
        /// </summary>
        protected DiscoverDerivedConfigurationTypesService ConfigurationTypesService { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementCollectionViewModel"/>
        /// </summary>
        /// <param name="parentElementModel">The parent <see cref="ElementViewModel"/>.</param>
        /// <param name="declaringProperty">The <see cref="PropertyDescriptor"/> of the <see cref="ConfigurationElement"/> property containing this collection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "As designed")]
        public ElementCollectionViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
            this.thisElementCollection = declaringProperty.GetValue(parentElementModel.ConfigurationElement) as ConfigurationElementCollection;
            this.IsPolymorphicCollection = ConfigurationType.FindGenericParent(typeof(PolymorphicConfigurationElementCollection<>)) != null;
            this.mergeableConfigurationCollection = MergeableConfigurationCollectionFactory.GetCreateMergeableCollection(thisElementCollection);

            Type polymorphicCollectionWithCustomElementType = ConfigurationType.FindGenericParent(typeof(NameTypeConfigurationElementCollection<,>));
            if (polymorphicCollectionWithCustomElementType != null)
            {
                customPolyporpicCollectionElementType = polymorphicCollectionWithCustomElementType.GetGenericArguments()[1];
            }

            configurationPropertyAttribute = base.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
            Debug.Assert(configurationPropertyAttribute != null);

            configurationCollectionAttribute = base.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();
            Debug.Assert(configurationCollectionAttribute != null);
        }

        /// <summary>
        /// Provides dependencies for <see cref="ElementCollectionViewModel"/> not provided via the constructor.
        /// </summary>
        /// <param name="configurationTypesService">The service for disovering derived configuration types.</param>
        [InjectionMethod]
        public void ElementCollectionViewModelServiceDependencies(DiscoverDerivedConfigurationTypesService configurationTypesService)
        {
            this.ConfigurationTypesService = configurationTypesService;
        }

        /// <summary>
        /// Creates or collections all the commands related to this <see cref="ElementViewModel"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            var baseCommands = base.CreateCustomCommands()
                                    .Union(GetPromotedCommands());

            return baseCommands.Union(new CommandModel[] { ContainingSection.CreateElementCollectionAddCommands(Attributes, this) });
        }

        /// <summary>
        /// The configuration element <see cref="Type"/>s contained by this collection.
        /// </summary>
        public Type CollectionElementType
        {
            get
            {
                return configurationCollectionAttribute.ItemType;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if this collection contains polymorphic items.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the collection maintains polymorphic items.
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        public bool IsPolymorphicCollection
        {
            get;
            protected set;
        }


        /// <summary>
        /// Returns the set of polymorphic collection element types this collection can hold.
        /// </summary>
        public virtual IEnumerable<Type> PolymorphicCollectionElementTypes
        {
            get
            {
                if (polymorphicCollectionElementTypes == null)
                {
                    var availablePolymorphicTypes = ConfigurationTypesService.FindAvailableConfigurationElementTypes(CollectionElementType);

                    if (customPolyporpicCollectionElementType != null) availablePolymorphicTypes = availablePolymorphicTypes.Union(new[] { customPolyporpicCollectionElementType });

                    // Apply type filters
                    availablePolymorphicTypes = availablePolymorphicTypes.Where(t => ConfigurationTypesService.CheckType(t)).ToArray();

                    polymorphicCollectionElementTypes = availablePolymorphicTypes
                        .FilterSelectSafe(x => new { ElementType = x, Browsable = TypeDescriptor.GetAttributes(x).OfType<BrowsableAttribute>().FirstOrDefault() })
                        .Where(x => x.Browsable == null || x.Browsable.Browsable)
                        .Select(x => x.ElementType)
                        .ToArray();
                }
                return polymorphicCollectionElementTypes;
            }
        }

        /// <summary>
        /// Gets a value indicating that this Element's <see cref="ElementViewModel.Path"/> is reliable 
        /// </summary>
        public override bool IsElementPathReliableXPath
        {
            get
            {
                if (ParentElement != null && !ParentElement.IsElementPathReliableXPath) return false;

                return true;
            }
        }

        /// <summary>
        /// Gets a string that can be appended to the parent's <see cref="ElementViewModel.Path"/> to compose a <see cref="ElementViewModel.Path"/> used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        protected override string GetLocalPathPart()
        {
            if (configurationPropertyAttribute.IsDefaultCollection) return "";

            return configurationPropertyAttribute.Name;
        }

        /// <summary>
        /// Determines if a <see cref="CollectionElementViewModel"/> is the first element in the collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="element"/> is the first element in the collection.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public bool IsFirst(CollectionElementViewModel element)
        {
            if (!ChildElements.Any()) return false;

            return ChildElements.First().ElementId == element.ElementId;
        }

        /// <summary>
        /// Determines if a <see cref="CollectionElementViewModel"/> is the last element in the collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>
        /// Returns <see langword="true"/> if the <paramref name="element"/> is the last element in the collection.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public bool IsLast(CollectionElementViewModel element)
        {
            if (!ChildElements.Any()) return false;

            return ChildElements.Last().ElementId == element.ElementId;
        }


        /// <summary>
        /// Gets all the <see cref="ElementViewModel"/> instances that are directly contained in this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
        /// </returns>
        protected override IEnumerable<ElementViewModel> GetAllChildElements()
        {
            var leaf = base.GetAllChildElements();

            var contained = thisElementCollection
                .OfType<ConfigurationElement>()
                .Select(x => new { Browasble = TypeDescriptor.GetAttributes(x).OfType<BrowsableAttribute>().FirstOrDefault(), Instance = x })
                .Where(x => x.Browasble == null || x.Browasble.Browsable)
                .Select(x => ContainingSection.CreateCollectionElement(this, x.Instance))
                .Cast<ElementViewModel>();

            return leaf.Union(contained);
        }

        /// <summary>
        /// Adds a new item of type <paramref name="elementType"/> to the collection.
        /// </summary>
        /// <param name="elementType">The <see cref="Type"/> of element to add.  It should be of, or derive from, the type indicated in <see cref="CollectionElementType"/>.</param>
        /// <returns>An <see cref="ElementViewModel"/> for the added type.</returns>
        public virtual ElementViewModel AddNewCollectionElement(Type elementType)
        {
            EnsureHasChildElements();

            var element = mergeableConfigurationCollection.CreateNewElement(elementType);
            var childElementModel = ContainingSection.CreateCollectionElement(this, element);



            if (childElementModel.NameProperty != null)
            {
                childElementModel.NameProperty.Value = CalculateNameFromType(elementType);
            }

            // add the new element to the configuration.
            mergeableConfigurationCollection.ResetCollection(
                thisElementCollection.OfType<ConfigurationElement>()
                    .Concat(new[] { element })
                    .ToArray());


            foreach (var property in childElementModel.Properties)
            {
                DesigntimeDefaultAttribute defaultDesigntimeValue = property.Attributes.OfType<DesigntimeDefaultAttribute>().FirstOrDefault();
                if (defaultDesigntimeValue != null)
                {
                    property.Value = property.ConvertFromBindableValueInvariant(defaultDesigntimeValue.BindableDefaultValue);
                }
            }

            childElementModel.Initialize(new InitializeContext());
            InitializeElementProperties(childElementModel);

            // add the new element to the view model.
            AddChildToView(childElementModel);

            Validate();

            return childElementModel;
        }

        private void ValidateDefiningProperty()
        {
            if (ParentElement == null) return;

            var declaringPropertyModel = ParentElement.Properties
                .Where(p => DeclaringProperty.Equals(p.DeclaringProperty)).FirstOrDefault();
            if (declaringPropertyModel == null) return;

            declaringPropertyModel.Validate();
        }

        private static void InitializeElementProperties(ElementViewModel childElementModel)
        {
            var properties = childElementModel.Properties;
            foreach (var propInitializer in properties)
            {
                propInitializer.Initialize(new InitializeContext());
            }
        }

        /// <summary>
        /// Deletes an element from the collection.
        /// </summary>
        /// <param name="element">The element to delete.</param>
        public void Delete(CollectionElementViewModel element)
        {
            //remove the element from configuration collection.
            var list =
                thisElementCollection.OfType<ConfigurationElement>().Where(x => x != element.ConfigurationElement).
                    ToArray();
            mergeableConfigurationCollection.ResetCollection(list);

            //remove the element from the view.
            RemoveChildFromView(element);

            Validate();

            element.OnDeleted();
        }

        #region calculate name methods

        private string CalculateNameFromType(Type elementType)
        {
            var displayNameAttribute =
                TypeDescriptor.GetAttributes(elementType).OfType<DisplayNameAttribute>().FirstOrDefault();

            string baseName = displayNameAttribute == null
                                  ? TypeDescriptor.GetClassName(elementType)
                                  : displayNameAttribute.DisplayName;

            return FindUniqueNewName(baseName);
        }

        /// <summary>
        /// Calculates a unique name based on the items in the collection.
        /// </summary>
        /// <param name="baseName">The base name to use in calculating the inique name.</param>
        /// <returns>A name that no other element in the collection matches.</returns>
        /// <remarks>
        /// The unique name is calculated by appending increasing integers values greater than 1 until a unique name is discovered.
        /// </remarks>
        public string FindUniqueNewName(string baseName)
        {
            int number = 1;
            while (true)
            {
                string proposedName = string.Format(CultureInfo.CurrentCulture,
                                                    Resources.NewCollectionElementNameFormat,
                                                    baseName,
                                                    number == 1 ? string.Empty : number.ToString(CultureInfo.CurrentCulture)).Trim();

                if (this.ChildElements.Any(x => x.NameProperty != null && (proposedName == x.NameProperty.Value as string)))
                    number++;
                else
                    return proposedName;
            }
        }

        #endregion

        ///<summary>
        /// Validates this <see cref="ElementViewModel"/> and its <see cref="ElementViewModel.Properties"/>.
        ///</summary>
        public override void Validate()
        {
            base.Validate();
            ValidateDefiningProperty();
        }

        #region Move Up & Down

        /// <summary>
        /// Moves a <see cref="CollectionElementViewModel"/> in this collection one position earlier in the <see cref="ElementViewModel.ChildElements"/> sequence.
        /// </summary>
        /// <param name="elementViewModel">The <see cref="CollectionElementViewModel"/> in this collection to move.</param>
        public void MoveUp(CollectionElementViewModel elementViewModel)
        {
            MoveElement(elementViewModel, -1);
        }

        /// <summary>
        /// Moves a <see cref="CollectionElementViewModel"/> in this collection one position later in the <see cref="ElementViewModel.ChildElements"/> sequence.
        /// </summary>
        /// <param name="elementViewModel">The <see cref="CollectionElementViewModel"/> in this collection to move.</param>
        public void MoveDown(CollectionElementViewModel elementViewModel)
        {
            MoveElement(elementViewModel, 1);
        }

        private void MoveElement(CollectionElementViewModel element, int moveDistance)
        {
            var list = thisElementCollection.OfType<ConfigurationElement>().ToArray();

            //move the element in the configuration collection.
            MoveConfigurationItem(list, element.ConfigurationElement, moveDistance);
            mergeableConfigurationCollection.ResetCollection(list);

            //move the element in the view.
            MoveConfigurationItem((ObservableCollection<ElementViewModel>)ChildElements, element, moveDistance);
        }

        private static void MoveConfigurationItem<T>(System.Collections.ObjectModel.ObservableCollection<T> elements, T element, int relativeMoveIndex) where T : class
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                if (elements[i] != element) continue;
                var newIndex = i + relativeMoveIndex;
                if (newIndex >= 0 && newIndex < elements.Count())
                {
                    var tmp = elements[newIndex];
                    elements[newIndex] = element;
                    elements[i] = tmp;
                    return;
                }
            }
        }

        private static void MoveConfigurationItem<T>(IList<T> elements, T element, int relativeMoveIndex) where T : class
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                if (elements[i] != element) continue;
                var newIndex = i + relativeMoveIndex;
                if (newIndex >= 0 && newIndex < elements.Count())
                {
                    var tmp = elements[newIndex];
                    elements[newIndex] = element;
                    elements[i] = tmp;
                    return;
                }
            }
        }

        #endregion
    }
}
