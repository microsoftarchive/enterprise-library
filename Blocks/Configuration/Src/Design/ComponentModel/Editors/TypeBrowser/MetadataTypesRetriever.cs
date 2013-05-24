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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Retrieves types from assemblies, skipping the types that cannot be loaded.
    /// </summary>
    public static class MetadataTypesRetriever
    {
        private const string CLSID_CorMetaDataDispenser = "E5CB7A31-7512-11d2-89CE-0080C792E5D8";
        private const string IID_IMetaDataDispenser = "809C652E-7396-11D2-9771-00A0C9B4D50C";
        private const string IID_IMetaDataImport = "7DAC8207-D3AE-4C75-9B67-92801A497D44";

        private const int MaxClassNameLength = 1024;

        /// <summary>
        /// Retrieves types from <paramref name="assembly"/>. 
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The types that could be retrieved from the assembly.</returns>
        public static IEnumerable<Type> GetAvailableTypes(Assembly assembly)
        {
            return GetAvailableTypeNames(assembly).Select(tn => assembly.GetType(tn, false)).Where(t => t != null);
        }

        private static IEnumerable<string> GetAvailableTypeNames(Assembly assembly)
        {
            var dispenserType = Type.GetTypeFromCLSID(new Guid(CLSID_CorMetaDataDispenser), false);
            if (dispenserType == null)
            {
                yield break;
            }

            IMetaDataDispenserPrivate dispenser;
            try
            {
                dispenser = (IMetaDataDispenserPrivate)Activator.CreateInstance(dispenserType);
            }
            catch
            {
                yield break;
            }

            var location = assembly.Location;
            var metaDataImportGuid = new Guid(IID_IMetaDataImport);
            object scope;
            IMetaDataImportPrivate import;
            try
            {
                dispenser.OpenScope(location, 0, ref metaDataImportGuid, out scope);
                import = (IMetaDataImportPrivate)scope;
            }
            catch
            {
                yield break;
            }

            IntPtr enumHandle = IntPtr.Zero;
            uint[] typeDefs = new uint[10];
            uint count = 0;
            import.EnumTypeDefs(ref enumHandle, typeDefs, (uint)typeDefs.Length, out count);

            try
            {
                while (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {

                        yield return GetFullTypeName(import, typeDefs[i]);
                    }

                    import.EnumTypeDefs(ref enumHandle, typeDefs, (uint)typeDefs.Length, out count);
                }
            }
            finally
            {
                import.CloseEnum(enumHandle);
            }
        }

        private static string GetFullTypeName(IMetaDataImportPrivate import, uint token)
        {
            uint actualNameLength;
            uint typeDefFlags;
            uint baseTypeToken;

            var fullTypeName = new StringBuilder(MaxClassNameLength);
            var currentTypeName = new StringBuilder(MaxClassNameLength + 1, MaxClassNameLength + 1);

            while (token != 0)
            {
                currentTypeName.Length = 0;

                import.GetTypeDefProps(token, currentTypeName, MaxClassNameLength, out actualNameLength, out typeDefFlags, out baseTypeToken);

                if (fullTypeName.Length > 0)
                {
                    currentTypeName.Append('+');
                }

                fullTypeName.Insert(0, currentTypeName.ToString());

                if (IsTdNested(typeDefFlags))
                {
                    import.GetNestedClassProps(token, out token);
                }
                else
                {
                    token = 0;
                }
            }

            return fullTypeName.ToString();
        }

        private static bool IsTdNested(uint typeDefFlags)
        {
            return (typeDefFlags & 0x00000007) >= 0x00000002;
        }

        [ComImport, Guid(IID_IMetaDataDispenser), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMetaDataDispenserPrivate
        {
            void DefineScope_Placeholder();
            void OpenScope([In, MarshalAs(UnmanagedType.LPWStr)] string szScope, [In] int dwOpenFlags, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object punk);
        }

        [ComImport, GuidAttribute(IID_IMetaDataImport), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMetaDataImportPrivate
        {
            void CloseEnum(IntPtr hEnum);

            void CountEnum_Placeholder();

            void ResetEnum_Placeholder();

            void EnumTypeDefs(ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)]uint[] rTypeDefs, uint cMax, out uint pcTypeDefs);

            void EnumInterfaceImpls_Placeholder();

            void EnumTypeRefs_Placeholder();

            void FindTypeDefByName_Placeholder();

            void GetScopeProps_Placeholder();

            void GetModuleFromScope_Placeholder();

            void GetTypeDefProps(uint td, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder szTypeDef, uint cchTypeDef, out uint pchTypeDef, out uint pdwTypeDefFlags, out uint ptkExtends);

            void GetInterfaceImplProps_Placeholder();

            void GetTypeRefProps_Placeholder();

            void ResolveTypeRef_Placeholder();

            void EnumMembers_Placeholder();

            void EnumMembersWithName_Placeholder();

            void EnumMethods_Placeholder();

            void EnumMethodsWithName_Placeholder();

            void EnumFields_Placeholder();

            void EnumFieldsWithName_Placeholder();

            void EnumParams_Placeholder();

            void EnumMemberRefs_Placeholder();

            void EnumMethodImpls_Placeholder();

            void EnumPermissionSets_Placeholder();

            void FindMember_Placeholder();

            void FindMethod_Placeholder();

            void FindField_Placeholder();

            void FindMemberRef_Placeholder();

            void GetMethodProps_Placeholder();

            void GetMemberRefProps_Placeholder();

            void EnumProperties_Placeholder();

            void EnumEvents_Placeholder();

            void GetEventProps_Placeholder();

            void EnumMethodSemantics_Placeholder();

            void GetMethodSemantics_Placeholder();

            void GetClassLayout_Placeholder();

            void GetFieldMarshal_Placeholder();

            void GetRVA_Placeholder();

            void GetPermissionSetProps_Placeholder();

            void GetSigFromToken_Placeholder();

            void GetModuleRefProps_Placeholder();

            void EnumModuleRefs_Placeholder();

            void GetTypeSpecFromToken_Placeholder();

            void GetNameFromToken_Placeholder();

            void EnumUnresolvedMethods_Placeholder();

            void GetUserString_Placeholder();

            void GetPinvokeMap_Placeholder();

            void EnumSignatures_Placeholder();

            void EnumTypeSpecs_Placeholder();

            void EnumUserStrings_Placeholder();

            void GetParamForMethodIndex_Placeholder();

            void EnumCustomAttributes_Placeholder();

            void GetCustomAttributeProps_Placeholder();

            void FindTypeRef_Placeholder();

            void GetMemberProps_Placeholder();

            void GetFieldProps_Placeholder();

            void GetPropertyProps_Placeholder();

            void GetParamProps_Placeholder();

            void GetCustomAttributeByName_Placeholder();

            bool IsValidToken_Placeholder();

            void GetNestedClassProps(uint tdNestedClass, out uint ptdEnclosingClass);

            void GetNativeCallConvFromSig_Placeholder();

            void IsGlobal_Placeholder();
        }
    }
}
