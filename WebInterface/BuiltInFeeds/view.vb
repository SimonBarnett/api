Imports System.ComponentModel.Composition
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Xml

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "view")>
<ExportMetadata("Hidden", True)>
Public Class EndPointMap : Inherits iFeed : Implements xmlFeed

    Overrides Sub ProcessReq(ByVal context As HttpContext)

        With context.Response
            .Clear()
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8

            Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
            With objX
                .WriteStartDocument()
                .WriteStartElement("api")
                .WriteAttributeString("name", context.Request.Url.Host)
                For Each env As String In Environments()
                    If String.Compare(requestEnv, env) = 0 Or requestEnv.Length = 0 Then
                        .WriteStartElement("env")
                        .WriteAttributeString("name", env)

                        If Not IsNothing(feeds) Then
                            For Each feed As Lazy(Of xmlFeed, xmlFeedProps) In feeds

                                Select Case feed.Metadata.Hidden
                                    Case False


                                        .WriteStartElement("feed")
                                        .WriteAttributeString("name", String.Format("{0}.ashx", feed.Metadata.EndPoint))
                                        .WriteAttributeString("type", "mef")

                                        Dim par As Dictionary(Of String, String)
                                        Try
                                            With TryCast(feed.Value, iFeed)
                                                .SetMeta(feed.Metadata)
                                                par = .Params

                                            End With

                                            For Each p As String In par.Keys
                                                .WriteStartElement("param")
                                                .WriteAttributeString("name", String.Format("{0}", p))
                                                .WriteAttributeString("type", String.Format("{0}", par(p)))
                                                .WriteEndElement()
                                            Next
                                        Catch : End Try

                                        .WriteEndElement()

                                End Select

                            Next
                        End If

                        With feedDir
                            If .Exists Then
                                For Each f As FileInfo In .GetFiles("*.sql")
                                    With objX
                                        .WriteStartElement("feed")
                                        .WriteAttributeString("name", String.Format("{0}.ashx", f.Name.Split(".")(0)))
                                        .WriteAttributeString("type", "fso")

                                        Dim par As Dictionary(Of String, String) = fsoParams(f)
                                        For Each p As String In par.Keys
                                            .WriteStartElement("param")
                                            .WriteAttributeString("name", String.Format("{0}", p))
                                            .WriteAttributeString("type", String.Format("{0}", par(p)))
                                            .WriteEndElement()
                                        Next
                                        .WriteEndElement()
                                    End With
                                Next
                            End If
                        End With

                        Dim sp As Dictionary(Of String, Integer) = spXML(env)
                        For Each f As String In sp.Keys
                            .WriteStartElement("feed")
                            .WriteAttributeString("name", String.Format("{0}.ashx", f.Substring(4)))
                            .WriteAttributeString("type", "dbo")

                            Dim par As Dictionary(Of String, String) = spParams(env, sp(f))
                            For Each p As String In par.Keys
                                .WriteStartElement("param")
                                .WriteAttributeString("name", String.Format("{0}", p))
                                .WriteAttributeString("type", String.Format("{0}", par(p)))
                                .WriteEndElement()
                            Next

                            .WriteEndElement()
                        Next

                        If Not IsNothing(handlers) Then
                            For Each hdlr As Lazy(Of xmlHandler, xmlHandlerProps) In handlers
                                .WriteStartElement("handler")
                                .WriteAttributeString("name", String.Format("{0}.ashx", hdlr.Metadata.EndPoint))
                                .WriteEndElement()
                            Next
                        End If

                        .WriteEndElement()
                    End If
                Next

                .WriteEndElement() 'End Settings 
                .WriteEndDocument()

                .Flush()
                .Close()

            End With

        End With

    End Sub

    Public Function fsoParams(f As FileInfo) As Dictionary(Of String, String)

        Dim ret As New Dictionary(Of String, String)
        Dim Query As String

        Using SR As New StreamReader(f.FullName)
            query = SR.ReadToEnd
            Dim declaration As New Regex("(?<=declare.*@)[A-Z0-9]*", RegexOptions.IgnoreCase)
            For Each m As Match In declaration.Matches(query)
                Dim tp As New Regex(String.Format("(?<=declare.@{0}.)[a-zA-Z0-9]*", m.Value), RegexOptions.IgnoreCase)
                ret.Add(m.Value, tp.Matches(Query)(0).Value.ToLower)

            Next
        End Using

        Return ret

    End Function

End Class
