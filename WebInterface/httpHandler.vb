Imports System.IO
Imports System.Web
Imports System.ComponentModel.Composition
Imports System.ComponentModel.Composition.Hosting
Imports System.Web.Configuration

Imports PriPROC6.PriSock
Imports PriPROC6.svcMessage
Imports PriPROC6.Interface.Message
Imports System.Text
Imports System.Xml

Public Class httpHandler : Implements IHttpHandler

#Region "Imported MEF Enumerables"

    <ImportMany()>
    Public Property Messages As IEnumerable(Of Lazy(Of msgInterface, msgInterfaceData))

    <ImportMany()>
    Public Property handlers As IEnumerable(Of Lazy(Of xmlHandler, xmlHandlerProps))

    <ImportMany()>
    Public Property feeds As IEnumerable(Of Lazy(Of xmlFeed, xmlFeedProps))

#End Region

#Region "Private Properties"

    ReadOnly Property BinCatalog As Primitives.ComposablePartCatalog
        Get
            Return New DirectoryCatalog(
                Path.Combine(
                    HttpContext.Current.Request.PhysicalApplicationPath,
                    "bin"
                )
            )
        End Get
    End Property

    ReadOnly Property Service As String
        Get
            Return WebConfigurationManager.AppSettings("service")
        End Get
    End Property

    ReadOnly Property logPort As Integer
        Get
            Return WebConfigurationManager.AppSettings("logPort")
        End Get
    End Property

    ReadOnly Property EndpointRequest As String
        Get
            Return Split(HttpContext.Current.Request.RawUrl.Split("?")(0), "/").Last
        End Get
    End Property

    ReadOnly Property isEndPoint(Endpoint As EndPoint) As Boolean
        Get
            Return _
            String.Compare(
                String.Format(
                    "{0}.ashx",
                    Endpoint.Name
                ),
                   EndpointRequest,
                    True
            ) = 0
        End Get
    End Property

    Private _msgFactory As msgFactory = Nothing
    ReadOnly Property msgFactory As msgFactory
        Get
            Return _msgFactory
        End Get
    End Property

#End Region

#Region "Implements IHttpHandler"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Using log As New oMsgLog(
            context.Request.Url.Host.ToString,
            EvtLogSource.WEB,
            EvtLogVerbosity.Verbose,
            LogEntryType.Information
        )

            Try
                Dim catalog = New AggregateCatalog()
                catalog.Catalogs.Add(BinCatalog)

                Dim container As New CompositionContainer(catalog)
                container.ComposeParts(Me)

                _msgFactory = New msgFactory(Messages)

                log.LogData.AppendFormat(
                    "Received {0} {1} from {2}.",
                    context.Request.HttpMethod.ToUpper,
                    EndpointRequest,
                    context.Request.UserHostAddress
                ).AppendLine()

                Dim f As Boolean = False
                Select Case context.Request.HttpMethod.ToLower
                    Case "get"
                        If Not IsNothing(feeds) Then
                            For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                                With feed
                                    .Value.SetMeta(.Metadata)
                                    If isEndPoint(TryCast(.Value, EndPoint)) Then
                                        .Value.ProcessRequest(context, log, msgFactory)
                                        f = True
                                        Exit For
                                    End If
                                End With
                            Next
                        End If

                    Case "post"
                        If Not IsNothing(handlers) Then
                            For Each hdlr As Lazy(Of xmlHandler, xmlHandlerProps) In handlers
                                With hdlr
                                    .Value.SetMeta(.Metadata)
                                    If isEndPoint(TryCast(hdlr.Value, EndPoint)) Then
                                        .Value.ProcessRequest(context, log, msgFactory)
                                        f = True
                                        Exit For
                                    End If
                                End With
                            Next
                        End If

                End Select

                If Not f Then
                    context.Response.StatusCode = 404
                    log.setException(
                        New Exception(
                            String.Format(
                                "Requested EndPoint not found: {0}.",
                                EndpointRequest
                            )
                        )
                    )

                End If

            Catch ex As Exception
                log.setException(ex)
                With context.Response
                    .Clear()
                    .ContentType = "text/xml"
                    .ContentEncoding = Encoding.UTF8
                    Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                    With objX
                        .WriteStartDocument()
                        .WriteStartElement("response")
                        .WriteAttributeString("status", CStr(500))
                        .WriteAttributeString("message", ex.Message)
                        .WriteEndElement() 'End Settings 
                        .WriteEndDocument()
                        .Flush()
                        .Close()
                    End With
                End With

            Finally
                Try
                    Using cli As New iClient(Service, logPort, 1)
                        cli.Send(msgFactory.EncodeRequest("log", log))
                    End Using

                Catch : End Try

            End Try

        End Using

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#End Region

End Class
