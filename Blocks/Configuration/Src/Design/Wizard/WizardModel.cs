//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{

    /// <summary>
    /// The <see cref="WizardModel"/> is the base class for a multi-step wizard.
    /// </summary>
    /// <remarks>
    /// The WizardModel is used in conjunction with <see cref="IWizardStep"/> items
    /// to gather data from the user and apply the changes.
    /// 
    /// Inheritors of WizardModel can add <see cref="IWizardStep"/> (or an
    /// instance the convenience class <see cref="WizardStep"/>) using the <see cref="AddStep"/>
    /// method.  The wizard will start with the <see cref="CurrentStep"/> oriented to
    /// the first <see cref="IWizardStep"/>.
    /// 
    /// <see cref="WizardView"/> provides a view over <see cref="WizardModel"/> and
    /// takes advantage of siting each step in a section of the view as well as 
    /// connecting to the various commands offered by the wizard (<see cref="NextCommand"/>, 
    /// <see cref="PreviousCommand"/>, <see cref="FinishCommand"/>).
    /// </remarks>
    /// <example>
    /// 
    /// public class MyWizard : WizardModel
    /// {
    ///   public MyWizard(IUIServiceWpf uiService) : base(uiService)
    ///   {
    ///      AddStep(new FirstStep());
    ///      AddStep(new SecondStep());
    ///   }
    /// 
    ///   public override Title { get { return "My Wizard"; } }
    /// }
    /// </example>
    public abstract class WizardModel : INotifyPropertyChanged
    {
        private readonly IUIServiceWpf uiService;
        readonly IList<IWizardStep> steps = new List<IWizardStep>();

        /// <summary>
        /// Initializes an instance of <see cref="WizardModel"/>.
        /// </summary>
        /// <param name="uiService">The UI service used to display any dialogs the wizard may need.</param>
        protected WizardModel(IUIServiceWpf uiService)
        {
            if (uiService == null)
            {
                throw new ArgumentNullException("uiService");
            }

            this.uiService = uiService;

            nextCommand = new DelegateCommand((o) => Next(), (o) => NextCanExecute());
            previousCommand = new DelegateCommand((o) => Previous(), (o) => PreviousCanExecute());
            finishCommand = new DelegateCommand((o) => Finish(), (o) => FinishCanExecute());
        }

        /// <summary>
        /// Gets a title for the wizard.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public string Title
        {
            get
            {
                try
                {
                    return GetWizardTitle();
                }
                catch(Exception ex)
                {
                    ConfigurationLogWriter.LogException(ex);
                    return Resources.WizardRetrievingTitleError;
                }
            }
        }

        /// <summary>
        /// Retrieves the wizard title.
        /// </summary>
        /// <returns>The title for the wizard dispalyed by the <see cref="WizardView"/>.</returns>
        /// <remarks>
        /// This is invoked by <see cref="Title"/>.</remarks>
        protected abstract string GetWizardTitle();

        private IWizardStep currentStep;

        /// <summary>
        /// The current <see cref="IWizardStep"/> of the wizard.
        /// </summary>
        public IWizardStep CurrentStep
        {
            get { return currentStep; }
            private set
            {
                if (currentStep != null)
                {
                    currentStep.PropertyChanged -= StepPropertyChanged;
                }

                currentStep = value;

                if (currentStep != null)
                {
                    currentStep.PropertyChanged += StepPropertyChanged;
                }

                OnPropertyChanged("CurrentStep");
                RaiseCommandsExecuteChanged();
            }
        }

        private void StepPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsValid")
            {
                RaiseCommandsExecuteChanged();
            }
        }

        private void RaiseCommandsExecuteChanged()
        {
            nextCommand.RaiseCanExecuteChanged();
            previousCommand.RaiseCanExecuteChanged();
            finishCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Adds a <see cref="IWizardStep"/> to the steps maintained by <see cref="WizardModel"/>.
        /// </summary>
        /// <param name="step">The <see cref="IWizardStep"/> to add.</param>
        protected void AddStep(IWizardStep step)
        {
            steps.Add(step);
            if (steps.Count == 1)
            {
                CurrentStep = step;
            }
        }

        /// <summary>
        /// The wizard steps.
        /// </summary>
        public IEnumerable<IWizardStep> Steps
        {
            get { return steps.ToArray(); }
        }

        /// <summary>
        /// Moves to the next wizard step if the <see cref="CurrentStep"/> <see cref="IsValid"/> is true.
        /// </summary>
        public void Next()
        {
            if (CurrentStep.IsValid)
            {
                var currentIdx = steps.IndexOf(CurrentStep);
                CurrentStep = steps[++currentIdx];
            }
        }

        /// <summary>
        /// Determines if the wizard is valid.
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        protected virtual bool IsValid()
        {
            foreach (var step in steps)
            {
                if (!step.IsValid) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Executes each <see cref="IWizardStep"/> in the wizard.
        /// </summary>
        protected virtual void Execute()
        {
            foreach (var step in steps)
            {
                step.Execute();
            }
        }

        #region NextCommand
        /// <summary>
        /// Determines if the <see cref="NextCommand"/> can be executed.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if it can be excuted, <see langword="false"/> otherwise.</returns>
        protected virtual bool NextCanExecute()
        {
            return CurrentStep != null && CurrentStep.IsValid && !IsLast(CurrentStep);
        }

        private readonly DelegateCommand nextCommand;

        /// <summary>
        /// The <see cref="ICommand"/> for executing the next command.
        /// </summary>
        public ICommand NextCommand
        {
            get { return nextCommand; }
        }
        #endregion

        #region PreviousCommand


        /// <summary>
        /// Evaluates if the <see cref="PreviousCommand"/> can execute.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the <see cref="CurrentStep"/> is not <see langword="null"/>
        /// and there is a previous step.  Otherwise, returns <see langword="false"/></returns>
        protected virtual bool PreviousCanExecute()
        {
            return CurrentStep != null && !IsFirst(CurrentStep);
        }

        /// <summary>
        /// Moves to the previous <see cref="IWizardStep"/>
        /// </summary>
        /// <remarks>
        /// Will only change <see cref="CurrentStep"/> if there is a previous step to move to, otherwise the <see cref="CurrentStep"/> is not changed.</remarks>
        public void Previous()
        {
            var idx = steps.IndexOf(CurrentStep);
            if (idx > 0)
            {
                CurrentStep = steps[idx - 1];
            }
        }

        private readonly DelegateCommand previousCommand;

        /// <summary>
        /// The <see cref="ICommand"/> for moving to the previous <see cref="IWizardStep"/>.
        /// </summary>
        public ICommand PreviousCommand
        {
            get { return previousCommand; }
        }
        #endregion

        #region FinishCommand


        /// <summary>
        /// Evaluates if the <see cref="FinishCommand"/> can be executed.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if it can execute.  Otherwise, returns <see langword="false"/>.</returns>
        protected virtual bool FinishCanExecute()
        {
            return IsLast(CurrentStep) && IsValid();
        }

        /// <summary>
        /// Completes the wizard by executing each individual step.
        /// </summary>
        /// <remarks>
        /// Errors raised by a step will be presented in a dialog if
        /// <see cref="IUIServiceWpf"/> was provided during construction.
        /// </remarks>
        public virtual void Finish()
        {
            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                if (uiService != null)
                {
                    uiService.ShowMessageWpf(ex.Message, Resources.WizardErrorDuringExecutionTitle, MessageBoxButton.OK);
                }
                else
                {
                    throw;
                }
            }

            if (OnCloseAction != null)
            {
                OnCloseAction();
            }
        }

        private readonly DelegateCommand finishCommand;

        /// <summary>
        /// The <see cref="ICommand"/> to finish the wizard.
        /// </summary>
        public ICommand FinishCommand
        {
            get { return finishCommand; }
        }


        /// <summary>
        /// The action invoked by the <see cref="WizardModel"/> to close the wizard.
        /// </summary>
        /// <remarks>
        /// This may be used by views hosting the <see cref="WizardModel"/> to detect
        /// when the view should close.
        /// </remarks>
        public Action OnCloseAction { get; set; }

        #endregion

        private bool IsFirst(IWizardStep step)
        {
            return steps.Any() && steps.First() == step;
        }

        private bool IsLast(IWizardStep step)
        {
            return steps.Any() && steps.Last() == step;
        }


        /// <summary>
        /// Invoked when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
