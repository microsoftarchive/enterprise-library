using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// Validates a file path for existence.
    ///</summary>
    public class FilePathValidator : Validator
    {
        private readonly bool errorsAsWarnings = true;

        ///<summary>
        /// Initializes an instance of <see cref="FilePathValidator"/>.
        ///</summary>
        public FilePathValidator()
        {
        }


        [Dependency]
        public IApplicationModel ApplicationModel { get; set; }
        

        /// <summary>
        /// Validate the range data for the given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate, this is expected to be a <see cref="Property"/>
        /// </param>
        /// <param name="errors">
        /// The collection to add any errors that occur during the validation.
        /// </param>		
        protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
        {
            var property = instance as Property;
            if (property == null) return;

            var fileName = value;
            
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            try
            {
                if (!Path.IsPathRooted(fileName))
                {
                    string contextPath = ContextPath;                    
                    fileName = Path.Combine(contextPath, fileName);
                }

                if (!File.Exists(fileName))
                {
                    errors.Add(new ValidationError(property,
                                                   string.Format(CultureInfo.CurrentCulture,
                                                                 "Could not locate the file or part of the path for {0}",
                                                                 fileName), errorsAsWarnings));

                }
            }
            catch (ArgumentException e)
            {
                errors.Add(new ValidationError(property, e.Message, errorsAsWarnings));
                return;
            }
        }

        /// <summary>
        /// Returns the path which should be used to validate relative paths.
        /// </summary>
        /// <value>The path from the current configuration file.</value>
        protected virtual string ContextPath
        {
            get
            {
                string configFile = ApplicationModel.ConfigurationFilePath;
                if (string.IsNullOrEmpty(configFile)) return string.Empty;
                return Path.GetDirectoryName(configFile);
            }
        }
    }
}
