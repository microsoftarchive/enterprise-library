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
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// View model for the <see cref="TypeBrowser"/>.
    /// </summary>
    public class TypeBrowserViewModel : INotifyPropertyChanged, INodeCreator
    {
        readonly string typeConstraintDisplay;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowserViewModel"/> class with a type filter predicate.
        /// </summary>
        /// <param name="typeFilter">The type filter predicate.</param>
        public TypeBrowserViewModel(Func<Type, bool> typeFilter)
            : this(typeFilter, new NullServiceProvider())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowserViewModel"/> class with a 
        /// <see cref="TypeBuildNodeConstraint"/> and a service provider.
        /// </summary>
        /// <param name="constraint">The constraint to use when filtering types.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public TypeBrowserViewModel(TypeBuildNodeConstraint constraint, IServiceProvider serviceProvider)
            : this(constraint.Matches, serviceProvider)
        {
            typeConstraintDisplay = constraint.GetDisplayString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowserViewModel"/> class with a 
        /// type filter predicate and a service provider.
        /// </summary>
        /// <param name="typeFilter">The type filter predicate.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public TypeBrowserViewModel(Func<Type, bool> typeFilter, IServiceProvider serviceProvider)
        {
            this.typeFilter = typeFilter;
            this.serviceProvider = serviceProvider;

            this.AssemblyGroups = new ObservableCollection<AssemblyGroupNode>();
            this.GenericTypeParameters = new ObservableCollection<GenericTypeParameter>();
        }

        /// <summary>
        /// Updates the assembly groups for the view model.
        /// </summary>
        /// <param name="assemblyGroups">The new set of assembly groups.</param>
        public void UpdateAssemblyGroups(IEnumerable<AssemblyGroup> assemblyGroups)
        {
            this.AssemblyGroups.Clear();
            foreach (var group in assemblyGroups)
            {
                this.AssemblyGroups.Add(new AssemblyGroupNode(group, this));
            }
        }

        /// <summary>
        /// Resolves the type represented by the current selection and the values for the generic type 
        /// parameters, if any.
        /// </summary>
        /// <returns>Returns the type represented by the current selection.</returns>
        public Type ResolveType()
        {
            var resolvedType = this.concreteType;
            if (resolvedType != null)
            {
                if (resolvedType.IsGenericTypeDefinition)
                {
                    var genericTypeArguments =
                        this.GenericTypeParameters.Select(
                        gtp =>
                        {
                            if (gtp.TypeArgument == null)
                            {
                                throw new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Properties.Resources.ErrorGenericTypeParameterNotSet,
                                        gtp.TypeParameter.Name));
                            }
                            return gtp.TypeArgument;
                        }).ToArray();

                    resolvedType = resolvedType.MakeGenericType(genericTypeArguments);
                }

                return resolvedType;
            }

            throw new InvalidOperationException(Properties.Resources.ErrorNoSelectedType);
        }

        /// <summary>
        /// Gets the collection of assembly groups.
        /// </summary>
        public ObservableCollection<AssemblyGroupNode> AssemblyGroups { get; private set; }

        /// <summary>
        /// Gets the collection of generic type parameters associated to the currently selected type.
        /// </summary>
        public ObservableCollection<GenericTypeParameter> GenericTypeParameters { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the currently selected type has generic parameters.
        /// </summary>
        public bool HasGenericParameters
        {
            get
            {
                return this.GenericTypeParameters.Count > 0;
            }
        }

        /// <summary>
        /// Gets the title for the view model.
        /// </summary>
        public string Title
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, Resources.TypeBrowserTitleFormat, typeConstraintDisplay);
            }
        }

        /// <summary>
        /// Gets or sets the type name input for the view model.
        /// </summary>
        /// <remarks>Setting a value triggers a search.</remarks>
        public string TypeName
        {
            get
            {
                return this.typeName;
            }
            set
            {
                if (this.typeName != value)
                {
                    this.typeName = value;

                    InitiateSearch();

                    this.Notify("TypeName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected type.
        /// </summary>
        public Type ConcreteType
        {
            get { return this.concreteType; }
            set
            {
                if (this.concreteType != value)
                {
                    this.concreteType = value;

                    this.Notify("ConcreteType");
                }
            }
        }

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies that a property has changed its value.
        /// </summary>
        /// <param name="property">The name of the changed property.</param>
        protected void Notify(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void InitiateSearch()
        {
            if (!updateFromSelection)
            {
                var typeName = this.typeName;
                var newSearch = new SearchAction(typeName, this.AssemblyGroups);

                newSearch.Completed += (s, args) =>
                {
                    SearchAction completedSearch = s as SearchAction;
                    SearchAction currentSearch = this.currentSearch;
                    this.currentSearch = null;
                    if (completedSearch == currentSearch)
                    {
                        TypeNode entry = args.Result as TypeNode;
                        this.UpdateSelection(entry);
                        if (entry != null)
                        {
                            // don't want to react to updates when searching
                            entry.PropertyChanged -= this.OnTypeNodePropertyChanged;
                            entry.IsSelected = true;
                            entry.PropertyChanged += this.OnTypeNodePropertyChanged;
                        }
                    }
                };
                if (this.currentSearch != null)
                {
                    this.currentSearch.Abort();
                }
                this.ClearSelection();
                this.currentSearch = newSearch;
                this.currentSearch.Run();
            }
        }

        private void ClearSelection()
        {
            if (this.selectedTypeNode != null)
            {
                this.selectedTypeNode.IsSelected = false;
            }
        }

        private void OnTypeNodePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsSelected")
            {
                TypeNode updatedTypeNode = (TypeNode)sender;
                if (updatedTypeNode.IsSelected && updatedTypeNode != this.selectedTypeNode)
                {
                    this.UpdateSelection(updatedTypeNode);
                    this.updateFromSelection = true;
                    try
                    {
                        this.TypeName = this.selectedTypeNode.FullName;
                    }
                    finally
                    {
                        this.updateFromSelection = false;
                    }
                }
            }
        }

        private void UpdateSelection(TypeNode entry)
        {
            this.selectedTypeNode = entry;

            this.GenericTypeParameters.Clear();
            if (this.selectedTypeNode != null && this.selectedTypeNode.Data.IsGenericTypeDefinition)
            {
                foreach (var argument in this.selectedTypeNode.Data.GetGenericArguments())
                {
                    this.GenericTypeParameters.Add(new GenericTypeParameter(argument, this.serviceProvider));
                }
            }
            Notify("HasGenericParameters");

            this.ConcreteType = entry != null ? entry.Data : null;
        }

        #region INodeCreator implementation

        TypeNode INodeCreator.CreateTypeNode(Type type)
        {
            var node = new TypeNode(type);
            node.PropertyChanged += OnTypeNodePropertyChanged;

            return node;
        }

        NamespaceNode INodeCreator.CreateNamespaceNode(string name)
        {
            return new NamespaceNode(name);
        }

        AssemblyNode INodeCreator.CreateAssemblyNode(Assembly assembly)
        {
            return new AssemblyNode(assembly, this, this.typeFilter);
        }

        #endregion

        private class NullServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                return null;
            }
        }

        private readonly Func<Type, bool> typeFilter;
        private readonly IServiceProvider serviceProvider;

        private bool updateFromSelection;
        private string typeName;
        private TypeNode selectedTypeNode;
        private Type concreteType;

        private SearchAction currentSearch;
    }

    /// <summary>
    /// Interface for the creation of nodes.
    /// </summary>
    public interface INodeCreator
    {
        /// <summary>
        /// Creates a new type node.
        /// </summary>
        TypeNode CreateTypeNode(Type type);

        /// <summary>
        /// Creates a new namespace node.
        /// </summary>
        NamespaceNode CreateNamespaceNode(string name);

        /// <summary>
        /// Creates a new assembly node.
        /// </summary>
        AssemblyNode CreateAssemblyNode(Assembly assembly);
    }

    #region Node classes

    /// <summary>
    /// Base class for nodes in the type browser view model.
    /// </summary>
    public class Node : INotifyPropertyChanged
    {
        private bool isExpanded;
        private bool isSelected;
        private Visibility visibility;

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies that a property has changed its value.
        /// </summary>
        /// <param name="property">The name of the changed property.</param>
        protected void Notify(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Gets or sets the expanded state.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    this.Notify("IsExpanded");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selection state.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.Notify("IsSelected");
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return this.visibility;
            }
            set
            {
                if (this.visibility != value)
                {
                    this.visibility = value;
                    this.Notify("Visibility");
                }
            }
        }
    }

    /// <summary>
    /// Represents a type.
    /// </summary>
    public class TypeNode : Node
    {
        private string displayName;
        private string fullName;
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNode"/> class for a type.
        /// </summary>
        /// <param name="type">The represented type.</param>
        public TypeNode(Type type)
        {
            this.type = type;
            this.displayName = GetTypeName(type);
            this.fullName = type.FullName;
        }

        private string GetTypeName(Type type)
        {
            return type.IsNested ? GetTypeName(type.DeclaringType) + '+' + type.Name : type.Name;
        }

        /// <summary>
        /// Gets the represented type.
        /// </summary>
        public Type Data
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }
    }

    /// <summary>
    /// Represents a namespace.
    /// </summary>
    public class NamespaceNode : Node
    {
        private string displayName;
        private ObservableCollection<TypeNode> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceNode"/> class for a namespace name.
        /// </summary>
        /// <param name="name">The namespace name.</param>
        public NamespaceNode(string name)
        {
            this.displayName = name ?? "";
            this.types = new ObservableCollection<TypeNode>();
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        /// <summary>
        /// Gets the nodes representing the types in the represented namespace.
        /// </summary>
        public ObservableCollection<TypeNode> Types
        {
            get
            {
                return this.types;
            }
        }
    }

    /// <summary>
    /// Represents an assembly.
    /// </summary>
    public class AssemblyNode : Node
    {
        private readonly Assembly assembly;
        private readonly string displayName;
        private readonly INodeCreator nodeCreator;
        private readonly Func<Type, bool> typeFilter;
        private ObservableCollection<NamespaceNode> namespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNode"/> class.
        /// </summary>
        /// <param name="assembly">The represented assembly.</param>
        /// <param name="nodeCreator">The node creator.</param>
        /// <param name="typeFilter">The type filter predicate.</param>
        public AssemblyNode(Assembly assembly, INodeCreator nodeCreator, Func<Type, bool> typeFilter)
        {
            this.assembly = assembly;
            this.displayName = assembly.GetName().Name;
            this.nodeCreator = nodeCreator;
            this.typeFilter = typeFilter ?? (t => true);
            this.Visibility = this.Namespaces.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        /// <summary>
        /// Gets the nodes representing the namespaces in the represented assembly.
        /// </summary>
        public ObservableCollection<NamespaceNode> Namespaces
        {
            get
            {
                if (this.namespaces == null)
                {
                    this.namespaces = new ObservableCollection<NamespaceNode>();

                    IEnumerable<Type> types;

                    types = this.GetAvailableTypes(this.assembly)
                                .Where(this.typeFilter)
                                .OrderBy(t => t.Namespace)
                                .ThenBy(t => t.FullName);

                    NamespaceNode namespaceNode = null;
                    foreach (var type in types)
                    {
                        var namespaceName = type.Namespace ?? string.Empty;
                        if (namespaceNode == null
                            || !StringComparer.Ordinal.Equals(namespaceNode.DisplayName, namespaceName))
                        {
                            namespaceNode = this.nodeCreator.CreateNamespaceNode(namespaceName);
                            this.namespaces.Add(namespaceNode);
                        }

                        namespaceNode.Types.Add(this.nodeCreator.CreateTypeNode(type));
                    }
                }
                return this.namespaces;
            }
        }

        private IEnumerable<Type> GetAvailableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (FileLoadException) { }
            catch (TypeLoadException) { }
            catch (ReflectionTypeLoadException) { }

            return MetadataTypesRetriever.GetAvailableTypes(assembly);
        }
    }

    /// <summary>
    /// Represents an assembly group.
    /// </summary>
    public class AssemblyGroupNode : Node
    {
        private readonly AssemblyGroup assemblyGroup;
        private readonly string displayName;
        private readonly INodeCreator nodeCreator;
        private ObservableCollection<AssemblyNode> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyGroupNode"/> class.
        /// </summary>
        /// <param name="assemblyGroup">The represented assembly group.</param>
        /// <param name="nodeCreator">The node creator.</param>
        public AssemblyGroupNode(AssemblyGroup assemblyGroup, INodeCreator nodeCreator)
        {
            this.assemblyGroup = assemblyGroup;
            this.displayName = assemblyGroup.Name;
            this.nodeCreator = nodeCreator;
            this.IsExpanded = true;
            this.Visibility = this.Assemblies.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        /// <summary>
        /// Gets the nodes representing the assemblies in the represented assembly group.
        /// </summary>
        public ObservableCollection<AssemblyNode> Assemblies
        {
            get
            {
                if (this.assemblies == null)
                {
                    this.assemblies =
                        new ObservableCollection<AssemblyNode>(
                            this.assemblyGroup.Assemblies
                                .OrderBy(a => a.GetName().Name)
                                .FilterSelectSafe(a => this.nodeCreator.CreateAssemblyNode(a)));
                }
                return this.assemblies;
            }
        }
    }

    #endregion

    /// <summary>
    /// Represents a generic type parameter in a generic type.
    /// </summary>
    public class GenericTypeParameter : Property
    {
        private static readonly Attribute[] attributes = new Attribute[] { 
            new EditorAttribute(typeof(TypeSelectionEditor), typeof(UITypeEditor)),
            new EditorWithReadOnlyTextAttribute(true),
            new ResourceDescriptionAttribute(typeof(Properties.Resources), "GenericTypeParameter"),
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTypeParameter"/> class.
        /// </summary>
        /// <param name="typeParameter">The represented generic type parameter.</param>
        /// <param name="serviceProvider">A service provider.</param>
        public GenericTypeParameter(Type typeParameter, IServiceProvider serviceProvider)
            : base(serviceProvider, null, null, attributes)
        {
            this.TypeParameter = typeParameter;
        }

        /// <summary>
        /// Gets the represented type parameter.
        /// </summary>
        public Type TypeParameter { get; private set; }

        /// <summary>
        /// Gets or sets the concrete type specified for the represented type parameter.
        /// </summary>
        public Type TypeArgument { get; set; }

        /// <summary>
        /// Gets a value indicating if the property is required.
        /// </summary>
        public override bool IsRequired
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public override string PropertyName
        {
            get
            {
                return this.TypeParameter.Name;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the property.
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return typeof(Type);
            }
        }

        /// <summary>
        /// Gets the underlying, stored value.
        /// </summary>
        protected override object GetValue()
        {
            return this.TypeArgument;
        }

        /// <summary>
        /// Sets the underlying, stored value.
        /// </summary>
        protected override void SetValue(object value)
        {
            var valueAsString = value as string;
            if (valueAsString != null)
            {
                this.TypeArgument = Type.GetType(valueAsString);
            }
            else
            {
                this.TypeArgument = (Type)value;
            }
        }
    }
}
