Imports System.ComponentModel.Composition

Namespace Web.BuiltIn

    <Export(GetType(xmlFeed))>
    <ExportMetadata("EndPoint", "dbo")>
    <ExportMetadata("Hidden", True)>
    Public Class dboFeed : Inherits iFeed : Implements xmlFeed


    End Class

End Namespace
