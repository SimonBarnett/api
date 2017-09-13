Imports System.Web
Imports System.Web.Configuration

Public Class priForm : Implements IDisposable

    Private _parent As priForm = Nothing

    Private _name As String
    Private _columns As New List(Of String)
    Private _Subforms As New Dictionary(Of String, priForm)

    Private _forms As List(Of String)
    Private _rowid As Integer = 0
    Private _rows As Data

#Region "Constructor"

    Sub New(Name As String, ParamArray Columns() As String)
        _name = Name
        _rows = New Data(Me)
        _forms = New List(Of String)
        forms.Add(Name)
        For Each c As String In Columns
            _columns.Add(c)
        Next
    End Sub

#End Region

#Region "Public Properties"

    Public Property Parent As priForm
        Get
            Return _parent
        End Get
        Set(value As priForm)
            _parent = value
        End Set
    End Property

    Public ReadOnly Property myParent As priForm
        Get
            Dim f As priForm = Me
            While Not f.Parent Is Nothing
                f = f.Parent
            End While
            Return f
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property Subforms As Dictionary(Of String, priForm)
        Get
            Return _Subforms
        End Get
    End Property

    Public ReadOnly Property Columns As List(Of String)
        Get
            Return _columns
        End Get
    End Property

    ReadOnly Property oDataHost As Uri
        Get
            Try
                Return New Uri(WebConfigurationManager.AppSettings("oDataHost"))

            Catch ex As Exception
                Return New Uri("http://localhost:8080")

            End Try
        End Get
    End Property

#End Region

#Region "Parent only properties"

    Public Property forms As List(Of String)
        Get
            If Me.Parent Is Nothing Then
                Return _forms
            Else
                Throw New NotImplementedException
            End If
        End Get
        Set(value As List(Of String))
            If Me.Parent Is Nothing Then
                _forms = value
            Else
                Throw New NotImplementedException
            End If
        End Set
    End Property

    Public Property rowid As Integer
        Get
            If Me.Parent Is Nothing Then
                Return _rowid
            Else
                Throw New NotImplementedException
            End If
        End Get
        Set(value As Integer)
            If Me.Parent Is Nothing Then
                _rowid = value
            Else
                Throw New NotImplementedException
            End If
        End Set
    End Property

    Public Property rows As Data
        Get
            If Me.Parent Is Nothing Then
                Return _rows
            Else
                Throw New NotImplementedException
            End If
        End Get
        Set(value As Data)
            If Me.Parent Is Nothing Then
                _rows = value
            Else
                Throw New NotImplementedException
            End If
        End Set
    End Property

#End Region

#Region "Methods"

    Public Function AddForm(Name As String, ParamArray Columns() As String) As priForm
        If Not myParent.forms.Contains(Name) Then
            myParent.forms.Add(Name)
            _Subforms.Add(Name, New priForm(Name, Columns))
            _Subforms(Name).Parent = Me
            Return _Subforms(Name)

        Else
            Throw New Exception(String.Format("Form {0} may only be defined once per loading.", Name))

        End If

    End Function

    Public Function AddRow(ParamArray RowData() As String) As priRow

        rows.Add(New priRow(Me, RowData))
        Return rows.Last

    End Function

    Public Function AddRow(Parent As priRow, ParamArray RowData() As String) As priRow

        If Parent.SubForms.Keys.Contains(Me.Name) Then
            Parent.Add(New priRow(Me, RowData))
            Return Parent.Last

        Else
            Throw New Exception(
                String.Format(
                    "Cannot add row of type '{0}' to row of type '{1}'.",
                    Name,
                    Parent.FormName
                )
            )

        End If

    End Function

#Region "Posting"

    Public Function Post(ByRef ex As Exception, ByVal Env As String, Optional Port As Integer = 8080) As Boolean
        Dim uploadRequest As Net.HttpWebRequest = CType(
        Net.HttpWebRequest.Create(
            String.Format(
                "http://localhost:{0}/{1}",
                Port.ToString,
                Env
            )
        ),
        Net.HttpWebRequest
    )
        Return rows.Post(ex, uploadRequest)
    End Function

    Public Function Post(ByRef ex As Exception, ByVal host As Uri) As Boolean

        Dim prot As String
        Select Case host.Scheme
            Case Uri.UriSchemeHttp
                prot = "http"
            Case Uri.UriSchemeHttps
                prot = "https"
            Case Else
                Throw New UriFormatException
        End Select

        Dim uploadRequest As Net.HttpWebRequest = CType(
            Net.HttpWebRequest.Create(
                String.Format(
                    "{0}://{1}:{2}/{3}",
                    prot,
                    host.Host,
                    host.Port.ToString(),
                    Split(host.AbsolutePath, "/")(1)
                )
            ),
            Net.HttpWebRequest
        )
        Return rows.Post(ex, uploadRequest)

    End Function

    Public Function Post(ByRef ex As Exception) As Boolean
        Dim host As Uri = oDataHost
        Dim prot As String
        Select Case host.Scheme
            Case Uri.UriSchemeHttp
                prot = "http"
            Case Uri.UriSchemeHttps
                prot = "https"
            Case Else
                Throw New UriFormatException
        End Select

        Dim uploadRequest As Net.HttpWebRequest = CType(
            Net.HttpWebRequest.Create(
                String.Format(
                    "{0}://{1}:{2}/{3}",
                    prot,
                    host.Host,
                    host.Port.ToString(),
                    HttpContext.Current.Request("environment")
                )
            ),
            Net.HttpWebRequest
        )
        Return rows.Post(ex, uploadRequest)

    End Function

    Public Overrides Function toString() As String
        Return rows.toString()

    End Function

#End Region

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                'rows.Clear()
                'rows = New Data
                'forms = New List(Of String)
                'rowid = 0
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class