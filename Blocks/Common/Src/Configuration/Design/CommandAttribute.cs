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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System.Resources;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// Attribute used to decorate a designtime View Model element with an executable command. E.g. a context menu item that allows
    /// the user to perform an action in the elements context.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Assembly, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class, specifying the Command Model Type.
        /// </summary>
        /// <remarks>
        /// The Command Model Type should derive from the CommandModel class in the Configuration.Design assembly. <br/>
        /// As this attribute can be applied to the configuration directly and we dont want to force a dependency on the Configuration.Design assembly <br/>
        /// You can specify the Command Model Type in a loosy coupled fashion.
        /// </remarks>
        /// <param name="commandModelTypeName">The fully qualified name of the Command Model Type.</param>
        public CommandAttribute(string commandModelTypeName)
        {
            if (string.IsNullOrEmpty(commandModelTypeName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "commandModelTypeName");

            this.CommandModelTypeName = commandModelTypeName;
            this.Replace = CommandReplacement.NoCommand;
            this.CommandPlacement = CommandPlacement.ContextCustom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class, specifying the Command Model Type.
        /// </summary>
        /// <remarks>
        /// The Command Model Type should derive from the CommandModel class in the Configuration.Design assmbly. <br/>
        /// As this attribute can be applied to the configuration directly and we dont want to force a dependency on the Configuration.Design assembly <br/>
        /// You can specify the Command Model Type in a loosy coupled fashion.
        /// </remarks>
        /// <param name="commandModelType">The Command Model Type.</param>
        public CommandAttribute(Type commandModelType)
            :this(commandModelType != null ? commandModelType.AssemblyQualifiedName : string.Empty)
        {
        }

        /// <summary>
        /// Gets or sets the name of the resource, used to return a localized title that will be shown for this command in the UI (User Interface).
        /// </summary>
        public string TitleResourceName { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource, used to return a localized title that will be shown for this command in the UI (User Interface).
        /// </summary>
        public Type TitleResourceType { get; set; }

        /// <summary>
        /// Gets the title that will be shown for this command in the UI (User Interface).
        /// </summary>
        public string Title
        {
            get
            {
                if (TitleResourceName != null && TitleResourceType != null)
                {
                    EnsureTitleLoaded();
                }
                return title;
            }
            set
            {
                title = value;
            }
        }

        private string title;

        private bool resourceLoaded;
        private void EnsureTitleLoaded()
        {
            if (resourceLoaded) return;

            var rm = new ResourceManager(TitleResourceType);

            try
            {
                title = rm.GetString(TitleResourceName);
            }
            catch (MissingManifestResourceException)
            {
                title = TitleResourceName;
            }

            resourceLoaded = true;
        }

        /// <summary/>
        public CommandReplacement Replace { get; set; }

        /// <summary/>
        public CommandPlacement CommandPlacement { get; set; }

        /// <summary>
        /// Gets or Sets the Command Model Type Name for this command. <br/>
        /// The Command Model Type will be used at runtime to display and execute the command.<br/>
        /// Command Model Types should derive from the CommandModel class in the Configuration.Design assembly. 
        /// </summary>
        public string CommandModelTypeName { get; set; }

        /// <summary>
        /// Gets the Command Model Type for this command. <br/>
        /// The Command Model Type will be used at runtime to display and execute the command.<br/>
        /// Command Model Types should derive from the CommandModel class in the Configuration.Design assembly. 
        /// </summary>
        public Type CommandModelType
        {
            get { return Type.GetType(CommandModelTypeName, true); }
        }

        /// <summary>
        /// Defines the keyboard gesture for this command.
        /// </summary>
        /// <example>
        ///     command.KeyGesture = "Ctrl+1";
        /// </example>
        public string KeyGesture { get; set; }
    }

    /// <summary/>
    public enum CommandReplacement
    {
        /// <summary/>
        DefaultAddCommandReplacement,

        /// <summary/>
        DefaultDeleteCommandReplacement,

        /// <summary/>
        NoCommand
    }


    /// <summary/>
    public enum CommandPlacement
    {
        /// <summary/>
        FileMenu,

        /// <summary/>
        BlocksMenu,

        /// <summary/>
        ContextAdd,

        /// <summary/>
        ContextCustom,

        /// <summary/>
        ContextDelete,
    }
}
