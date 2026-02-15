// ═══════════════════════════════════════════════════════════
// FICHIER : GestionnaireEtats.cs
// DESCRIPTION : Gère les transitions entre les différents états du jeu
// PATTERN : State Machine (Machine à états)
// RESPONSABILITÉ : Changer d'état et déléguer Update/Draw à l'état actuel
// ═══════════════════════════════════════════════════════════

using MonJeuRTS.Etats;
using MonJeuRTS.Utilitaires;

namespace MonJeuRTS.Moteur
{
    /// <summary>
    /// Gestionnaire centralisé des états du jeu.
    /// 
    /// Principe de fonctionnement :
    /// 1. Le jeu a toujours UN état actif (Menu, Partie ou Pause)
    /// 2. Quand on change d'état :
    ///    - L'ancien état est "fermé" (Sortir() est appelé)
    ///    - Le nouvel état est "ouvert" (Entrer() est appelé)
    /// 3. À chaque frame, on délègue Update et Draw à l'état actuel
    /// 
    /// Exemple de flux :
    /// Menu → (clic "Jouer") → Partie → (appui Échap) → Pause → (clic "Reprendre") → Partie
    /// </summary>
    public class GestionnaireEtats
    {
        // █ PROPRIÉTÉS PRIVÉES █
        
        // L'état actuellement actif
        private IEtatJeu? etatActuel;
        
        // L'état qui était actif avant la pause (pour revenir après)
        private IEtatJeu? etatAvantPause;
        
        
        // █ PROPRIÉTÉS PUBLIQUES █
        
        /// <summary>
        /// Indique si le jeu est actuellement en pause.
        /// Utilisé pour savoir si on peut revenir à l'état précédent.
        /// </summary>
        public bool EstEnPause { get; private set; }
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise le gestionnaire sans état actif.
        /// L'état initial sera défini par JeuPrincipal.
        /// </summary>
        public GestionnaireEtats()
        {
            etatActuel = null;
            etatAvantPause = null;
            EstEnPause = false;
        }
        
        
        // █ MÉTHODES PUBLIQUES █
        
        /// <summary>
        /// Change l'état actuel du jeu.
        /// Gère automatiquement l'appel à Sortir() et Entrer().
        /// </summary>
        /// <param name="nouvelEtat">Le nouvel état vers lequel transitionner</param>
        public void ChangerEtat(IEtatJeu nouvelEtat)
        {
            if (nouvelEtat == null)
            {
                Logger.Ecrire("Tentative de changement vers un état null !", NiveauLog.Erreur);
                return;
            }
            
            // Log du changement d'état
            string nomAncienEtat = etatActuel?.GetType().Name ?? "Aucun";
            string nomNouvelEtat = nouvelEtat.GetType().Name;
            Logger.Ecrire($"Changement d'état : {nomAncienEtat} → {nomNouvelEtat}", NiveauLog.Info);
            
            // Sortie de l'ancien état (nettoyage)
            etatActuel?.Sortir();
            
            // Transition vers le nouvel état
            etatActuel = nouvelEtat;
            
            // Entrée dans le nouvel état (initialisation)
            etatActuel.Entrer();
            
            // Réinitialise le flag de pause (sauf si on entre en pause)
            if (nouvelEtat.GetType().Name != "EtatPause")
            {
                EstEnPause = false;
            }
        }
        
        /// <summary>
        /// Met le jeu en pause et sauvegarde l'état actuel.
        /// Permet de revenir à l'état précédent après.
        /// </summary>
        /// <param name="etatPause">L'état de pause à afficher</param>
        public void MettreEnPause(IEtatJeu etatPause)
        {
            if (EstEnPause)
            {
                Logger.Ecrire("Le jeu est déjà en pause !", NiveauLog.Attention);
                return;
            }
            
            // Sauvegarde l'état actuel pour pouvoir y revenir
            etatAvantPause = etatActuel;
            EstEnPause = true;
            
            Logger.Ecrire("Mise en pause du jeu", NiveauLog.Info);
            
            // Change vers l'état de pause
            ChangerEtat(etatPause);
        }
        
        /// <summary>
        /// Reprend le jeu depuis la pause.
        /// Retourne à l'état qui était actif avant la pause.
        /// </summary>
        public void ReprendreDepuisPause()
        {
            if (!EstEnPause)
            {
                Logger.Ecrire("Le jeu n'est pas en pause !", NiveauLog.Attention);
                return;
            }
            
            if (etatAvantPause == null)
            {
                Logger.Ecrire("Aucun état sauvegardé avant la pause !", NiveauLog.Erreur);
                return;
            }
            
            Logger.Ecrire("Reprise du jeu depuis la pause", NiveauLog.Info);
            
            // Retourne à l'état précédent
            IEtatJeu etatASauvegarder = etatAvantPause;
            etatAvantPause = null;
            EstEnPause = false;
            
            ChangerEtat(etatASauvegarder);
        }
        
        /// <summary>
        /// Met à jour l'état actuel.
        /// Délègue simplement l'appel à l'état courant.
        /// </summary>
        /// <param name="deltaTime">Temps écoulé depuis la dernière frame</param>
        public void MettreAJour(float deltaTime)
        {
            etatActuel?.MettreAJour(deltaTime);
        }
        
        /// <summary>
        /// Dessine l'état actuel.
        /// Délègue simplement l'appel à l'état courant.
        /// </summary>
        public void Dessiner()
        {
            etatActuel?.Dessiner();
        }
    }
}
