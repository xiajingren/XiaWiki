
### efcore迁移

``` bash
## /XiaWiki/XiaWiki.Infrastructure
dotnet-ef migrations add FirstCreateDataBase -s ../XiaWiki.WebUI/XiaWiki.WebUI.csproj
```

``` bash
cd /workspace/XiaWiki/XiaWiki.WebUI/bin/Release/net8.0/publish
nohup dotnet XiaWiki.WebUI.dll --Urls=http://*:80 &

ps -a

pkill -f "dotnet XiaWiki.WebUI.dll .*:80"
```
