using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Assembly, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary/>
        public CommandAttribute(string commandModelTypeName)
        {
            this.CommandModelTypeName = commandModelTypeName;
            this.Replace = CommandReplacement.NoCommand;
            this.CommandPlacement = CommandPlacement.ContextCustom;
        }

        /// <summary/>
        public string Title { get; set; }

        /// <summary/>
        public CommandReplacement Replace { get; set; }

        /// <summary/>
        public CommandPlacement CommandPlacement { get; set; }

        /// <summary/>
        public string CommandModelTypeName { get; set; }

        /// <summary/>
        public Type CommandModelType
        {
            get { return Type.GetType(CommandModelTypeName, true); }
        }
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
