Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Configuration
Imports System.Xml
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage

Public MustInherit Class EndPoint : Implements IDisposable

    Public MustOverride Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)

#Region "Public Properties"

    Private _EndPoint As String
    Public Property Name As String
        Get
            Return _EndPoint
        End Get
        Set(value As String)
            _EndPoint = value
        End Set
    End Property

    ReadOnly Property EndpointRequest As String
        Get
            Try
                Return Split(HttpContext.Current.Request.RawUrl.Split("?")(0), "/").Last
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Private _log As oMsgLog
    Public Property log As oMsgLog
        Get
            Return _log
        End Get
        Set(value As oMsgLog)
            _log = value
        End Set
    End Property

    Private _msgFactory As msgFactory
    Public Property msgfactory As msgFactory
        Get
            Return _msgFactory
        End Get
        Set(value As msgFactory)
            _msgFactory = value
        End Set
    End Property

    Private _BubbleID As String = System.Guid.NewGuid.ToString
    ''' <summary>
    ''' The transaction ID. Returned as a host header to client
    ''' </summary>
    ''' <returns></returns>
    Public Property BubbleID As String
        Get
            Return _BubbleID
        End Get
        Set(value As String)
            _BubbleID = value
        End Set
    End Property

    ''' <summary>
    ''' The requested Priority environment
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property requestEnv As String
        Get
            Try
                Return HttpContext.Current.Request("environment")
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

#End Region

#Region "DB Connection"

    Private ReadOnly Property PriorityDBConnection
        Get
            Try
                Return WebConfigurationManager.ConnectionStrings("priority").ConnectionString

            Catch
                Throw New Exception("The service is not configured.")

            End Try

        End Get
    End Property

    Private _cn As SqlConnection = Nothing
    Public Property dbConnection As SqlConnection
        Get
            Try
                If _cn Is Nothing Then Throw New Exception("Is nothing")
                If _cn.State = ConnectionState.Closed Then Throw New Exception("Not connected")

            Catch ex As Exception
                log.LogData.AppendFormat("Opening datasource: {0}", PriorityDBConnection).AppendLine()
                _cn = New SqlConnection()
                With _cn
                    .ConnectionString = PriorityDBConnection
                    .Open()
                End With

            End Try
            Return _cn

        End Get
        Set(value As SqlConnection)
            _cn = value
        End Set
    End Property

#End Region

#Region "Execute SQL"

#Region "Logging"

    Friend Function LogStartSQL(Method As String, ByRef Statement As String) As Date
        log.LogData.AppendFormat("Executing {0}:", Method.ToUpper).AppendLine.Append(Statement).AppendLine().AppendLine()
        Return Now
    End Function

    Friend Sub LogEndSQL(starttime As Date)
        log.LogData.AppendFormat(
            "Completed in {0} seconds.",
            (Now - starttime).ToString.Replace("00:", "")
        ).AppendLine()
    End Sub

    Friend Sub newSQLLog()
        With HttpContext.Current
            .Response.AddHeader("BubbleID", BubbleID)
            Dim command As New SqlCommand(
            String.Format(
                My.Resources.log,
                BubbleID,
                requestEnv,
                .Request.HttpMethod,
                EndpointRequest
            ), dbConnection)
            command.ExecuteNonQuery()
        End With
    End Sub

    Friend Sub setLog(log As oMsgLog)
        Dim logstr As String = log.LogData.ToString _
        .Replace(Chr(10), "") _
        .Replace(vbTab, "   ") _
        .Replace("'", "'+char(39)+'")
        Dim command As New SqlCommand(
            String.Format(
                My.Resources.setLog,
                BubbleID,
                logstr,
                CInt(log.EntryType)
            ), dbConnection)
        command.ExecuteNonQuery()
    End Sub

    Friend Sub setXml(x As String)
        Dim command As New SqlCommand(
            String.Format(
                My.Resources.setXML,
                BubbleID,
                x.Replace("'", "'+char(39)+'")
            ), dbConnection)
        command.ExecuteNonQuery()
    End Sub

    ''' <summary>
    ''' Set a key value of the saved XML.
    ''' Duplicates keys throw an error
    ''' unless optional Overwrite is True.
    ''' </summary>
    ''' <param name="x">The key value from the foreign system</param>
    ''' <param name="Overwrite">Associate this log with saved key</param>
    Public Sub setKey(x As String, Optional Overwrite As Boolean = False)
        Try
            Dim command As New SqlCommand(
                String.Format(
                    My.Resources.setfKey,
                    BubbleID,
                    x
                ), dbConnection)
            command.ExecuteNonQuery()

        Catch ex As SqlException
            If ex.Number = 2601 Then
                If Not Overwrite Then _
                    Throw New Exception(
                        String.Format(
                            "Duplicate foreign key: '{0}'",
                            x
                        )
                    )

                Dim command As New SqlCommand(
                String.Format(
                    My.Resources.setfKeyReplace,
                    BubbleID,
                    x
                ), dbConnection)
                command.ExecuteNonQuery()

            Else
                Throw (ex)
            End If

        Catch ex As Exception
            Throw (ex)

        End Try

    End Sub

#End Region

#Region "Stringbuilder support"

    Public Function ExecuteReader(ByRef sqlString As Text.StringBuilder) As SqlClient.SqlDataReader
        Return ExecuteReader(sqlString.ToString)
    End Function

    Public Function ExecuteScalar(ByRef sqlString As Text.StringBuilder) As String
        Return ExecuteScalar(sqlString.ToString)
    End Function

    Public Function ExecuteXmlReader(ByRef sqlString As Text.StringBuilder) As XmlReader
        Return ExecuteXmlReader(sqlString.ToString)
    End Function

    Public Function ExecuteNonQuery(ByRef sqlString As Text.StringBuilder) As Integer
        Return ExecuteNonQuery(sqlString.ToString)
    End Function

#End Region

    ''' <summary>
    ''' A List of valid Priority Companies
    ''' </summary>
    ''' <returns>List(Of String)</returns>
    Public Function Environments() As List(Of String)
        Dim ret As New List(Of String)
        Dim command As New SqlCommand(
            "use system; " &
            "select DNAME from ENVIRONMENT where DNAME <> ''",
            dbConnection
        )
        Dim rs As SqlDataReader = command.ExecuteReader
        While rs.Read
            ret.Add(rs("DNAME"))
        End While
        rs.Close()
        Return ret
    End Function

    Public Function ExecuteReader(ByRef sqlString As String) As SqlClient.SqlDataReader
        Dim ret As SqlDataReader
        Dim command As New SqlCommand(sqlString.ToString, dbConnection)
            Dim start As Date = LogStartSQL("Reader", sqlString)
            ret = command.ExecuteReader
            LogEndSQL(start)
        Return ret
    End Function

    Public Function ExecuteScalar(ByRef sqlString As String) As String
        Dim ret As String
        Dim command As New SqlCommand(sqlString, dbConnection)
        Dim start As Date = LogStartSQL("Scalar", sqlString)
        ret = command.ExecuteScalar.ToString
        LogEndSQL(start)
        Return ret
    End Function

    Public Function ExecuteXmlReader(ByRef sqlString As String) As XmlReader
        Dim ret As XmlReader
        Dim command As New SqlCommand(sqlString, dbConnection)
        Dim start As Date = LogStartSQL("XmlReader", sqlString)
        ret = command.ExecuteXmlReader
        LogEndSQL(start)
        Return ret
    End Function

    Public Function ExecuteNonQuery(ByRef sqlString As String) As Integer
        Dim ret As Integer
        Dim command As New SqlCommand(sqlString, dbConnection)
        Dim start As Date = LogStartSQL("NonQuery", sqlString)
        ret = command.ExecuteNonQuery
        LogEndSQL(start)
        Return ret
    End Function

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                _cn.Close()
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

