# Plan de Mise en Œuvre : Traduction et Correction du Plugin MSCC

## Phase 1 : Préparation et Extraction
L'objectif est d'extraire le code source du plugin de la version anglaise pour pouvoir le modifier dans le contexte de la version française.

- [ ] Task: Créer le répertoire source du plugin dans `SuperMSConfig-Francais` (ex: `SuperMSConfig-Francais/PluginMSCC-FR`).
- [ ] Task: Copier les fichiers sources (`.cs`, `.Designer.cs`, `.resx`, `.csproj`) depuis `SuperMSConfig-Anglais/PluginMSCC`.
- [ ] Task: Ajuster le fichier `.csproj` du plugin pour pointer vers les références correctes (notamment `PluginInterface.dll`).
- [ ] Task: Vérifier que le plugin se compile isolément en français.

## Phase 2 : Correction de la Base de Données (JSON)
Correction de l'encodage et des caractères corrompus.

- [ ] Task: Ouvrir `plugins/PluginMSCleanupCompanion/MSCleanupCompanion.json` et le ré-enregistrer en UTF-8 (sans BOM).
- [ ] Task: Automatiser ou corriger manuellement les séquences corrompues (ex: `ÃƒÂ©` -> `é`).
- [ ] Task: Traduire les descriptions qui sont encore en anglais.
- [ ] Task: Valider la structure JSON.

## Phase 3 : Traduction de l'Interface Utilisateur
Modification du code C# pour la francisation.

- [ ] Task: Traduire les propriétés `Text` dans `MSCCPluginControl.Designer.cs` (Boutons, Labels, ToolStripItems).
- [ ] Task: Traduire les chaînes de caractères statiques dans `MSCCPluginControl.cs` (PluginInfo, Messages de MessageBox).
- [ ] Task: Harmoniser le titre du plugin si nécessaire.

## Phase 4 : Intégration et Build
Assurer que la version française utilise le nouveau plugin traduit.

- [ ] Task: Compiler le nouveau plugin `MSCleanupCompanion-FR.dll`.
- [ ] Task: Mettre à jour `build-fr.ps1` pour s'assurer qu'il utilise le plugin traduit (ou écraser l'existant dans `plugins/`).
- [ ] Task: Lancer une compilation complète du projet français.

## Phase 5 : Vérification Finale
- [ ] Task: Lancer `SuperMSConfig-FR.exe` et ouvrir le plugin MSCC.
- [ ] Task: Vérifier l'affichage correct des caractères accentués dans la liste.
- [ ] Task: Vérifier que tous les éléments du menu contextuel sont en français.
- [ ] Task: Conductor - User Manual Verification 'Phase 5' (Protocol in workflow.md)
