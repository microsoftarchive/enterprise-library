using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    public class AddApplicationBlockCommandAttribute : CommandAttribute
    {

        private readonly string sectionName;
        private readonly Type configurationSectionType;

        /// <summary/>
        public AddApplicationBlockCommandAttribute(string title, string sectionName, Type configurationSectionType)
            :base(CommonDesignTime.CommandTypeNames.AddApplicationBlockCommand)
        {
            CommandPlacement = CommandPlacement.BlocksMenu;

            base.Title = title;
            this.sectionName = sectionName;
            this.configurationSectionType = configurationSectionType;
        }

        /// <summary/>
        public string SectionName
        {
            get { return sectionName; }
        }

        /// <summary/>
        public Type ConfigurationSectionType
        {
            get { return configurationSectionType; }
        }
    }
}
