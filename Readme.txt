MICROSOFT ENTERPRISE LIBRARY 5.0 SILVERLIGHT INTEGRATION PACK
Community Technology Preview Drop 4
4/11/2011

What's in this preview?
- Validation Application Block for Silverlight
- Caching Application Block for Silverlight
  -- in-memory cache
  -- isolated storage cache
- Logging Application Block for Silverlight
  -- Notification TraceListener
  -- IsolatedStorage TraceListener
  -- Read API (to be used with IsolatedStorage TraceListener)
  -- RemoteService TraceListener
  -- Logging Service (default implementation to be used with RemoteService TraceListener)
- Policy Injection handlers:
  -- Validation handler
  -- Logging handler
  -- Exception handling handler
- Interception (assemblies only; for source, see http://unity.codeplex.com/SourceControl/changeset/changes/63122)

Pre-requisites: 
1) Silverlight SDK (http://www.silverlight.net/getstarted)
2) Unity for Silverlight 2.0 (included in the drop)
3) Interception extension for Unity for Silverlight 2.0 (included in the drop)
  
Additionally, in order to run unit tests, the following are also required:
4) Moq (v4.0 or later)
  -- Copy Silverlight4-specific assemblies to Lib\Silverlight\ThirdParty\Moq
  -- Copy .NET4.0 assemblies to Lib\Desktop\ThirdParty\Moq
5) Silverlight 4 Toolkit (http://silverlight.codeplex.com/)
  -- Copy Microsoft.Silverlight.Testing.dll and Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight to Lib\Silverlight\UnitTestFramework

More information & demos at the Enterprise Library Silverlight Integration Pack home page:
http://entlib.codeplex.com/wikipage?title=EntLib5Silverlight