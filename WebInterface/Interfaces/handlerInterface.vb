Imports System.Web
Imports PriPROC6.Interface.Message
Imports PriPROC6.svcMessage

Public Enum eHandlerStyle
    xml
    stream
End Enum

Public Interface xmlHandler
    Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory)
    Sub SetMeta(ByRef Metadata As xmlHandlerProps)

End Interface

Public Interface xmlHandlerProps
    ReadOnly Property EndPoint As String
    ReadOnly Property HandlerStyle As eHandlerStyle

End Interface
