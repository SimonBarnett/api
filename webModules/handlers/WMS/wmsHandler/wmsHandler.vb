Imports System.ComponentModel.Composition
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports PriPROC6.Interface.Web

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "wmshandler")>
<ExportMetadata("Hidden", False)>
Public Class wmsHandler : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas

        End With
    End Sub

    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        log.LogData.AppendFormat("Running in company {0}.", requestEnv).AppendLine()

        With w
            .WriteStartElement("response")
            .WriteEndElement()
        End With

    End Sub



End Class