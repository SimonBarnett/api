<h1>Installation</h1>
Install the <a href="https://www.iis.net/downloads/microsoft/url-rewrite">MS URL re-write module</a>.<br><br>

Add VIEW/PATCH REST actions to .ashx handlers in:

```C:\Windows\System32\inetsrv\config\applicationHost.config```

Copy the <a href="https://github.com/SimonBarnett/api/tree/master/web">web folder</a> to 

```c:\inetpub\api```

<h2>In MSSQL:</h2>
<li>Add the login NT AUTHORITY\NETWORK SERVICE to the Priority database

<h2>In IIS:</h2>
<li>Add an application to the Priority website with c:\inetpub\api as the root directory.
<li>Set the identity of the API application pool to NetworkService
<li>Set the server\instance of the Priority database connection in iis "connection strings"
<li>Optionaly - set the host/port of the NodeJS service in "application settings"

<h2>To install .net templates</h2>
<li>Copy endpoint templates from <a href="https://localhost/api/templates">the template folder</a> to:

```\My Documents\Visual Studio Version\Templates\ItemTemplates\Language\```