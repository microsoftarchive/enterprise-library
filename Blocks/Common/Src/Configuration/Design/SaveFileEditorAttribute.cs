using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// Specifies additional metadata for the SaveFileEditor editor.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Property)]
    public class SaveFileEditorAttribute : Attribute
    {
        private string filter;

        /// <summary>
        /// Initialize a new instance of the <see cref="SaveFileEditorAttribute"/> class with the <see cref="Type"/> containing the resources and the resource key.
        /// </summary>
        /// <param name="resourceType">The <see cref="Type"/> containing the resources.</param>
        /// <param name="filterResourceKey">The resource key that specify the default filter.</param>
        public SaveFileEditorAttribute(Type resourceType, string filterResourceKey)
        {
            if (null == resourceType) throw new ArgumentNullException("resourceType");

            this.filter = ResourceStringLoader.LoadString(resourceType.FullName, filterResourceKey, resourceType.Assembly);
            this.CheckFileExists = true;
        }

        /// <summary>
        /// Gets the filter string that determines what types of files are displayed.
        /// </summary>
        /// <value>
        /// The filter for the dialog.
        /// </value>
        public string Filter
        {
            get { return this.filter; }
        }

        /// <summary>
        /// Gets or sets whether the Open File Dialog should only allow existing files to be selected.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the Open File Dialog is used to open existing files. Otherwise <see langword="false"/>.
        /// </value>
        public bool CheckFileExists
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the Save File Dialog should display a prompt message if the file already exists.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the Save File Dialog can be used to replace existing files. Otherwise <see langword="false"/>.
        /// </value>
        public bool OverwritePrompt
        {
            get;
            set;
        }
    }
}
