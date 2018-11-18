Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "hours")>
<ExportMetadata("Hidden", False)>
Public Class hours : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas

        End With
    End Sub

    Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)

        Dim resp As apiResponse = Nothing
        Dim hours As billableJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(Of billableJSON)(Request.ReadToEnd)

        hours.cascade()

        Select Case hours.type
            Case "Q" ' Service calls
                log.LogData.AppendFormat("Service Call Log for ({0}).", hours.name).AppendLine()
                resp = ReportService(hours)

            Case "p" ' Projects
                log.LogData.AppendFormat("Project Log for ({0}).", hours.name).AppendLine()
                resp = ReportProject(hours)

            Case "O" ' Sales Orders
                log.LogData.AppendFormat("Sales Order Log for ({0}).", hours.name).AppendLine()
                With hours
                    .type = "p"
                    .order = .doc
                    .doc = ExecuteScalar(String.Format(My.Resources.so, requestEnv, .doc))
                    If .doc.Length = 0 Then Throw New Exception(String.Format("Invalid Sales Order: {0}", .order))
                End With
                resp = ReportProject(hours)

        End Select


        With w
            If Not resp.response < 200 Or resp.response > 299 Then
                log.EntryType = LogEntryType.Err
            End If

            .WriteStartElement("apiresult")

            .WriteAttributeString("response", resp.response)
            .WriteAttributeString("message", resp.message)

            log.LogData.AppendFormat("Response: ({0}).", resp.response.ToString).AppendLine()
            log.LogData.AppendFormat("{0}.", resp.message).AppendLine()

            For Each m As apiError In resp.msgs
                If Not m.Loaded Then
                    .WriteStartElement("line")
                    .WriteAttributeString("name", m.Line)
                    .WriteAttributeString("message", m.message)
                    .WriteEndElement() 'End Settings 
                    log.LogData.AppendFormat("line {0}: {1}", m.Line, m.message).AppendLine()
                End If
            Next

            .WriteEndElement() 'End Settings 

        End With

    End Sub

    Private Function ReportProject(hours As billableJSON) As Exception
        Using project As New priForm(
            "TRANSORDER_q",
            "DOCNO",
            "CURDATE",
            "USERLOGIN",
            "WBS",
            "PARTNAME",
            "ORDNAME",
            "OLINE",
            "TQUANT",
            "ACTDES"
         )

            For Each i As itemJSON In hours.append
                With i
                    project.AddRow(
                        hours.doc,
                        Timestamp(),
                        hours.name,
                        i.wbs,
                        i.part,
                        hours.order,
                        i.line,
                        i.qty,
                        i.desc
                    )
                End With
            Next

            log.LogData.Append("Sending: ").AppendLine.Append(project.toString).AppendLine()

            Dim ex As Exception = Nothing
            project.Post(ex)
            If Not TypeOf ex Is apiResponse Then
                Throw (ex)
            Else
                Return ex
            End If

        End Using

    End Function

    Private Function ReportService(hours As billableJSON) As Exception
        Dim SC As New priForm(
            "DOCUMENTS_Q",
            "DOCNO",
            "TYPE"
         )

        Dim report = SC.AddForm(
            "TRANSORDER_QW",
            "CURDATE",
            "TECHNICIANNAME",
            "PARTNAME",
            "PDES",
            "TQUANT"
        )

        Dim this As priRow = SC.AddRow(hours.doc, hours.type)

        For Each i As itemJSON In hours.append
            With i
                report.AddRow(
                    this,
                    Timestamp(),
                    hours.name,
                    i.part,
                    i.desc,
                    i.qty
                )
            End With
        Next

        log.LogData.Append("Sending: ").AppendLine.Append(SC.toString).AppendLine()

        Dim ex As Exception = Nothing
        SC.Post(ex)

        If Not TypeOf ex Is apiResponse Then
            Throw (ex)
        Else
            Return ex
        End If

    End Function

    Private Function Timestamp() As String
        Return String.Format(
            "{0}-{1}-{2}T00:00Z",
            DatePart(DateInterval.Year, Now()),
            Strings.Right("00" & DatePart(DateInterval.Month, Now()), 2),
            Strings.Right("00" & DatePart(DateInterval.Day, Now()), 2)
           )

    End Function

End Class