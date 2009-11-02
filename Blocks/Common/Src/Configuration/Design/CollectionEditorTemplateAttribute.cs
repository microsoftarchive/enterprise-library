using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    public class CollectionEditorTemplateAttribute : Attribute
    {
        readonly string headerTemplate;
        readonly string itemTemplate;

        /// <summary/>
        public CollectionEditorTemplateAttribute(string headerTemplate, string itemTemplate)
        {
            this.headerTemplate = headerTemplate;
            this.itemTemplate = itemTemplate;
        }

        /// <summary/>
        public string ItemTemplate
        {
            get { return itemTemplate; }
        }

        /// <summary/>
        public string HeaderTemplate
        {
            get { return headerTemplate; }
        }
    }
}
