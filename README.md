# C# Basic Logger
Lightweight, basic C# logging utility.


###Using this library
Either import the .csproj or compile it and add a reference to BasicLogger.dll

In order to use this in your project, simply assign a log folder name.  By default, the logs will show up in ``C:\<current user>\AppData\Local\BasicLogger\``

However, you can assign a folder name, and it will, by default, put it in your local appdata folder.

```C#
using BasicLogger;

protected void Start() 
{
  Logger.LogFolderName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
}
```
This will put your logs in your local app data folder, with the name of your project.
``C:\kminehart\AppData\Local\MyProjectAssemblyName\``

You are free to use and distribute this application however you see fit.
