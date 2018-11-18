Imports System.ComponentModel.Composition

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "dbo")>
<ExportMetadata("Hidden", True)>
Public Class dboFeed : Inherits iFeed : Implements xmlFeed


End Class

