Imports PriPROC6.Interface.Web

Public Class orderLoading : Inherits priForm

    Private _OrderItems As priForm
    Public Property Orderitems As priForm
        Get
            Return _OrderItems
        End Get
        Set(value As priForm)
            _OrderItems = value
        End Set
    End Property

    Private _DeliveryAddress As priForm
    Public Property DeliveryAddress As priForm
        Get
            Return _DeliveryAddress
        End Get
        Set(value As priForm)
            _DeliveryAddress = value
        End Set
    End Property

    Sub New()
        MyBase.New("ORDERS", "CUSTNAME", "REFERENCE")
        _OrderItems = Me.AddForm("ORDERITEMS", "PARTNAME", "TQUANT")
        _DeliveryAddress = Me.AddForm("SHIPTO2", "ADDRESS", "ADDRESS2", "ADDRESS3",
                                    "STATENAME", "ZIP", "COUNTRYNAME"
                                )
    End Sub

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