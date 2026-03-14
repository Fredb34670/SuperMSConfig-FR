# Script de compilation pour SuperMSConfig-FR

$ProjectDir = "SuperMSConfig-Francais"
$OutputDir = "bin-fr"
$ReleaseDir = "Release-FR"

Write-Host "Nettoyage du dossier de sortie..." -ForegroundColor Cyan
if (Test-Path $OutputDir) {
    Remove-Item -Path $OutputDir -Recurse -Force
}
if (Test-Path $ReleaseDir) {
    Remove-Item -Path $ReleaseDir -Recurse -Force
}

Write-Host "Compilation de l'application..." -ForegroundColor Cyan
dotnet build $ProjectDir -c Release -o $OutputDir

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
    
    Write-Host "Création du pack de distribution ($ReleaseDir)..." -ForegroundColor Cyan
    New-Item -ItemType Directory -Path $ReleaseDir -Force
    Copy-Item -Path "$OutputDir\*" -Destination $ReleaseDir -Recurse -Exclude "*.pdb" -Force

    Write-Host "Prêt ! L'exécutable se trouve dans le dossier '$OutputDir'." -ForegroundColor Yellow
    Write-Host "Le pack prêt pour la distribution est dans '$ReleaseDir'." -ForegroundColor Green
} else {
    Write-Host "Échec de la compilation." -ForegroundColor Red
}
