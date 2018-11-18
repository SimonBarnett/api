' C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin\x64>xsd M:\xsd\basda\ORDER-V3.xsd /c /l:vb /o:M:\xsd\basda
Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.Xml.Serialization
Imports System.IO

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "basda-order-v3")>
<ExportMetadata("Hidden", False)>
Public Class TestHandler : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub XmlStylesheet(ByRef Schemas As Schema.XmlSchemaSet)
        With Schemas
            .Add(
                "urn:schemas-basda-org:2000:purchaseOrder:xdr:3.01",
                New XmlTextReader(
                    New StringReader(
                        My.Resources.basda_order_v3
                    )
                )
            )
        End With
    End Sub

    Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)

        log.LogData.AppendFormat("Running in company {0}.", requestEnv).AppendLine()

        Dim s As New XmlSerializer(GetType(Order))
        Dim o As Basda.Order = s.Deserialize(Request)

        Dim ORDERS As New priForm("ORDERS", "CUSTNAME", "REFERENCE")
        Dim ORDERITEMS = ORDERS.AddForm("ORDERITEMS", "PARTNAME", "TQUANT")

        With o
            setKey(.OrderReferences.BuyersOrderNumber.Value)
            Dim Ord As priRow = ORDERS.AddRow(
                .Buyer.BuyerReferences.SuppliersCodeForBuyer,
                .OrderReferences.BuyersOrderNumber.Value
            )

            For Each i In .OrderLine
                With i
                    ORDERITEMS.AddRow(
                        Ord,
                        .Product(0).SuppliersProductCode,
                        CDbl(.Quantity.Amount)
                    )
                End With
            Next

        End With

        Dim ex As Exception = Nothing
        ORDERS.Post(ex)
        If Not TypeOf ex Is apiResponse Then Throw (ex)

        w.WriteStartElement("response")
        TryCast(ex, apiResponse).toXML(w)
        w.WriteEndElement()

    End Sub

End Class