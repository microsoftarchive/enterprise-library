MICROSOFT ENTERPRISE LIBRARY 5.0 SILVERLIGHT INTEGRATION PACK
Community Technology Preview Drop 3
3/28/2011

What's in this preview?
- Validation Application Block for Silverlight
- Caching Application Block for Silverlight
- Logging Application Block for Silverlight (includes Notification TraceListener and IsolatedStorage TraceListener)
- Policy Injection handlers:
  -- Validation handler
  -- Logging handler
- Interception (assemblies only; for source, see http://unity.codeplex.com/SourceControl/changeset/changes/63122)

Pre-requisites: 
1) Silverlight SDK (http://www.silverlight.net/getstarted)
2) Blend SDK (http://www.microsoft.com/downloads/details.aspx?FamilyID=D197F51A-DE07-4EDF-9CBA-1F1B4A22110D)
3) Unity for Silverlight 2.0 (included in the drop)
4) Interception extension for Unity for Silverlight 2.0 (included in the drop)
  
Additionally, in order to run unit tests, the following are also required:
5) Moq (v4.0 or later)
  -- Copy Silverlight4-specific assemblies to Lib\Silverlight\ThirdParty\Moq
  -- Copy .NET4.0 assemblies to Lib\Desktop\ThirdParty\Moq
6) Silverlight 4 Toolkit (http://silverlight.codeplex.com/)
  -- Copy Microsoft.Silverlight.Testing.dll and Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight to Lib\Silverlight\UnitTestFramework

More information & demos at the Enterprise Library Silverlight Integration Pack home page:
http://entlib.codeplex.com/wikipage?title=EntLib5Silverlight