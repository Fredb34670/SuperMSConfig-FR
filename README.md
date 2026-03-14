# SuperMSConfig (Version Française)

SuperMSConfig est un outil avancé de configuration pour Windows 10/11, permettant de gérer les "habitudes" du système (vie privée, sécurité, interface, etc.) et d'optimiser l'expérience utilisateur.

Cette branche est la version **intégralement francisée** de l'application originale.

## Fonctionnalités

- **Gestion des Habitudes** : Activez ou désactivez facilement des centaines de paramètres Windows.
- **Support des Plugins** : Intégration de plugins comme *Ask Clipilot* pour une assistance par IA.
- **Interface Francisée** : Tous les menus, descriptions et messages d'erreur sont traduits.
- **Portabilité** : Pas d'installation requise, exécutez simplement le binaire compilé.

## Structure du Projet

- `SuperMSConfig-Francais/` : Code source principal de l'application.
- `plugins/` : Sources des plugins inclus.
- `source/` : Fichiers templates et configurations JSON.
- `conductor/` : Documentation technique du projet.

## Compilation

Pour compiler le projet vous-même :
1. Assurez-vous d'avoir le SDK .NET installé.
2. Exécutez le script PowerShell `build-fr.ps1` à la racine.
3. Le résultat se trouvera dans le dossier `bin-fr/`.

## Crédits

- Application originale par **Belim** (Builtbybel).
- Francisation par **Fredb34670**.

---
*Optimisez votre Windows en toute simplicité.*
