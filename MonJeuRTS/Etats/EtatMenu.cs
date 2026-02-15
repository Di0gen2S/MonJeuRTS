// ═══════════════════════════════════════════════════════════
// FICHIER : EtatMenu.cs
// DESCRIPTION : État du menu principal
// RESPONSABILITÉS : Afficher l'écran de démarrage et démarrer la partie
// ═══════════════════════════════════════════════════════════

using System.Numerics;
using Raylib_cs;
using MonJeuRTS.Utilitaires;

namespace MonJeuRTS.Etats
{
    /// <summary>
    /// État du menu principal affiché au démarrage du jeu.
    /// 
    /// Fonctionnalités :
    /// - Affiche le titre du jeu
    /// - Affiche un bouton "Jouer"
    /// - Détecte le clic sur le bouton pour démarrer la partie
    /// </summary>
    public class EtatMenu : IEtatJeu
    {
        // █ PROPRIÉTÉS PRIVÉES █
        
        // Référence vers le gestionnaire d'états (pour pouvoir changer d'état)
        private readonly Action<IEtatJeu> changerEtatCallback;
        
        // Position et taille du bouton "Jouer"
        private Rectangle rectangleBoutonJouer;
        
        // Couleur du bouton (change au survol)
        private Color couleurBouton;
        
        
        // █ CONSTANTES █
        
        private const int LARGEUR_BOUTON = 200;
        private const int HAUTEUR_BOUTON = 60;
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise l'état du menu.
        /// </summary>
        /// <param name="changerEtat">Callback pour changer d'état (fourni par JeuPrincipal)</param>
        public EtatMenu(Action<IEtatJeu> changerEtat)
        {
            this.changerEtatCallback = changerEtat;
        }
        
        
        // █ IMPLÉMENTATION DE IEtatJeu █
        
        /// <summary>
        /// Appelé à l'entrée dans le menu.
        /// Initialise la position du bouton.
        /// </summary>
        public void Entrer()
        {
            Logger.Ecrire("Entrée dans l'état Menu", NiveauLog.Info);
            
            // Centre le bouton à l'écran
            int posX = (1280 - LARGEUR_BOUTON) / 2;
            int posY = (720 / 2) + 50;  // Un peu en dessous du centre
            
            rectangleBoutonJouer = new Rectangle(posX, posY, LARGEUR_BOUTON, HAUTEUR_BOUTON);
            couleurBouton = Color.DarkGreen;
        }
        
        /// <summary>
        /// Appelé à la sortie du menu.
        /// Rien à nettoyer pour l'instant.
        /// </summary>
        public void Sortir()
        {
            Logger.Ecrire("Sortie de l'état Menu", NiveauLog.Info);
        }
        
        /// <summary>
        /// Mise à jour de la logique du menu.
        /// Détecte le survol et le clic sur le bouton.
        /// </summary>
        public void MettreAJour(float deltaTime)
        {
            // Position de la souris
            Vector2 positionSouris = Raylib.GetMousePosition();
            
            // Vérifie si la souris survole le bouton
            bool sourisSurBouton = Raylib.CheckCollisionPointRec(positionSouris, rectangleBoutonJouer);
            
            // Change la couleur du bouton au survol
            if (sourisSurBouton)
            {
                couleurBouton = Color.Green;
                
                // Détecte le clic
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    Logger.Ecrire("Bouton 'Jouer' cliqué - Démarrage de la partie", NiveauLog.Info);
                    
                    // Change vers l'état Partie
                    changerEtatCallback(new EtatPartie(changerEtatCallback));
                }
            }
            else
            {
                couleurBouton = Color.DarkGreen;
            }
        }
        
        /// <summary>
        /// Dessine le menu à l'écran.
        /// </summary>
        public void Dessiner()
        {
            // Fond bleu foncé
            Raylib.ClearBackground(new Color(20, 30, 50, 255));
            
            // Titre du jeu
            string titre = "MON JEU RTS";
            int largeurTitre = Raylib.MeasureText(titre, 60);
            Raylib.DrawText(titre, (1280 - largeurTitre) / 2, 200, 60, Color.Gold);
            
            // Sous-titre
            string sousTitre = "Command & Conquer Style";
            int largeurSousTitre = Raylib.MeasureText(sousTitre, 20);
            Raylib.DrawText(sousTitre, (1280 - largeurSousTitre) / 2, 270, 20, Color.LightGray);
            
            // Bouton "Jouer"
            Raylib.DrawRectangleRec(rectangleBoutonJouer, couleurBouton);
            Raylib.DrawRectangleLinesEx(rectangleBoutonJouer, 3, Color.White);
            
            // Texte du bouton
            string texteBouton = "JOUER";
            int largeurTexte = Raylib.MeasureText(texteBouton, 30);
            int posXTexte = (int)rectangleBoutonJouer.X + (LARGEUR_BOUTON - largeurTexte) / 2;
            int posYTexte = (int)rectangleBoutonJouer.Y + (HAUTEUR_BOUTON - 30) / 2;
            Raylib.DrawText(texteBouton, posXTexte, posYTexte, 30, Color.White);
            
            // Instructions en bas
            string instructions = "Cliquez sur 'Jouer' pour commencer";
            int largeurInstructions = Raylib.MeasureText(instructions, 16);
            Raylib.DrawText(instructions, (1280 - largeurInstructions) / 2, 650, 16, Color.Gray);
        }
    }
}
