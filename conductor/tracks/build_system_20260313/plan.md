# Plan d'Implémentation : Système de Compilation

## Phase 1 : Analyse du Projet
- [ ] Task: Analyser `SuperMSConfig.csproj`.
    - [ ] Déterminer la version du framework .NET.
    - [ ] Vérifier les chemins des références (DLLs).
- [ ] Task: Vérifier la présence des bibliothèques nécessaires dans le répertoire racine.

## Phase 2 : Configuration du Build
- [ ] Task: Corriger le fichier `.csproj` si les chemins de décompilation sont incorrects.
- [ ] Task: Préparer les ressources (Images, Icônes) pour l'inclusion.

## Phase 3 : Script de Compilation
- [ ] Task: Créer `build-fr.ps1` (PowerShell).
    - [ ] Script pour restaurer les packages (si nécessaire).
    - [ ] Commande de compilation.
    - [ ] Copie des fichiers JSON (`plugins/`, `source/`) vers le dossier de sortie.

## Phase 4 : Exécution et Validation
- [ ] Task: Lancer la compilation.
- [ ] Task: Vérifier la présence de l'exécutable dans le dossier de sortie.
- [ ] Task: Conductor - User Manual Verification 'Validation Build' (Protocol in workflow.md).
