Imports System.Web
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage

Public Interface xmlFeed
    Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)
    Sub SetMeta(ByRef Metadata As xmlFeedProps)
    Sub Install(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)

End Interface

Public Interface xmlFeedProps
    ReadOnly Property EndPoint As String

End Interface


