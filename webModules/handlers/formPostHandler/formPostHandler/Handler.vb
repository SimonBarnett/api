Imports System.ComponentModel.Composition
Imports System.Xml
Imports Newtonsoft.Json
Imports System.Web
Imports System.IO
Imports System.Reflection
Imports PriPROC6.Interface.Web
Imports PriPROC6.Interface.oData

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "formPostHandler")>
<ExportMetadata("Hidden", False)>
Public Class formPostHandler : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub ProcessRequest(context As HttpContext)

        Try
            With context
                If String.IsNullOrEmpty(.Request("$fm")) Then Throw New Exception("Invalid Form.")

                Dim f As Boolean = False
                For Each t As Type In Assembly.GetExecutingAssembly.GetTypes
                    If t.BaseType Is GetType(oForm) And String.Compare(t.Name, .Request("$fm"), True) = 0 Then

                        f = True
                        Dim ld As oForm = Activator.CreateInstance(t, Assembly.GetExecutingAssembly, Nothing)
                        Dim n As oRow = ld.GetType.GetMethod("AddRow").Invoke(ld, Nothing)

                        For Each I In n.GetType.GetProperties()
                            If I.CanWrite And Not String.IsNullOrEmpty(.Request(I.Name)) Then
                                n.GetType.GetProperty(I.Name).SetValue(n, .Request(I.Name))

                            End If
                        Next

                        ld.Post()

                    End If

                Next

                If Not f Then
                    Throw New Exception(String.Format("Unknown Form: {0}.", .Request("$fm")))

                End If

            End With

        Catch ex As Exception

        End Try

    End Sub

End Class