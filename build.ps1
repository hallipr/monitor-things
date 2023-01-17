$root = $PSScriptRoot
rm $root/out -Recurse -Force
dotnet publish $root/app/MonitorThings.Api -o $root/out