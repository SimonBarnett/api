Imports System.ComponentModel.Composition
Imports System.Web
Imports PriPROC6.Interface.Message
Imports PriPROC6.Interface.Web
Imports PriPROC6.svcMessage

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "log")>
Public Class logFeed : Inherits iFeed : Implements xmlFeed

#Region "Base Methods"

    Shadows Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlFeed.ProcessRequest
        MyBase.ProcessRequest(context, log, msgFactory)
    End Sub

    Shadows Sub SetMeta(ByRef Metadata As xmlFeedProps) Implements xmlFeed.SetMeta
        MyBase.SetMeta(Metadata)
    End Sub

    Shadows Sub Install(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlFeed.Install
        MyBase.Install(context, log, msgFactory)
    End Sub

#End Region

    Overrides Function Query(Optional view As String = Nothing) As String
        Select Case view
            Case Else
                Return My.Resources.Query

        End Select

    End Function

    Overrides Function InstallQuery() As String
        Return My.Resources.Install

    End Function

End Class
