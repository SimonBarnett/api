Imports System.ComponentModel.Composition
Imports PriPROC6.Interface.Web

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "testfeed")>
<ExportMetadata("Hidden", False)>
Public Class TestFeed : Inherits iFeed : Implements xmlFeed

    Overrides Function Query() As String
        Return My.Resources.query

    End Function

    Overrides Function InstallQuery() As String
        Return My.Resources.install

    End Function

End Class
