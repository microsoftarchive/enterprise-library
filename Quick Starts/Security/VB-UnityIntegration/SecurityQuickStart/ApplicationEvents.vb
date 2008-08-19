Imports Microsoft.Practices.Unity
Imports Microsoft.Practices.Unity.Configuration
Imports System.Configuration
Imports System.Web.Security

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private container As IUnityContainer

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                container = New UnityContainer
                Dim section As UnityConfigurationSection = _
                    DirectCast(ConfigurationManager.GetSection("unity"), UnityConfigurationSection)
                section.Containers.Default.Configure(container)

                container.RegisterInstance(Of MembershipProvider)(Membership.Provider)
                container.RegisterInstance(Of RoleProvider)(Roles.Provider)

                Application.ApplicationContext.MainForm = container.Resolve(Of QuickStartForm)()
            Catch ex As Exception
                QuickStartForm.DisplayErrors(ex)
                e.Cancel = True
            End Try
        End Sub
    End Class

End Namespace

