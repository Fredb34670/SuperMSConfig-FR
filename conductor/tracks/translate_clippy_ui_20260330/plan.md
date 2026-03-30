# Plan de Mise en Œuvre : Traduction du Plugin Clippy Companion

## Phase 1 : Préparation et Extraction
L'objectif est d'extraire le code source du plugin de la version anglaise.

- [x] Task: Créer le répertoire source du plugin (ex: `PluginMSNavigator-FR`).
- [x] Task: Copier les fichiers sources (`.cs`, `.Designer.cs`, `.resx`, `.csproj`) depuis `SuperMSConfig-Anglais/PluginMSNavigator`.
- [x] Task: Ajuster le fichier `.csproj` pour utiliser le format SDK moderne et pointer vers les références correctes.

## Phase 2 : Correction de la Base de Données (JSON)
- [x] Task: Traduire et corriger l'encodage du fichier `plugins/PluginMSNavigator/MSNavigator.json`.

## Phase 3 : Traduction de l'Interface Utilisateur
- [x] Task: Traduire les propriétés `Text` dans `MSNavigatorPluginControl.Designer.cs`.
- [x] Task: Traduire les chaînes de caractères statiques dans `MSNavigatorPluginControl.cs`.

## Phase 4 : Intégration et Build
- [x] Task: Compiler le nouveau plugin `MSNavigator.dll`.
- [x] Task: Mettre à jour `build-fr.ps1` si nécessaire (normalement il copie déjà tout le contenu de `plugins/`).

## Phase 5 : Vérification Finale
- [x] Task: Lancer `SuperMSConfig-FR.exe` et ouvrir le plugin Clippy Companion.
- [x] Task: Conductor - User Manual Verification 'Phase 5' (Protocol in workflow.md).
