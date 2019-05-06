Imports System.ComponentModel.Composition
Imports System.Xml
Imports Newtonsoft.Json
Imports System.Web
Imports System.IO
Imports System.Reflection
Imports PriPROC6.Interface.Web

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "newtempltest")>
<ExportMetadata("Hidden", False)>
Public Class newtempltest : Inherits iHandler : Implements xmlHandler

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
        'MyBase.StreamHandler(w, Request)

    End Sub

    ''' <summary>
    ''' Overrides XML handler with an XML document for manual parsing.
    ''' </summary>
    ''' <param name="w">The response stream as a XmlTextWriter</param>
    ''' <param name="Request">The request as an XmlDocument</param>
    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        'log.LogData.AppendFormat("Running in company {0}.", requestEnv).AppendLine()

        'Using F As New CUSTOMERS(Assembly.GetExecutingAssembly)
        '    With F
        '        With .AddRow()
        '            .CUSTNAME = "TQ000043"
        '            .BUSINESSTYPE = "BusType"
        '            .OWNERLOGIN = "SimonB"
        '            .CREATEDDATE = Now

        '            If Not .Get() Then Throw .Exception

        '            With .CUSTPERSONNEL.AddRow
        '                .NAME = "Joe Bloggs"
        '                .AGENTCODE = "007"
        '                .CIVFLAG = "N"

        '                If .Post Then

        '                End If

        '            End With

        '            .ADDRESS3 = "test"
        '            If .Patch() Then

        '            End If

        '        End With

        '    End With

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

    End Sub

End Class