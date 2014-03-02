###GZip Compression for Windows Phone WebClient

UPDATE: THIS PROJECT HAS BEEN DISCONTINUED. PLEASE USE THE OFFICIAL MICROSOFT PROVIDED HTTP CLIENT LIBRARY: https://www.nuget.org/packages/Microsoft.Net.Http

Now that Microsoft finally provides GZip compression for Windows Phone, I encourage you to use their official nuget package instead.

======================


(c) Copyright Morten Nielsen.

This source is subject to the Microsoft Public License (Ms-PL).
Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
All other rights reserved.

Available for direct install from NuGet: https://nuget.org/packages/SharpGIS.GZipWebClient

To use this code, explicitly use the SharpGIS.GZipWebClient class where you would normally use a WebClient instance. Ex:
```c-sharp
   WebClient client = new SharpGIS.WebClient(); //Yes the type is still WebClient since that's what it inherits from :-)
   //continue with this WebClient as you used to
```
Or you can automatically opt in for all existing WebClients by adding the following code during application start up: 
```c-sharp
   WebRequest.RegisterPrefix("http://", SharpGIS.WebRequestCreator.GZip);
   WebRequest.RegisterPrefix("https://", SharpGIS.WebRequestCreator.GZip);
```
This will change ALL web requests both those that uses WebClient and those that uses the lower-level HttpWebRequest, as well as any 3rd party library to start using compressed requests. Please test thoroughly - I've seen some issues with a few 3rd party libraries that doesn't work well with a different HttpWebRequest object, but for the most part this just works.

Blogpost on this utility available here: http://www.sharpgis.net/post/2011/08/28/GZIP-Compressed-Web-Requests-in-WP7-Take-2.aspx
