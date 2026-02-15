// ═══════════════════════════════════════════════════════════
// FICHIER : EtatPartie.cs
// DESCRIPTION : État du jeu actif (où se déroule la partie)
// RESPONSABILITÉS : Gérer la logique du jeu et afficher le monde
// ═══════════════════════════════════════════════════════════

using Raylib_cs;
using MonJeuRTS.Utilitaires;

namespace MonJeuRTS.Etats
{
    /// <summary>
    /// État représentant une partie en cours.
    /// 
    /// Pour l'instant (Phase 1), c'est juste un écran vert avec du texte.
    /// Dans les prochaines phases, on ajoutera ici :
    /// - La carte avec tiles
    /// - Les unités et bâtiments
    /// - Le HUD avec les ressources
    /// - La caméra pour naviguer
    /// - Les systèmes de sélection et déplacement
    /// </summary>
    public class EtatPartie : IEtatJeu
    {
        // █ PROPRIÉTÉS PRIVÉES █
        
        // Référence vers le gestionnaire d'états (pour mettre en pause)
        private readonly Action<IEtatJeu> changerEtatCallback;
        
        // Temps écoulé dans cette partie (en secondes)
        private float tempsPartie;
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise l'état de la partie.
        /// </summary>
        /// <param name="changerEtat">Callback pour changer d'état</param>
        public EtatPartie(Action<IEtatJeu> changerEtat)
        {
            this.changerEtatCallback = changerEtat;
            tempsPartie = 0f;
        }
        
        
        // █ IMPLÉMENTATION DE IEtatJeu █
        
        /// <summary>
        /// Appelé au démarrage de la partie.
        /// Ici on initialisera plus tard : la carte, les unités, etc.
        /// </summary>
        public void Entrer()
        {
            Logger.Ecrire("Entrée dans l'état Partie - Début du jeu", NiveauLog.Info);
            tempsPartie = 0f;
            
            // TODO Phase 2 : Initialiser le gestionnaire de ressources
            // TODO Phase 3 : Charger la carte
            // TODO Phase 4 : Créer les unités de départ
        }
        
        /// <summary>
        /// Appelé à la sortie de la partie (retour au menu ou fermeture).
        /// </summary>
        public void Sortir()
        {
            Logger.Ecrire("Sortie de l'état Partie", NiveauLog.Info);
            
            // TODO Phase 2+ : Nettoyer les ressources (textures, sons, etc.)
        }
        
        /// <summary>
        /// Mise à jour de la logique du jeu.
        /// Pour l'instant, détecte juste la touche Échap pour mettre en pause.
        /// </summary>
        public void MettreAJour(float deltaTime)
        {
            // Accumule le temps de jeu
            tempsPartie += deltaTime;
            
            // Détection de la touche Échap pour mettre en pause
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                Logger.Ecrire("Touche Échap appuyée - Mise en pause", NiveauLog.Info);
                changerEtatCallback(new EtatPause(changerEtatCallback));
            }
            
            // TODO Phase 2 : Mettre à jour le système de ressources
            // TODO Phase 3 : Mettre à jour la caméra (déplacement avec WASD)
            // TODO Phase 4 : Mettre à jour les unités (déplacement, récolte)
            // TODO Phase 5 : Mettre à jour les bâtiments (construction, production)
            // TODO Phase 6 : Mettre à jour les combats
        }
        
        /// <summary>
        /// Dessine le jeu à l'écran.
        /// Pour l'instant, fond vert avec des informations de debug.
        /// </summary>
        public void Dessiner()
        {
            // Fond vert (comme un terrain d'herbe simplifié)
            Raylib.ClearBackground(new Color(34, 139, 34, 255)); // ForestGreen
            
            // Titre temporaire
            Raylib.DrawText("PARTIE EN COURS", 20, 20, 30, Color.White);
            
            // Informations de debug
            Raylib.DrawText($"Temps de jeu : {tempsPartie:F1} secondes", 20, 60, 20, Color.White);
            Raylib.DrawText("Appuyez sur ÉCHAP pour mettre en pause", 20, 90, 20, Color.LightGray);
            
            // Zone centrale avec message
            string message = "Phase 1 : Architecture de base OK !";
            int largeurMessage = Raylib.MeasureText(message, 40);
            Raylib.DrawText(message, (1280 - largeurMessage) / 2, 300, 40, Color.Yellow);
            
            string info = "Prochaine phase : Système de ressources";
            int largeurInfo = Raylib.MeasureText(info, 20);
            Raylib.DrawText(info, (1280 - largeurInfo) / 2, 360, 20, Color.White);
            
            // TODO Phase 2 : Dessiner le HUD avec les ressources
            // TODO Phase 3 : Dessiner la carte avec les tiles
            // TODO Phase 4 : Dessiner les unités et leur sélection
            // TODO Phase 5 : Dessiner les bâtiments
            // TODO Phase 6 : Dessiner les effets de combat
        }
    }
}
