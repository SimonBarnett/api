Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Xml
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage

Public MustInherit Class iHandler : Inherits EndPoint

#Region "Metadata"

    Private _HandlerStyle As eHandlerStyle
    Public Sub SetMeta(ByRef Metadata As xmlHandlerProps)
        With Metadata
            Name = .EndPoint
            _HandlerStyle = .HandlerStyle
        End With
    End Sub

#End Region

#Region "Overridable stubs"

    Public Overridable Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)

    End Sub

    Public Overridable Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)
        Throw New NotImplementedException()
    End Sub

    Public Overridable Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)
        Throw New NotImplementedException()
    End Sub

#End Region

#Region "Process Request"

    Dim _thisRequest As New XmlDocument
    Public Overrides Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)

        MyBase.log = log
        MyBase.msgfactory = msgFactory

        Dim StatusCode As Integer = 200
        Dim StatusMessage As String = "Ok"

        Dim _SecondSave As String = Nothing
        Dim reader As StreamReader = Nothing

        With context
            Try
                ' Read from stream                
                reader = New StreamReader(.Request.InputStream)
                Dim x As String = reader.ReadToEnd
                While Not String.Compare(x.Substring(0, 1), "<") = 0
                    x = x.Substring(1)
                End While

                ' Add XSDs
                XmlStylesheet(_thisRequest.Schemas)
                If _thisRequest.Schemas.Schemas.Count > 0 Then
                    log.LogData.AppendFormat("{0} XSDs loaded.", _thisRequest.Schemas.Schemas.Count).AppendLine()
                End If

                ' And load
                _thisRequest.LoadXml(x)

            Catch ex As Exception
                log.setException(ex)
                StatusCode = 500
                StatusMessage = ex.Message

            Finally
                With reader
                    .Close()
                    .Dispose()
                End With

            End Try

            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("response")
                    .WriteAttributeString("status", CStr(StatusCode))
                    .WriteAttributeString("message", StatusMessage)

                    Select Case _HandlerStyle
                        Case eHandlerStyle.stream
                            reader.BaseStream.Position = 0
                            log.LogData.Append("Passing to stream handler.").AppendLine()
                            StreamHandler(objX, reader)

                        Case Else
                            log.LogData.Append("Passing to XML Document handler.").AppendLine()
                            XMLHandler(objX, _thisRequest)

                    End Select

                    .WriteEndElement() 'End Settings 
                    .WriteEndDocument()

                    .Flush()
                    .Close()

                End With

            End With

        End With


    End Sub

#End Region

End Class
