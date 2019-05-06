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
            .Add(
                "dev_medatechuk_com_OrderSchema_1",
                New XmlTextReader(
                    New StringReader(
                        My.Resources.dev_medatechuk_com_OrderSchema_1
                    )
                )
            )
        End With
    End Sub

    ''' <summary>
    ''' Overrides XML handler with a StreamReader for business object parsing.
    ''' </summary>
    ''' <param name="w"></param>
    ''' <param name="Request"></param>
    Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)

        Dim s As New XmlSerializer(GetType(request))
        Dim o As request = s.Deserialize(Request)

        With o
            With .customer
                If String.IsNullOrEmpty(.name) Then
                    If .address.Count = 1 Then
                        .name = NewCustomer(.contact, .address(0))
                    Else
                        .name = NewCustomer(.contact, .address(eAddressType.Billing))
                    End If

                End If
            End With

            NewOrder(.customer)

            If Not .customer.order.payment Is Nothing Then _
                NewPayment(.customer)

        End With

        With w
            .WriteStartElement("response")
            .WriteEndElement()

        End With

    End Sub

    Private Function NewCustomer(contact As requestCustomerContact, address As requestCustomerAddress) As String

        Dim custname As String = ExecuteScalar(
            String.Format(
                "SELECT dbo.sp_CustByEmail('{0}')",
                contact.email
            )
        )

        If Not String.Compare(custname.Substring(0, 1), "+") = 0 Then
            Return custname
        Else
            custname = custname.Substring(1)
        End If

        Using cust As New CustomerLoading
            Dim r As priRow = cust.AddRow(
                custname, contact.name,
                contact.phone, contact.email,
                address.address1, address.address2, address.address3,
                address.county, address.postcode, address.country
            )

            Dim ex As Exception = Nothing
            cust.Post(ex)
            If Not TypeOf ex Is apiResponse Then Throw (ex)
            With TryCast(ex, apiResponse)
                log.LogData.AppendFormat("{0}: {1}", .response, .Message)
                For Each msg As apiError In .msgs
                    log.LogData.AppendFormat("  Ln {0}: {1}", msg.Line, msg.message)
                Next

                With .msgs(0)
                    If .Loaded Then
                        custname = .resultKeys("CUSTNAME")
                    Else
                        Throw New Exception(.message)
                    End If

                End With
            End With

        End Using

        Return custname

    End Function

    Private Sub NewOrder(ByRef customer As requestCustomer)

        With customer
            log.LogData.AppendFormat("Creating order {0} for customer {1}.", .order.orderref, .name, .desc)

            Using ord As New orderLoading
                Dim o As priRow = ord.addRow(.name, .order.orderref)

                For Each orderitem As requestCustomerOrderItem In .order.item
                    With orderitem
                        ord.Orderitems.addRow(o, .name, .qty)

                    End With

                Next

                If Not .address(eAddressType.Delivery) Is Nothing Then
                    With .address(eAddressType.Delivery)
                        ord.DeliveryAddress.addRow(o,
                            .address1, .address2, .address3,
                            .county, .postcode, .country
                        )

                    End With

                End If

                Dim ex As Exception = Nothing
                ord.Post(ex)
                If Not TypeOf ex Is apiResponse Then Throw (ex)
                With TryCast(ex, apiResponse)
                    log.LogData.AppendFormat("{0}: {1}", .response, .Message)
                    For Each msg As apiError In .msgs
                        log.LogData.AppendFormat("  Ln {0}: {1}", msg.Line, msg.message)
                    Next

                    With .msgs(0)
                        If .Loaded Then
                            customer.order.PriorityOrdName = .resultKeys("ORDNAME")
                        Else
                            Throw New Exception(.message)
                        End If

                    End With

                End With

            End Using

        End With

    End Sub

    Private Sub NewPayment(ByRef customer As requestCustomer)

        With customer
            log.LogData.AppendFormat("Creating payment for order #{0}.", .order.PriorityOrdName)
            Using pay As New paymentLoading
                Dim p As priRow = pay.AddRow(.name, .order.PriorityOrdName)
                With .order.payment
                    pay.TPAYMENT2.AddRow(p, "7", .value)

                End With

                Dim ex As Exception = Nothing
                pay.Post(ex)
                If Not TypeOf ex Is apiResponse Then Throw (ex)
                With TryCast(ex, apiResponse)
                    log.LogData.AppendFormat("{0}: {1}", .response, .Message)
                    For Each msg As apiError In .msgs
                        log.LogData.AppendFormat("  Ln {0}: {1}", msg.Line, msg.message)
                        If Not msg.Loaded Then
                            Throw New Exception(.Message)
                        End If
                    Next

                End With

            End Using

        End With

    End Sub

End Class