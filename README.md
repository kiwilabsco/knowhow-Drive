![Home page](https://content.screencast.com/users/KeremDurak/folders/Snagit/media/707ec0b2-776c-47c0-9fd8-6ac479296742/07.09.2020-00.49.png)

This is a simple knowledge management application that takes advantage of Google Drive to store data and Elasticsearch to query the stored data. 
This application should provide useful introduction to: 
* .NET Google Drive API integration
* Querying with Elasticsearch NEST Client
* .NET Core and .NET Desktop Development

**Here are some basic features**:<br />
It allows querying from the document content by making use of the [Export API](https://developers.google.com/drive/api/v3/reference/files/export)<br />
<img src="https://content.screencast.com/users/KeremDurak/folders/Snagit/media/cbf1ae2c-4466-4fec-8c90-3b562ce32174/07.09.2020-01.09.GIF" width="400">
<br />
You can log in with your Google account and index the contents of any Drive folder you want using the desktop application:

![gif2](https://content.screencast.com/users/KeremDurak/folders/Snagit/media/8f279423-b625-4947-a9e0-d3f407f1d93f/07.09.2020-01.29.GIF) <br />
You can query via partial words using [Regexp query](https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-regexp-query.html#query-dsl-regexp-query): <br />
<img src="https://content.screencast.com/users/KeremDurak/folders/Snagit/media/3bb214f4-1b19-4141-8b9e-3ba8b55345c7/07.09.2020-01.49.GIF" width="400">
<br />

You must add your Elasticsearch URI to `/AppSettings.json` and `/KnowledgeDrive/AppSettings.json`before running the application.
