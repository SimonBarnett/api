Imports System.Web
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Xml
Imports System.Text

Public Class ViewTree

    Public Sub Refresh(ByRef tree As TreeView)
        Dim Ex As Exception = Nothing
        Dim ret As XmlDocument = Post(Ex)
        If Not Ex Is Nothing Then Throw (Ex)

        Dim api As TreeNode
        With tree
            api = NewTreeNode("root", ret.SelectSingleNode("api").Attributes("name").Value)
            If .Nodes.Contains(api) Then
                api = .Nodes().Item(0)
            End If
        End With

        With api
            .Text = ret.SelectSingleNode("api").Attributes("name").Value

        End With

        'With api
        '    For Each env As XmlNode In ret.SelectSingleNode("api").SelectNodes("env")
        '        Dim envNode As New TreeNode
        '        With envNode
        '            .Text = env.Attributes("name").Value

        '            Dim feeds As New TreeNode
        '            With feeds
        '                .Text = "Feeds"
        '                For Each feed As XmlNode In env.SelectNodes("feed")
        '                    Dim fd As New TreeNode
        '                    With fd
        '                        .Text = feed.Attributes("name").Value
        '                    End With
        '                    .ChildNodes.Add(fd)
        '                Next
        '            End With
        '            .ChildNodes.Add(feeds)

        '            Dim handlers As New TreeNode
        '            With handlers
        '                .Text = "Handlers"

        '                For Each hndlr As XmlNode In env.SelectNodes("handler")
        '                    Dim hlr As New TreeNode
        '                    With hlr
        '                        .Text = hndlr.Attributes("name").Value
        '                    End With
        '                Next

        '            End With
        '            .ChildNodes.Add(handlers)

        '        End With

        '        .ChildNodes.Add(envNode)

        '    Next
        'End With

    End Sub

    Private ReadOnly Property url As String
        Get
            With HttpContext.Current.Request.Url
                Return String.Format(
                    "{0}/api",
                    Split(.ToString, "/api")(0)
                )
            End With
        End Get
    End Property

    Public Function Post(ByRef Ex As Exception) As XmlDocument
        Dim ret As New XmlDocument

        Dim uploadRequest As Net.HttpWebRequest = CType(
            Net.WebRequest.Create(url), Net.HttpWebRequest
        )

        Dim uploadResponse As Net.HttpWebResponse = Nothing
        Dim requestStream As Stream = Nothing
        Dim x As String

        Ex = Nothing

        Try
            uploadRequest.Timeout = 5 * 60 * 1000
            uploadRequest.Method = "VIEW"
            uploadRequest.Proxy = Nothing
            'uploadRequest.SendChunked = True

            uploadResponse = uploadRequest.GetResponse()

            Dim readstream As New StreamReader(uploadResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"))
            With readstream
                x = .ReadToEnd
                While Not (String.Compare(x.Substring(0, 1), "<") = 0 Or String.Compare(x.Substring(0, 1), "{") = 0)
                    x = x.Substring(1)
                End While
                If Not x.Length > 0 Then
                    Throw New Exception("Invalid data.")
                End If
            End With
            ret.LoadXml(x)

        Catch exep As UriFormatException
            Ex = New Exception(String.Format("Invalid URL: {0}", exep.Message))

        Catch exep As Net.WebException
            Ex = New Exception(String.Format("Connection Error: {0}", exep.Message))

        Catch exep As Exception
            Ex = New Exception(String.Format("Posting failed: {0}", exep.Message))

        Finally
            ' Clean up the streams
            If Not IsNothing(uploadResponse) Then
                uploadResponse.Close()
            End If
            If Not IsNothing(requestStream) Then
                requestStream.Close()
            End If
        End Try

        Return ret

    End Function

    Private Function NewTreeNode(ID As String, Text As String, Optional img As Image = Nothing) As TreeNode
        Dim ret As New TreeNode
        With ret
            .Value = ID
            .Text = Text
        End With
        Return ret

    End Function
End Class
