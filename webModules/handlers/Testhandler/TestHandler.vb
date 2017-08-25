Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports PriPROC6.svcMessage
Imports PriPROC6.Interface.Message

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "testhandler")>
<ExportMetadata("HandlerStyle", eHandlerStyle.xml)>
Public Class TestHandler : Inherits iHandler : Implements xmlHandler

#Region "Base Methods"

    Shadows Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlHandler.ProcessRequest
        MyBase.ProcessRequest(context, log, msgFactory)
    End Sub

    Shadows Sub SetMeta(ByRef Metadata As xmlHandlerProps) Implements xmlHandler.SetMeta
        MyBase.SetMeta(Metadata)
    End Sub

#End Region

    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas

        End With
    End Sub

    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        log.LogData.AppendFormat("Running in company {0}.", requestEnv).AppendLine()

        Using cust As New priForm("CUSTOMERS", "CUSTNAME", "CUSTDES")
            With cust
                Dim contacts = .AddForm("CUSTPERSONNEL", "NAME", "FIRM", "EMAIL")
                Dim tasks = .AddForm("CUSTNOTES", "SUBJECT", "CURDATE", "STIME", "ETIME")
                Dim addy = contacts.AddForm("BILLTO", "ADDRESS", "ADDRESS2", "ZIP")

                Dim r As priRow = .AddRow("Cust0001", "Customer")

                addy.AddRow(
                    contacts.AddRow(
                        r, "Si B", "Customer", "si@ntsa.org.uk"
                    ),
                    "48 Great Park", "Leyland", "pr25 3un"
                )

                addy.AddRow(
                    contacts.AddRow(
                        r, "jo B", "Customer", "jo@ntsa.org.uk"
                    ),
                    "49 Great Park", "Leyland", "pr25 3un"
                )

                With contacts
                    .AddRow(r, "Emilie B", "Customer", Nothing)
                End With

                With tasks
                    .AddRow(r, "A Task", "2017-08-28T00:00Z", "10:00", "12:00")
                    .AddRow(r, "B Task", "2017-08-29T00:00Z", "12:00", "11:00")
                End With

            End With

            setKey("test duplication", True)

            Dim ex As Exception = Nothing
            cust.Post(ex)
            If Not TypeOf ex Is apiResponse Then Throw (ex)
            TryCast(ex, apiResponse).toXML(w)

        End Using

    End Sub

End Class