Imports System.ComponentModel.Composition

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "log")>
<ExportMetadata("Hidden", True)>
Public Class logFeed : Inherits iFeed : Implements xmlFeed

    Overrides Function Query() As String
        Return My.Resources.Query

    End Function

End Class
