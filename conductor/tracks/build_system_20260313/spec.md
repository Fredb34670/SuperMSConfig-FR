# Spécification : Mise en place du Système de Compilation

## 1. Objectif
Produire un exécutable fonctionnel nommé `SuperMSConfig-FR.exe` qui intègre toutes les traductions effectuées (code source C# et fichiers JSON).

## 2. Éléments Concernés
- **Code Source** : Le dossier `SuperMSConfig-Francais/`.
- **Fichier Projet** : `SuperMSConfig-Francais/SuperMSConfig.csproj`.
- **Dépendances** :
    - `Newtonsoft.Json.dll`
    - `PluginInterface.dll`
    - `Microsoft.Web.WebView2.*`
- **Output** : Un nouveau dossier `bin-fr/` contenant l'application prête à l'emploi.

## 3. Contraintes Techniques
- **Framework** : Utiliser `dotnet build` ou MSBuild (selon ce qui est détecté dans le `.csproj`).
- **Architecture** : Cibler l'architecture originale (probablement x64 ou AnyCPU).
- **Ressources** : S'assurer que les icônes et fichiers `.resx` sont correctement inclus.

## 4. Critères de Réussite
- Le script de build s'exécute sans erreur.
- Un fichier `SuperMSConfig-FR.exe` est généré.
- L'exécutable se lance et affiche l'interface en français.
