# Guide de Style C# (.NET)

## 1. Conventions de Nommage
- **Classes, Méthodes, Propriétés, Interfaces :** PascalCase (ex: `MaClasse`, `CalculerTotal`, `IdUtilisateur`).
- **Variables locales, Paramètres :** camelCase (ex: `nomClient`, `index`).
- **Constantes :** SCREAMING_SNAKE_CASE ou PascalCase (selon contexte existant).
- **Champs privés :** `_camelCase` (ex: `_logger`).

## 2. Formatage
- **Indentation :** 4 espaces (pas de tabulations).
- **Accolades :** Style K&R ou Allman (nouvelle ligne) - *S'aligner sur le style dominant du code décompilé.*
- **Lignes vides :** Une ligne vide entre les méthodes et les propriétés pour la lisibilité.

## 3. Bonnes Pratiques (Spécifique Francisation)
- **Chaînes de Caractères :**
  - Ne jamais coder en dur des chaînes utilisateur dans la logique métier.
  - Utiliser des fichiers de ressources (.resx) ou des constantes centralisées si possible.
  - Commenter clairement l'origine de la chaîne (ex: "Menu Principal > Fichier").

## 4. Commentaires
- **Langue :** Français.
- **Documentation XML :** Utiliser `///` pour documenter les classes et méthodes publiques modifiées.
- **TODOs :** Marquer les zones nécessitant une traduction future avec `// TODO: Traduire ...`.
