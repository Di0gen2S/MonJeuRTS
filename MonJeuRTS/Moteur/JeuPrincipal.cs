// ═══════════════════════════════════════════════════════════
// FICHIER : JeuPrincipal.cs
// DESCRIPTION : Classe principale gérant la boucle de jeu
// RESPONSABILITÉS : Initialisation, Update, Draw, gestion fenêtre
// PATTERN : Game Loop (boucle de jeu classique)
// ═══════════════════════════════════════════════════════════

using Raylib_cs;
using MonJeuRTS.Etats;
using MonJeuRTS.Utilitaires;

namespace MonJeuRTS.Moteur
{
    /// <summary>
    /// Classe principale du jeu gérant la boucle de jeu et l'initialisation.
    /// 
    /// Architecture :
    /// 1. Initialisation (fenêtre, ressources)
    /// 2. Boucle principale :
    ///    - Update : Met à jour la logique
    ///    - Draw : Dessine à l'écran
    /// 3. Fermeture (nettoyage des ressources)
    /// 
    /// Cette classe orchestre tous les autres systèmes :
    /// - GestionnaireEtats (Menu, Partie, Pause)
    /// - Horloge (temps, FPS)
    /// - Logger (logs de débogage)
    /// </summary>
    public class JeuPrincipal
    {
        // █ CONSTANTES █
        
        private const int LARGEUR_FENETRE = 1280;
        private const int HAUTEUR_FENETRE = 720;
        private const string TITRE_FENETRE = "Mon Jeu RTS - Phase 1";
        private const int FPS_CIBLE = 60;
        
        
        // █ PROPRIÉTÉS PRIVÉES █
        
        // Gestionnaire des états du jeu (Menu, Partie, Pause)
        private GestionnaireEtats? gestionnaireEtats;
        
        // Horloge pour gérer le temps
        private Horloge? horloge;
        
        // Flag pour savoir si le jeu doit continuer à tourner
        private bool jeuActif;
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise le jeu mais ne le démarre pas encore.
        /// Le démarrage se fait via la méthode Executer().
        /// </summary>
        public JeuPrincipal()
        {
            jeuActif = false;
            gestionnaireEtats = null;
            horloge = null;
        }
        
        
        // █ MÉTHODES PUBLIQUES █
        
        /// <summary>
        /// Lance le jeu (initialisation + boucle principale).
        /// Cette méthode bloque jusqu'à la fermeture du jeu.
        /// </summary>
        public void Executer()
        {
            Initialiser();
            BouclePrincipale();
            Fermer();
        }
        
        
        // █ MÉTHODES PRIVÉES - CYCLE DE VIE █
        
        /// <summary>
        /// Initialise tous les systèmes du jeu.
        /// Appelé UNE SEULE FOIS au démarrage.
        /// </summary>
        private void Initialiser()
        {
            Logger.Ecrire("=== INITIALISATION DU JEU ===", NiveauLog.Info);
            
            // 1. Initialisation de la fenêtre Raylib
            Logger.Ecrire($"Création de la fenêtre {LARGEUR_FENETRE}x{HAUTEUR_FENETRE}", NiveauLog.Info);
            Raylib.InitWindow(LARGEUR_FENETRE, HAUTEUR_FENETRE, TITRE_FENETRE);
            Raylib.SetTargetFPS(FPS_CIBLE);
            Raylib.SetExitKey(KeyboardKey.Null);
            
            // 2. Initialisation de l'horloge
            horloge = new Horloge();
            Logger.Ecrire("Horloge initialisée", NiveauLog.Info);
            
            // 3. Initialisation du gestionnaire d'états
            gestionnaireEtats = new GestionnaireEtats();
            Logger.Ecrire("Gestionnaire d'états initialisé", NiveauLog.Info);
            
            // 4. Définition de l'état initial (Menu)
            EtatMenu menuInitial = new EtatMenu(ChangerEtat);
            gestionnaireEtats?.ChangerEtat(menuInitial);
            
            // 5. Le jeu est prêt à tourner
            jeuActif = true;
            Logger.Ecrire("=== INITIALISATION TERMINÉE ===", NiveauLog.Info);
        }
        
        /// <summary>
        /// Boucle principale du jeu (Update + Draw).
        /// Tourne à 60 FPS jusqu'à ce que l'utilisateur ferme la fenêtre.
        /// </summary>
        private void BouclePrincipale()
        {
            Logger.Ecrire("=== ENTRÉE DANS LA BOUCLE PRINCIPALE ===", NiveauLog.Info);
            
            // Boucle infinie jusqu'à fermeture de la fenêtre
            while (jeuActif && !Raylib.WindowShouldClose())
            {
                // 1. Mise à jour de l'horloge
                horloge?.MettreAJour();
                
                // 2. Mise à jour de la logique du jeu
                MettreAJour(horloge?.DeltaTime ?? 0f);
                
                // 3. Dessin du jeu à l'écran
                Dessiner();
            }
            
            Logger.Ecrire("=== SORTIE DE LA BOUCLE PRINCIPALE ===", NiveauLog.Info);
        }
        
        /// <summary>
        /// Ferme proprement le jeu et libère les ressources.
        /// Appelé UNE SEULE FOIS à la fin.
        /// </summary>
        private void Fermer()
        {
            Logger.Ecrire("=== FERMETURE PROPRE DU JEU ===", NiveauLog.Info);
            
            // Fermeture de la fenêtre Raylib
            Raylib.CloseWindow();
            Logger.Ecrire("Fenêtre Raylib fermée", NiveauLog.Info);
            
            // TODO Phase 2+ : Libérer les textures chargées
            // TODO Phase 2+ : Sauvegarder les préférences du joueur
        }
        
        
        // █ MÉTHODES PRIVÉES - BOUCLE DE JEU █
        
        /// <summary>
        /// Met à jour la logique du jeu.
        /// Appelé 60 fois par seconde (60 FPS).
        /// </summary>
        /// <param name="deltaTime">Temps écoulé depuis la dernière frame (en secondes)</param>
        private void MettreAJour(float deltaTime)
        {
            // Délègue la mise à jour à l'état actuel
            gestionnaireEtats?.MettreAJour(deltaTime);
            
            // TODO Phase 2+ : Mettre à jour les systèmes globaux
            // Exemple : système de particules, musique de fond, etc.
        }
        
        /// <summary>
        /// Dessine le jeu à l'écran.
        /// Appelé 60 fois par seconde (60 FPS).
        /// </summary>
        private void Dessiner()
        {
            // Début du dessin Raylib
            Raylib.BeginDrawing();
            
            // Délègue le dessin à l'état actuel
            gestionnaireEtats?.Dessiner();
            
            // Affichage du FPS en haut à droite (pour débogage)
            AfficherFPS();
            
            // Fin du dessin Raylib
            Raylib.EndDrawing();
        }
        
        
        // █ MÉTHODES PRIVÉES - HELPERS █
        
        /// <summary>
        /// Callback pour changer d'état depuis les états eux-mêmes.
        /// Cette méthode est passée en paramètre aux états.
        /// </summary>
        /// <param name="nouvelEtat">Le nouvel état vers lequel transitionner</param>
        private void ChangerEtat(IEtatJeu nouvelEtat)
        {
            // Cas spécial : si on passe en pause, utiliser MettreEnPause
            if (nouvelEtat is EtatPause etatPause)
            {
                // Donne la référence au gestionnaire pour pouvoir reprendre
                if (gestionnaireEtats != null)
                {
                    etatPause.DefinirGestionnaireEtats(gestionnaireEtats);
                }
                gestionnaireEtats?.MettreEnPause(etatPause);
            }
            else
            {
                // Changement d'état normal
                gestionnaireEtats?.ChangerEtat(nouvelEtat);
            }
        }
        
        /// <summary>
        /// Affiche les FPS en haut à droite de l'écran.
        /// Utile pour vérifier les performances.
        /// </summary>
        private void AfficherFPS()
        {
            string texteFPS = $"FPS: {horloge?.FPS ?? 0}";
            int largeurTexte = Raylib.MeasureText(texteFPS, 20);
            
            // Position en haut à droite avec un peu de marge
            int posX = LARGEUR_FENETRE - largeurTexte - 10;
            int posY = 10;
            
            // Fond semi-transparent noir
            Raylib.DrawRectangle(posX - 5, posY - 5, largeurTexte + 10, 30, new Color(0, 0, 0, 150));
            
            // Texte FPS en vert si >= 60, jaune si >= 30, rouge sinon
            Color couleurFPS = (horloge?.FPS ?? 0) >= 60 ? Color.Green :
                               (horloge?.FPS ?? 0) >= 30 ? Color.Yellow :
                               Color.Red;
            
            Raylib.DrawText(texteFPS, posX, posY, 20, couleurFPS);
        }
    }
}
