# Plan d'Implémentation : Système de Compilation

## Phase 1 : Analyse du Projet
- [x] Task: Analyser `SuperMSConfig.csproj`.
    - [x] Déterminer la version du framework .NET.
    - [x] Vérifier les chemins des références (DLLs).
- [x] Task: Vérifier la présence des bibliothèques nécessaires dans le répertoire racine.

## Phase 2 : Configuration du Build
- [x] Task: Corriger le fichier `.csproj` si les chemins de décompilation sont incorrects.
    - [x] Vérifier les chemins relatifs des ressources.
- [x] Task: Préparer les ressources (Images, Icônes) pour l'inclusion.

## Phase 3 : Script de Compilation
- [x] Task: Créer `build-fr.ps1` (PowerShell).
    - [x] Script pour restaurer les packages (si nécessaire).
    - [x] Commande de compilation.
    - [x] Copie des fichiers JSON (`plugins/`, `source/`) vers le dossier de sortie.

## Phase 4 : Exécution et Validation
- [x] Task: Lancer la compilation.
- [x] Task: Vérifier la présence de l'exécutable dans le dossier de sortie.
- [x] Task: Conductor - User Manual Verification 'Validation Build' (Protocol in workflow.md).
