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
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    public class TypeBrowserViewModel : INotifyPropertyChanged, INodeCreator
    {
        public TypeBrowserViewModel(IEnumerable<AssemblyGroup> assemblyGroups)
            : this(assemblyGroups, null)
        { }

        public TypeBrowserViewModel(IEnumerable<AssemblyGroup> assemblyGroups, Func<Type, bool> typeFilter)
        {
            this.typeFilter = typeFilter;

            this.AssemblyGroups = new ObservableCollection<AssemblyGroupNode>();
            this.GenericTypeParameters = new ObservableCollection<GenericTypeParameter>();

            this.UpdateAssemblyGroups(assemblyGroups);
        }

        public void UpdateAssemblyGroups(IEnumerable<AssemblyGroup> assemblyGroups)
        {
            this.AssemblyGroups.Clear();
            foreach (var group in assemblyGroups)
            {
                this.AssemblyGroups.Add(new AssemblyGroupNode(group, this));
            }
        }

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

        public ObservableCollection<AssemblyGroupNode> AssemblyGroups { get; private set; }

        public ObservableCollection<GenericTypeParameter> GenericTypeParameters { get; private set; }

        public bool HasGenericParameters
        {
            get
            {
                return this.GenericTypeParameters.Count > 0;
            }
        }

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

        public ICommand OkCommand
        {
            get { return new OkCommand(); }
        }

        public ICommand AddAssembliesCommand
        {
            get { return new AddAssembliesCommand(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                //this.ClearSelection();
                this.currentSearch = newSearch;
                this.currentSearch.Run();
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
                    this.GenericTypeParameters.Add(new GenericTypeParameter(argument));
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

        private readonly Func<Type, bool> typeFilter;

        private bool updateFromSelection;
        private string typeName;
        private TypeNode selectedTypeNode;
        private Type concreteType;

        private SearchAction currentSearch;
    }

    public interface INodeCreator
    {
        TypeNode CreateTypeNode(Type type);
        NamespaceNode CreateNamespaceNode(string name);
        AssemblyNode CreateAssemblyNode(Assembly assembly);
    }

    #region Node classes

    public class Node : INotifyPropertyChanged
    {
        private bool isExpanded;
        private bool isSelected;
        private Visibility visibility;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

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

    public class TypeNode : Node
    {
        private string displayName;
        private string fullName;
        private Type type;

        public TypeNode(Type type)
        {
            this.type = type;
            this.displayName = type.Name;
            this.fullName = type.FullName;
        }

        public Type Data
        {
            get
            {
                return this.type;
            }
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }
    }

    public class NamespaceNode : Node
    {
        private string displayName;
        private ObservableCollection<TypeNode> types;

        public NamespaceNode(string name)
        {
            this.displayName = name ?? "";
            this.types = new ObservableCollection<TypeNode>();
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public ObservableCollection<TypeNode> Types
        {
            get
            {
                return this.types;
            }
        }
    }

    public class AssemblyNode : Node
    {
        private readonly Assembly assembly;
        private readonly string displayName;
        private readonly INodeCreator nodeCreator;
        private readonly Func<Type, bool> typeFilter;
        private ObservableCollection<NamespaceNode> namespaces;

        public AssemblyNode(Assembly assembly, INodeCreator nodeCreator, Func<Type, bool> typeFilter)
        {
            this.assembly = assembly;
            this.displayName = assembly.GetName().Name;
            this.nodeCreator = nodeCreator;
            this.typeFilter = typeFilter ?? (t => true);
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public ObservableCollection<NamespaceNode> Namespaces
        {
            get
            {
                if (this.namespaces == null)
                {
                    this.namespaces = new ObservableCollection<NamespaceNode>();

                    var types =
                        this.assembly.GetTypes()
                            .Where(this.typeFilter)
                            .OrderBy(t => t.Namespace)
                            .ThenBy(t => t.Name);

                    NamespaceNode namespaceNode = null;
                    foreach (var type in types)
                    {
                        var namespaceName = type.Namespace;
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
    }

    public class AssemblyGroupNode : Node
    {
        private readonly AssemblyGroup assemblyGroup;
        private readonly string displayName;
        private readonly INodeCreator nodeCreator;
        private ObservableCollection<AssemblyNode> assemblies;

        public AssemblyGroupNode(AssemblyGroup assemblyGroup, INodeCreator nodeCreator)
        {
            this.assemblyGroup = assemblyGroup;
            this.displayName = assemblyGroup.Name;
            this.nodeCreator = nodeCreator;
            this.IsExpanded = true;
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public ObservableCollection<AssemblyNode> Assemblies
        {
            get
            {
                if (this.assemblies == null)
                {
                    this.assemblies =
                        new ObservableCollection<AssemblyNode>(
                            this.assemblyGroup.Assemblies.Select(a => this.nodeCreator.CreateAssemblyNode(a)));
                }
                return this.assemblies;
            }
        }
    }

    #endregion

    public class GenericTypeParameter : Property
    {
        public GenericTypeParameter(Type typeParameter)
            : base(null, null, null, new Attribute[] { new EditorAttribute(typeof(TypeSelectionEditor), typeof(UITypeEditor)) })
        {
            this.TypeParameter = typeParameter;
        }

        public Type TypeParameter { get; private set; }
        public Type TypeArgument { get; set; }

        public override string Description
        {
            get
            {
                return "Generic parameter";
            }
        }

        public override string DisplayName
        {
            get
            {
                return this.TypeParameter.Name;
            }
        }

        public override bool IsRequired
        {
            get
            {
                return true;
            }
        }

        public override string PropertyName
        {
            get
            {
                return this.TypeParameter.Name;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return typeof(Type);
            }
        }

        protected override object GetValue()
        {
            return this.TypeArgument;
        }

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

    internal class OkCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }

    internal class AddAssembliesCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
