If(Test-Path .\Prerender\output)
{
    Remove-Item -Path .\Prerender\output -Recurse
}
 
dotnet publish .\PGDevOpsTips.Web\PGDevOpsTips.Web.csproj -c Release -o Prerender/output --nologo
Push-Location .\Prerender
npx react-snap
Get-ChildItem ".\output\wwwroot\*.html" -Recurse | ForEach-Object { 
    $HtmlFileContent = (Get-Content -Path $_.FullName -Raw);
    $HtmlFileContent = $HtmlFileContent.Replace('<script>var Module; window.__wasmmodulecallback__(); delete window.__wasmmodulecallback__;</script><script src="_framework/dotnet.6.0.25.fveiv4nhbd.js" defer="" integrity="sha256-1jUGCCSGH9JWhkqbHdQk8JBjBI8K/VC63En5zlypVps=" crossorigin="anonymous"></script>','')
    Set-Content -Path $_.FullName -Value $HtmlFileContent
}
Pop-Location
