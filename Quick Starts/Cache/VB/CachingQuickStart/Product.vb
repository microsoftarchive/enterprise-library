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

<Serializable(), _
XmlRoot("product")> _
Public Class Product
    Private prodId As String
    Private prodPrice As Double
    Private prodName As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal id As String, ByVal name As String, ByVal price As Double)
        Me.prodId = id
        Me.prodName = name
        Me.prodPrice = price
    End Sub

    <XmlAttributeAttribute("name")> _
    Public Property ProductName() As String
        Get
            Return Me.prodName
        End Get
        Set(ByVal Value As String)
            Me.prodName = Value
        End Set
    End Property

    <XmlAttributeAttribute("id")> _
    Public Property ProductID() As String
        Get
            Return Me.prodId
        End Get
        Set(ByVal Value As String)
            Me.prodId = Value
        End Set
    End Property

    <XmlAttributeAttribute("price")> _
    Public Property ProductPrice() As Double
        Get
            Return Me.prodPrice
        End Get
        Set(ByVal Value As Double)
            Me.prodPrice = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim res As String = "Product: ID=" & Me.prodId & _
            ", Name=" & Me.prodName & _
            ", Price=" & Convert.ToString(Me.prodPrice)
        Return res
    End Function

End Class

