Imports System.ComponentModel.Composition
Imports System.Web
Imports PriPROC6.Interface.Web

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "$projectname$")>
<ExportMetadata("Hidden", False)>
Public Class $projectname$ : Inherits iFeed : Implements xmlFeed

    ''' <summary>
    ''' Override the processing of the current context.
    ''' </summary>
    ''' <param name="context">The current HTTP context</param>
    Overrides Sub ProcessReq(ByVal context As HttpContext)

    End Sub

    ''' <summary>
    ''' The feed Query.
    ''' </summary>
    ''' <returns>The SQL Query string for this MEF feed.</returns>
    Overrides Function Query() As String
        Return My.Resources.Query

    End Function

    ''' <summary>
    ''' The sql command containing the dependant SQL objects
    ''' </summary>
    ''' <returns>The install QSL for this Feed</returns>
    Overrides Function InstallQuery() As String
        Return My.Resources.install

    End Function

End Class
