Imports PriPROC6.Interface.Web

Public Class orderLoading : Inherits priForm

    Private _OrderItems As OrderOrderItems
    Public Property Orderitems As OrderOrderItems
        Get
            Return _OrderItems
        End Get
        Set(value As OrderOrderItems)
            _OrderItems = value
        End Set
    End Property

    Private _DeliveryAddress As OrderDeliveryAddress
    Public Property DeliveryAddress As OrderDeliveryAddress
        Get
            Return _DeliveryAddress
        End Get
        Set(value As OrderDeliveryAddress)
            _DeliveryAddress = value
        End Set
    End Property

    Sub New()
        MyBase.New("ORDERS", "CUSTNAME", "REFERENCE")
        _OrderItems = New OrderOrderItems(Me) 'Me.AddForm("ORDERITEMS", "PARTNAME", "TQUANT")
        _DeliveryAddress = New OrderDeliveryAddress(Me)

    End Sub

    Overloads Function addRow(CUSTNAME As String, REFERENCE As String) As priRow
        Return MyBase.AddRow(CUSTNAME, REFERENCE)

    End Function

End Class


Public Class OrderOrderItems

    Private _this As priForm
    Sub New(ByRef parent As priForm)
        _this = parent.AddForm("ORDERITEMS", "PARTNAME", "TQUANT")

    End Sub

    Function addRow(ByRef o As priRow, PARTNAME As String, TQUANT As String) As priRow
        Return _this.AddRow(o, PARTNAME, TQUANT)

    End Function

End Class

Public Class OrderDeliveryAddress
    Private _this As priForm
    Sub New(ByRef parent As priForm)
        _this = parent.AddForm(
            "SHIPTO2", "ADDRESS", "ADDRESS2", "ADDRESS3",
            "STATENAME", "ZIP", "COUNTRYNAME"
        )

    End Sub

    Function addRow(ByRef o As priRow, ADDRESS As String, ADDRESS2 As String, ADDRESS3 As String, STATENAME As String, ZIP As String, COUNTRYNAME As String) As priRow
        Return _this.AddRow(o, ADDRESS, ADDRESS2, ADDRESS3, STATENAME, ZIP, COUNTRYNAME)

    End Function

End Class

Public Class paymentLoading : Inherits priForm

    Private _TPAYMENT2 As priForm
    Public Property TPAYMENT2 As priForm
        Get
            Return _TPAYMENT2
        End Get
        Set(value As priForm)
            _TPAYMENT2 = value
        End Set

    End Property

    Sub New()
        MyBase.New("TINVOICES", "ACCNAME", "REFERENCE")
        _TPAYMENT2 = Me.AddForm("TPAYMENT2", "PAYMENTCODE", "QPRICE")

    End Sub

End Class

Public Class CustomerLoading : Inherits priForm
    Sub New()
        MyBase.New(
            "CUSTOMERS", "CUSTNAME", "CUSTDES",
            "PHONE", "EMAIL",
            "ADDRESS", "ADDRESS2", "ADDRESS3",
            "STATENAME", "ZIP", "COUNTRYNAME"
           )

    End Sub

End Class