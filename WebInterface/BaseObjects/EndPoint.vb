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

    Private ReadOnly Property dbConnection As SqlConnection
        Get
            log.LogData.AppendFormat("Opening datasource: {0}", PriorityDBConnection).AppendLine()
            Dim _connection As New SqlConnection()
            With _connection
                .ConnectionString = PriorityDBConnection
                .Open()
            End With
            Return _connection
        End Get
    End Property

#End Region

#Region "Log SQL commands"

    Public Function LogStartSQL(Method As String, ByRef Statement As String) As Date
        log.LogData.AppendFormat("Executing {0}:", Method.ToUpper).AppendLine.Append(Statement).AppendLine().AppendLine()
        Return Now
    End Function

    Public Sub LogEndSQL(starttime As Date)
        log.LogData.AppendFormat(
            "Completed in {0} seconds.",
            (Now - starttime).ToString.Replace("00:", "")
        ).AppendLine()
    End Sub

#End Region

#Region "Execute SQL"

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

