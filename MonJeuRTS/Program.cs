// ═══════════════════════════════════════════════════════════
// FICHIER : Program.cs
// DESCRIPTION : Point d'entrée principal de l'application
// RÔLE : Initialise et lance le jeu
// ═══════════════════════════════════════════════════════════

using MonJeuRTS.Moteur;
using MonJeuRTS.Utilitaires;

namespace MonJeuRTS
{
    /// <summary>
    /// Classe principale contenant le point d'entrée de l'application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Point d'entrée principal du programme.
        /// Cette méthode est appelée au lancement de l'application.
        /// </summary>
        /// <param name="args">Arguments de ligne de commande (non utilisés pour l'instant)</param>
        static void Main(string[] args)
        {
            // Initialisation du système de logs
            // Cela permet de déboguer facilement en affichant des messages
            Logger.Initialiser("logs_jeu.txt");
            Logger.Ecrire("=== DÉMARRAGE DU JEU RTS ===", NiveauLog.Info);
            
            // Création de l'instance principale du jeu
            // JeuPrincipal gère toute la boucle de jeu (Update/Draw)
            JeuPrincipal jeu = new JeuPrincipal();
            
            // Lancement de la boucle principale
            // Cette méthode ne retournera que lorsque le joueur ferme le jeu
            jeu.Executer();
            
            // Nettoyage et fermeture propre
            Logger.Ecrire("=== FERMETURE DU JEU ===", NiveauLog.Info);
            Logger.Fermer();
        }
    }
}
