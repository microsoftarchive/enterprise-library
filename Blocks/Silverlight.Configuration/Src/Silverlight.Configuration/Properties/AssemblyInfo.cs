using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Properties;

[assembly: AssemblyTitle("Enterprise Library Configuration for Silverlight")]
[assembly: AssemblyDescription("Enterprise Library Configuration for Silverlight")]
[assembly: AssemblyVersion("5.0.414.0")]

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]

// TODO : Remove when targeting 3.5
[assembly: SecurityRules(SecurityRuleSet.Level1)]

[assembly: HandlesSection(CachingSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
            CachingSettings.SectionName,
            typeof(CachingSettings),
            TitleResourceName = "AddCachingSettings",
            TitleResourceType = typeof(CachingResources),
            CommandModelTypeName = CachingDesignTime.CommandTypeNames.AddCachingBlockCommand)]


