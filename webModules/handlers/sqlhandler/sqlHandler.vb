Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports PriPROC6.svcMessage
Imports PriPROC6.Interface.Message
Imports System.Web.Configuration
Imports System.Data.SqlClient

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "sqlhandler")>
<ExportMetadata("HandlerStyle", eHandlerStyle.xml)>
Public Class sqlHandler : Inherits iHandler : Implements xmlHandler

#Region "thisRequest Properties"

    Private thisRequest As New XmlDocument
    Private Enum eRequestType
        Tabular = 1
        Scalar = 2
        NonQuery = 3
    End Enum

    Private ReadOnly Property RequestSQL() As String
        Get
            Try
                Return thisRequest.SelectSingleNode("//GetRequest/SQL").InnerText
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Private ReadOnly Property RequestEnvironment() As String
        Get
            Try
                Return thisRequest.SelectSingleNode("//GetRequest/Environment").InnerText

            Catch ex As Exception
                Return WebConfigurationManager.AppSettings("Environment")

            End Try
        End Get
    End Property

    Private ReadOnly Property RequestType() As eRequestType
        Get
            Try
                Select Case thisRequest.SelectSingleNode("//GetRequest/RequestType").InnerText.ToLower
                    Case "tabular"
                        Return eRequestType.Tabular
                    Case "scalar"
                        Return eRequestType.Scalar
                    Case "nonquery"
                        Return eRequestType.NonQuery
                    Case Else
                        Throw New Exception("Unknown RequestType")
                End Select
            Catch ex As Exception
                Return eRequestType.Tabular
            End Try
        End Get
    End Property

#End Region

#Region "Base Methods"

    Shadows Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlHandler.ProcessRequest
        MyBase.ProcessRequest(context, log, msgFactory)
    End Sub

    Shadows Sub SetMeta(ByRef Metadata As xmlHandlerProps) Implements xmlHandler.SetMeta
        MyBase.SetMeta(Metadata)
    End Sub

#End Region

    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas

        End With
    End Sub

    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        thisRequest = Request
        With w
            .WriteStartElement("result")

            Select Case RequestType
                Case eRequestType.Tabular
                    Dim dataReader As SqlDataReader = ExecuteReader(SQL)
                    .WriteStartElement("columns")
                    If dataReader.FieldCount > 0 Then
                        For Each col As Data.DataRow In dataReader.GetSchemaTable.Rows
                            .WriteStartElement("column")
                            .WriteAttributeString("name", col.Item("ColumnName"))
                            .WriteAttributeString("type", col.Item("DataType").ToString)
                            .WriteEndElement() 'End column 
                        Next
                        .WriteEndElement() 'End columns 
                    End If

                    .WriteStartElement("rows")
                    If dataReader.HasRows Then
                        dataReader.Read()
                        Do
                            .WriteStartElement("row")
                            For i As Integer = 0 To dataReader.FieldCount - 1
                                .WriteAttributeString("_" & i.ToString, dataReader.Item(i).ToString)
                            Next
                            .WriteEndElement() 'End row 
                        Loop While dataReader.Read()

                    End If
                    .WriteEndElement() 'End rows                     

                Case eRequestType.Scalar
                    .WriteRaw(ExecuteScalar(SQL))

                Case eRequestType.NonQuery
                    ExecuteNonQuery(SQL)

            End Select

            .WriteEndElement()
        End With

    End Sub

    Private Function SQL()
        Return String.Format("use [{1}];{0}{2}", vbCrLf, RequestEnvironment, RequestSQL)
    End Function

End Class
