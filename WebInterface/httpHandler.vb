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

Public Class httpHandler
    Inherits EndPoint
    Implements IHttpHandler

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

    ReadOnly Property LogServer As String
        Get
            Try
                Return WebConfigurationManager.AppSettings("LogServer")

            Catch ex As Exception
                Return String.Empty

            End Try
        End Get
    End Property

    ReadOnly Property logPort As Integer
        Get
            Try
                Return WebConfigurationManager.AppSettings("logPort")

            Catch ex As Exception
                Return String.Empty

            End Try
        End Get
    End Property

    ReadOnly Property isEndPoint(Endpoint As EndPoint) As Boolean
        Get
            Return _
            String.Compare(
                Endpoint.Name,
                HttpContext.Current.Request("endpoint"),
                True
            ) = 0
        End Get
    End Property

#End Region

#Region "Implements IHttpHandler"

    Public Overloads Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Using log As New oMsgLog(
            context.Request.Url.Host.ToString,
            EvtLogSource.WEB,
            EvtLogVerbosity.Verbose,
            LogEntryType.Information
        )

            Try
                MyBase.log = log
                newSQLLog()

                Dim catalog = New AggregateCatalog()
                catalog.Catalogs.Add(BinCatalog)

                Dim container As New CompositionContainer(catalog)
                container.ComposeParts(Me)

                Dim mefmsg As New Dictionary(Of String, msgInterface)
                For Each msg As Lazy(Of msgInterface, msgInterfaceData) In _Messages
                    If Not mefmsg.Keys.Contains(String.Format("{0}:{1}", msg.Metadata.verb, msg.Metadata.msgType)) Then
                        With msg.Value
                            .msgType = msg.Metadata.msgType
                        End With
                        mefmsg.Add(String.Format("{0}:{1}", msg.Metadata.verb, msg.Metadata.msgType), msg.Value)
                    End If
                Next

                msgfactory = New msgFactory(mefmsg)

                log.LogData.AppendFormat(
                    "Received {0} /{1} from {2}.",
                    context.Request.HttpMethod.ToUpper,
                    context.Request("endpoint"),
                    context.Request.UserHostAddress
                ).AppendLine()

                Dim f As Boolean = False
                Select Case context.Request.HttpMethod.ToLower
                    Case "patch"
                        Dim env As List(Of String) = Environments()
                        env.Add("system")
                        If env.Contains(context.Request("environment")) Then
                            If Not IsNothing(feeds) Then
                                For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                                    With feed
                                        .Value.SetMeta(.Metadata)
                                        If isEndPoint(TryCast(.Value, EndPoint)) Then
                                            With TryCast(.Value, EndPoint)
                                                .BubbleID = BubbleID
                                                .dbConnection = dbConnection
                                            End With
                                            .Value.Install(context, log, msgfactory)
                                            f = True
                                            Exit For
                                        End If
                                    End With
                                Next
                            End If
                        End If

                    Case "view"

                        f = True
                        With context.Response
                            .Clear()
                            .ContentType = "text/xml"
                            .ContentEncoding = Encoding.UTF8
                            Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                            With objX
                                .WriteStartDocument()
                                .WriteStartElement("api")

                                For Each env As String In Environments()
                                    .WriteStartElement("env")
                                    .WriteAttributeString("name", env)
                                    .WriteEndElement()
                                Next

                                If Not IsNothing(feeds) Then
                                    For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                                        Select Case feed.Metadata.EndPoint.ToLower
                                            Case "log", "logdetail"

                                            Case Else
                                                .WriteStartElement("feed")
                                                .WriteAttributeString("name", String.Format("{0}.ashx", feed.Metadata.EndPoint))
                                                .WriteEndElement()

                                        End Select

                                    Next
                                End If

                                If Not IsNothing(handlers) Then
                                    For Each hdlr As Lazy(Of xmlHandler, xmlHandlerProps) In handlers
                                        .WriteStartElement("handler")
                                        .WriteAttributeString("name", String.Format("{0}.ashx", hdlr.Metadata.EndPoint))
                                        .WriteEndElement()
                                    Next
                                End If

                                .WriteEndElement() 'End Settings 
                                .WriteEndDocument()

                                .Flush()
                                .Close()

                            End With
                        End With

                    Case "get"
                        Dim env As List(Of String) = Environments()
                        env.Add("system")
                        If env.Contains(context.Request("environment")) Then
                            If Not IsNothing(feeds) Then
                                For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                                    With feed
                                        .Value.SetMeta(.Metadata)
                                        If Not HttpContext.Current.Request.Url.ToString.Contains("?log&") Then
                                            If isEndPoint(TryCast(.Value, EndPoint)) Then
                                                With TryCast(.Value, EndPoint)
                                                    .BubbleID = BubbleID
                                                    .dbConnection = dbConnection
                                                End With
                                                .Value.ProcessRequest(context, log, msgfactory)
                                                f = True
                                                Exit For
                                            End If

                                        Else
                                            If String.Compare(
                                                TryCast(.Value, EndPoint).Name,
                                                "log",
                                                True) _
                                            = 0 Then
                                                With TryCast(.Value, EndPoint)
                                                    .BubbleID = BubbleID
                                                    .dbConnection = dbConnection
                                                End With
                                                .Value.ProcessRequest(context, log, msgfactory)
                                                f = True
                                                Exit For
                                            End If

                                        End If
                                    End With
                                Next
                            End If
                        End If

                    Case "post"
                        If Environments.Contains(context.Request("environment")) Then
                            If Not IsNothing(handlers) Then
                                For Each hdlr As Lazy(Of xmlHandler, xmlHandlerProps) In handlers
                                    With hdlr
                                        .Value.SetMeta(.Metadata)
                                        If isEndPoint(TryCast(.Value, EndPoint)) Then
                                            With TryCast(.Value, EndPoint)
                                                .BubbleID = BubbleID
                                                .dbConnection = dbConnection
                                            End With
                                            .Value.ProcessRequest(context, log, msgfactory)
                                            f = True
                                            Exit For
                                        End If
                                    End With
                                Next
                            End If
                        End If

                End Select

                If Not f Then
                    context.Response.StatusCode = 404
                    log.setException(
                        New Exception(
                            String.Format(
                                "Requested EndPoint not found: {0}.",
                                context.Request("endpoint")
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
                        .WriteAttributeString("stacktr", ex.StackTrace)
                        .WriteEndElement() 'End Settings 
                        .WriteEndDocument()
                        .Flush()
                        .Close()
                    End With

                End With

            Finally
                setLog(log)

            End Try

        End Using

    End Sub

    Public Overrides Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)
        Throw New NotImplementedException()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#End Region

End Class
