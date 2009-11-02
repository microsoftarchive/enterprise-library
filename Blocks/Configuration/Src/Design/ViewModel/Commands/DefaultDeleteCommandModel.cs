using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel
{
    public class DefaultDeleteCommandModel : CommandModel
    {
        ElementViewModel elementViewModel;

        public DefaultDeleteCommandModel(ElementViewModel elementViewModel)
        {
            this.elementViewModel = elementViewModel;
        }

        public override string Title
        {
            get
            {
                return string.Format("Delete {0}", elementViewModel.Name); // todo: move to resource
            }
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            elementViewModel.Delete();
        }
        public override CommandPlacement Placement
        {
            get { return CommandPlacement.ContextDelete; }
        }
    }
}
