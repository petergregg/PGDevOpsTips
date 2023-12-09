#!/bin/bash
rm -rf Prerender/output
dotnet publish .\/PGDevOpsTips.Web\/PGDevOpsTips.Web.csproj -c Release -o Prerender\/output --nologo
pushd Prerender
npx react-snap
find ./output -name "*.html" | while read htmlFile; do
    #sed -i 's/<base href="\/"/<base href="\/PGDevOpsTips\/"/g' $htmlFile
    sed -i 's/<script type="text\/javascript">var Module; window.__wasmmodulecallback__(); delete window.__wasmmodulecallback__;<\/script><script src="_framework\/dotnet.6.0.22.916mi3e43m.js" defer="" integrity="sha256-Vpgzm0BMIfPGXG2SCtEChw5HmXlQJIIS0eyBG8kP+u4=" crossorigin="anonymous"><\/script>//g' $htmlFile
done
popd