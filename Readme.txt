MICROSOFT ENTERPRISE LIBRARY 5.0 SILVERLIGHT INTEGRATION PACK
Final Release
5/12/2011


Release announcement: http://blogs.msdn.com/b/agile/archive/2011/05/11/silverlight-integration-pack-for-microsoft-enterprise-library-5-0-released.aspx

What's in this release?

- Caching Application Block with support for:
  -- In-memory cache 
  -- Isolated storage cache 
  -- Expiration and scavenging policies 
  -- Notification of cache purging 

- Validation Application Block with support for:
  -- Multi-level complex validation
  -- Attribute-based specification of validation rules
  -- Configuration-based specification of validation rules
  -- Simple cross-field validation
  -- Self-validation
  -- Cross-tier validation (through WCF RIA Services integration)
  -- Multiple rule-sets
  -- Meta data type for updating entities with external classes in Silverlight
  -- Rich set of built-in validators
 
- Logging Application Block, including:
  -- Notification trace listener
  -- Isolated storage trace listener
  -- Remote service trace listener with support of batch logging
  -- Implementation of a WCF Remote logging service that integrates with the desktop version of the Logging Application Block
  -- Logging filters
  -- Tracing 
  -- Logging settings runtime change API
 
- Exception Handling Application Block, including:
  -- Simple configurable, policy-based mechanism for dealing with exceptions consistently
  -- Wrap handler
  -- Replace handler
  -- Logging handler
 
- Unity Application Block – a dependency injection container
- Dependency injection container independence (Unity ships with the Enterprise Library, but can be replaced with a different container)
- Unity Interception mechanism, with support for:
  -- Virtual method interception
  -- Interface interception
 
- Policy Injection Application Block, including:
  -- Validation handler
  -- Exception Handling handler
  -- Logging handler
 
- Flexible configuration options, including: ◦XAML-based configuration support
  -- Asynchronous configuration loading
  -- Interactive configuration console supporting profiles (desktop vs. Silverlight)
  -- Translation tool for XAML config (needed to convert conventional XML configuration files):
     --- Standalone command-line tool 
     --- Config console wizard
     --- MS Build task
  -- Programmatic configuration support via a fluent interface 

- StockTrader V2 Reference Implementation (RI) (via a separate download - http://go.microsoft.com/fwlink/?LinkId=217987)

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