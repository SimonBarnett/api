<h1>Installation</h1>
Install the <a href="https://www.iis.net/downloads/microsoft/url-rewrite">MS URL re-write module</a>.<br><br>
Scroll down to Download URL Rewrite Module 2.1 section and download relevant file.<br><br>
English: x86 or x64 dependant on the machine your installing on.<br><br>
Once download complete double click to execute and click run, accept license agreement and click install and then finish when it's done.<br><br>

Add VIEW/PATCH REST actions to .ashx handlers in:
```C:\Windows\System32\inetsrv\config\applicationHost.config```

Open file applicationHost.config with notepad.<br><br>
Find all lines with .ashx and add ',VIEW,PATCH' after DEBUG but before ".<br><br>

Copy the <a href="https://github.com/SimonBarnett/api/web.zip">web folder</a> to 

```c:\inetpub\api```

<h2>In MSSQL:</h2>
Add the login NT AUTHORITY\NETWORK SERVICE to the Priority database
<li>Open SMSS and log in with Admin Credentials.
<li>Expand Security then Logins, right click Logins and select New Login
<li>Click Search box next to Login Name and then click advanced, Find now.
<li>From the list created pick NETWORK SERVICE and Click OK twice.
<li>This should take you back to the Login - New screen.
<li>Select Server Roles and add sysadmin, now click OK?

<h2>In IIS:</h2>
Add an application to the Priority website with c:\inetpub\api as the root directory.
<li>Open IIS Manager, expand the tree until Default Web Site is displayed.
<li>This should list the Priority rich Installed files/folders. 
<li>Right click Application Pools select Add Application Pool

```Name = Api, 
.NET CLR Version = .NET CLR Version v4.0.30319
Managed pipeline mode = Integrated```

<li>Click OK


Set the identity of the API application pool to NetworkService
<li>Select Api in application pools and click on advanced settings on the right hand side menu
<li>Under section Process Model select item Identity click on box with three dots to change this to NetworkService
<li>click OK

Add an application to the Priority website with c:\inetpub\api as the root directory.
<li>Right click Default Web Site and select Add Application.

```Alias = API
Application pool = Api
Physical Path = C:\inetpub\API```

<li>Click OK

Set the server\instance of the Priority database connection in iis "connection strings"	
<li>Under Default Web Site select the API Application you just created and in the middle pane double click Connection Strings. 
<li>Select the priority string and click edit on the right hand menu. 
<li>In the Custom Box change the server name to the one you have locally.

<h2>Optionaly</h2> 
<li>set the host/port of the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a> in "application settings"

<h2>Setting up the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a></h2>
Handlers require the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a> to post oData to Priority. This should be installed seperately.

<h2>To install .net templates</h2>
<li>Copy endpoint templates from <a href="https://localhost/api/templates">the template folder</a> to:

```\My Documents\Visual Studio Version\Templates\ItemTemplates\Language\```