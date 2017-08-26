<h1>IIS API</h1>

<img src="https://github.com/SimonBarnett/api/blob/master/templates.png">

<h2>Summary</h2>
The API is an IIS Application, <a href="https://github.com/SimonBarnett/api/blob/master/install.md">installed</a> on the Priority website e.g. 

````https://erpdemo.emerge-it.co.uk/api/{company}/{endpoint}.ashx````

Once the IIS Application is <a href="https://github.com/SimonBarnett/api/blob/master/install.md">installed</a> it provides a <a href="https://docs.microsoft.com/en-us/dotnet/framework/mef/">managed extensibility framework</a> that allows us to dynamically deploy new end-points (either feeds or handlers). 

Project managers can build new end-points using the .net templates provided and deploy them by simply by copying the edited template to the /bin folder of the API.

Handler end-points use the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a> to load data into Priority. This service must be installed seperately.

<h2>Feed End-points</h2>
<li>Feed end-points provide data to third party applications
<li>The end-point is an sql query
<li>Read more about <a href="https://github.com/SimonBarnett/api/blob/master/feeds.md">feeds</a>.

<h2>Handler End-points</h2>
<li>Handler end-points receive data from a third party. 
<li>The end-point builds a loading 
<li>The loading is processed by the <a href="https://github.com/SimonBarnett/apiLoad">nodeJS loading service</a>.
<li>Read more about <a href="https://github.com/SimonBarnett/api/blob/master/handlers.md">handlers</a>.

<h2>Installation</h2>
A seperate help file for installation of the IIS application is available <a href="https://github.com/SimonBarnett/api/blob/master/install.md">here</a>.
