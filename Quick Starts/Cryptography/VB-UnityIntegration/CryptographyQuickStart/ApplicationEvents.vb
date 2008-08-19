Imports Microsoft.Practices.Unity
Imports Microsoft.Practices.Unity.Configuration

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Dim container As IUnityContainer

        ' Called on application startup - create our container and resolve the
        ' main form for the application through it.
        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            Try

                QuickStartForm.CreateKeys()

            Catch ioe As IOException

                MessageBox.Show(String.Format(My.Resources.UnableToWriteKeyFileErrorMessage, ioe.Message), _
                    My.Resources.KeyFileErrorTitle, _
                    MessageBoxButtons.OK, _
                    MessageBoxIcon.Error)

            End Try


            container = New UnityContainer()
            Dim section As UnityConfigurationSection = DirectCast(ConfigurationManager.GetSection("unity"), UnityConfigurationSection)
            section.Containers.Default.Configure(container)

            Dim mainForm As Form = container.Resolve(Of QuickStartForm)()
            My.Application.ApplicationContext.MainForm = mainForm
        End Sub

    End Class

End Namespace

