<h1>IIS API</h1>

<img src="https://github.com/SimonBarnett/api/blob/master/templates.png">

Extensible XML API using MEF modules.

<h2>Installation</h2>
Install re-write module:
https://www.iis.net/downloads/microsoft/url-rewrite

Update and add VIEW/PATCH REST actions to .ashx handlers in:
C:\Windows\System32\inetsrv\config\applicationHost.config

Copy the <a href="https://github.com/SimonBarnett/api/tree/master/web">web folder</a> to c:\inetpub\api
Add an application to the Priority website with c:\inetpub\api as the root directory.
Set the identity of the API application pool to NetworkService

<h3>In MSSQL:</h3>
Add the login NT AUTHORITY\NETWORK SERVICE to the Priority database

<h3>In IIS:</h3>
-Set the server\instance of the Priority database connection in iis "connection strings"
-optionaly set the host/port of the NodeJS service in "application settings"

<h3>To install .net templates</h3>
Copy endpoint templates from <a href="https://localhost/api/templates">the template folder</a> to:
\My Documents\Visual Studio Version\Templates\ItemTemplates\Language\