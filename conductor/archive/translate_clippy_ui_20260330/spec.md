# Spécification du Track : Traduction du Plugin Clippy Companion (Microsoft Navigator)

## Aperçu
Ce track vise à franciser complètement le plugin "Clippy Companion", également appelé "Microsoft Navigator". Cela inclut l'interface utilisateur (boutons, labels, placeholders) et les textes informatifs.

## Objectifs
- Traduire tous les éléments de l'interface utilisateur (New Chat, Model, Send, etc.).
- Traduire les textes d'aide et les conseils (tips).
- Traduire les descriptions et placeholders de recherche.
- Corriger les éventuels problèmes d'encodage (accents) dans les fichiers JSON associés.

## Exigences Fonctionnelles
### 1. Traduction de l'Interface (MSNavigatorPluginControl)
- **Titre :** "Clippy Companion" -> (Peut rester tel quel ou devenir "Assistant Clippy").
- **Boutons :** 
    - "New Chat" -> "Nouvelle discussion".
    - "Model" -> "Modèle".
    - "Send" -> "Envoyer".
- **Placeholder :** "Type your question" -> "Tapez votre question".
- **Texte de bas de page :** "Microsoft Navigator is your modern..." -> Traduire en français.
- **Conseils (Tips) :** "Here's a tip! To get popular apps..." -> Traduire en français.

### 2. Correction de la Base de Données (MSNavigator.json)
- Vérifier et corriger l'encodage (UTF-8).
- Traduire les descriptions d'applications ou de réglages à l'intérieur du JSON.

## Critères d'Acceptation
- Le plugin se compile sans erreur.
- L'interface utilisateur est 100% en français.
- Tous les accents s'affichent correctement.
- Le plugin est correctement chargé par l'application principale SuperMSConfig-FR.
