Imports PriPROC6.Loading

Public Class ldDef : Inherits Definition

    Sub New()
        With Me
            .Clear()
            .Table = "ZWEB_ORDERS"
            .Procedure = "ZWEB_LOADORDER"

            .AddColumn(1) = New LoadColumn("CUSTOMER_ID", tColumnType.typeCHAR)

        End With
    End Sub

End Class
