# Flux de Travail Conductor (Standard)

Ce document définit les processus de développement, de test et de validation pour le projet SuperMSConfig (Francisation).

## 1. Cycle de Vie d'une Tâche (Task Lifecycle)
Chaque tâche définie dans un plan de piste (Track Plan) doit suivre ce cycle :

1.  **Sélection :** Choisir une tâche dans `plan.md` et marquer son statut `[ ]` -> `[/]`.
2.  **Branche :** Créer une branche Git dédiée : `feat/<track-id>/<task-short-desc>` ou `fix/...`.
3.  **Implémentation (TDD) :**
    -   *Red :* Écrire un test qui échoue (reproduisant le bug ou définissant la feature).
    -   *Green :* Écrire le code minimal pour passer le test.
    -   *Refactor :* Améliorer le code sans changer le comportement.
4.  **Validation Locale :** Exécuter tous les tests (`dotnet test`).
5.  **Commit :** Commit avec un message conventionnel (ex: `feat(ui): Traduire le menu principal`).
6.  **Code Review (Self) :** Vérifier que le code respecte `code_styleguides/csharp.md`.
7.  **Merge :** Fusionner la branche dans `main` (ou `develop`).
8.  **Clôture :** Marquer la tâche comme terminée dans `plan.md` (`[x]`).

## 2. Standards de Qualité
- **Couverture de Code :** Maintenir une couverture de tests > 80% pour le nouveau code.
- **Tests Unitaires :** Obligatoires pour toute logique métier modifiée.
- **Tests UI :** Recommandés pour vérifier les traductions (ex: screenshots, tests manuels documentés).

## 3. Gestion des Commits
- **Format :** Conventional Commits (`type(scope): description`).
  - Types : `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`.
- **Fréquence :** Un commit par tâche logique complétée. Ne pas commiter du code cassé.

## 4. Documentation
- Mettre à jour `spec.md` si des changements de spécifications surviennent.
- Documenter les décisions techniques importantes dans le code ou dans un fichier `ADR` (Architecture Decision Record) si nécessaire.
