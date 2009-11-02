using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    public class AddSateliteProviderCommandAttribute : CommandAttribute
    {
        string sectionName;

        /// <summary/>
        public AddSateliteProviderCommandAttribute(string sectionName) 
            : base(CommonDesignTime.CommandTypeNames.AddSateliteProviderCommand)
        {
            this.sectionName = sectionName;

            CommandPlacement = CommandPlacement.ContextAdd;
            Replace = CommandReplacement.DefaultAddCommandReplacement;
        }

        /// <summary/>
        public string SectionName
        {
            get { return sectionName; }
        }
    }
}
