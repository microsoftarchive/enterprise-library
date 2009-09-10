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
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;


namespace Console.Wpf.ViewModel.Services
{
    /// <summary>
    /// night not need this service
    /// </summary>
    public class ElementLookup
    {
        List<SectionViewModel> sections = new List<SectionViewModel>();

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type scope, Type configurationType)
        {
            var scopeElements = FindInstancesOfConfigurationType(scope);
            return FindInstancesOfConfigurationType(scopeElements, configurationType);
        }

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(ElementViewModel scope, Type configurationType)
        {
            return FindInstancesOfConfigurationType(new[] { scope }, configurationType);
        }

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type configurationType)
        {
            return FindInstancesOfConfigurationType(BuildGlobalScope(), configurationType);
        }

        private IEnumerable<ElementViewModel> BuildGlobalScope()
        {
            var sectionsThemSelves = sections.Cast<ElementViewModel>();

            return sectionsThemSelves;
        }

        private IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(IEnumerable<ElementViewModel> scope, Type configurationType)
        {
            var roots = scope;
            var descendentsOfRoots = scope.SelectMany(x => x.DescendentElements());
            var allElements = roots.Union(descendentsOfRoots);

            return allElements.Where(x => configurationType.IsAssignableFrom(x.ConfigurationType));
        }

        public void AddSection(SectionViewModel sectionModel)
        {
            sections.Add(sectionModel);
        }

        public IEnumerable<IElementExtendedPropertyProvider> FindExtendedPropertyProviders()
        {
            var roots = BuildGlobalScope();
            var descendentsOfRoots = roots.SelectMany(x => x.DescendentElements());
            var allElements = roots.Union(descendentsOfRoots);

            return allElements.OfType<IElementExtendedPropertyProvider>();
        }
    }
}
