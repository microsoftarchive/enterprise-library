The files included in this folder may be used for configuring the "Enterprise Library Semantic Logging Service" host.
The configuration will use the same settings located in "Global.SetupEventTracing()" function for the out-of-process service host.

You can download the out-of-process .exe from http://go.microsoft.com/fwlink/p/?LinkID=290903
You can configure the application to use the Semantic Logging Application Block in out-of-process mode by
changing the UseInprocEventTracing setting in the Web.config file to false.

Usage: 
- Copy SemanticLogging-svc.xml to the location of the host service "SemanticLogging-svc.exe". 
- Start the service as console mode or Windows service. For further info run "SemanticLogging-svc -h".