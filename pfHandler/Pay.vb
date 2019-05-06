Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO
Imports Newtonsoft.Json

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "Pay")>
<ExportMetadata("Hidden", False)>
Public Class Pay : Inherits iHandler : Implements xmlHandler

    ''' <summary>
    ''' Overrides XML handler with a StreamReader for business object parsing.
    ''' </summary>
    ''' <param name="w"></param>
    ''' <param name="Request"></param>
    Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)
        'MyBase.StreamHandler(w, Request)

    End Sub



End Class