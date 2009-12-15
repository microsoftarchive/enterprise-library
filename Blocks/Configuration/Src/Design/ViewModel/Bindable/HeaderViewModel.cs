using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class HeaderViewModel : ViewModel
    {
        string title;
        IEnumerable<CommandModel> commands;

        public HeaderViewModel(string title, IEnumerable<CommandModel> commands)
            :this(title)
        {
            this.commands = commands;
        }

        public HeaderViewModel(string title)
        {
            this.title = title;
        }


        public string Title
        {
            get { return title; }
        }

        public IEnumerable<CommandModel> Commands
        {
            get { return commands; }
        }


    }
}
