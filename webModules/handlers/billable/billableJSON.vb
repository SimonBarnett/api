Public Class billableJSON

    Private _name As String
    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Private _line As String
    Public Property line As String
        Get
            Return _line
        End Get
        Set(value As String)
            _line = value
        End Set
    End Property

    Private _wbs As String
    Public Property wbs As String
        Get
            Return _wbs
        End Get
        Set(value As String)
            _wbs = value
        End Set
    End Property

    Private _doc As String
    Public Property doc As String
        Get
            Return _doc
        End Get
        Set(value As String)
            _doc = value
        End Set
    End Property

    Private _order As String
    Public Property order As String
        Get
            Return _order
        End Get
        Set(value As String)
            _order = value
        End Set
    End Property

    Private _type As String
    Public Property type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    Private _append As New List(Of itemJSON)
    Public Property append As List(Of itemJSON)
        Get
            Return _append
        End Get
        Set(value As List(Of itemJSON))
            _append = value
        End Set
    End Property

    Public Sub cascade()
        If Not line Is Nothing Then
            For Each i As itemJSON In append
                If i.line Is Nothing Then
                    i.line = line
                End If
            Next
        End If

        If Not wbs Is Nothing Then
            For Each i As itemJSON In append
                If i.wbs Is Nothing Then
                    i.wbs = wbs
                End If
            Next
        End If
    End Sub

End Class

Public Class itemJSON

    Private _part As String
    Public Property part As String
        Get
            Return _part
        End Get
        Set(value As String)
            _part = value
        End Set
    End Property

    Private _desc As String
    Public Property desc As String
        Get
            Return _desc
        End Get
        Set(value As String)
            _desc = value
        End Set
    End Property

    Private _wbs As String
    Public Property wbs As String
        Get
            Return _wbs
        End Get
        Set(value As String)
            _wbs = value
        End Set
    End Property

    Private _qty As String = Nothing
    Public Property qty As String
        Get
            If _qty Is Nothing Then
                Return Nothing
            Else
                Return String.Format("#{0}", _qty)
            End If
        End Get
        Set(value As String)
            _qty = value
        End Set
    End Property

    Private _line As String = Nothing
    Public Property line As String
        Get
            If _line Is Nothing Then
                Return Nothing
            Else
                Return String.Format("#{0}", _line)
            End If

        End Get
        Set(value As String)
            _line = value
        End Set
    End Property

End Class
