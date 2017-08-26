# api
XML API

Extensible XML API using MEF modules.

Install re-write module:
https://www.iis.net/downloads/microsoft/url-rewrite

Update and add VIEW/PATCH REST actions to .ashx handlers in:
C:\Windows\System32\inetsrv\config\applicationHost.config

Copy the folder https://github.com/SimonBarnett/api/tree/master/web to c:\inetpub\api
Add an application to the Priority website with c:\inetpub\api as the root directory.
Set the identity of the API application pool to NetworkService

Add the login NT AUTHORITY\NETWORK SERVICE to the Priority database 

Set the server\instance of the Priority database connection in iis "connection strings"

Copy templates to:
\My Documents\Visual Studio Version\Templates\ItemTemplates\Language\