Imports System.Web
Imports System.Text
Imports System.Xml

Public Class httpHandler : Inherits EndPoint

#Region "Implements IHttpHandler"

    Public Overrides Sub ProcessRequest(ByVal context As HttpContext)

        Dim f As Boolean = False
        With context.Response
            .Clear()
            .ContentEncoding = Encoding.UTF8

            Try
                newSQLLog()
                Select Case httpMethod
                    Case "patch", "get", "post"
                        If ValidEnv Then
                            Select Case httpMethod
                                Case "get", "patch"
                                    For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                                        With TryCast(feed.Value, iFeed)
                                            .SetMeta(feed.Metadata)
                                            If .Match Then
                                                Select Case httpMethod
                                                    Case "get"
                                                        .ProcessRequest(context)

                                                    Case "patch"
                                                        .Install(context)

                                                End Select
                                                f = True
                                                Exit For
                                            End If
                                        End With
                                    Next

                                Case "post"
                                    If Not IsNothing(handlers) Then
                                        For Each hdlr As Lazy(Of xmlHandler, xmlHandlerProps) In handlers
                                            With hdlr
                                                With TryCast(hdlr.Value, iHandler)
                                                    .SetMeta(hdlr.Metadata)
                                                    If .Match Then
                                                        .ProcessRequest(context)
                                                        f = True
                                                        Exit For
                                                    End If
                                                End With
                                            End With
                                        Next
                                    End If

                            End Select

                        End If 'ValidEnv

                    Case "view"
                        f = True
                        For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds
                            If String.Compare(feed.Metadata.EndPoint, "view", True) = 0 Then
                                With TryCast(feed.Value, iFeed)
                                    .SetMeta(feed.Metadata)
                                    .ProcessRequest(context)
                                End With
                            End If
                        Next

                End Select

                If Not f Then
                    context.Response.StatusCode = 404
                    log.setException(
                        New Exception(
                            String.Format(
                                "Requested EndPoint not found: {0} /{1}/{2}.{3}",
                                httpMethod,
                                requestEnv,
                                requestEndpoint,
                                FileType
                            )
                        )
                    )

                End If

            Catch ex As Exception
                log.setException(ex)
                ToStream(ex)

            Finally
                .Flush()
                .Close()

                setLog()

            End Try

        End With

    End Sub

#End Region

End Class
