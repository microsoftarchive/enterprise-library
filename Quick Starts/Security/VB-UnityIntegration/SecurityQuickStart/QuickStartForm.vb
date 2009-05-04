'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Security Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================



Public Class QuickStartForm
    Inherits System.Windows.Forms.Form

    Private viewerProcess As Process = Nothing
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic SecurityQS1"

    ' Windows forms for database management
    Private addUserRoleForm As AddUserRoleForm
    Private userRoleForm As UserRoleForm
    Private usersForm As UsersForm

    ' Windows Form used for Authentication scenario:
    Private credentialsForm As CredentialsForm
    Private roleAuthForm As RoleAuthorizationForm

    ' Form to obtain profile information
    Private profileForm As ProfileForm

    Private identity As IIdentity           ' Identity for authenticated users
    Private token As IToken                 ' Token for valid identity
    Private authenticated As Boolean        ' Data provided by user resulted in authenticated identity
    Private cache As ISecurityCacheProvider ' Security cache to handle tokens

    ' Providers
    Private ruleProvider As IAuthorizationProvider
    Private profile As ProfileInformation
    Private membership As MembershipProvider
    Private roles As RoleProvider

    ' Roles to be used:
    Private Const role1 As String = "Employee"
    Private Const role2 As String = "Developer"
    Private Const role3 As String = "Manager"
    Private Const role4 As String = "Executive"
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Private WithEvents label1 As System.Windows.Forms.Label

    Public Shared AppForm As System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents groupBox As System.Windows.Forms.GroupBox
    Friend WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Friend WithEvents quitButton As System.Windows.Forms.Button
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents deleteRoleButton As System.Windows.Forms.Button
    Friend WithEvents addRoleButton As System.Windows.Forms.Button
    Friend WithEvents deleteUserButton As System.Windows.Forms.Button
    Friend WithEvents databaseResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents createUserButton As System.Windows.Forms.Button
    Friend WithEvents tabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents expireButton As System.Windows.Forms.Button
    Friend WithEvents retrieveButton As System.Windows.Forms.Button
    Friend WithEvents obtainTokenButton As System.Windows.Forms.Button
    Friend WithEvents authenticationResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents authenticateUsingCredentialsButton As System.Windows.Forms.Button
    Friend WithEvents tabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents authorizationResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents authorizeUsingIdentityRoleRulesButton As System.Windows.Forms.Button
    Friend WithEvents tabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents writeProfileButton As System.Windows.Forms.Button
    Friend WithEvents readProfileButton As System.Windows.Forms.Button
    Friend WithEvents profileResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents tabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents rolesResultsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents determineRolesButton As System.Windows.Forms.Button
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QuickStartForm))
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.tabPage0 = New System.Windows.Forms.TabPage
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.createUserButton = New System.Windows.Forms.Button
        Me.deleteRoleButton = New System.Windows.Forms.Button
        Me.deleteUserButton = New System.Windows.Forms.Button
        Me.addRoleButton = New System.Windows.Forms.Button
        Me.databaseResultsTextBox = New System.Windows.Forms.TextBox
        Me.tabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.authenticateUsingCredentialsButton = New System.Windows.Forms.Button
        Me.expireButton = New System.Windows.Forms.Button
        Me.retrieveButton = New System.Windows.Forms.Button
        Me.obtainTokenButton = New System.Windows.Forms.Button
        Me.authenticationResultsTextBox = New System.Windows.Forms.TextBox
        Me.tabPage2 = New System.Windows.Forms.TabPage
        Me.authorizationResultsTextBox = New System.Windows.Forms.TextBox
        Me.authorizeUsingIdentityRoleRulesButton = New System.Windows.Forms.Button
        Me.tabPage3 = New System.Windows.Forms.TabPage
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.writeProfileButton = New System.Windows.Forms.Button
        Me.readProfileButton = New System.Windows.Forms.Button
        Me.profileResultsTextBox = New System.Windows.Forms.TextBox
        Me.tabPage4 = New System.Windows.Forms.TabPage
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.determineRolesButton = New System.Windows.Forms.Button
        Me.rolesResultsTextBox = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.tabPage0.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.tabPage3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.tabPage4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        resources.ApplyResources(Me.groupBox, "groupBox")
        Me.groupBox.Name = "groupBox"
        Me.groupBox.TabStop = False
        '
        'viewWalkthroughButton
        '
        resources.ApplyResources(Me.viewWalkthroughButton, "viewWalkthroughButton")
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        '
        'quitButton
        '
        resources.ApplyResources(Me.quitButton, "quitButton")
        Me.quitButton.Name = "quitButton"
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage0)
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Controls.Add(Me.tabPage3)
        Me.tabControl1.Controls.Add(Me.tabPage4)
        resources.ApplyResources(Me.tabControl1, "tabControl1")
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        '
        'tabPage0
        '
        Me.tabPage0.Controls.Add(Me.groupBox2)
        Me.tabPage0.Controls.Add(Me.databaseResultsTextBox)
        resources.ApplyResources(Me.tabPage0, "tabPage0")
        Me.tabPage0.Name = "tabPage0"
        '
        'groupBox2
        '
        Me.groupBox2.BackColor = System.Drawing.SystemColors.Info
        Me.groupBox2.Controls.Add(Me.createUserButton)
        Me.groupBox2.Controls.Add(Me.deleteRoleButton)
        Me.groupBox2.Controls.Add(Me.deleteUserButton)
        Me.groupBox2.Controls.Add(Me.addRoleButton)
        Me.groupBox2.ForeColor = System.Drawing.SystemColors.InfoText
        resources.ApplyResources(Me.groupBox2, "groupBox2")
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.TabStop = False
        '
        'createUserButton
        '
        resources.ApplyResources(Me.createUserButton, "createUserButton")
        Me.createUserButton.Name = "createUserButton"
        '
        'deleteRoleButton
        '
        resources.ApplyResources(Me.deleteRoleButton, "deleteRoleButton")
        Me.deleteRoleButton.Name = "deleteRoleButton"
        '
        'deleteUserButton
        '
        resources.ApplyResources(Me.deleteUserButton, "deleteUserButton")
        Me.deleteUserButton.Name = "deleteUserButton"
        '
        'addRoleButton
        '
        resources.ApplyResources(Me.addRoleButton, "addRoleButton")
        Me.addRoleButton.Name = "addRoleButton"
        '
        'databaseResultsTextBox
        '
        resources.ApplyResources(Me.databaseResultsTextBox, "databaseResultsTextBox")
        Me.databaseResultsTextBox.Name = "databaseResultsTextBox"
        Me.databaseResultsTextBox.ReadOnly = True
        Me.databaseResultsTextBox.TabStop = False
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.GroupBox3)
        Me.tabPage1.Controls.Add(Me.expireButton)
        Me.tabPage1.Controls.Add(Me.retrieveButton)
        Me.tabPage1.Controls.Add(Me.obtainTokenButton)
        Me.tabPage1.Controls.Add(Me.authenticationResultsTextBox)
        resources.ApplyResources(Me.tabPage1, "tabPage1")
        Me.tabPage1.Name = "tabPage1"
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox3.Controls.Add(Me.authenticateUsingCredentialsButton)
        Me.GroupBox3.ForeColor = System.Drawing.SystemColors.InfoText
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'authenticateUsingCredentialsButton
        '
        resources.ApplyResources(Me.authenticateUsingCredentialsButton, "authenticateUsingCredentialsButton")
        Me.authenticateUsingCredentialsButton.Name = "authenticateUsingCredentialsButton"
        '
        'expireButton
        '
        resources.ApplyResources(Me.expireButton, "expireButton")
        Me.expireButton.Name = "expireButton"
        '
        'retrieveButton
        '
        resources.ApplyResources(Me.retrieveButton, "retrieveButton")
        Me.retrieveButton.Name = "retrieveButton"
        '
        'obtainTokenButton
        '
        resources.ApplyResources(Me.obtainTokenButton, "obtainTokenButton")
        Me.obtainTokenButton.Name = "obtainTokenButton"
        '
        'authenticationResultsTextBox
        '
        resources.ApplyResources(Me.authenticationResultsTextBox, "authenticationResultsTextBox")
        Me.authenticationResultsTextBox.Name = "authenticationResultsTextBox"
        Me.authenticationResultsTextBox.ReadOnly = True
        Me.authenticationResultsTextBox.TabStop = False
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.authorizationResultsTextBox)
        Me.tabPage2.Controls.Add(Me.authorizeUsingIdentityRoleRulesButton)
        resources.ApplyResources(Me.tabPage2, "tabPage2")
        Me.tabPage2.Name = "tabPage2"
        '
        'authorizationResultsTextBox
        '
        resources.ApplyResources(Me.authorizationResultsTextBox, "authorizationResultsTextBox")
        Me.authorizationResultsTextBox.Name = "authorizationResultsTextBox"
        Me.authorizationResultsTextBox.ReadOnly = True
        Me.authorizationResultsTextBox.TabStop = False
        '
        'authorizeUsingIdentityRoleRulesButton
        '
        resources.ApplyResources(Me.authorizeUsingIdentityRoleRulesButton, "authorizeUsingIdentityRoleRulesButton")
        Me.authorizeUsingIdentityRoleRulesButton.Name = "authorizeUsingIdentityRoleRulesButton"
        '
        'tabPage3
        '
        Me.tabPage3.Controls.Add(Me.GroupBox4)
        Me.tabPage3.Controls.Add(Me.profileResultsTextBox)
        resources.ApplyResources(Me.tabPage3, "tabPage3")
        Me.tabPage3.Name = "tabPage3"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox4.Controls.Add(Me.writeProfileButton)
        Me.GroupBox4.Controls.Add(Me.readProfileButton)
        Me.GroupBox4.ForeColor = System.Drawing.SystemColors.InfoText
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        '
        'writeProfileButton
        '
        resources.ApplyResources(Me.writeProfileButton, "writeProfileButton")
        Me.writeProfileButton.Name = "writeProfileButton"
        '
        'readProfileButton
        '
        resources.ApplyResources(Me.readProfileButton, "readProfileButton")
        Me.readProfileButton.Name = "readProfileButton"
        '
        'profileResultsTextBox
        '
        resources.ApplyResources(Me.profileResultsTextBox, "profileResultsTextBox")
        Me.profileResultsTextBox.Name = "profileResultsTextBox"
        Me.profileResultsTextBox.ReadOnly = True
        Me.profileResultsTextBox.TabStop = False
        '
        'tabPage4
        '
        Me.tabPage4.Controls.Add(Me.GroupBox5)
        Me.tabPage4.Controls.Add(Me.rolesResultsTextBox)
        resources.ApplyResources(Me.tabPage4, "tabPage4")
        Me.tabPage4.Name = "tabPage4"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox5.Controls.Add(Me.determineRolesButton)
        Me.GroupBox5.ForeColor = System.Drawing.SystemColors.InfoText
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        '
        'determineRolesButton
        '
        resources.ApplyResources(Me.determineRolesButton, "determineRolesButton")
        Me.determineRolesButton.Name = "determineRolesButton"
        '
        'rolesResultsTextBox
        '
        resources.ApplyResources(Me.rolesResultsTextBox, "rolesResultsTextBox")
        Me.rolesResultsTextBox.Name = "rolesResultsTextBox"
        Me.rolesResultsTextBox.ReadOnly = True
        Me.rolesResultsTextBox.TabStop = False
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        resources.ApplyResources(Me.groupBox1, "groupBox1")
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        resources.ApplyResources(Me.label2, "label2")
        Me.label2.Name = "label2"
        '
        'logoPictureBox
        '
        resources.ApplyResources(Me.logoPictureBox, "logoPictureBox")
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.TabStop = False
        '
        'label1
        '
        Me.label1.BackColor = System.Drawing.SystemColors.Info
        Me.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.label1.ForeColor = System.Drawing.SystemColors.InfoText
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'QuickStartForm
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.groupBox.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage0.ResumeLayout(False)
        Me.tabPage0.PerformLayout()
        Me.groupBox2.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage2.PerformLayout()
        Me.tabPage3.ResumeLayout(False)
        Me.tabPage3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.tabPage4.ResumeLayout(False)
        Me.tabPage4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New(ByVal ruleProvider As IAuthorizationProvider, _
                   ByVal cacheProvider As ISecurityCacheProvider, _
                   ByVal membershipProvider As MembershipProvider, _
                   ByVal roleProvider As RoleProvider, _
                   ByVal childForms As QuickStartChildForms)
        Me.New()
        Me.ruleProvider = ruleProvider
        Me.cache = cacheProvider
        Me.membership = membershipProvider
        Me.roles = roleProvider

        addUserRoleForm = childForms.AddUserRoleForm
        userRoleForm = childForms.UserRoleForm
        usersForm = childForms.UsersForm
        credentialsForm = childForms.CredentialsForm
        roleAuthForm = childForms.RoleAuthorizationForm
        profileForm = childForms.ProfileForm

    End Sub

    Public Shared Sub DisplayErrors(ByVal e As Exception)
        Dim errorMsg As String = String.Format(My.Resources.QuickStartErrorMessage, e.Message)

        Dim result As DialogResult = MessageBox.Show(errorMsg, My.Resources.QuickStartErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Application.Exit()
    End Sub
    Private Function GetEmbeddedImage(ByVal resourceName As String) As Image
        Dim resourceStream As Stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName)

        If (resourceStream Is Nothing) Then
            Return Nothing
        End If

        Dim img As Image = Image.FromStream(resourceStream)

        Return img
    End Function

    Private Sub DisplayDatabaseResults(ByVal results As String)
        Me.databaseResultsTextBox.Text &= results & Environment.NewLine

        Me.databaseResultsTextBox.SelectAll()
        Me.databaseResultsTextBox.ScrollToCaret()
    End Sub

    Private Sub DisplayAuthenticationResults(ByVal results As String)
        Me.authenticationResultsTextBox.Text &= results & Environment.NewLine

        Me.authenticationResultsTextBox.SelectAll()
        Me.authenticationResultsTextBox.ScrollToCaret()
    End Sub

    Private Sub DisplayAuthorizationResults(ByVal results As String)
        Me.authorizationResultsTextBox.Text &= results & Environment.NewLine

        Me.authorizationResultsTextBox.SelectAll()
        Me.authorizationResultsTextBox.ScrollToCaret()
    End Sub

    Private Sub DisplayProfileResults(ByVal results As String)
        Me.profileResultsTextBox.Text &= results & Environment.NewLine

        Me.profileResultsTextBox.SelectAll()
        Me.profileResultsTextBox.ScrollToCaret()
    End Sub

    Private Sub DisplayRolesResults(ByVal results As String)
        Me.rolesResultsTextBox.Text &= results & Environment.NewLine

        Me.rolesResultsTextBox.SelectAll()
        Me.rolesResultsTextBox.ScrollToCaret()
    End Sub

    Private Sub QuickStartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            ' Initialize image to embedded logo
            Me.logoPictureBox.Image = GetEmbeddedImage("logo.gif")

        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub createUserButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles createUserButton.Click
        Try

            Me.credentialsForm.Text = My.Resources.CreateUserTitleMessage
            If (Me.credentialsForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Me.Cursor = Cursors.WaitCursor

                membership.CreateUser(credentialsForm.Username, credentialsForm.Password)

                Me.usersForm.ResetDataControls()
                Me.addUserRoleForm.ResetDataControls()
                Me.userRoleForm.ResetDataControls()

                Me.DisplayDatabaseResults(String.Format(My.Resources.UserCreatedMessage, Me.credentialsForm.Username))

            End If

        Catch mcuex As MembershipCreateUserException
            MessageBox.Show(mcuex.Message, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            DisplayErrors(ex)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub deleteUserButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteUserButton.Click
        Try
            If (Me.usersForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Me.Cursor = Cursors.WaitCursor

                membership.DeleteUser(usersForm.UserName)

                Me.usersForm.ResetDataControls()
                Me.addUserRoleForm.ResetDataControls()
                Me.userRoleForm.ResetDataControls()

                Me.DisplayDatabaseResults(String.Format(My.Resources.DeleteUserMessage, Me.usersForm.UserName))

                Me.Cursor = Cursors.Arrow
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub addRoleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addRoleButton.Click
        Try
            If (Me.addUserRoleForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Me.Cursor = Cursors.WaitCursor

                If Not Roles.RoleExists(Me.addUserRoleForm.Role) Then
                    Roles.CreateRole(Me.addUserRoleForm.Role)
                End If

                Try
                    Roles.AddUsersToRole(New String() {Me.addUserRoleForm.UserName}, Me.addUserRoleForm.Role)

                    Me.usersForm.ResetDataControls()
                    Me.addUserRoleForm.ResetDataControls()
                    Me.userRoleForm.ResetDataControls()

                    Me.DisplayDatabaseResults(String.Format(My.Resources.AddUserRoleMessage, _
                        Me.addUserRoleForm.UserName, Me.addUserRoleForm.Role))
                Catch ex As System.Configuration.Provider.ProviderException
                    MessageBox.Show(ex.Message, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    Me.Cursor = Cursors.Arrow
                End Try
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub deleteRoleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteRoleButton.Click
        Try
            If (Me.userRoleForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Me.Cursor = Cursors.WaitCursor

                Roles.RemoveUserFromRole(Me.userRoleForm.UserName, Me.userRoleForm.Role)

                Me.usersForm.ResetDataControls()
                Me.addUserRoleForm.ResetDataControls()
                Me.userRoleForm.ResetDataControls()

                Me.DisplayDatabaseResults(String.Format(My.Resources.DeleteUserRoleMessage, _
                    Me.userRoleForm.UserName, Me.userRoleForm.Role))

                Me.Cursor = Cursors.Arrow
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    ' Scenario: Authenticate a user using name and password credentials
    Private Sub authenticateUsingCredentialsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles authenticateUsingCredentialsButton.Click
        Try
            ' Prompt the user for name and password
            Me.credentialsForm.Text = My.Resources.AuthenticateTitleMessage

            If (Me.credentialsForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Me.Cursor = Cursors.WaitCursor

                Dim username As String = Me.credentialsForm.Username
                Dim password As String = Me.credentialsForm.Password

                Dim passwordBytes As Byte() = ASCIIEncoding.ASCII.GetBytes(password)

                Me.authenticated = membership.ValidateUser(username, password)

                If (Me.authenticated) Then
                    identity = New GenericIdentity(username, membership.Name)
                    Me.DisplayAuthenticationResults(String.Format(My.Resources.ValidCredentialsMessage, username))
                    Me.roleAuthForm.SetUserName(username)
                Else
                    Me.DisplayAuthenticationResults(String.Format(My.Resources.InvalidCredentialsMessage, username))
                    Me.roleAuthForm.SetUserName("")
                End If

                Me.Cursor = Cursors.Arrow
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    ' Scenario: obtain a token for an authenticated user
    Private Sub obtainTokenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles obtainTokenButton.Click
        Try
            ' This sceanrio requires an identity, obtained when the 'Authenticate a user using name and password credentials'
            ' is executed.
            If (Not Me.identity Is Nothing) Then
                ' Cache the identity. The SecurityCache will generate a token which is then
                ' returned to us.
                Me.token = Me.cache.SaveIdentity(Me.identity)

                Me.DisplayAuthenticationResults(String.Format(My.Resources.CreateTokenMessage, Me.token.Value))

            Else
                ' Tell the user that this scenario requires an authenticated user
                Me.DisplayAuthenticationResults(My.Resources.CreateTokenRequiresIdentityMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    ' Scenario: retrieve a cached identity using a token
    Private Sub retrieveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles retrieveButton.Click
        Try
            If (Not Me.token Is Nothing) Then
                ' Retrieves the identity previously saved by using the corresponding token
                Dim savedIdentity As IIdentity = Me.cache.GetIdentity(Me.token)

                If (Not savedIdentity Is Nothing) Then
                    Me.DisplayAuthenticationResults(String.Format(My.Resources.RetrieveIdentityMessage, _
                        savedIdentity.Name, _
                        savedIdentity.AuthenticationType))
                Else
                    ' Token is not valid - it was likely expired.
                    Me.DisplayAuthenticationResults(My.Resources.ExpiredTokenErrorMessage)
                End If
            Else
                ' Scenerio requires that an identity was created by authenticating using credentials
                Me.DisplayAuthenticationResults(My.Resources.RetrieveIdentityErrorMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    ' Scenario: expire a token, removing the cached identity
    Private Sub expireButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles expireButton.Click
        Try
            If (Not Me.token Is Nothing) Then
                ' Expires the identity previously saved by using the corresponding token
                Me.cache.ExpireIdentity(Me.token)

                Me.DisplayAuthenticationResults(My.Resources.ExpireTokenMessage)
            Else
                ' Scenerio requires that an identity was previously cached
                Me.DisplayAuthenticationResults(My.Resources.ExpireTokenErrorMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    ' Scenario: authorize a user using the IdentityRoleRulesProvider
    Private Sub authorizeUsingIdentityRoleRulesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles authorizeUsingIdentityRoleRulesButton.Click
        Try
            If (Me.roleAuthForm.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then

                Cursor = System.Windows.Forms.Cursors.WaitCursor

                Dim identity As String = Me.roleAuthForm.Identity
                Dim rule As String = Me.roleAuthForm.Rule

                ' Get the roles for the current user and create an IPrincipal
                Dim userRoles As String() = Roles.GetRolesForUser(identity)
                Dim principal As IPrincipal = New GenericPrincipal(New GenericIdentity(identity), userRoles)

                If (Not Me.ruleProvider Is Nothing) Then
                    ' Put the list of roles into a string for displaying to the user
                    Dim rolesText As New StringBuilder()
                    For Each role As String In userRoles
                        rolesText.Append(role)
                        rolesText.Append(", ")
                    Next
                    If (rolesText.Length > 0) Then
                        rolesText.Remove(rolesText.Length - 2, 2)
                    End If
                    Me.DisplayAuthorizationResults(String.Format(My.Resources.IdentityRoleMessage, identity, rolesText.ToString()))

                    ' Try to authorize using selected rule
                    Dim authorized As Boolean = Me.ruleProvider.Authorize(principal, rule)
                    If (authorized) Then
                        Me.DisplayAuthorizationResults(String.Format(My.Resources.RuleResultTrueMessage, rule) & Environment.NewLine)
                    Else
                        Me.DisplayAuthorizationResults(String.Format(My.Resources.RuleResultFalseMessage, rule) & Environment.NewLine)
                    End If
                End If
                Cursor = System.Windows.Forms.Cursors.Arrow
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub writeProfileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeProfileButton.Click
        Try
            If (Not Me.identity Is Nothing) Then

                If (Not Me.profile Is Nothing) Then
                    Me.profileForm.Profile.FirstName = Me.profile.FirstName
                    Me.profileForm.Profile.LastName = Me.profile.LastName
                    Me.profileForm.Profile.Theme = Me.profile.Theme
                End If

                If (Me.profileForm.ShowDialog() = Windows.Forms.DialogResult.OK) Then

                    Me.Cursor = Cursors.WaitCursor

                    Me.profile = Me.profileForm.Profile

                    ' Write the profile to the configured ASP.NET Profile provider
                    Dim userProfile As ProfileBase = ProfileBase.Create(Me.identity.Name)
                    userProfile("FirstName") = Me.profile.FirstName
                    userProfile("LastName") = Me.profile.LastName
                    userProfile("Theme") = Me.profile.Theme
                    userProfile.Save()

                    Me.DisplayProfileResults(String.Format(My.Resources.ProfileUpdatedMessage, Me.identity.Name))

                    Me.Cursor = Cursors.Arrow

                End If
            Else
                Me.DisplayProfileResults(My.Resources.NullIdentityMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub readProfileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readProfileButton.Click
        Try
            If (Not Me.identity Is Nothing) Then

                ' Read the profile from the configured ASP.NET Profile provider
                Dim userProfile As ProfileBase = ProfileBase.Create(Me.identity.Name)
                Dim profile As New ProfileInformation()
                profile.FirstName = DirectCast(userProfile("FirstName"), String)
                profile.LastName = DirectCast(userProfile("LastName"), String)
                profile.Theme = DirectCast(userProfile("Theme"), ProfileTheme)

                If (Not profile Is Nothing) Then

                    Dim BackColor As System.Drawing.Color = System.Drawing.Color.White

                    Select Case profile.Theme
                        Case ProfileTheme.Spring
                            BackColor = System.Drawing.Color.YellowGreen
                            Exit Select
                        Case ProfileTheme.Summer
                            BackColor = System.Drawing.Color.Yellow
                            Exit Select
                        Case ProfileTheme.Fall
                            BackColor = System.Drawing.Color.Goldenrod
                            Exit Select
                        Case ProfileTheme.Winter
                            BackColor = System.Drawing.Color.GhostWhite
                            Exit Select
                    End Select

                    Me.groupBox1.BackColor = BackColor

                    Me.DisplayProfileResults(String.Format(My.Resources.UserProfileMessage, Me.identity.Name, profile.ToString()))
                Else
                    Me.DisplayProfileResults(My.Resources.ProfileNotFoundMessage)
                End If
            Else
                Me.DisplayProfileResults(My.Resources.NullIdentityMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub determineRolesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles determineRolesButton.Click
        Try
            If (Not Me.identity Is Nothing) Then

                Me.Cursor = Cursors.WaitCursor

                Me.rolesResultsTextBox.Text = ""
                Me.rolesResultsTextBox.Update()

                Dim userRoles As String() = Roles.GetRolesForUser(Me.identity.Name)
                Dim principal As IPrincipal = New GenericPrincipal(Me.identity, userRoles)

                If (Not principal Is Nothing) Then

                    Me.DisplayRolesResults(String.Format(My.Resources.CheckingRolesMessage, principal.Identity.Name))

                    Me.DisplayRolesResults(String.Format(My.Resources.UserRoleMessage, role1, Convert.ToString(principal.IsInRole(role1))))
                    Me.DisplayRolesResults(String.Format(My.Resources.UserRoleMessage, role2, Convert.ToString(principal.IsInRole(role2))))
                    Me.DisplayRolesResults(String.Format(My.Resources.UserRoleMessage, role3, Convert.ToString(principal.IsInRole(role3))))
                    Me.DisplayRolesResults(String.Format(My.Resources.UserRoleMessage, role4, Convert.ToString(principal.IsInRole(role4))))
                End If

                Me.Cursor = Cursors.Arrow

            Else
                Me.DisplayRolesResults(My.Resources.NullIdentityMessage)
            End If
        Catch ex As Exception
            DisplayErrors(ex)
        End Try
    End Sub

    Private Sub quitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitButton.Click
        Me.Close()
    End Sub
    Private Function GetHelpViewerExecutable() As String
        Dim commonX86 As String = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)")
        If Not String.IsNullOrEmpty(commonX86) Then
            Dim pathX86 As String = Path.Combine(commonX86, "Microsoft Shared\Help 9\dexplore.exe")
            If File.Exists(pathX86) Then
                Return pathX86
            End If
        End If
        Dim common As String = Environment.GetEnvironmentVariable("CommonProgramFiles")
        Return Path.Combine(common, "Microsoft Shared\Help 9\dexplore.exe")
    End Function
    Private Sub viewWalkthroughButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewWalkthroughButton.Click
        ' Process has never been started. Initialize and launch the viewer.
        If (Me.viewerProcess Is Nothing) Then
            ' Initialize the Process information for the help viewer
            Me.viewerProcess = New Process

            Me.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable()
            Me.viewerProcess.StartInfo.Arguments = HelpViewerArguments
            Me.viewerProcess.Start()
        ElseIf (Me.viewerProcess.HasExited) Then
            ' Process previously started, then exited. Start the process again.
            Me.viewerProcess.Start()
        Else
            ' Process was already started - bring it to the foreground
            Dim hWnd As IntPtr = Me.viewerProcess.MainWindowHandle
            If (NativeMethods.IsIconic(hWnd)) Then
                NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE)
            End If
            NativeMethods.SetForegroundWindow(hWnd)
        End If

    End Sub
End Class

