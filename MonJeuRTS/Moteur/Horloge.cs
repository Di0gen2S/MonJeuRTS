// ═══════════════════════════════════════════════════════════
// FICHIER : Horloge.cs
// DESCRIPTION : Gestion du temps dans le jeu
// RESPONSABILITÉS : Calculer deltaTime, FPS, temps total écoulé
// ═══════════════════════════════════════════════════════════

using Raylib_cs;

namespace MonJeuRTS.Moteur
{
    /// <summary>
    /// Horloge du jeu pour gérer le temps et les FPS.
    /// Responsabilités :
    /// - Calculer le deltaTime (temps écoulé depuis la dernière frame)
    /// - Suivre le temps total écoulé
    /// - Calculer les FPS (frames par seconde)
    /// </summary>
    public class Horloge
    {
        // █ PROPRIÉTÉS PUBLIQUES █
        
        /// <summary>
        /// Temps écoulé depuis la dernière frame en secondes.
        /// Utilisé pour rendre les mouvements indépendants de la vitesse de la machine.
        /// Exemple : vitesse * deltaTime pour un mouvement fluide
        /// </summary>
        public float DeltaTime { get; private set; }
        
        /// <summary>
        /// Temps total écoulé depuis le démarrage du jeu en secondes.
        /// </summary>
        public float TempsTotal { get; private set; }
        
        /// <summary>
        /// Nombre de frames par seconde (FPS) actuelles.
        /// Utile pour déboguer les performances.
        /// </summary>
        public int FPS { get; private set; }
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise l'horloge avec des valeurs par défaut.
        /// </summary>
        public Horloge()
        {
            DeltaTime = 0f;
            TempsTotal = 0f;
            FPS = 0;
        }
        
        
        // █ MÉTHODES PUBLIQUES █
        
        /// <summary>
        /// Met à jour l'horloge à chaque frame.
        /// DOIT être appelé au début de chaque cycle Update.
        /// </summary>
        public void MettreAJour()
        {
            // Raylib fournit le deltaTime directement
            DeltaTime = Raylib.GetFrameTime();
            
            // Accumule le temps total
            TempsTotal += DeltaTime;
            
            // Récupère les FPS actuels
            FPS = Raylib.GetFPS();
        }
        
        /// <summary>
        /// Réinitialise l'horloge (utile lors du changement d'état de jeu).
        /// </summary>
        public void Reinitialiser()
        {
            TempsTotal = 0f;
            DeltaTime = 0f;
        }
        
        /// <summary>
        /// Retourne une chaîne de débogage avec les informations de l'horloge.
        /// </summary>
        public override string ToString()
        {
            return $"FPS: {FPS} | DeltaTime: {DeltaTime:F4}s | Temps total: {TempsTotal:F2}s";
        }
    }
}
