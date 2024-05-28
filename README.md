
### efcore迁移

``` bash
## /Xia.Wiki/Xia.Wiki.Infrastructure
dotnet-ef migrations add FirstCreateDataBase -s ../Xia.Wiki.WebUI/Xia.Wiki.WebUI.csproj
```

``` bash
cd /workspace/Xia.Wiki/Xia.Wiki.WebUI/bin/Release/net8.0/publish
nohup dotnet Xia.Wiki.WebUI.dll --Urls=http://*:80 &

ps -a

pkill -f "dotnet Xia.Wiki.WebUI.dll .*:80"
```
