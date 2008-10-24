//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Management;
using System.Text;
using System.Xml;
using System.Web.Security;
using System.Windows.Forms;
using System.Web.Profile;

using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Configuration.Provider;

namespace SecurityQuickStart
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class QuickStartForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button viewWalkthroughButton;
		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.TabPage tabPage0;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TextBox databaseResultsTextBox;
		private System.Windows.Forms.Button expireButton;
		private System.Windows.Forms.Button retrieveButton;
		private System.Windows.Forms.Button obtainTokenButton;
		private System.Windows.Forms.TextBox authenticationResultsTextBox;
		private System.Windows.Forms.Button authenticateUsingCredentialsButton;
		private System.Windows.Forms.TextBox authorizationResultsTextBox;
		private System.Windows.Forms.Button authorizeUsingIdentityRoleRulesButton;
		private System.Windows.Forms.Button writeProfileButton;
		private System.Windows.Forms.Button readProfileButton;
		private System.Windows.Forms.TextBox profileResultsTextBox;
		private System.Windows.Forms.TextBox rolesResultsTextBox;
		private System.Windows.Forms.Button determineRolesButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Process viewerProcess = null;
		private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic SecurityQS1";

		// Windows forms for database management
		private AddUserRoleForm addUserRoleForm;
		private UserRoleForm userRoleForm;
		private UsersForm usersForm;
		// Windows Form used for Authentication scenario:
		CredentialsForm credentialsForm;
		// Form to obtain profile information
		private ProfileForm profileForm;
		private ProfileInformation profile;
		// Form to obtain authorization information
		private RoleAuthorizationForm roleAuthForm;

		private IIdentity identity;		// Identity for authenticated users
		private IToken token;					// Token for valid identity
		private bool authenticated;		// Data provided by user resulted in authenticated identity

		// Manages the database used by the providers configured to use database
		//SecurityDatabaseManager databaseManager;

		// Providers
		//private IAuthenticationProvider authenticationProvider;
		private IAuthorizationProvider ruleProvider;
		//private IRolesProvider rolesProvider;
		//	private IProfileProvider profileProvider;
		private ISecurityCacheProvider cache;	// Security cache to handle tokens
		private MembershipProvider membership; // Current ASP.NET membership provider

		// Roles to be used:
		private const string role1 = "Employee";
		private const string role2 = "Developer";
		private const string role3 = "Manager";
		private const string role4 = "Executive";
		private GroupBox groupBox2;
		private Button deleteRoleButton;
		private Button addRoleButton;
		private Button deleteUserButton;
		private Button createUserButton;
		private GroupBox groupBox3;
		private GroupBox groupBox4;
		private GroupBox groupBox5;
		private Panel migrationAdvicePanel;
		private Label label1;

		public static System.Windows.Forms.Form AppForm;

		public QuickStartForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public QuickStartForm(
			IAuthorizationProvider ruleProvider,
			ISecurityCacheProvider cacheProvider,
			MembershipProvider membershipProvider,
			QuickstartChildForms childForms)
			: this()
		{
			this.ruleProvider = ruleProvider;
			this.cache = cacheProvider;
			this.membership = membershipProvider;

			this.addUserRoleForm = childForms.AddUserRoleForm;
			this.userRoleForm = childForms.UserRoleForm;
			this.usersForm = childForms.UsersForm;
			this.credentialsForm = childForms.CredentialsForm;
			this.roleAuthForm = childForms.RoleAuthForm;
			this.profileForm = childForms.ProfileForm;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickStartForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage0 = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.deleteRoleButton = new System.Windows.Forms.Button();
			this.addRoleButton = new System.Windows.Forms.Button();
			this.deleteUserButton = new System.Windows.Forms.Button();
			this.createUserButton = new System.Windows.Forms.Button();
			this.databaseResultsTextBox = new System.Windows.Forms.TextBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.expireButton = new System.Windows.Forms.Button();
			this.retrieveButton = new System.Windows.Forms.Button();
			this.obtainTokenButton = new System.Windows.Forms.Button();
			this.authenticationResultsTextBox = new System.Windows.Forms.TextBox();
			this.authenticateUsingCredentialsButton = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.authorizationResultsTextBox = new System.Windows.Forms.TextBox();
			this.authorizeUsingIdentityRoleRulesButton = new System.Windows.Forms.Button();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.writeProfileButton = new System.Windows.Forms.Button();
			this.readProfileButton = new System.Windows.Forms.Button();
			this.profileResultsTextBox = new System.Windows.Forms.TextBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.rolesResultsTextBox = new System.Windows.Forms.TextBox();
			this.determineRolesButton = new System.Windows.Forms.Button();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.viewWalkthroughButton = new System.Windows.Forms.Button();
			this.quitButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.migrationAdvicePanel = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage0.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.groupBox.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.migrationAdvicePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.White;
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.logoPictureBox);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// logoPictureBox
			// 
			resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
			this.logoPictureBox.Name = "logoPictureBox";
			this.logoPictureBox.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage0);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tabPage0
			// 
			this.tabPage0.Controls.Add(this.groupBox2);
			this.tabPage0.Controls.Add(this.databaseResultsTextBox);
			resources.ApplyResources(this.tabPage0, "tabPage0");
			this.tabPage0.Name = "tabPage0";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.Info;
			this.groupBox2.Controls.Add(this.deleteRoleButton);
			this.groupBox2.Controls.Add(this.addRoleButton);
			this.groupBox2.Controls.Add(this.deleteUserButton);
			this.groupBox2.Controls.Add(this.createUserButton);
			this.groupBox2.ForeColor = System.Drawing.SystemColors.InfoText;
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// deleteRoleButton
			// 
			resources.ApplyResources(this.deleteRoleButton, "deleteRoleButton");
			this.deleteRoleButton.Name = "deleteRoleButton";
			this.deleteRoleButton.Click += new System.EventHandler(this.deleteRoleButton_Click);
			// 
			// addRoleButton
			// 
			resources.ApplyResources(this.addRoleButton, "addRoleButton");
			this.addRoleButton.Name = "addRoleButton";
			this.addRoleButton.Click += new System.EventHandler(this.addRoleButton_Click);
			// 
			// deleteUserButton
			// 
			resources.ApplyResources(this.deleteUserButton, "deleteUserButton");
			this.deleteUserButton.Name = "deleteUserButton";
			this.deleteUserButton.Click += new System.EventHandler(this.deleteUserButton_Click);
			// 
			// createUserButton
			// 
			resources.ApplyResources(this.createUserButton, "createUserButton");
			this.createUserButton.Name = "createUserButton";
			this.createUserButton.Click += new System.EventHandler(this.createUserButton_Click);
			// 
			// databaseResultsTextBox
			// 
			resources.ApplyResources(this.databaseResultsTextBox, "databaseResultsTextBox");
			this.databaseResultsTextBox.Name = "databaseResultsTextBox";
			this.databaseResultsTextBox.ReadOnly = true;
			this.databaseResultsTextBox.TabStop = false;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.expireButton);
			this.tabPage1.Controls.Add(this.retrieveButton);
			this.tabPage1.Controls.Add(this.obtainTokenButton);
			this.tabPage1.Controls.Add(this.authenticationResultsTextBox);
			resources.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			// 
			// expireButton
			// 
			resources.ApplyResources(this.expireButton, "expireButton");
			this.expireButton.Name = "expireButton";
			this.expireButton.Click += new System.EventHandler(this.expireButton_Click);
			// 
			// retrieveButton
			// 
			resources.ApplyResources(this.retrieveButton, "retrieveButton");
			this.retrieveButton.Name = "retrieveButton";
			this.retrieveButton.Click += new System.EventHandler(this.retrieveButton_Click);
			// 
			// obtainTokenButton
			// 
			resources.ApplyResources(this.obtainTokenButton, "obtainTokenButton");
			this.obtainTokenButton.Name = "obtainTokenButton";
			this.obtainTokenButton.Click += new System.EventHandler(this.obtainTokenButton_Click);
			// 
			// authenticationResultsTextBox
			// 
			resources.ApplyResources(this.authenticationResultsTextBox, "authenticationResultsTextBox");
			this.authenticationResultsTextBox.Name = "authenticationResultsTextBox";
			this.authenticationResultsTextBox.ReadOnly = true;
			this.authenticationResultsTextBox.TabStop = false;
			// 
			// authenticateUsingCredentialsButton
			// 
			resources.ApplyResources(this.authenticateUsingCredentialsButton, "authenticateUsingCredentialsButton");
			this.authenticateUsingCredentialsButton.Name = "authenticateUsingCredentialsButton";
			this.authenticateUsingCredentialsButton.Click += new System.EventHandler(this.authenticateUsingCredentialsButton_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.authorizationResultsTextBox);
			this.tabPage2.Controls.Add(this.authorizeUsingIdentityRoleRulesButton);
			resources.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			// 
			// authorizationResultsTextBox
			// 
			resources.ApplyResources(this.authorizationResultsTextBox, "authorizationResultsTextBox");
			this.authorizationResultsTextBox.Name = "authorizationResultsTextBox";
			this.authorizationResultsTextBox.ReadOnly = true;
			this.authorizationResultsTextBox.TabStop = false;
			// 
			// authorizeUsingIdentityRoleRulesButton
			// 
			resources.ApplyResources(this.authorizeUsingIdentityRoleRulesButton, "authorizeUsingIdentityRoleRulesButton");
			this.authorizeUsingIdentityRoleRulesButton.Name = "authorizeUsingIdentityRoleRulesButton";
			this.authorizeUsingIdentityRoleRulesButton.Click += new System.EventHandler(this.authorizeUsingIdentityRoleRulesButton_Click);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.groupBox4);
			this.tabPage3.Controls.Add(this.profileResultsTextBox);
			resources.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			// 
			// writeProfileButton
			// 
			resources.ApplyResources(this.writeProfileButton, "writeProfileButton");
			this.writeProfileButton.Name = "writeProfileButton";
			this.writeProfileButton.Click += new System.EventHandler(this.writeProfileButton_Click);
			// 
			// readProfileButton
			// 
			resources.ApplyResources(this.readProfileButton, "readProfileButton");
			this.readProfileButton.Name = "readProfileButton";
			this.readProfileButton.Click += new System.EventHandler(this.readProfileButton_Click);
			// 
			// profileResultsTextBox
			// 
			resources.ApplyResources(this.profileResultsTextBox, "profileResultsTextBox");
			this.profileResultsTextBox.Name = "profileResultsTextBox";
			this.profileResultsTextBox.ReadOnly = true;
			this.profileResultsTextBox.TabStop = false;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.groupBox5);
			this.tabPage4.Controls.Add(this.rolesResultsTextBox);
			resources.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			// 
			// rolesResultsTextBox
			// 
			resources.ApplyResources(this.rolesResultsTextBox, "rolesResultsTextBox");
			this.rolesResultsTextBox.Name = "rolesResultsTextBox";
			this.rolesResultsTextBox.ReadOnly = true;
			this.rolesResultsTextBox.TabStop = false;
			// 
			// determineRolesButton
			// 
			resources.ApplyResources(this.determineRolesButton, "determineRolesButton");
			this.determineRolesButton.Name = "determineRolesButton";
			this.determineRolesButton.Click += new System.EventHandler(this.determineRolesButton_Click);
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.viewWalkthroughButton);
			this.groupBox.Controls.Add(this.quitButton);
			resources.ApplyResources(this.groupBox, "groupBox");
			this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox.Name = "groupBox";
			this.groupBox.TabStop = false;
			// 
			// viewWalkthroughButton
			// 
			resources.ApplyResources(this.viewWalkthroughButton, "viewWalkthroughButton");
			this.viewWalkthroughButton.Name = "viewWalkthroughButton";
			this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
			// 
			// quitButton
			// 
			this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.quitButton, "quitButton");
			this.quitButton.Name = "quitButton";
			this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.Info;
			this.groupBox3.Controls.Add(this.authenticateUsingCredentialsButton);
			this.groupBox3.ForeColor = System.Drawing.SystemColors.InfoText;
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.SystemColors.Info;
			this.groupBox4.Controls.Add(this.readProfileButton);
			this.groupBox4.Controls.Add(this.writeProfileButton);
			this.groupBox4.ForeColor = System.Drawing.SystemColors.InfoText;
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			// 
			// groupBox5
			// 
			this.groupBox5.BackColor = System.Drawing.SystemColors.Info;
			this.groupBox5.Controls.Add(this.determineRolesButton);
			this.groupBox5.ForeColor = System.Drawing.SystemColors.InfoText;
			resources.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			// 
			// migrationAdvicePanel
			// 
			this.migrationAdvicePanel.BackColor = System.Drawing.SystemColors.Info;
			this.migrationAdvicePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.migrationAdvicePanel.Controls.Add(this.label1);
			this.migrationAdvicePanel.ForeColor = System.Drawing.SystemColors.InfoText;
			resources.ApplyResources(this.migrationAdvicePanel, "migrationAdvicePanel");
			this.migrationAdvicePanel.Name = "migrationAdvicePanel";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// QuickStartForm
			// 
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.quitButton;
			this.Controls.Add(this.migrationAdvicePanel);
			this.Controls.Add(this.groupBox);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "QuickStartForm";
			this.Load += new System.EventHandler(this.QuickStartForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage0.ResumeLayout(false);
			this.tabPage0.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.groupBox.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.migrationAdvicePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                section.Containers.Default.Configure(container);

                container.RegisterInstance<MembershipProvider>(Membership.Provider);
                container.RegisterInstance<RoleProvider>(Roles.Provider);

                AppForm = container.Resolve<QuickStartForm>();

                // Setup unhandled exception handlers
                AppDomain.CurrentDomain.UnhandledException += // CLR
                  new UnhandledExceptionEventHandler(OnUnhandledException);

                // Unhandled Forms exceptions will be delivered to our ThreadException handler
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(AppThreadException);

                Application.Run(AppForm);
            }
            catch (Exception e)
            {
                HandleUnhandledException(e);
            }
        }

		// CLR unhandled exception
		private static void OnUnhandledException(Object sender, UnhandledExceptionEventArgs e)
		{
			HandleUnhandledException(e.ExceptionObject);
		}

		/// <summary>
		/// Displays dialog with information about exceptions that occur in the application. 
		/// </summary>
		private static void AppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
		{
			HandleUnhandledException(e.Exception);
		}

		static void HandleUnhandledException(Object o)
		{
			string error;
			Exception ex = o as Exception;
			if (ex != null)
			{ // Report System.Exception info
				error = ex.Message;
			}
			else
			{ // Report exception Object info
				error = o.ToString();
			}
			string errorMsg = string.Format(Properties.Resources.QuickStartErrorMessage, error);

			DialogResult result = MessageBox.Show(errorMsg, Properties.Resources.QuickStartErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);

            Application.Exit(); // Shutting down
        }

		private void QuickStartForm_Load(object sender, System.EventArgs e)
		{
			// Initialize image to embedded logo
			this.logoPictureBox.Image = GetEmbeddedImage("SecurityQuickStart.logo.gif");

			// Initializes the Enterprise Library authorization and security caching providers
			// The ASP.NET Membership and Profile providers do not need to be initialized in this way
			//this.ruleProvider = AuthorizationFactory.GetAuthorizationProvider("RuleProvider");
			//this.cache = SecurityCacheFactory.GetSecurityCacheProvider("Caching Store Provider");

			// Initialize forms displayed for user interaction
			//this.addUserRoleForm = new AddUserRoleForm();
			//this.userRoleForm = new UserRoleForm();
			//this.usersForm = new UsersForm();
			//this.credentialsForm = new CredentialsForm();
			//this.roleAuthForm = new RoleAuthorizationForm();
			//this.profileForm = new ProfileForm();
		}

		/// <summary>
		/// Loads the specified image from the resource file
		/// </summary>
		private System.Drawing.Image GetEmbeddedImage(string resourceName)
		{
			Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);

			if (resourceStream == null)
			{
				return null;
			}

			System.Drawing.Image img = System.Drawing.Image.FromStream(resourceStream);

			return img;
		}


		private void DisplayDatabaseResults(string results)
		{
			this.databaseResultsTextBox.Text += results + Environment.NewLine;

			this.databaseResultsTextBox.SelectAll();
			this.databaseResultsTextBox.ScrollToCaret();
		}

		private void DisplayAuthenticationResults(string results)
		{
			this.authenticationResultsTextBox.Text += results + Environment.NewLine;

			this.authenticationResultsTextBox.SelectAll();
			this.authenticationResultsTextBox.ScrollToCaret();
		}

		private void DisplayAuthorizationResults(string results)
		{
			this.authorizationResultsTextBox.Text += results + Environment.NewLine;

			this.authorizationResultsTextBox.SelectAll();
			this.authorizationResultsTextBox.ScrollToCaret();
		}

		private void DisplayProfileResults(string results)
		{
			this.profileResultsTextBox.Text += results + Environment.NewLine;

			this.profileResultsTextBox.SelectAll();
			this.profileResultsTextBox.ScrollToCaret();
		}

		private void DisplayRolesResults(string results)
		{
			this.rolesResultsTextBox.Text += results + Environment.NewLine;

			this.rolesResultsTextBox.SelectAll();
			this.rolesResultsTextBox.ScrollToCaret();
		}

		/// <summary>
		/// Scenario: Authenticate a user using name and password credentials
		/// </summary>
		private void authenticateUsingCredentialsButton_Click(object sender, System.EventArgs e)
		{
			// Prompt the user for name and password
			this.credentialsForm.Text = Properties.Resources.AuthenticateTitleMessage;
			DialogResult result = this.credentialsForm.ShowDialog();

			if (result == DialogResult.OK)
			{
				using (new WaitCursorHelper(this))
				{
					string username = this.credentialsForm.Username;
					string password = this.credentialsForm.Password;

					this.authenticated = membership.ValidateUser(username, password);

					if (this.authenticated)
					{
						identity = new GenericIdentity(username, membership.Name);
						this.DisplayAuthenticationResults(string.Format(Properties.Resources.ValidCredentialsMessage, username));
						this.roleAuthForm.SetUserName(username);
					}
					else
					{
						this.DisplayAuthenticationResults(string.Format(Properties.Resources.InvalidCredentialsMessage, username));
						this.roleAuthForm.SetUserName("");
					}
				}
			}
		}

		/// <summary>
		/// Scenario: obtain a token for an authenticated user
		/// </summary>
		private void obtainTokenButton_Click(object sender, System.EventArgs e)
		{
			// This sceanrio requires an identity, obtained when the 'Authenticate a user using name and password credentials'
			// is executed.
			if (this.identity != null)
			{
				// Cache the identity. The SecurityCache will generate a token which is then
				// returned to us.
				this.token = this.cache.SaveIdentity(this.identity);

				this.DisplayAuthenticationResults(string.Format(Properties.Resources.CreateTokenMessage, this.token.Value));
			}
			else
			{
				// Tell the user that this scenario requires an authenticated user
				this.DisplayAuthenticationResults(Properties.Resources.CreateTokenRequiresIdentityMessage);
			}
		}

		/// <summary>
		/// Scenario: retrieve a cached identity using a token
		/// </summary>
		private void retrieveButton_Click(object sender, System.EventArgs e)
		{
			if (this.token != null)
			{
				// Retrieves the identity previously saved by using the corresponding token
				IIdentity savedIdentity = this.cache.GetIdentity(this.token);

				if (savedIdentity != null)
				{
					this.DisplayAuthenticationResults(string.Format(Properties.Resources.RetrieveIdentityMessage,
						savedIdentity.Name,
						savedIdentity.AuthenticationType));
				}
				else
				{
					// Token is not valid - it was likely expired.
					this.DisplayAuthenticationResults(Properties.Resources.ExpiredTokenErrorMessage);
				}
			}
			else
			{
				// Scenerio requires that an identity was created by authenticating using credentials
				this.DisplayAuthenticationResults(Properties.Resources.RetrieveIdentityErrorMessage);
			}
		}

		/// <summary>
		/// Scenario: expire a token, removing the cached identity
		/// </summary>
		private void expireButton_Click(object sender, System.EventArgs e)
		{
			if (this.token != null)
			{
				// Expires the identity previously saved by using the corresponding token
				this.cache.ExpireIdentity(this.token);

				this.DisplayAuthenticationResults(Properties.Resources.ExpireTokenMessage);
			}
			else
			{
				// Scenerio requires that an identity was previously cached
				this.DisplayAuthenticationResults(Properties.Resources.ExpireTokenErrorMessage);
			}
		}

		/// <summary>
		/// Scenario: authorize a user using the IdentityRoleRulesProvider
		/// </summary>
		private void authorizeUsingIdentityRoleRulesButton_Click(object sender, System.EventArgs e)
		{
			DialogResult result = this.roleAuthForm.ShowDialog();
			if (result == DialogResult.OK)
			{
				Cursor = System.Windows.Forms.Cursors.WaitCursor;

				string identity = this.roleAuthForm.Identity;
				string rule = this.roleAuthForm.Rule;

				// Get the roles for the current user and create an IPrincipal
				string[] roles = Roles.GetRolesForUser(identity);
				IPrincipal principal = new GenericPrincipal(new GenericIdentity(identity), roles);

				if (this.ruleProvider != null)
				{
					// Put the list of roles into a string for displaying to the user
					StringBuilder rolesText = new StringBuilder();
					foreach (string role in roles)
					{
						rolesText.Append(role);
						rolesText.Append(", ");
					}
					if (rolesText.Length > 0)
					{
						rolesText.Remove(rolesText.Length - 2, 2);
					}
					this.DisplayAuthorizationResults(string.Format(Properties.Resources.IdentityRoleMessage, identity, rolesText.ToString()));

					// Try to authorize using selected rule
					bool authorized = this.ruleProvider.Authorize(principal, rule);
					if (authorized)
					{
						this.DisplayAuthorizationResults(string.Format(Properties.Resources.RuleResultTrueMessage, rule) + Environment.NewLine);
					}
					else
					{
						this.DisplayAuthorizationResults(string.Format(Properties.Resources.RuleResultFalseMessage, rule) + Environment.NewLine);
					}


				}
				Cursor = System.Windows.Forms.Cursors.Arrow;
			}
		}

		private void writeProfileButton_Click(object sender, System.EventArgs e)
		{
			if (this.identity != null)
			{
				if (this.profile != null)
				{
					this.profileForm.Profile.FirstName = this.profile.FirstName;
					this.profileForm.Profile.LastName = this.profile.LastName;
					this.profileForm.Profile.Theme = this.profile.Theme;
				}

				DialogResult result = this.profileForm.ShowDialog();
				if (result == DialogResult.OK)
				{
					using (new WaitCursorHelper(this))
					{
						this.profile = this.profileForm.Profile;

						// Write the profile to the configured ASP.NET Profile provider
						ProfileBase userProfile = ProfileBase.Create(this.identity.Name);
						userProfile["FirstName"] = this.profile.FirstName;
						userProfile["LastName"] = this.profile.LastName;
						userProfile["Theme"] = this.profile.Theme;
						userProfile.Save();

						this.DisplayProfileResults(string.Format(Properties.Resources.ProfileUpdatedMessage, this.identity.Name));
					}
				}
			}
			else
			{
				this.DisplayProfileResults(Properties.Resources.NullIdentityMessage);
			}
		}

		private void readProfileButton_Click(object sender, System.EventArgs e)
		{
			if (this.identity != null)
			{
				// Read the profile from the configured ASP.NET Profile provider
				ProfileBase userProfile = ProfileBase.Create(this.identity.Name);
				profile = new ProfileInformation();
				profile.FirstName = (string)userProfile["FirstName"];
				profile.LastName = (string)userProfile["LastName"];
				profile.Theme = (ProfileTheme)userProfile["Theme"];

				if (profile != null)
				{
					System.Drawing.Color backColor = System.Drawing.Color.White;

					switch (profile.Theme)
					{
						case ProfileTheme.Spring:
							backColor = System.Drawing.Color.YellowGreen;
							break;
						case ProfileTheme.Summer:
							backColor = System.Drawing.Color.Yellow;
							break;
						case ProfileTheme.Fall:
							backColor = System.Drawing.Color.Goldenrod;
							break;
						case ProfileTheme.Winter:
							backColor = System.Drawing.Color.GhostWhite;
							break;
					}

					this.groupBox1.BackColor = backColor;

					this.DisplayProfileResults(string.Format(Properties.Resources.UserProfileMessage, this.identity.Name, profile.ToString()));
				}
				else
				{
					this.DisplayProfileResults(Properties.Resources.ProfileNotFoundMessage);
				}
			}
			else
			{
				this.DisplayProfileResults(Properties.Resources.NullIdentityMessage);
			}
		}

		private void determineRolesButton_Click(object sender, System.EventArgs e)
		{
			if (this.identity != null)
			{
				using (new WaitCursorHelper(this))
				{
					this.rolesResultsTextBox.Text = "";
					this.rolesResultsTextBox.Update();

					string[] roles = Roles.GetRolesForUser(this.identity.Name);
					IPrincipal principal = new GenericPrincipal(this.identity, roles);

					if (principal != null)
					{
						this.DisplayRolesResults(string.Format(Properties.Resources.CheckingRolesMessage, principal.Identity.Name));

						this.DisplayRolesResults(string.Format(Properties.Resources.UserRoleMessage, role1, Convert.ToString(principal.IsInRole(role1))));
						this.DisplayRolesResults(string.Format(Properties.Resources.UserRoleMessage, role2, Convert.ToString(principal.IsInRole(role2))));
						this.DisplayRolesResults(string.Format(Properties.Resources.UserRoleMessage, role3, Convert.ToString(principal.IsInRole(role3))));
						this.DisplayRolesResults(string.Format(Properties.Resources.UserRoleMessage, role4, Convert.ToString(principal.IsInRole(role4))));
					}

				}
			}
			else
			{
				this.DisplayRolesResults(Properties.Resources.NullIdentityMessage);
			}
		}

		private void quitButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Creates a new user in the QuickStarts database.
		/// </summary>
		private void createUserButton_Click(object sender, System.EventArgs e)
		{
			this.credentialsForm.Text = Properties.Resources.CreateUserTitleMessage;
			DialogResult result = this.credentialsForm.ShowDialog();
			if (result == DialogResult.OK)
			{
				using (new WaitCursorHelper(this))
				{
					try
					{

						membership.CreateUser(this.credentialsForm.Username, this.credentialsForm.Password);

						this.usersForm.ResetDataControls();
						this.addUserRoleForm.ResetDataControls();
						this.userRoleForm.ResetDataControls();

						this.DisplayDatabaseResults(string.Format(Properties.Resources.UserCreatedMessage, this.credentialsForm.Username));

					}
					catch (MembershipCreateUserException ex)
					{
						MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void deleteUserButton_Click(object sender, System.EventArgs e)
		{
			if (this.usersForm.ShowDialog() == DialogResult.OK)
			{
				using (new WaitCursorHelper(this))
				{
					membership.DeleteUser(this.usersForm.UserName);

					this.usersForm.ResetDataControls();
					this.addUserRoleForm.ResetDataControls();
					this.userRoleForm.ResetDataControls();

					this.DisplayDatabaseResults(string.Format(Properties.Resources.DeleteUserMessage, this.usersForm.UserName));
				}
			}
		}

		private void addRoleButton_Click(object sender, System.EventArgs e)
		{
			if (this.addUserRoleForm.ShowDialog() == DialogResult.OK)
			{
				using (new WaitCursorHelper(this))
				{
					if (!Roles.RoleExists(this.addUserRoleForm.Role))
					{
						Roles.CreateRole(this.addUserRoleForm.Role);
					}
					try
					{
						Roles.AddUsersToRole(new string[] { this.addUserRoleForm.UserName }, this.addUserRoleForm.Role);

						this.usersForm.ResetDataControls();
						this.addUserRoleForm.ResetDataControls();
						this.userRoleForm.ResetDataControls();

						this.DisplayDatabaseResults(string.Format(Properties.Resources.AddUserRoleMessage,
							this.addUserRoleForm.UserName, this.addUserRoleForm.Role));
					}
					catch (ProviderException ex)
					{
						MessageBox.Show(ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void deleteRoleButton_Click(object sender, System.EventArgs e)
		{
			if (this.userRoleForm.ShowDialog() == DialogResult.OK)
			{
				using (new WaitCursorHelper(this))
				{
					Roles.RemoveUserFromRole(this.userRoleForm.UserName, this.userRoleForm.Role);

					this.usersForm.ResetDataControls();
					this.addUserRoleForm.ResetDataControls();
					this.userRoleForm.ResetDataControls();

					this.DisplayDatabaseResults(string.Format(Properties.Resources.DeleteUserRoleMessage,
						this.userRoleForm.UserName, this.userRoleForm.Role));
				}
			}
		}

		/// <summary>
		/// Returns the path and executable name for the help viewer.
		/// </summary>
		private string GetHelpViewerExecutable()
		{
            string commonX86 = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
            if (!string.IsNullOrEmpty(commonX86))
            {
                string pathX86 = Path.Combine(commonX86, @"Microsoft Shared\Help 9\dexplore.exe");
                if (File.Exists(pathX86))
                {
                    return pathX86;
                }
            }
            string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
            return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
        }

		private void viewWalkthroughButton_Click(object sender, System.EventArgs e)
		{
			// Process has never been started. Initialize and launch the viewer.
			if (this.viewerProcess == null)
			{
				// Initialize the Process information for the help viewer
				this.viewerProcess = new Process();

				this.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable();
				this.viewerProcess.StartInfo.Arguments = HelpViewerArguments;
				this.viewerProcess.Start();
			}
			else if (this.viewerProcess.HasExited)
			{
				// Process previously started, then exited. Start the process again.
				this.viewerProcess.Start();
			}
			else
			{
				// Process was already started - bring it to the foreground
				IntPtr hWnd = this.viewerProcess.MainWindowHandle;
				if (NativeMethods.IsIconic(hWnd))
				{
					NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
				}
				NativeMethods.SetForegroundWindow(hWnd);
			}
		}

	}
}
