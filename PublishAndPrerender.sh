#!/bin/bash
set -euo pipefail

rm -rf Prerender/output

dotnet publish ./PGDevOpsTips.Web/PGDevOpsTips.Web.csproj -c Release -o Prerender/output --nologo

pushd Prerender > /dev/null
npx react-snap

# react-snap saves the scripts Blazor injects at runtime. They must be removed:
# Blazor adds both again when the page starts, with the correct file name for this build.
find ./output/wwwroot -name "*.html" -print0 | while IFS= read -r -d '' htmlFile; do
    #sed -i 's/<base href="\/"/<base href="\/PGDevOpsTips\/"/g' "$htmlFile"

    # 1. Any inline script that calls the callback (var Module; window.__wasmmodulecallback__(); ...)
    perl -0777 -pi -e 's{<script[^>]*>[^<]*__wasmmodulecallback__[^<]*</script>}{}g' "$htmlFile"

    # 2. The runtime script, whatever its file name or attribute order (dotnet.js, dotnet.6.0.x.hash.js, ...)
    perl -0777 -pi -e 's{<script[^>]*src=["\x27]?_framework/dotnet[^\s>"\x27]*["\x27]?[^>]*>\s*</script>}{}g' "$htmlFile"

    if grep -q '__wasmmodulecallback__' "$htmlFile"; then
        echo "WARNING: loader snippet still present in $htmlFile" >&2
    fi
done
popd > /dev/null