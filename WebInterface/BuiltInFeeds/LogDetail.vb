Imports System.ComponentModel.Composition
Imports System.Web

Namespace Web.BuiltIn

    <Export(GetType(xmlFeed))>
    <ExportMetadata("EndPoint", "logdetail")>
    <ExportMetadata("Hidden", True)>
    Public Class LogDetailFeed : Inherits iFeed : Implements xmlFeed

        Overrides Function Query() As String
            Select Case HttpContext.Current.Request("view")
                Case "xml"
                    Return My.Resources.logXML

                Case Else
                    Return My.Resources.logDetail

            End Select

        End Function

    End Class

End Namespace