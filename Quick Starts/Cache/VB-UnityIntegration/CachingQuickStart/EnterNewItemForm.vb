'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Caching Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


Public Class EnterNewItemForm
    Inherits System.Windows.Forms.Form

    Private exp As ExpirationType
    Private absTime As DateTime

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        InitializeControls()

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
    Friend WithEvents expirationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents label9 As System.Windows.Forms.Label
    Friend WithEvents priceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents nameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents keyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents label7 As System.Windows.Forms.Label
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents priorityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EnterNewItemForm))
        Me.expirationComboBox = New System.Windows.Forms.ComboBox
        Me.label10 = New System.Windows.Forms.Label
        Me.label9 = New System.Windows.Forms.Label
        Me.priceTextBox = New System.Windows.Forms.TextBox
        Me.nameTextBox = New System.Windows.Forms.TextBox
        Me.keyTextBox = New System.Windows.Forms.TextBox
        Me.label7 = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.priorityComboBox = New System.Windows.Forms.ComboBox
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'expirationComboBox
        '
        Me.expirationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.expirationComboBox, "expirationComboBox")
        Me.expirationComboBox.Items.AddRange(New Object() {resources.GetString("expirationComboBox.Items"), resources.GetString("expirationComboBox.Items1"), resources.GetString("expirationComboBox.Items2"), resources.GetString("expirationComboBox.Items3")})
        Me.expirationComboBox.Name = "expirationComboBox"
        '
        'label10
        '
        resources.ApplyResources(Me.label10, "label10")
        Me.label10.Name = "label10"
        '
        'label9
        '
        resources.ApplyResources(Me.label9, "label9")
        Me.label9.Name = "label9"
        '
        'priceTextBox
        '
        resources.ApplyResources(Me.priceTextBox, "priceTextBox")
        Me.priceTextBox.Name = "priceTextBox"
        '
        'nameTextBox
        '
        resources.ApplyResources(Me.nameTextBox, "nameTextBox")
        Me.nameTextBox.Name = "nameTextBox"
        '
        'keyTextBox
        '
        resources.ApplyResources(Me.keyTextBox, "keyTextBox")
        Me.keyTextBox.Name = "keyTextBox"
        '
        'label7
        '
        resources.ApplyResources(Me.label7, "label7")
        Me.label7.Name = "label7"
        '
        'label6
        '
        resources.ApplyResources(Me.label6, "label6")
        Me.label6.Name = "label6"
        '
        'label5
        '
        resources.ApplyResources(Me.label5, "label5")
        Me.label5.Name = "label5"
        '
        'priorityComboBox
        '
        Me.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.priorityComboBox, "priorityComboBox")
        Me.priorityComboBox.Items.AddRange(New Object() {resources.GetString("priorityComboBox.Items"), resources.GetString("priorityComboBox.Items1"), resources.GetString("priorityComboBox.Items2"), resources.GetString("priorityComboBox.Items3")})
        Me.priorityComboBox.Name = "priorityComboBox"
        '
        'label4
        '
        resources.ApplyResources(Me.label4, "label4")
        Me.label4.Name = "label4"
        '
        'label3
        '
        resources.ApplyResources(Me.label3, "label3")
        Me.label3.Name = "label3"
        '
        'label2
        '
        resources.ApplyResources(Me.label2, "label2")
        Me.label2.Name = "label2"
        '
        'label1
        '
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'groupBox2
        '
        resources.ApplyResources(Me.groupBox2, "groupBox2")
        Me.groupBox2.Controls.Add(Me.cancelationButton)
        Me.groupBox2.Controls.Add(Me.okButton)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.TabStop = False
        '
        'cancelationButton
        '
        resources.ApplyResources(Me.cancelationButton, "cancelationButton")
        Me.cancelationButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelationButton.Name = "cancelationButton"
        '
        'okButton
        '
        resources.ApplyResources(Me.okButton, "okButton")
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Name = "okButton"
        '
        'EnterNewItemForm
        '
        Me.AcceptButton = Me.okButton
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.cancelationButton
        Me.Controls.Add(Me.expirationComboBox)
        Me.Controls.Add(Me.label10)
        Me.Controls.Add(Me.label9)
        Me.Controls.Add(Me.priceTextBox)
        Me.Controls.Add(Me.nameTextBox)
        Me.Controls.Add(Me.keyTextBox)
        Me.Controls.Add(Me.label7)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.priorityComboBox)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.groupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EnterNewItemForm"
        Me.ShowInTaskbar = False
        Me.groupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public ReadOnly Property Expiration() As ExpirationType
        Get
            Return Me.exp
        End Get
    End Property

    Public ReadOnly Property Priority() As CacheItemPriority
        Get
            Return CType(Me.priorityComboBox.SelectedIndex + 1, CacheItemPriority)
        End Get
    End Property

    Public ReadOnly Property AbsoluteTime() As DateTime
        Get
            Return Me.absTime
        End Get
    End Property

    Private Sub InitializeControls()
        Me.priorityComboBox.SelectedIndex = 1

        ' The form defaults to NeverExpired as the expiration for new items
        Me.expirationComboBox.SelectedIndex = 0
        Me.exp = ExpirationType.AbsoluteTime
    End Sub

    Protected Overrides Sub OnActivated(ByVal e As EventArgs)
        ' Clear the fields and set focus to the cache key text box
        MyBase.OnActivated(e)
    End Sub

    Public ReadOnly Property ProductID() As String
        Get
            Return Me.keyTextBox.Text
        End Get
    End Property

    Public ReadOnly Property ProductShortName() As String
        Get
            Return Me.nameTextBox.Text
        End Get
    End Property

    Public ReadOnly Property ProductPrice() As Double
        Get
            Return Convert.ToDouble(Me.priceTextBox.Text)
        End Get
    End Property

    Private Function ValidateInput() As Boolean
        Dim result As Boolean = (Me.keyTextBox.Text.CompareTo("") <> 0)
        result = result And (Me.nameTextBox.Text.CompareTo("") <> 0)
        result = result And (Me.priceTextBox.Text.CompareTo("") <> 0)
        Return result
    End Function

    Private Sub EnterNewItemForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Pick a pre-selected absolute expiration time, one day from the current
        ' current date and time.
        Me.absTime = DateTime.Now.Add(TimeSpan.FromMinutes(1))

        ' Set the description in the combobox to display the resulting absolute time
        Me.expirationComboBox.Items(0) = "AbsoluteTime - " & _
            Me.AbsoluteTime.ToShortDateString & " " & _
            Me.AbsoluteTime.ToShortTimeString

        Me.keyTextBox.Focus()
        Me.keyTextBox.Clear()
        Me.nameTextBox.Clear()
        Me.priceTextBox.Clear()
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.ValidateInput()) Then

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show(My.Resources.InvalidInputMessage, My.Resources.QuickStartTitleMessage, MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub cancelationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub expirationComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles expirationComboBox.SelectedIndexChanged
        Select Case Me.expirationComboBox.SelectedIndex
            Case 0
                Me.exp = ExpirationType.AbsoluteTime
            Case 1
                Me.exp = ExpirationType.SlidingTime
            Case 2
                Me.exp = ExpirationType.ExtendedFormat
            Case 3
                Me.exp = ExpirationType.FileDependency
        End Select
    End Sub
End Class
