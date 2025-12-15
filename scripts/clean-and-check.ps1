# Delete build artefacts and search for any manual TargetFrameworkAttribute
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue .\Adventure-project\bin, .\Adventure-project\obj
dotnet clean .\Adventure-project\Adventure-project.csproj
Write-Host "Searching for manual TargetFrameworkAttribute occurrences..."
Get-ChildItem -Recurse -Filter *.cs | Select-String "TargetFrameworkAttribute" | ForEach-Object {
  Write-Host "$($_.Path):$($_.LineNumber): $($_.Line.Trim())"
}