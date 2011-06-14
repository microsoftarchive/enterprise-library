<?xml version="1.0" encoding="utf-8" ?>
<Profile environmentCommandsEnabled="false">
  <Platform>Silverlight</Platform>
  <ApplicationTitleFormat>Enterprise Library Configuration for Silverlight - {0} {1}</ApplicationTitleFormat>
  <TypeFilters>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Caching"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Common"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Data"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Logging.Database"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Security"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography"/>
    <Assembly name="Microsoft.Practices.EnterpriseLibrary.Logging"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.IsolatedStorageTraceListenerData, Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.NotificationTraceListenerData, Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.RemoteServiceTraceListenerData, Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.PriorityFilterData, Microsoft.Practices.EnterpriseLibrary.Logging" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.CategoryFilterData, Microsoft.Practices.EnterpriseLibrary.Logging" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LogEnabledFilterData, Microsoft.Practices.EnterpriseLibrary.Logging" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LogCallHandlerData, Microsoft.Practices.EnterpriseLibrary.Logging" matchKind="Allow"/>
    <Type name="Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.CustomMatchingRuleData, Microsoft.Practices.EnterpriseLibrary.PolicyInjection" />
    <Type name="Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.PerformanceCounterCallHandlerData, Microsoft.Practices.EnterpriseLibrary.PolicyInjection" />
    <Type name="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.CustomCallHandlerData, Microsoft.Practices.EnterpriseLibrary.Common" />
    <Type name="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling" />
    <Type name="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddLoggingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddCachingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddDatabaseBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddInstrumentationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddConfigurationSourcesBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands.AddApplicationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" sectionName="securityConfiguration"/>
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands.AddApplicationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" sectionName="securityCryptographyConfiguration"/>
    <AddBlockCommand name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands.AddApplicationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" sectionName="appSettings"/>
    <Command name="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard.LogExceptionsToDatabaseCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime" />
  </TypeFilters>
</Profile>
