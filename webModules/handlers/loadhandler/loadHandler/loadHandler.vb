Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports PriPROC6.svcMessage
Imports PriPROC6.PriSock
Imports PriPROC6.Interface.Message
Imports System.IO

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "loadhandler")>
<ExportMetadata("HandlerStyle", eHandlerStyle.stream)>
Public Class loadHandler : Inherits iHandler : Implements xmlHandler

#Region "Base Methods"

    Shadows Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlHandler.ProcessRequest
        MyBase.ProcessRequest(context, log, msgFactory)
    End Sub

    Shadows Sub SetMeta(ByRef Metadata As xmlHandlerProps) Implements xmlHandler.SetMeta
        MyBase.SetMeta(Metadata)
    End Sub

#End Region

    Public Overrides Sub StreamHandler(ByRef Request As StreamReader)
        Using cli As New iClient(
            HttpContext.Current.GetSection("appSettings")("Service"),
            HttpContext.Current.GetSection("appSettings")("loadport")
        )
            Dim resp = msgfactory.Decode(cli.Send(msgfactory.EncodeRequest("loading", New oMsgLoading(Request))))
            If Not resp.ErrCde = 200 Then
                Throw New Exception(resp.errMsg)
            End If

        End Using
    End Sub

End Class