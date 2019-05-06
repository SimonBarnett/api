Imports System.ComponentModel.Composition
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml
Imports PriPROC6.Interface.oData

Namespace Web.BuiltIn

    <Export(GetType(xmlFeed))>
    <ExportMetadata("EndPoint", "schema")>
    <ExportMetadata("Hidden", True)>
    Public Class oschema : Inherits iFeed : Implements xmlFeed

        Overrides Sub ProcessReq(ByVal context As HttpContext)
            Using objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("Schema")

                    Try
                        Dim r As Object
                        Using client As New oClient("/$metadata", "GET")
                            r = client.GetResponse
                            If TypeOf (r) Is oException Then
                                Throw New Exception(TryCast(r, oException).Message)

                            Else
                                With TryCast(r, Net.WebResponse)
                                    Dim reader As New StreamReader(.GetResponseStream)
                                    Dim str As String = reader.ReadToEnd()
                                    str = str.Substring(InStr(str, "<EntityType") - 1)
                                    str = Split(str, "</Schema>")(0)

                                    Dim doc As New XmlDocument
                                    doc.LoadXml(String.Format("<Schema>{0}</Schema>", str))

                                    Dim schema As New XmlDocument
                                    schema.Load(ExecuteXmlReader(My.Resources.schema))
                                    For Each entity As XmlNode In schema.SelectNodes("Schema/Entity")

                                        objX.WriteStartElement("EntityType")
                                        objX.WriteAttributeString("Name", entity.Attributes("Name").Value)

                                        Dim e As XmlNode = doc.SelectSingleNode(
                                    String.Format(
                                        "//EntityType[@Name='{0}']",
                                        entity.Attributes("Name").Value
                                    )
                                )

                                        objX.WriteStartElement("Key")
                                        For Each k As XmlNode In e.SelectNodes("Key/PropertyRef")
                                            objX.WriteStartElement("PropertyRef")
                                            objX.WriteAttributeString("Name", k.Attributes("Name").Value)
                                            objX.WriteEndElement()
                                        Next
                                        objX.WriteEndElement()

                                        For Each p As XmlNode In e.SelectNodes("Property")
                                            Dim ed As XmlNode = entity.SelectSingleNode(String.Format("Property[@Name='{0}']", p.Attributes("Name").Value))
                                            If Not ed Is Nothing Then
                                                objX.WriteStartElement("Property")
                                                objX.WriteAttributeString("Name", p.Attributes("Name").Value)
                                                objX.WriteAttributeString("Title", ed.Attributes("title").Value)
                                                objX.WriteAttributeString("Type", p.Attributes("Type").Value)
                                                If Not p.Attributes("MaxLength") Is Nothing Then
                                                    objX.WriteAttributeString("MaxLength", p.Attributes("MaxLength").Value)
                                                End If

                                                objX.WriteAttributeString("Mandatory", ed.Attributes("mandatory").Value)
                                                objX.WriteAttributeString("ReadOnly", ed.Attributes("readonly").Value)
                                                objX.WriteEndElement()

                                            End If
                                        Next

                                        For Each n As XmlNode In e.SelectNodes("NavigationProperty")
                                            Dim x As String = Split(Split(n.Attributes("Type").Value, "Priority.OData.")(1), ")")(0)
                                            If Not schema.SelectSingleNode(
                                        String.Format(
                                            "//Entity[@Name='{0}']",
                                            x
                                        )
                                    ) Is Nothing Then
                                                objX.WriteStartElement("NavigationProperty")
                                                objX.WriteAttributeString("Name", x)
                                                objX.WriteEndElement()

                                            End If

                                        Next

                                        objX.WriteEndElement()
                                    Next

                                End With

                            End If

                        End Using

                    Catch ex As Exception
                        log.setException(ex)
                        Throw (ex)

                    End Try

                End With

            End Using

        End Sub

    End Class

End Namespace