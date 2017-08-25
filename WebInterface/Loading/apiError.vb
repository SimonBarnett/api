Imports System.Xml
Imports Newtonsoft.Json.Linq

Public Class apiResponse : Inherits Exception

#Region "Constructor"

    Public Sub New(json As JObject)
        _response = json.SelectToken("apiResponse").SelectToken("response")
        _message = json.SelectToken("apiResponse").SelectToken("message")

        For Each m As JObject In json.SelectToken("apiResponse").SelectToken("msgs")
            msgs.Add(New apiError(m))
        Next

    End Sub

#End Region

#Region "Methods"

    Public Sub toXML(ByRef w As XmlTextWriter)
        With w
            .WriteStartElement("apiresult")
            .WriteAttributeString("status", CStr(Me.response))
            .WriteAttributeString("message", Me.message)

            For Each m As apiError In Me.msgs
                m.toXML(w)
            Next

            .WriteEndElement() 'End Settings 

        End With
    End Sub

#End Region

#Region "Properties"

    Private _response As Integer
    Public Property response As Integer
        Get
            Return _response
        End Get
        Set(value As Integer)
            _response = value
        End Set
    End Property

    Private _message As String
    Public Overrides ReadOnly Property message As String
        Get
            Return _message
        End Get
    End Property

    Private _msgs As New List(Of apiError)
    Public Property msgs As List(Of apiError)
        Get
            Return _msgs
        End Get
        Set(value As List(Of apiError))
            _msgs = value
        End Set
    End Property

#End Region

End Class

Public Class apiError

#Region "Constructor"
    Sub New(m As JObject)
        Try
            _Line = m.SelectToken("line")
            _Loaded = (m.SelectToken("loaded") = "Y")
            If Not Loaded Then
                _message = m.SelectToken("message")

            Else
                For Each i As JObject In m.SelectToken("resultKeys").Children
                    With _resultKeys
                        _resultKeys.Add(
                            i.Properties(0).Name,
                            DirectCast(
                                i.Properties(0).Value,
                                JValue
                            ).Value
                        )
                    End With
                Next

            End If

        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Properties"

    Private _Line As Integer
    Public ReadOnly Property Line As Integer
        Get
            Return _Line
        End Get
    End Property

    Private _Loaded As Boolean
    Public ReadOnly Property Loaded As Boolean
        Get
            Return _Loaded
        End Get
    End Property

    Private _message As String
    Public ReadOnly Property message As String
        Get
            Return _message
        End Get
    End Property

    Private _resultKeys As New Dictionary(Of String, String)
    Public ReadOnly Property resultKeys As Dictionary(Of String, String)
        Get
            Return _resultKeys
        End Get
    End Property

#End Region

#Region "Methods"
    Public Overloads Function toString() As String
        Dim str As New System.Text.StringBuilder
        str.AppendFormat("Ln {0}: {1}", _Line, _Loaded.ToString)
        If Loaded Then
            str.Append(" Keys: {")
            For Each k As String In _resultKeys.Keys
                str.AppendFormat("{0}='{1}'", k, _resultKeys(k))
                If Not _resultKeys.Last.Key = k Then str.Append(",")
            Next
            str.Append("}")
        Else
            str.AppendFormat(" : {0}", _message)
        End If
        Return str.ToString
    End Function

    Public Sub toXML(ByRef w As XmlTextWriter)
        With w
            .WriteStartElement("row")
            .WriteAttributeString("line", _Line)
            .WriteAttributeString("loaded", _Loaded.ToString)
            If Loaded Then
                For Each k As String In _resultKeys.Keys
                    .WriteStartElement("key")
                    .WriteAttributeString("name", k)
                    .WriteAttributeString("value", _resultKeys(k))
                    .WriteEndElement()
                Next

            Else
                .WriteAttributeString("msg", _message)

            End If

            .WriteEndElement()

        End With
    End Sub

#End Region

End Class