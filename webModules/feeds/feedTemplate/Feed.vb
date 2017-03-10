Imports System.ComponentModel.Composition
Imports System.Web
Imports PriPROC6.Interface.Message
Imports PriPROC6.Interface.Web
Imports PriPROC6.svcMessage

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "FeedName")>
Public Class FeedName : Inherits iFeed : Implements xmlFeed

#Region "Base Methods"

    Shadows Sub ProcessRequest(ByRef context As HttpContext, ByRef log As oMsgLog, ByRef msgFactory As msgFactory) Implements xmlFeed.ProcessRequest
        MyBase.ProcessRequest(context, log, msgFactory)
    End Sub

    Shadows Sub SetMeta(ByRef Metadata As xmlFeedProps) Implements xmlFeed.SetMeta
        MyBase.SetMeta(Metadata)
    End Sub
    Overrides ReadOnly Property Query As String
        Get
            Return My.Resources.Query
        End Get
    End Property

#End Region

End Class
