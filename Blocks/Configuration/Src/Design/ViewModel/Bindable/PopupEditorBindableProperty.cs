using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Input;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class PopupEditorBindableProperty : BindableProperty
    {
        Property property;
        UITypeEditor popupEditor;

        public PopupEditorBindableProperty(Property property)
            :base(property)
        {
            this.property = property;

            this.popupEditor = property.Attributes
                        .OfType<EditorAttribute>()
                        .Where(x => Type.GetType(x.EditorBaseTypeName, false) == typeof(UITypeEditor))
                        .Select(x => Type.GetType(x.EditorTypeName))
                        .Select(x => Activator.CreateInstance(x))
                        .Cast<UITypeEditor>()
                        .First();
        }

        public UITypeEditor PopupEditor
        {
            get 
            {
                return popupEditor;
            }
        }

        //ICommand for ease of Binding
        public ICommand LaunchEditor 
        { 
            get 
            {
                return new DelegateCommand(arg => property.Value = PopupEditor.EditValue(property, property, property.Value));
            } 
        }

        public bool TextReadOnly
        {
            get;
            set;
        }

        public override object GetEditor(Type editorBaseType)
        {
            if (editorBaseType == typeof(UITypeEditor))
            {
                return PopupEditor;
            }

            return null;
        }
    }
}
