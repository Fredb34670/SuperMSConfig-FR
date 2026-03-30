# Spécification du Track : Traduction et Correction du Plugin MSCC

## Aperçu
Ce track vise à franciser complètement le plugin "Microsoft Cleanup Companion" (MSCC), à corriger les problèmes d'encodage dans sa base de données JSON et à traduire son interface utilisateur ainsi que son menu contextuel.

## Objectifs
- Traduire tous les libellés de l'interface utilisateur (boutons, étiquettes, boîtes de dialogue).
- Traduire l'intégralité du menu contextuel (mdlMenu).
- Corriger les caractères corrompus dans `MSCleanupCompanion.json` (problèmes d'encodage UTF-8).
- Harmoniser la terminologie avec le reste de l'application SuperMSConfig-FR.

## Exigences Fonctionnelles
### 1. Traduction de l'Interface (MSCCPluginControl)
- **Bouton de suppression :** "Remove selected apps" -> "Supprimer les applications sélectionnées".
- **Champ de recherche :** "Search" -> "Rechercher".
- **Statut :** "Status" -> "Statut".
- **Titre :** "Microsoft Cleanup Companion" (Optionnel : Traduire ou garder tel quel, à confirmer).
- **Messages :** Traduire les messages d'information du plugin (Plugin info...).

### 2. Traduction du Menu (contextMenu)
- **Scan again :** "Scanner à nouveau".
- **Select all :** "Tout sélectionner".
- **Show all installed :** "Afficher tout ce qui est installé".
- **Select custom db... :** "Sélectionner une base de données personnalisée...".
- **Plugin info... :** "Informations sur le plugin...".

### 3. Correction de la Base de Données (MSCleanupCompanion.json)
- Ré-encoder le fichier en UTF-8 sans BOM.
- Remplacer les séquences de caractères corrompues (ex: `ÃƒÂ©`) par les accents corrects (ex: `é`).
- Traduire les descriptions restées en anglais si nécessaire.

## Critères d'Acceptation
- Le plugin se compile sans erreur dans l'environnement français.
- L'interface utilisateur est 100% en français.
- Tous les caractères accentués s'affichent correctement dans la liste des applications.
- Le menu contextuel est entièrement traduit et fonctionnel.

## Hors Scope
- Modification de la logique de suppression des applications.
- Ajout de nouvelles fonctionnalités au plugin.
