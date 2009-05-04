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

Public Class DataProvider

    Public Const dataFileName As String = "CachingQuickStartData.xml"

    Public Sub New()

    End Sub

    Public Function GetProductByID(ByVal anID As String) As Product

        Dim product As Product = Nothing
        Dim reader As XmlTextReader = New XmlTextReader(dataFileName)
        reader.MoveToContent()

        Dim doc As XmlDocument = New XmlDocument
        doc.LoadXml(reader.ReadOuterXml())

        Dim productNode As XmlNode = doc.SelectSingleNode("products/product[@id=" & anID & "]")
        If (Not productNode Is Nothing) Then
            product = New Product(productNode.Attributes("id").Value, _
                            productNode.Attributes("name").Value, _
                            Convert.ToDouble(productNode.Attributes("price").Value))
        End If

        reader.Close()

        Return product
    End Function

    Public Function GetProductList() As List(Of Product)
        Dim list As List(Of Product) = New List(Of Product)
        Dim reader As XmlTextReader = New XmlTextReader(dataFileName)
        reader.MoveToContent()
        Dim doc As XmlDocument = New XmlDocument
        doc.LoadXml(reader.ReadOuterXml())
        Dim nodes As XmlNodeList = doc.SelectNodes("products/product")
        For Each node As XmlNode In nodes
            Dim newProduct As Product = New Product(node.Attributes("id").Value, _
                                            node.Attributes("name").Value, _
                                            Convert.ToDouble(node.Attributes("price").Value))
            list.Add(newProduct)
        Next node
        reader.Close()

        Return list
    End Function
End Class
