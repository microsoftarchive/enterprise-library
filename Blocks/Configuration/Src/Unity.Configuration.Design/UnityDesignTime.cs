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

using System.ComponentModel;
using System.Configuration;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Configuration.Design;
using Microsoft.Practices.Unity.Configuration.Design.Commands;
using Microsoft.Practices.Unity.Configuration.Design.Editors;
using Microsoft.Practices.Unity.Configuration.Design.Validation;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity
{
    public static class UnityDesignTime
    {
        #region Nested type: MetadataTypes

        /// <summary/>
        public static class MetadataTypes
        {
            #region Nested type: AliasElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (AliasElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<AliasElement>))]
            public abstract class AliasElementCollectionMetadata
            {
            }

            #endregion


            #region Nested type: ArrayElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ArrayElement))]
            [ResourceDisplayName(typeof(DesignResources), "ArrayElementDisplayName")]
            public abstract class ArrayElementMetadata
            {
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (object), TypeSelectorIncludes.All)]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }

                [Editor(typeof (PopupCollectionEditor), typeof (UITypeEditor))]
                [DesignTimeReadOnly(false)]
                [EditorWithReadOnlyText(true)]
                public ParameterValueElementCollection Values { get; set; }
            }

            #endregion

            #region Nested type: AssemblyElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (AssemblyElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<AssemblyElement>))]
            public abstract class AssemblyElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: AssemblyElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (AssemblyElement))]
            public abstract class AssemblyElementMetadata
            {
                [TypeConverter(typeof (AssemblySuggestionTypeConverter))]
                public string Name { get; set; }
            }

            #endregion

            #region Nested type: ConstructorElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ConstructorElement))]
            [ViewModel(typeof (ConstructorElementViewModel))]
            [Command(typeof (AddRegistrationConstructorCommand),
                TitleResourceName = "AddConstructor",
                TitleResourceType = typeof (DesignResources),
                CommandPlacement = CommandPlacement.ContextAdd,
                Replace = CommandReplacement.DefaultAddCommandReplacement)]
            [ResourceDisplayName(typeof (DesignResources), "ConstructorElementDisplayName")]
            public abstract class ConstructorElementMetadata
            {
                [PromoteCommands]
                public ParameterElementCollection Parameters { get; set; }
            }

            #endregion

            #region Nested type: ContainerElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ContainerElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<ContainerElement>))]
            [ResourceDisplayName(typeof (DesignResources), "ContainerElementDisplayName")]
            public abstract class ContainerElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: ContainerElementMetadata

            /// <summary/>
            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ContainerElement))]
            [ViewModel(typeof (ContainerElementViewModel))]
            [NameProperty("Name", NamePropertyDisplayFormat = "Container '{0}'")]
            [ResourceDisplayName(typeof (DesignResources), "ContainerElementDisplayName")]
            public abstract class ContainerElementMetadata
            {
                [DesigntimeDefault("")]
                public string Name { get; set; }

                [PromoteCommands]
                public InstanceElementCollection Instances { get; set; }

                [PromoteCommands]
                public RegisterElementCollection Registrations { get; set; }

                [PromoteCommands]
                public ContainerExtensionElementCollection Extensions { get; set; }
            }

            #endregion

            #region Nested type: ContainerExtensionElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ContainerExtensionElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<ContainerExtensionElement>))]
            public abstract class ContainerExtensionElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: ContainerExtensionElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ContainerExtensionElement))]
            [NameProperty("TypeName", NamePropertyDisplayFormat = "Extension type {0}")]
            [ResourceDisplayName(typeof(DesignResources), "ContainerExtensionElementDisplayName")]
            [TypePickingCommand(Replace=CommandReplacement.DefaultAddCommandReplacement)]
            public abstract class ContainerExtensionElementMetadata
            {
                [DesigntimeDefault("")]
                [ResourceDisplayName(typeof(DesignResources), "ContainerExtensionTypeName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof(UnityContainerExtension), TypeSelectorIncludes.All)]
                [Validation(typeof (TypeNameValidator))]
                [Validation(typeof(ExtensionTypeValidator))]
                public string TypeName { get; set; }
            }

            #endregion

            #region Nested type: DependencyElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (DependencyElement))]
            [ResourceDisplayName(typeof(DesignResources), "DependencyElementDisplayName")]
            public abstract class DependencyElementMetadata
            {
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof(object), TypeSelectorIncludes.All)]
                [ResourceDisplayName(typeof(DesignResources), "DependencyElementTypeNameDisplayName")]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }
            }

            #endregion

            #region Nested type: InjectionMemberElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (InjectionMemberElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<InjectionMemberElement>))]
            [ViewModel(typeof (InjectionMemberCollectionViewModel))]
            [ResourceDisplayName(typeof(DesignResources), "InjectionMembersCollectionDisplayName")]
            public abstract class InjectionMemberElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: InstanceElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (InstanceElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<InstanceElement>))]
            public abstract class InstanceElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: InstancElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (InstanceElement))]
            [ViewModel(typeof (InstanceElementViewModel))]
            [ResourceDisplayName(typeof (DesignResources), "InstanceElementDisplayName")]
            [NameProperty("TypeName", NamePropertyDisplayFormat = "Instance '{0}'")]
            public abstract class InstancElementMetadata
            {
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (object), TypeSelectorIncludes.All)]
                [ResourceDisplayName(typeof (DesignResources), "InstanceTypeNameDisplayName")]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }

                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (TypeConverter), TypeSelectorIncludes.All)]
                [ResourceDisplayName(typeof (DesignResources), "InstanceTypeConverterTypeNameDisplayName")]
                [Validation(typeof (TypeNameValidator))]
                [Validation(typeof (TypeConverterNameValidator))]
                public string TypeConverterTypeName { get; set; }
            }

            #endregion

            #region Nested type: LifetimeElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (LifetimeElement))]
            public abstract class LifetimeElementMetadata
            {
                [ResourceCategory(typeof (DesignResources), "CategoryLifetime")]
                [ResourceDisplayName(typeof (DesignResources), "LifetimeElemenetTypeConverterTypeNameDisplayName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (TypeConverter))]
                [Validation(typeof (TypeNameValidator))]
                [Validation(typeof (TypeConverterNameValidator))]
                public string TypeConverterTypeName { get; set; }

                [ResourceCategory(typeof (DesignResources), "CategoryLifetime")]
                [ResourceDisplayName(typeof (DesignResources), "LifetimeElementTypeNameDisplayName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (LifetimeManager))]
                public string TypeName { get; set; }

                [ResourceCategory(typeof (DesignResources), "CategoryLifetime")]
                [ResourceDisplayName(typeof (DesignResources), "LifetimeElementValueDisplayName")]
                public string Value { get; set; }
            }

            #endregion

            #region Nested type: MethodElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (MethodElement))]
            [ViewModel(typeof (MethodElementViewModel))]
            [Command(typeof (AddRegistrationMethodCommand),
                TitleResourceName = "AddMethod",
                TitleResourceType = typeof (DesignResources),
                CommandPlacement = CommandPlacement.ContextAdd,
                Replace = CommandReplacement.DefaultAddCommandReplacement)]
            [NameProperty("Name", NamePropertyDisplayFormat = "Injection Method '{0}'")]
            [ResourceDisplayName(typeof (DesignResources), "MethodElementDisplayName")]
            public abstract class MethodElementMetadata
            {
                [DesigntimeDefault("")]
                public string Name { get; set; }

                [PromoteCommands]
                public ParameterElementCollection Parameters { get; set; }
            }

            #endregion

            #region Nested type: NamespaceElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (NamespaceElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<NamespaceElement>))]
            public abstract class NamespaceElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: NamespaceElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (NamespaceElement))]
            public abstract class NamespaceElementMetadata
            {
                [TypeConverter(typeof (NamespaceSuggestionTypeConverter))]
                public string Name { get; set; }
            }

            #endregion

            #region Nested type: OptionalElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (OptionalElement))]
            [ResourceDisplayName(typeof(DesignResources), "OptionalElementDisplayName")]
            public abstract class OptionalElementMetadata
            {
                [ResourceDisplayName(typeof(DesignResources), "OptionalElementTypeNameDisplayName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (object), TypeSelectorIncludes.All)]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }
            }

            #endregion

            #region Nested type: ParameterElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ParameterElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<ParameterElement>))]
            public abstract class ParameterElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: ParameterElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ParameterElement))]
            [ViewModel(typeof (ParameterElementViewModel))]
            [NameProperty("Name")]
            [CloneableConfigurationElementType(typeof(ParameterElementCloneable))]
            [ResourceDisplayName(typeof(DesignResources), "ParameterElementDisplayName")]
            public abstract class ParameterElementMetadata
            {
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }
            }

            #endregion

            #region Nested type: ParameterValueCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ParameterValueElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<ParameterValueElement>))]
            [ViewModel(typeof (ParameterValueCollectionViewModel))]
            [ConfigurationCollection(typeof (ParameterValueElement))]
            public abstract class ParameterValueCollectionMetadata
            {
            }

            #endregion

            #region Nested type: ParameterValueElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [ConfigurationCollection(typeof (ParameterValueElement))]
            [RegisterAsMetadataType(typeof (ParameterValueElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<ParameterValueElement>))]
            public class ParameterValueElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: PropertyElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (PropertyElement))]
            [ViewModel(typeof (PropertyElementViewModel))]
            [Command(typeof (AddRegistrationPropertyCommand),
                TitleResourceName = "AddProperty",
                TitleResourceType = typeof (DesignResources),
                CommandPlacement = CommandPlacement.ContextAdd,
                Replace = CommandReplacement.DefaultAddCommandReplacement)]
            [NameProperty("Name", NamePropertyDisplayFormat = "Injection Property '{0}'")]
            [ResourceDisplayName(typeof (DesignResources), "PropertyElementDisplayName")]
            [CloneableConfigurationElementType(typeof(PropertyElementCloneable))]
            public abstract class PropertyElementMetadata
            {
                [DesigntimeDefault("")]
                public string Name { get; set; }
            }

            #endregion

            #region Nested type: RegisterElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (RegisterElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<RegisterElement>))]
            public abstract class RegisterElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: RegisterElementMetadata

            /// <summary/>
            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (RegisterElement))]
            [ViewModel(typeof (RegisterElementViewModel))]
            [TypePickingCommand(TitleResourceName = "AddRegistrationTypePicker",
                TitleResourceType = typeof (DesignResources))]
            [NameProperty("TypeName", NamePropertyDisplayFormat = "Registration '{0}'")]
            [ResourceDisplayName(typeof (DesignResources), "RegisterElementDisplayName")]
            public abstract class RegisterElementMetadata
            {
                [DesigntimeDefault("")]
                public string Name { get; set; }

                [PromoteCommands]
                public InjectionMemberElementCollection InjectionMembers { get; set; }

                [ResourceDisplayName(typeof(DesignResources), "RegistrationTypeNameDisplayName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (object), TypeSelectorIncludes.All)]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }

                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof(object), TypeSelectorIncludes.BaseType | TypeSelectorIncludes.NonpublicTypes)]
                [Validation(typeof (MapToNameValidator))]
                public string MapToName { get; set; }
            }

            #endregion

            #region Nested type: SectionExtensionElementCollectionMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (SectionExtensionElementCollection))]
            [MergeableConfigurationCollectionType(
                typeof (MergeableDeserializableConfigurationElementCollection<SectionExtensionElement>))]
            public abstract class SectionExtensionElementCollectionMetadata
            {
            }

            #endregion

            #region Nested type: TypeAliasElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (AliasElement))]
            [NameProperty("Alias")]
            [ViewModel(typeof (AliasElementViewModel))]
            [TypePickingCommand(Replace = CommandReplacement.DefaultAddCommandReplacement)]
            [ResourceDisplayName(typeof (DesignResources), "TypeAliasDisplayName")]
            [CloneableConfigurationElementType(typeof(AliasElementCloneable))]
            public abstract class TypeAliasElementMetadata
            {
                [ViewModel(typeof(AliasProperty))]
                public string Alias { get; set; } 

                [ResourceDisplayName(typeof(DesignResources), "AliasTypeNameDisplayName")]
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof(object), TypeSelectorIncludes.All)]
                [Validation(typeof (TypeNameValidator))]
                public string TypeName { get; set; }
            }

            #endregion

            #region Nested type: UnityConfigurationSectionMetadata

            [EnvironmentalOverrides(false)]
            [ViewModel(typeof (UnitySectionViewModel))]
            [RegisterAsMetadataType(typeof (UnityConfigurationSection))]
            [ResourceDisplayName(typeof(DesignResources), "UnityConfigurationSectionDisplayName")]
            public abstract class UnityConfigurationSectionMetadata
            {
                [DesignTimeReadOnly(false)]
                [Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
                public NamespaceElementCollection Namespaces { get; set; }

                [DesignTimeReadOnly(false)]
                [Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
                public AssemblyElementCollection Assemblies { get; set; }

                [ResourceDisplayName(typeof (DesignResources), "TypeAliasesPropertyDisplayName")]
                public AliasElementCollection TypeAliases { get; set; }
            }

            #endregion

            #region Nested type: ValueElementMetadata

            [EnvironmentalOverrides(false)]
            [RegisterAsMetadataType(typeof (ValueElement))]
            [ResourceDisplayName(typeof(DesignResources), "ValueElementDisplayName")]
            public abstract class ValueElementMetadata
            {
                [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
                [BaseType(typeof (TypeConverter))]
                [Validation(typeof (TypeNameValidator))]
                [Validation(typeof (TypeConverterNameValidator))]
                public string TypeConverterTypeName { get; set; }
            }

            #endregion
        }

        #endregion
    }
}
