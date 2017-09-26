Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Xml
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage

Public MustInherit Class iFeed : Inherits EndPoint

#Region "Metadata"

    Public Sub SetMeta(ByRef Metadata As xmlFeedProps)
        With Metadata
            Name = .EndPoint
        End With
    End Sub

#End Region

#Region "Overridable stubs"

    ''' <summary>
    ''' The sql query that will be executed.
    ''' Parameters are caputured from the GET request.
    ''' </summary>
    ''' <param name="View">Optional view GET parameter</param>
    ''' <returns></returns>
    Public MustOverride Function Query(Optional View As String = Nothing) As String

    ''' <summary>
    ''' Any SQL CREATEs for functions used by the query.
    ''' PATCHing the MEF feed executes the CREATE in the selected environment.
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Function InstallQuery() As String

#End Region

#Region "Process Request"

    Public Sub Install(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)
        MyBase.log = log
        MyBase.msgfactory = msgFactory

        With log.LogData
            .AppendFormat("Installing SQL from {0}.", Name).AppendLine()
            For Each statement As String In Split(InstallQuery, vbCrLf & "go" & vbCrLf,, CompareMethod.Text)
                ExecuteNonQuery(String.Format("use {0}; {1}", requestEnv, statement))
            Next

        End With

    End Sub

    Public Overrides Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)

        MyBase.log = log
        MyBase.msgfactory = msgFactory

        With context.Response
            .Clear()
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8
        End With

        Using objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
            With objX
                .WriteStartDocument()
                Try
                    objX.WriteNode(
                        ExecuteXmlReader(
                            String.Format(
                                "use {0}; {1}",
                                context.Request("environment"),
                                Statement
                            )
                        ),
                        True
                    )

                Catch ex As Exception
                    log.setException(ex)
                    .WriteStartElement("ERROR")
                    .WriteStartAttribute("MESSAGE")
                    .WriteValue(ex.Message)
                    .WriteEndAttribute()
                    .WriteEndElement()

                Finally
                    .WriteEndDocument()
                    .Flush()
                    .Close()

                End Try

            End With

        End Using

    End Sub

    Private Function Statement()

        Dim sqlString As String
        Dim GETRequest As New GetParams()

        If GETRequest.Keys.Contains("view") Then
            sqlString = Query(GETRequest("view").ToLower)
        Else
            sqlString = Query()
        End If

        ' --- 20/04/2013 - si
        ' --- Impliment SQL parameters from the request string.

        ' --- 09/03/2017 - si
        ' --- Added support for non-character data types in request.

        ' Check the .sql for mandatory fields
        Dim Mandatory As Regex = New Regex(
            "declare.*@.*--.*mandatory",
            RegexOptions.IgnoreCase
        )
        ' Iterate through the mandatory fields
        For Each m As Match In Mandatory.Matches(sqlString)
            Dim expectedVar As String =
                Trim(m.Value.Substring(m.Value.IndexOf("@") + 1).Split(" ")(0))

            ' If mandatory parameter is missing then throw an error
            If Not GETRequest.Keys.Contains(expectedVar) Then
                Throw New Exception(
                    String.Format(
                        "The {0} parameter is mandatory.",
                        expectedVar
                    )
                )
            ElseIf GETRequest(expectedVar).Length = 0 Then
                Throw New Exception(
                    String.Format(
                        "The {0} parameter is mandatory.",
                        expectedVar
                    )
                )
            End If
        Next

        ' Iterate through the parameters in the request string
        For Each k As String In GETRequest.Keys
            Dim declaration As Regex = New Regex(
                String.Format(
                    "declare.*@{0}.*",
                    k
                ),
                RegexOptions.IgnoreCase
            )
            ' Check the .SQL for matching DECLARE statements
            If declaration.IsMatch(sqlString) Then

                ' Get the parameter name from the DECLARE statement
                Dim MatchVal As String = declaration.Match(sqlString).Value
                Dim VarName As String = Trim(
                    MatchVal.Substring(MatchVal.IndexOf("@")).Split(" ")(0)
                )

                ' Get the type of the variable
                Dim vType As Regex = New Regex(
                    "[A-Za-z]+"
                )
                Dim VarType As String = vType.Matches(Split(MatchVal, VarName)(1))(0).Value.ToLower

                ' Find the SET statement
                Dim SetStatement As Regex = New Regex(
                    String.Format(
                        "set.*@{0} .*=.*",
                        k
                    ),
                    RegexOptions.IgnoreCase
                )

                ' Find the IN statement
                Dim inStatement As Regex = New Regex(
                    String.Format(
                        "in.*\(.*@{0}.*?\)",
                        k
                    ),
                    RegexOptions.IgnoreCase
                )

                ' Find the Count statement
                Dim CountStatement As Regex = New Regex(
                    String.Format(
                        "set.*@{0}count.*=.*[0-9]+",
                        k
                    ),
                    RegexOptions.IgnoreCase
                )

                If SetStatement.Match(sqlString).Success Then
                    ' Update the SET statement for this parameter
                    sqlString = SetStatement.Replace(
                        sqlString,
                        String.Format(
                            "set {0} = {1}",
                            VarName,
                            Apostrophe(GETRequest(k), VarType)
                        )
                    )

                ElseIf inStatement.Match(sqlString).Success Then
                    sqlString = inStatement.Replace(
                        sqlString,
                        String.Format(
                            "in ({0})",
                            String.Format(
                                "'{0}'",
                                GETRequest(k).Replace(",", "', '")
                            )
                        )
                    )
                    sqlString = CountStatement.Replace(
                        sqlString,
                        String.Format(
                            "set @{0}count = {1}",
                            k,
                            UBound(GETRequest(k).Split(",")) + 1
                        )
                    )

                End If
            End If
        Next
        Return sqlString
    End Function

#End Region

#Region "Private Functions"

    Function Apostrophe(Value As String, vType As String)
        Select Case vType
            Case "char", "varchar", "text", "nchar", "nvarchar", "ntext"
                Return String.Format("'{0}'", Value)

            Case Else
                Return String.Format("{0}", Value)

        End Select

    End Function

#End Region

End Class
