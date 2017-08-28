<h1>IIS API Endpoint - Feeds</h1>

<h2>Summary</h2>
<li>Feed end-points provide data to third party applications.
<li>The feed contains a FOR XML sql statement that provides data to the feed.
<li>The feed also provides an install statement, containing supporting SQL functions

This <a href="https://github.com/SimonBarnett/api/blob/master/webModules/feeds/TestFeed/TestFeed.vb">Example</a></h2> demonstrates a feed.

<h2>Endpoint installation</h2>
<li>Build the endpoint .dll file
<l1>Copy the .dll to the /bin folder of the API application
<li>PATCHing a feed endpoint will run the install in the selected company

<h2>MEF endpoint Definition</h2>
The endpoint definition contains:
<li>the endpoint type (either xmlHandler or xmlFeed)
<li>the name of the EndPoint 

```
<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "testfeed")>
Public Class TestFeed : Inherits iFeed : Implements xmlFeed
```
