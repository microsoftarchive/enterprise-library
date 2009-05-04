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

Public Class SelectMasterDataItemForm
    Inherits System.Windows.Forms.Form

    Private dataProvider As DataProvider = New DataProvider
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents itemsComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectMasterDataItemForm))
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.itemsComboBox = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.okButton.Name = "okButton"
        '
        'groupBox1
        '
        resources.ApplyResources(Me.groupBox1, "groupBox1")
        Me.groupBox1.Controls.Add(Me.itemsComboBox)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.TabStop = False
        '
        'itemsComboBox
        '
        Me.itemsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.itemsComboBox, "itemsComboBox")
        Me.itemsComboBox.Name = "itemsComboBox"
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
        'SelectMasterDataItemForm
        '
        Me.AcceptButton = Me.okButton
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.cancelationButton
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectMasterDataItemForm"
        Me.ShowInTaskbar = False
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public ReadOnly Property ItemToRead() As String
        Get
            Dim list As List(Of Product) = Me.dataProvider.GetProductList()
            Dim product As Product = list(Me.itemsComboBox.SelectedIndex)
            Return product.ProductID
        End Get
    End Property

    Public Sub PopulateProductListBox()
        If (Me.itemsComboBox.Items.Count = 0) Then
            Dim list As List(Of Product) = Me.dataProvider.GetProductList()
            Dim i As Integer
            For i = 0 To list.Count - 1
                Dim product As Product = list(i)
                itemsComboBox.Items.Add(product.ProductName)
            Next i
        End If
    End Sub

    Protected Overrides Sub OnActivated(ByVal e As EventArgs)
        MyBase.OnActivated(e)
        Me.itemsComboBox.Focus()
    End Sub

    Private Sub SelectMasterDataItemForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PopulateProductListBox()
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.itemsComboBox.SelectedIndex < 0) Then
            MessageBox.Show(My.Resources.InvalidSelectionMessage, My.Resources.QuickStartTitleMessage, _
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cancelationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
