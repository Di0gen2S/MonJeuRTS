// ═══════════════════════════════════════════════════════════
// FICHIER : Logger.cs
// DESCRIPTION : Système de logs pour déboguer le jeu
// RESPONSABILITÉS : Écrire des messages dans la console et dans un fichier
// PATTERN : Singleton (une seule instance partagée)
// ═══════════════════════════════════════════════════════════

using System;
using System.IO;

namespace MonJeuRTS.Utilitaires
{
    /// <summary>
    /// Niveaux de gravité des messages de log.
    /// Permet de filtrer les messages selon leur importance.
    /// </summary>
    public enum NiveauLog
    {
        Debug,      // Informations de débogage détaillées
        Info,       // Informations générales
        Attention,  // Avertissements (quelque chose d'inhabituel mais pas critique)
        Erreur      // Erreurs graves
    }
    
    /// <summary>
    /// Système de logs centralisé pour l'application.
    /// Écrit les messages dans la console ET dans un fichier texte.
    /// </summary>
    public static class Logger
    {
        // █ PROPRIÉTÉS PRIVÉES █
        
        // Flux d'écriture vers le fichier de logs
        private static StreamWriter? fluxFichier;
        
        // Indique si le logger a été initialisé
        private static bool estInitialise = false;
        
        // Chemin du fichier de logs
        private static string? cheminFichier;
        
        
        // █ MÉTHODES PUBLIQUES █
        
        /// <summary>
        /// Initialise le système de logs.
        /// DOIT être appelé une fois au démarrage de l'application.
        /// </summary>
        /// <param name="nomFichier">Nom du fichier de logs (ex: "jeu.log")</param>
        public static void Initialiser(string nomFichier)
        {
            if (estInitialise)
            {
                Console.WriteLine("[ATTENTION] Logger déjà initialisé !");
                return;
            }
            
            try
            {
                // Créer le fichier de logs (écrase l'ancien s'il existe)
                cheminFichier = nomFichier;
                fluxFichier = new StreamWriter(nomFichier, false); // false = écraser
                fluxFichier.AutoFlush = true; // Écrire immédiatement (pas de buffer)
                
                estInitialise = true;
                
                // Premier message
                Ecrire("Logger initialisé avec succès", NiveauLog.Info);
                Ecrire($"Fichier de logs : {Path.GetFullPath(nomFichier)}", NiveauLog.Info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Impossible d'initialiser le logger : {ex.Message}");
            }
        }
        
        /// <summary>
        /// Écrit un message de log dans la console et le fichier.
        /// </summary>
        /// <param name="message">Message à écrire</param>
        /// <param name="niveau">Niveau de gravité du message</param>
        public static void Ecrire(string message, NiveauLog niveau)
        {
            if (!estInitialise)
            {
                Console.WriteLine("[ATTENTION] Logger non initialisé ! Message ignoré.");
                return;
            }
            
            // Horodatage (heure actuelle)
            string horodatage = DateTime.Now.ToString("HH:mm:ss");
            
            // Préfixe selon le niveau
            string prefixe = ObtenirPrefixe(niveau);
            
            // Message complet formaté
            string messageComplet = $"[{horodatage}] {prefixe} {message}";
            
            // Écriture dans la console avec couleur selon le niveau
            EcrireConsoleAvecCouleur(messageComplet, niveau);
            
            // Écriture dans le fichier
            try
            {
                fluxFichier?.WriteLine(messageComplet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Impossible d'écrire dans le fichier de logs : {ex.Message}");
            }
        }
        
        /// <summary>
        /// Ferme proprement le système de logs.
        /// DOIT être appelé à la fin de l'application.
        /// </summary>
        public static void Fermer()
        {
            if (!estInitialise)
            {
                return;
            }
            
            Ecrire("Fermeture du système de logs", NiveauLog.Info);
            
            try
            {
                fluxFichier?.Close();
                fluxFichier?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Erreur lors de la fermeture du logger : {ex.Message}");
            }
            
            estInitialise = false;
        }
        
        
        // █ MÉTHODES PRIVÉES █
        
        /// <summary>
        /// Obtient le préfixe textuel selon le niveau de log.
        /// </summary>
        private static string ObtenirPrefixe(NiveauLog niveau)
        {
            return niveau switch
            {
                NiveauLog.Debug => "[DEBUG]",
                NiveauLog.Info => "[INFO]",
                NiveauLog.Attention => "[ATTENTION]",
                NiveauLog.Erreur => "[ERREUR]",
                _ => "[???]"
            };
        }
        
        /// <summary>
        /// Écrit dans la console avec une couleur selon le niveau de gravité.
        /// </summary>
        private static void EcrireConsoleAvecCouleur(string message, NiveauLog niveau)
        {
            // Sauvegarde de la couleur actuelle
            ConsoleColor couleurOriginale = Console.ForegroundColor;
            
            // Changement de couleur selon le niveau
            Console.ForegroundColor = niveau switch
            {
                NiveauLog.Debug => ConsoleColor.Gray,
                NiveauLog.Info => ConsoleColor.White,
                NiveauLog.Attention => ConsoleColor.Yellow,
                NiveauLog.Erreur => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
            
            // Écriture du message
            Console.WriteLine(message);
            
            // Restauration de la couleur originale
            Console.ForegroundColor = couleurOriginale;
        }
    }
}
