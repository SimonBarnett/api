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

    Public MustOverride ReadOnly Property Query As String

#End Region

#Region "Process Request"

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
                                context.GetSection("appSettings")("Environment"),
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

        Dim GETRequest As New GetParams()
        Dim sqlString As String = Query

        ' --- 20/04/2013 - si
        ' --- Impliment SQL parameters from the request string.

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
                    "declare.*@{0}",
                    k
                ),
                RegexOptions.IgnoreCase
            )
            ' Check the .SQL for matching DECLARE statements
            If declaration.IsMatch(sqlString) Then

                ' Get the parameter name from the DECLARE statement
                Dim MatchVal As String = declaration.Match(sqlString).Value
                Dim VarName As String = Trim(
                    MatchVal.Substring(
                        MatchVal.IndexOf("@")
                    )
                )

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
                            "set {0} = '{1}'",
                            VarName,
                            GETRequest(k)
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

End Class
