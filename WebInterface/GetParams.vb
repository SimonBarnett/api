Imports System.Web

Public Class GetParams : Inherits Dictionary(Of String, String)

    Public Sub New()
        With HttpContext.Current.Request
            If .RawUrl.IndexOf("?") > 0 Then
                For Each pair As String In .RawUrl.Substring(.RawUrl.IndexOf("?") + 1).Split("&")
                    If pair.IndexOf("=") > 0 Then
                        Add(pair.Split("=")(0), pair.Split("=")(1))
                    Else
                        Add(pair, "")
                    End If
                Next
            End If

        End With
    End Sub

End Class
