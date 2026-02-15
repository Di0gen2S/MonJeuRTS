// ═══════════════════════════════════════════════════════════
// FICHIER : IEtatJeu.cs
// DESCRIPTION : Interface pour tous les états du jeu
// PATTERN : State Machine (Machine à états)
// ═══════════════════════════════════════════════════════════

namespace MonJeuRTS.Etats
{
    /// <summary>
    /// Interface définissant le contrat pour tous les états du jeu.
    /// 
    /// Concept : Le jeu peut être dans différents états (Menu, Partie, Pause).
    /// Chaque état implémente cette interface et définit son propre comportement
    /// pour l'entrée, la sortie, la mise à jour et l'affichage.
    /// 
    /// États prévus :
    /// - EtatMenu : Écran de démarrage avec bouton "Jouer"
    /// - EtatPartie : Jeu actif (carte, unités, etc.)
    /// - EtatPause : Menu pause avec bouton "Reprendre" et "Quitter"
    /// </summary>
    public interface IEtatJeu
    {
        /// <summary>
        /// Appelé UNE FOIS lorsqu'on entre dans cet état.
        /// Utilisé pour initialiser les ressources spécifiques à cet état.
        /// Exemple : Charger les sprites du menu, initialiser la carte, etc.
        /// </summary>
        void Entrer();
        
        /// <summary>
        /// Appelé UNE FOIS lorsqu'on sort de cet état.
        /// Utilisé pour nettoyer les ressources temporaires.
        /// Exemple : Décharger les textures du menu pour libérer la mémoire.
        /// </summary>
        void Sortir();
        
        /// <summary>
        /// Appelé à CHAQUE FRAME pour mettre à jour la logique de cet état.
        /// Exemple : Gérer les clics de souris, déplacer les unités, etc.
        /// </summary>
        /// <param name="deltaTime">Temps écoulé depuis la dernière frame (en secondes)</param>
        void MettreAJour(float deltaTime);
        
        /// <summary>
        /// Appelé à CHAQUE FRAME pour dessiner cet état à l'écran.
        /// Exemple : Afficher le menu, dessiner la carte, afficher le HUD, etc.
        /// IMPORTANT : Ne pas faire de logique ici, seulement de l'affichage.
        /// </summary>
        void Dessiner();
    }
}
