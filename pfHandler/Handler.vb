Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO
Imports Newtonsoft.Json
Imports System.Xml.Serialization

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "pfHandler")>
<ExportMetadata("Hidden", False)>
Public Class pfHandler : Inherits iHandler : Implements xmlHandler

    ''' <summary>
    ''' Overides the XML schema for the handler
    ''' </summary>
    ''' <param name="Schemas">The XmlSchemaSet for the request</param>
    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas
            '    .Add(
            '        "urn:schemas-basda-org:2000:purchaseOrder:xdr:3.01",
            '        New XmlTextReader(
            '            New StringReader(
            '                My.Resources.basda_order_v3
            '            )
            '        )
            '    )
        End With
    End Sub

    ''' <summary>
    ''' Overrides XML handler with a StreamReader for business object parsing.
    ''' </summary>
    ''' <param name="w"></param>
    ''' <param name="Request"></param>
    Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)
        Dim s As New XmlSerializer(GetType(JDFProcessNode))
        Dim o As JDFProcessNode = s.Deserialize(Request)

        With w
            .WriteStartElement("response")
            .WriteEndElement()

        End With
    End Sub

    ''' <summary>
    ''' Overrides XML handler with an XML document for manual parsing.
    ''' </summary>
    ''' <param name="w">The response stream as a XmlTextWriter</param>
    ''' <param name="Request">The request as an XmlDocument</param>
    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        'log.LogData.AppendFormat("Running in company {0}.", requestEnv).AppendLine()

        'Using cust As New priForm("CUSTOMERS", "CUSTNAME", "CUSTDES")
        '    With cust
        '        Dim contacts = .AddForm("CUSTPERSONNEL", "NAME", "FIRM", "EMAIL")
        '        Dim tasks = .AddForm("CUSTNOTES", "SUBJECT", "CURDATE", "STIME", "ETIME")
        '        Dim addy = contacts.AddForm("BILLTO", "ADDRESS", "ADDRESS2", "ZIP")

        '        Dim r As priRow = .AddRow("Cust0001", "Customer")

        '        addy.AddRow(
        '            contacts.AddRow(
        '                r, "Si B", "Customer", "si@ntsa.org.uk"
        '            ),
        '            "48 Great Park", "Leyland", "pr25 3un"
        '        )

        '        addy.AddRow(
        '            contacts.AddRow(
        '                r, "jo B", "Customer", "jo@ntsa.org.uk"
        '            ),
        '            "49 Great Park", "Leyland", "pr25 3un"
        '        )

        '        With contacts
        '            .AddRow(r, "Emilie B", "Customer", Nothing)
        '        End With

        '        With tasks
        '            .AddRow(r, "A Task", "2017-08-28T00:00Z", "10:00", "12:00")
        '            .AddRow(r, "B Task", "2017-08-29T00:00Z", "12:00", "11:00")
        '        End With

        '    End With

        '    setKey(BubbleID)

        '    Dim ex As Exception = Nothing
        '    cust.Post(ex)
        '    If Not TypeOf ex Is apiResponse Then Throw (ex)
        '    TryCast(ex, apiResponse).toXML(w)

        'End Using

    End Sub

    ''' <summary>
    ''' Overrides handler for JSON data.
    ''' </summary>
    ''' <param name="w">The response stream as a JsonWriter</param>
    ''' <param name="json">The JSON request data</param>
    Public Overrides Sub jsonHandler(ByRef w As JsonWriter, ByRef json As String)
        'Dim e As BusinessObject = JsonConvert.DeserializeObject(json, GetType(BusinessObject))
        'e.write(w)

        With w
            .WriteStartObject()
            .WritePropertyName("response")
            .WriteEndObject()
        End With

    End Sub

End Class