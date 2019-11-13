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

        With context
            For Each t As Type In Assembly.GetExecutingAssembly.GetTypes
                If t.BaseType Is GetType(oForm) And String.Compare(t.Name, .Request("$fm"), True) = 0 Then

                    Try
                        Dim ld As oForm = Activator.CreateInstance(t, Assembly.GetExecutingAssembly, Nothing)
                        Dim n = ld.GetType.GetMethod("AddRow").Invoke(ld, Nothing)

                        For Each I In n.GetType.GetProperties()
                            If I.CanWrite And Not String.IsNullOrEmpty(.Request(I.Name)) Then
                                ld.GetType.GetProperty(I.Name).SetValue(ld, .Request(I.Name))

                            End If
                        Next

                        ld.Post()

                    Catch ex As Exception

                    End Try

                End If
            Next
        End With

    End Sub

End Class