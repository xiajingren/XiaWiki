
### efcore迁移

``` bash
## /BlogX/BlogX.Infrastructure
dotnet-ef migrations add FirstCreateDataBase -s ../BlogX.WebUI/BlogX.WebUI.csproj
```

``` bash
cd /workspace/BlogX/BlogX.WebUI/bin/Release/net8.0/publish
nohup dotnet BlogX.WebUI.dll --Urls=http://*:80 &

ps -a

pkill -f "dotnet BlogX.WebUI.dll .*:80"
```
