Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports Newtonsoft.Json
Imports System.IO

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "testhandler")>
<ExportMetadata("Hidden", False)>
Public Class TestHandler : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub jsonHandler(ByRef w As JsonWriter, ByRef json As String)
        'Throw New Exception
        Dim e As BusinessObject = JsonConvert.DeserializeObject(json, GetType(BusinessObject))
        e.write(w)

    End Sub

#Region "Json Business Object"

    Private Class BusinessObject
        Public Property entities As IList(Of ent)
        Public Sub write(ByRef w As JsonWriter)
            With w
                .WriteStartObject()
                .WritePropertyName("entities")

                .WriteStartArray()
                For Each en As ent In Me.entities
                    en.write(w)
                Next
                .WriteEndArray()

            End With
        End Sub
    End Class

    Private Class ent
        Public Property name As String
        Public Property keys As IList(Of String)
        Public Sub write(ByRef w As JsonWriter)
            With w
                .WriteStartObject()
                .WritePropertyName("name")
                .WriteValue(Me.name)
                .WritePropertyName("keys")
                .WriteStartArray()
                For Each key As String In Me.keys
                    .WriteValue(key)
                Next
                .WriteEndArray()
                .WriteEndObject()
            End With
        End Sub
    End Class

#End Region

End Class

