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

<Serializable()> _
Public Class ProductCacheRefreshAction
    Implements ICacheItemRefreshAction

    Public Sub Refresh(ByVal removedKey As String, ByVal expiredValue As Object, ByVal removalReason As CacheItemRemovedReason) Implements ICacheItemRefreshAction.Refresh
        ' Item has been removed from cache. Perform desired actions here, based upon
        ' the removal reason (e.g. refresh the cache with the item).
    End Sub
End Class
