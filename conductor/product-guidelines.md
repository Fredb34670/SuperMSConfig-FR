# Directives Produit : SuperMSConfig (Francisation)

## 1. Principes de Traduction
- **Clarté et Concision :** Les traductions doivent être directes et éviter les tournures complexes. Privilégier les termes techniques établis en français (ex: "Paramètres" pour "Settings", "Pilote" pour "Driver").
- **Ton :** Professionnel, neutre et informatif.
- **Cohérence :** Utiliser un glossaire commun pour les termes récurrents afin d'assurer l'uniformité à travers l'application (exe, dll, json).

## 2. Gestion des Fichiers Décompilés
- **Source de Vérité :** Le fichier `SuperMSConfig.exe` original reste la référence fonctionnelle.
- **Modification :** Seules les chaînes de caractères (strings) doivent être modifiées dans le code décompilé.
- **Validation :** Toute modification dans le code décompilé doit être testée pour vérifier qu'elle n'altère pas la logique du programme.

## 3. Recherche et Identification
- **Portée :** La recherche de textes à traduire s'étend au-delà de l'exécutable principal.
- **Cibles :**
    - `SuperMSConfig.exe` (Fonctions intégrées, menus hardcodés)
    - Fichiers DLLs (Descriptions, métadonnées, ressources embarquées)
    - Fichiers de configuration (JSON, XML, INI) contenant des descriptions ou commentaires visibles par l'utilisateur.
- **Outils :** Utilisation d'outils d'analyse hexadécimale ou de décompilateurs pour localiser les ressources de chaînes.

## 4. UX/UI
- **Adaptation :** Ajuster la taille des contrôles UI si le texte français est plus long que l'anglais (foisonnement).
- **Intégrité Visuelle :** Ne pas déformer les icônes ou les images.
