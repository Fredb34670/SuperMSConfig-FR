# Script de compilation pour SuperMSConfig-FR

$ProjectDir = "SuperMSConfig-Francais"
$OutputDir = "bin-fr"

Write-Host "Nettoyage du dossier de sortie..." -ForegroundColor Cyan
if (Test-Path $OutputDir) {
    Remove-Item -Path $OutputDir -Recurse -Force
}

Write-Host "Restauration des packages..." -ForegroundColor Cyan
dotnet restore "$ProjectDir/SuperMSConfig.csproj"

Write-Host "Compilation de l'application..." -ForegroundColor Cyan
dotnet build "$ProjectDir/SuperMSConfig.csproj" -c Release -o $OutputDir

if ($LASTEXITCODE -eq 0) {
    Write-Host "Compilation réussie !" -ForegroundColor Green
    
    Write-Host "Copie des DLLs de plugins..." -ForegroundColor Cyan
    # On copie les DLLs des plugins manuellement car le csproj ne gère que les JSON par commodité ici
    $Plugins = Get-ChildItem -Path "plugins" -Filter "*.dll" -Recurse
    foreach ($plugin in $Plugins) {
        $relativeDir = $plugin.DirectoryName.Substring((Get-Item "plugins").FullName.Length)
        $destDir = Join-Path "$OutputDir\plugins" $relativeDir
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force
        }
        Copy-Item $plugin.FullName -Destination $destDir -Force
    }

    Write-Host "Copie des modèles (source)..." -ForegroundColor Cyan
    if (Test-Path "source") {
        Copy-Item "source" -Destination $OutputDir -Recurse -Force
    }
    
    Write-Host "L'exécutable se trouve dans le dossier '$OutputDir'." -ForegroundColor Yellow
} else {
    Write-Host "Échec de la compilation." -ForegroundColor Red
}
