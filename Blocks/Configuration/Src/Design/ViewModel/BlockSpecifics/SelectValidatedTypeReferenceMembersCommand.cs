using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design;
using System.Reflection;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class SelectValidatedTypeReferenceMembersCommand  : CommandModel
    {
        TypeMemberChooser typeMemberChooser;

        ElementViewModel ruleSetElement;
        ElementViewModel validatedTypeReferenceElement;
        ElementCollectionViewModel fieldCollectionElement;
        ElementCollectionViewModel propertyCollectionElement;
        ElementCollectionViewModel methodsCollectionElement;

        public SelectValidatedTypeReferenceMembersCommand(TypeMemberChooser typeMemberChooser, ElementViewModel context, CommandAttribute attribute)
            : base(attribute)
        {
            if (context.ConfigurationType != typeof(ValidationRulesetData)) throw new InvalidOperationException();

            this.ruleSetElement = context;
            this.validatedTypeReferenceElement = context.AncesterElements().Where(x=>x.ConfigurationType == typeof(ValidatedTypeReference)).First();
            this.fieldCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedFieldReferenceCollection)).First();
            this.propertyCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedPropertyReferenceCollection)).First();
            this.methodsCollectionElement = (ElementCollectionViewModel)context.ChildElements.Where(x => x.ConfigurationType == typeof(ValidatedMethodReferenceCollection)).First();

            this.typeMemberChooser = typeMemberChooser;
        }


        private Type GetValidationType()
        {
            string typeString = string.Format("{0}, {1}",
                    validatedTypeReferenceElement.Property("Name").Value,
                    validatedTypeReferenceElement.Property("AssemblyName").Value);

            try
            {
                return Type.GetType(typeString);
            }
            catch
            {
                return null;
            }
        }

        public override void Execute(object parameter)
        {
            Type typeReferenceType = GetValidationType();

            foreach (var member in typeMemberChooser.ChooseMembers(typeReferenceType))
            {
                if (member is FieldInfo)
                {
                    var fieldElement = fieldCollectionElement.AddNewCollectionElement(typeof(ValidatedFieldReference));
                    fieldElement.Property("Name").Value = member.Name;
                }
                else if (member is PropertyInfo)
                {
                    var propertyElement = propertyCollectionElement.AddNewCollectionElement(typeof(ValidatedPropertyReference));
                    propertyElement.Property("Name").Value = member.Name;
                }
                else if (member is MethodInfo)
                {
                    var methodElement = methodsCollectionElement.AddNewCollectionElement(typeof(ValidatedMethodReference));
                    methodElement.Property("Name").Value = member.Name;
                }
            }
        }

        public override bool CanExecute(object parameter)
        {
            return GetValidationType() != null;
        }

        public override string Title
        {
            get
            {
                return "Select Members ...";
            }
        }

        public override string HelpText
        {
            get
            {
                return "Allows you to quickly select a number of validation targets";
            }
        }
    }
}
