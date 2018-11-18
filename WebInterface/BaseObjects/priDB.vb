'Imports System.Data.SqlClient
'Imports System.Web.Configuration
'Imports System.Xml
'Imports System.Web
'Imports PriPROC6.svcMessage

'Public Class priDB

'    Public Property log As oMsgLog
'        Get
'            Return TryCast(HttpContext.Current.Application("log"), oMsgLog)
'        End Get
'        Set(value As oMsgLog)
'            HttpContext.Current.Application("log") = value
'        End Set
'    End Property

'#Region "DB Connection"

'    Private ReadOnly Property PriorityDBConnection
'        Get
'            Try
'                Return WebConfigurationManager.ConnectionStrings("priority").ConnectionString

'            Catch
'                Throw New Exception("The service is not configured.")

'            End Try

'        End Get
'    End Property

'    Private _cn As SqlConnection = Nothing
'    Public Property dbConnection As SqlConnection
'        Get
'            Try
'                If _cn Is Nothing Then Throw New Exception("Is nothing")
'                If _cn.State = ConnectionState.Closed Then Throw New Exception("Not connected")

'            Catch ex As Exception
'                log.LogData.AppendFormat("Opening datasource: {0}", PriorityDBConnection).AppendLine()
'                _cn = New SqlConnection()
'                With _cn
'                    .ConnectionString = PriorityDBConnection
'                    .Open()
'                End With

'            End Try
'            Return _cn

'        End Get
'        Set(value As SqlConnection)
'            _cn = value
'        End Set
'    End Property

'#End Region

'#Region "Execute SQL"

'#Region "Stringbuilder support"

'    Public Function ExecuteReader(ByRef sqlString As Text.StringBuilder) As SqlClient.SqlDataReader
'        Return ExecuteReader(sqlString.ToString)
'    End Function

'    Public Function ExecuteScalar(ByRef sqlString As Text.StringBuilder) As String
'        Return ExecuteScalar(sqlString.ToString)
'    End Function

'    Public Function ExecuteXmlReader(ByRef sqlString As Text.StringBuilder) As XmlReader
'        Return ExecuteXmlReader(sqlString.ToString)
'    End Function

'    Public Function ExecuteNonQuery(ByRef sqlString As Text.StringBuilder) As Integer
'        Return ExecuteNonQuery(sqlString.ToString)
'    End Function

'#End Region

'    Friend Function LogStartSQL(Method As String, ByRef Statement As String) As Date
'        log.LogData.AppendFormat("Executing {0}:", Method.ToUpper).AppendLine.Append(Statement).AppendLine().AppendLine()
'        Return Now
'    End Function

'    Friend Sub LogEndSQL(starttime As Date)
'        log.LogData.AppendFormat(
'            "Completed in {0} seconds.",
'            (Now - starttime).ToString.Replace("00:", "")
'        ).AppendLine()
'    End Sub

'    ''' <summary>
'    ''' A List of valid Priority Companies
'    ''' </summary>
'    ''' <returns>List(Of String)</returns>
'    Public Function Environments() As List(Of String)
'        Dim ret As New List(Of String)
'        Dim command As New SqlCommand(
'            "use system; " &
'            "select DNAME from ENVIRONMENT where DNAME <> ''",
'            dbConnection
'        )
'        Dim rs As SqlDataReader = command.ExecuteReader
'        While rs.Read
'            ret.Add(rs("DNAME"))
'        End While
'        rs.Close()
'        Return ret
'    End Function

'    Public Function ExecuteReader(ByRef sqlString As String) As SqlClient.SqlDataReader
'        Dim ret As SqlDataReader
'        Dim command As New SqlCommand(sqlString.ToString, dbConnection)
'        Dim start As Date = LogStartSQL("Reader", sqlString)
'        ret = command.ExecuteReader
'        LogEndSQL(start)
'        Return ret
'    End Function

'    Public Function ExecuteScalar(ByRef sqlString As String) As String
'        Dim ret As String
'        Dim command As New SqlCommand(sqlString, dbConnection)
'        Dim start As Date = LogStartSQL("Scalar", sqlString)
'        Try
'            ret = command.ExecuteScalar.ToString
'        Catch
'            ret = ""
'        End Try
'        LogEndSQL(start)
'        Return ret
'    End Function

'    Public Function ExecuteXmlReader(ByRef sqlString As String) As XmlReader
'        Dim ret As XmlReader
'        Dim command As New SqlCommand(sqlString, dbConnection)
'        Dim start As Date = LogStartSQL("XmlReader", sqlString)
'        ret = command.ExecuteXmlReader
'        LogEndSQL(start)
'        Return ret
'    End Function

'    Public Function ExecuteNonQuery(ByRef sqlString As String) As Integer
'        Dim ret As Integer
'        Dim command As New SqlCommand(sqlString, dbConnection)
'        Dim start As Date = LogStartSQL("NonQuery", sqlString)
'        ret = command.ExecuteNonQuery
'        LogEndSQL(start)
'        Return ret
'    End Function

'#End Region

'End Class
