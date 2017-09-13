Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Xml
Imports System.Net
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage
Imports System.Web.Configuration

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

        Dim reader As StreamReader = Nothing
        With context
            Try
                ' Read from stream                
                reader = New StreamReader(.Request.InputStream)
                Dim x As String = reader.ReadToEnd
                While Not String.Compare(x.Substring(0, 1), "<") = 0
                    x = x.Substring(1)
                End While

                setXml(x)

                ' Add XSDs
                Dim ret As New XmlReaderSettings()
                XmlStylesheet(ret.Schemas)
                If ret.Schemas.Count > 0 Then
                    ret.ValidationType = ValidationType.Schema
                    log.LogData.AppendFormat("{0} XSDs loaded.", _thisRequest.Schemas.Schemas.Count).AppendLine()
                    reader.BaseStream.Position = 0

                    ' And load
                    _thisRequest.Load(XmlReader.Create(reader, ret))

                Else
                    If _HandlerStyle = eHandlerStyle.xml Then
                        ' And load
                        _thisRequest.LoadXml(x)
                    End If
                End If


            Catch ex As Exception
                With reader
                    .Close()
                    .Dispose()
                End With
                Throw New Exception(String.Format("Bad request: {0}", ex.Message))


            End Try

            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8

                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    '.WriteStartElement("response")

                    Select Case _HandlerStyle
                        Case eHandlerStyle.stream
                            reader.BaseStream.Position = 0
                            log.LogData.Append("Passing to stream handler.").AppendLine()
                            StreamHandler(objX, reader)

                        Case Else
                            log.LogData.Append("Passing to XML Document handler.").AppendLine()
                            XMLHandler(objX, _thisRequest)

                    End Select

                    '.WriteEndElement() 'End Settings 
                    .WriteEndDocument()

                    .Flush()
                    .Close()

                End With

            End With

        End With

        With reader
            .Close()
            .Dispose()
        End With

    End Sub

#End Region

End Class
