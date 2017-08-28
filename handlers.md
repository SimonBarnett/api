<h1>IIS API Endpoint - Handlers</h1>

<b>Note:</b> Handler endpoints require the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a> to load data into Priority. </h2>

<h2>Summary.</h2>
<li>Handler end-points receive data from third party applications. 
<li>Handlers can contain XSD files to validate received data
<li>A handler builds a loading with the <a href="https://github.com/SimonBarnett/apiLoad/tree/master/apiLoad">.Net API Load Library</a>
<li>The loading is processed by the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a>.

This <a href="https://github.com/SimonBarnett/api/blob/master/webModules/handlers/Testhandler/TestHandler.vb">example</a> uses the <a href="https://github.com/SimonBarnett/apiLoad/tree/master/apiLoad">.Net API Load Library</a> to load data into Priority.

<h2>Endpoint installation</h2>
<li>Build the endpoint .dll file
<l1>Copy the .dll to the /bin folder of the API application

<h2>MEF endpoint Definition</h2>
The endpoint definition contains:
<li>the endpoint type (either xmlHandler or xmlFeed)
<li>the name of the EndPoint 
<li>the HandlerStyle (either xml or stream)

```
<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "testhandler")>
<ExportMetadata("HandlerStyle", eHandlerStyle.xml)>
Public Class TestHandler : Inherits iHandler : Implements xmlHandler
```

To process XML data set the "HandlerStyle" to eHandlerStyle.xml and override the XMLHandler method.

```
Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)
```

To process non-XML data set the "HandlerStyle" to eHandlerStyle.stream and override the StreamHandler method.

```
Public Overrides Sub StreamHandler(ByRef w As XmlTextWriter, ByRef Request As StreamReader)
```
  
<h2>Posting data to the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a></h2>

Defining a Priority form and its columns:

```Visual Basic
Dim form As New priForm("{FORMNAME}", "{COLUMN1}", "{COLUMN2}", ...)
```

Add Subforms to the definition:

```Visual Basic
Dim subform = form.AddForm("{SUBFORMNAME}", "{COLUMN1}", "{COLUMN2}", ...)
```

Adding rows to the loading:

```Visual Basic
Dim r As priRow = form.AddRow("{COLUMN1_DATA}", "{COLUMN2_DATA}", ...)
```

Posting the loading to the ://server/{priority_company}:

```Visual Basic
Dim ex As Exception = Nothing
Post(ex, New Uri("http://localhost:8080/demo"))
If Not TypeOf ex Is apiResponse Then Throw (ex)
```

Handling the server <a href="https://github.com/SimonBarnett/apiLoad/blob/master/apiLoad/apiError.vb">response</a>:

```
With TryCast(ex, apiResponse)
    Console.WriteLine("{0}: {1}", .response, .message)
    For Each msg As apiError In .msgs		
        Console.WriteLine("  Ln {0}: {1}", msg.Line, msg.Loaded.ToString)
		If Loaded Then        
            For Each k As String In msg.resultKeys.Keys
                ' Contains key namepairs
            Next            
        Else
            ' Contains msg.message
        End If
    Next
End With
```