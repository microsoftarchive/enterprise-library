using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel
{
    [TypeConverter(typeof(CommandModelTypeConverter))]
    public class CommandModel : ICommand, INotifyPropertyChanged
    {
        public CommandModel(CommandAttribute commandAttribute)
        {
            Title = commandAttribute.Title;
            HelpText = string.Empty;
            Placement = commandAttribute.CommandPlacement;
            ChildCommands = Enumerable.Empty<CommandModel>();
        }

        protected CommandModel()
        {
        }

        public virtual CommandPlacement Placement
        {
            get;
            private set;
        }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public virtual string Title
        {
            get;
            private set;
        }

        public virtual Image Icon
        {
            get;
            private set;
        }

        public virtual string HelpText
        {
            get; 
            private set;
        }

        public virtual IEnumerable<CommandModel> ChildCommands
        {
            get;
            private set;
        }

        public virtual bool CanExecute(object parameter)
        {
            return false;
        }

        public virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
        }

        protected void DoPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }

    //TODO: why need this? ToString()?
    public class CommandModelTypeConverter : TypeConverter
    {

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof (string));
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {

            CommandModel command = value as CommandModel;
            if(command != null && (typeof(string) == destinationType))
            {
                return command.Title;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
