#!/bin/bash
rm -rf Prerender/output
dotnet publish .\/PGDevOpsTips.Web\/PGDevOpsTips.Web.csproj -c Release -o Prerender\/output --nologo
pushd Prerender
npx react-snap
find ./output/wwwroot -name "*.html" | while read htmlFile; do
    #sed -i 's/<base href="\/"/<base href="\/PGDevOpsTips\/"/g' $htmlFile
    sed -i 's/<script>var Module; window.__wasmmodulecallback__(); delete window.__wasmmodulecallback__;<\/script><script src="_framework\/dotnet.6.0.25.fveiv4nhbd.js" defer="" integrity="sha256-1jUGCCSGH9JWhkqbHdQk8JBjBI8K\/VC63En5zlypVps=" crossorigin="anonymous"><\/script>//g' $htmlFile
done
popd