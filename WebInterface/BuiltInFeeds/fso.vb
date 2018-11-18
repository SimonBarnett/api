Imports System.ComponentModel.Composition
Imports System.IO

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "fso")>
<ExportMetadata("Hidden", True)>
Public Class fsoFeed : Inherits iFeed : Implements xmlFeed

    Public Overrides Function Query() As String

        Using SR As New StreamReader(sqlFile.FullName)
            Return SR.ReadToEnd

        End Using

    End Function

End Class

