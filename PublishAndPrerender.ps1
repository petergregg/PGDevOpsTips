if (Test-Path .\Prerender\output) {
    Remove-Item -Path .\Prerender\output -Recurse
}

dotnet publish .\PGDevOpsTips.Web\PGDevOpsTips.Web.csproj -c Release -o Prerender/output --nologo
Push-Location .\Prerender
npx react-snap

$scriptWithCallback = '<script[^>]*>[^<]*__wasmmodulecallback__[^<]*</script>'
$dotnetScript       = '<script[^>]*src=["'']?_framework/dotnet[^\s>"'']*["'']?[^>]*>\s*</script>'
$utf8 = New-Object System.Text.UTF8Encoding $false

Get-ChildItem ".\output\wwwroot\*.html" -Recurse | ForEach-Object {
    $html = [System.IO.File]::ReadAllText($_.FullName)
    $html = [regex]::Replace($html, $scriptWithCallback, '')
    $html = [regex]::Replace($html, $dotnetScript, '')
    [System.IO.File]::WriteAllText($_.FullName, $html, $utf8)

    if ($html -match '__wasmmodulecallback__') {
        Write-Warning "Loader snippet still present in $($_.FullName)"
    }
}
Pop-Location