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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// The <see cref="LogExceptionsToDatabaseCommand"/> initiates a new instance of the <see cref="LogExceptionsToDatabase"/> wizard.
    /// </summary>
    /// <remarks>
    /// The <see cref="LogExceptionsToDatabaseCommand"/> verifies that the pre-requisites 
    /// are satisfied before allowing it to be enabled from the menu
    /// <br/>
    /// The pre-requisite are:
    /// <list>
    /// <item>Able to get the <see cref="CommonDesignTime.SectionType.LoggingSettings"/> as a type.</item>
    /// <item>Able to get the <see cref="CommonDesignTime.SectionType.ExceptionHandlingSettings"/> as a type.</item>
    /// <item>Able to get the <see cref="CommonDesignTime.SectionType.DatabaseSettings"/> as a type.</item>
    /// </list></remarks>
    public class LogExceptionsToDatabaseCommand : WizardCommand
    {
        private static string[] dependentTypes =
            new[] { CommonDesignTime.SectionType.LoggingSettings,
                    CommonDesignTime.SectionType.ExceptionHandlingSettings,
                    CommonDesignTime.SectionType.DatabaseSettings};

        ///<summary>
        /// Initializes a new instance of <see cref="LogExceptionsToDatabaseCommand"/>.
        ///</summary>
        ///<param name="attribute">The <see cref="CommandAttribute"/> used to define this command.</param>
        ///<param name="uiService">The user-interface service for displaying messages or dialogs.</param>
        ///<param name="resolver">The resolver to use to create <see cref="WizardModel"/> instances.</param>
        public LogExceptionsToDatabaseCommand(WizardCommandAttribute attribute,
                                              IUIServiceWpf uiService, IResolver<WizardModel> resolver) :
                                                  base(attribute, uiService, resolver)
        { }

        /// <summary>
        /// Determines if the command should be available by evaluating if it can retrieve
        /// the pre-requisite types.
        /// </summary>
        /// <param name="parameter">This is not used.</param>
        /// <returns><see langowrd="true"/> if the command can execute.<br/>
        /// Otherwise, returns <see langword="false"/></returns>
        protected override bool InnerCanExecute(object parameter)
        {
            return dependentTypes.Select(t => Type.GetType(t, false, true) != null).All(b => b);
        }
    }
}
