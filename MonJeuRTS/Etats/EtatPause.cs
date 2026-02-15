// ═══════════════════════════════════════════════════════════
// FICHIER : EtatPause.cs
// DESCRIPTION : État du menu pause
// RESPONSABILITÉS : Afficher le menu pause et gérer les boutons
// ═══════════════════════════════════════════════════════════

using System.Numerics;
using Raylib_cs;
using MonJeuRTS.Utilitaires;
using MonJeuRTS.Moteur;

namespace MonJeuRTS.Etats
{
    /// <summary>
    /// État du menu pause.
    /// 
    /// Fonctionnalités :
    /// - Affiche "PAUSE" à l'écran
    /// - Bouton "Reprendre" pour retourner au jeu
    /// - Bouton "Quitter" pour retourner au menu principal
    /// - Touche Échap pour reprendre rapidement
    /// </summary>
    public class EtatPause : IEtatJeu
    {
        // █ PROPRIÉTÉS PRIVÉES █
        
        // Référence vers le gestionnaire d'états
        private readonly Action<IEtatJeu> changerEtatCallback;
        
        // Référence vers le gestionnaire d'états (pour reprendre depuis pause)
        private GestionnaireEtats? gestionnaireEtats;
        
        // Rectangles des boutons
        private Rectangle rectangleBoutonReprendre;
        private Rectangle rectangleBoutonQuitter;
        
        // Couleurs des boutons (changent au survol)
        private Color couleurBoutonReprendre;
        private Color couleurBoutonQuitter;
        
        
        // █ CONSTANTES █
        
        private const int LARGEUR_BOUTON = 250;
        private const int HAUTEUR_BOUTON = 60;
        private const int ESPACEMENT_BOUTONS = 20;
        
        
        // █ CONSTRUCTEUR █
        
        /// <summary>
        /// Initialise l'état de pause.
        /// </summary>
        /// <param name="changerEtat">Callback pour changer d'état</param>
        public EtatPause(Action<IEtatJeu> changerEtat)
        {
            this.changerEtatCallback = changerEtat;
        }
        
        /// <summary>
        /// Définit la référence au gestionnaire d'états.
        /// Nécessaire pour utiliser ReprendreDepuisPause().
        /// </summary>
        public void DefinirGestionnaireEtats(GestionnaireEtats gestionnaire)
        {
            this.gestionnaireEtats = gestionnaire;
        }
        
        
        // █ IMPLÉMENTATION DE IEtatJeu █
        
        /// <summary>
        /// Appelé à l'entrée en pause.
        /// Initialise les positions des boutons.
        /// </summary>
        public void Entrer()
        {
            Logger.Ecrire("Entrée dans l'état Pause", NiveauLog.Info);
            
            // Centre les boutons verticalement et horizontalement
            int centreX = (1280 - LARGEUR_BOUTON) / 2;
            int centreY = 720 / 2;
            
            // Bouton "Reprendre" (au-dessus du centre)
            rectangleBoutonReprendre = new Rectangle(
                centreX,
                centreY - HAUTEUR_BOUTON - ESPACEMENT_BOUTONS / 2,
                LARGEUR_BOUTON,
                HAUTEUR_BOUTON
            );
            
            // Bouton "Quitter" (en dessous du centre)
            rectangleBoutonQuitter = new Rectangle(
                centreX,
                centreY + ESPACEMENT_BOUTONS / 2,
                LARGEUR_BOUTON,
                HAUTEUR_BOUTON
            );
            
            couleurBoutonReprendre = new Color(0, 150, 0, 255);  // Vert foncé
            couleurBoutonQuitter = new Color(150, 0, 0, 255);    // Rouge foncé
        }
        
        /// <summary>
        /// Appelé à la sortie de la pause.
        /// </summary>
        public void Sortir()
        {
            Logger.Ecrire("Sortie de l'état Pause", NiveauLog.Info);
        }
        
        /// <summary>
        /// Mise à jour de la logique du menu pause.
        /// Détecte les survols et clics sur les boutons.
        /// </summary>
        public void MettreAJour(float deltaTime)
        {
            // Position de la souris
            Vector2 positionSouris = Raylib.GetMousePosition();
            
            // Vérification survol bouton "Reprendre"
            bool sourisSurReprendre = Raylib.CheckCollisionPointRec(positionSouris, rectangleBoutonReprendre);
            bool sourisSurQuitter = Raylib.CheckCollisionPointRec(positionSouris, rectangleBoutonQuitter);
            
            // Gestion bouton "Reprendre"
            if (sourisSurReprendre)
            {
                couleurBoutonReprendre = new Color(0, 200, 0, 255);  // Vert clair
                
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    Logger.Ecrire("Bouton 'Reprendre' cliqué", NiveauLog.Info);
                    ReprendreJeu();
                }
            }
            else
            {
                couleurBoutonReprendre = new Color(0, 150, 0, 255);  // Vert foncé
            }
            
            // Gestion bouton "Quitter"
            if (sourisSurQuitter)
            {
                couleurBoutonQuitter = new Color(200, 0, 0, 255);  // Rouge clair
                
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    Logger.Ecrire("Bouton 'Quitter' cliqué - Retour au menu", NiveauLog.Info);
                    changerEtatCallback(new EtatMenu(changerEtatCallback));
                }
            }
            else
            {
                couleurBoutonQuitter = new Color(150, 0, 0, 255);  // Rouge foncé
            }
            
            // Touche Échap pour reprendre rapidement
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                Logger.Ecrire("Touche Échap appuyée - Reprise du jeu", NiveauLog.Info);
                ReprendreJeu();
            }
        }
        
        /// <summary>
        /// Dessine le menu pause à l'écran.
        /// </summary>
        public void Dessiner()
        {
            // Fond semi-transparent noir (assombrit le jeu en dessous)
            Raylib.DrawRectangle(0, 0, 1280, 720, new Color(0, 0, 0, 180));
            
            // Titre "PAUSE"
            string titre = "PAUSE";
            int largeurTitre = Raylib.MeasureText(titre, 80);
            Raylib.DrawText(titre, (1280 - largeurTitre) / 2, 150, 80, Color.White);
            
            // Bouton "Reprendre"
            Raylib.DrawRectangleRec(rectangleBoutonReprendre, couleurBoutonReprendre);
            Raylib.DrawRectangleLinesEx(rectangleBoutonReprendre, 3, Color.White);
            
            string texteReprendre = "REPRENDRE";
            int largeurReprendre = Raylib.MeasureText(texteReprendre, 30);
            int posXReprendre = (int)rectangleBoutonReprendre.X + (LARGEUR_BOUTON - largeurReprendre) / 2;
            int posYReprendre = (int)rectangleBoutonReprendre.Y + (HAUTEUR_BOUTON - 30) / 2;
            Raylib.DrawText(texteReprendre, posXReprendre, posYReprendre, 30, Color.White);
            
            // Bouton "Quitter"
            Raylib.DrawRectangleRec(rectangleBoutonQuitter, couleurBoutonQuitter);
            Raylib.DrawRectangleLinesEx(rectangleBoutonQuitter, 3, Color.White);
            
            string texteQuitter = "QUITTER";
            int largeurQuitter = Raylib.MeasureText(texteQuitter, 30);
            int posXQuitter = (int)rectangleBoutonQuitter.X + (LARGEUR_BOUTON - largeurQuitter) / 2;
            int posYQuitter = (int)rectangleBoutonQuitter.Y + (HAUTEUR_BOUTON - 30) / 2;
            Raylib.DrawText(texteQuitter, posXQuitter, posYQuitter, 30, Color.White);
            
            // Instructions en bas
            string instructions = "Appuyez sur ÉCHAP pour reprendre";
            int largeurInstructions = Raylib.MeasureText(instructions, 16);
            Raylib.DrawText(instructions, (1280 - largeurInstructions) / 2, 650, 16, Color.LightGray);
        }
        
        
        // █ MÉTHODES PRIVÉES █
        
        /// <summary>
        /// Reprend le jeu depuis la pause.
        /// Utilise le gestionnaire d'états pour revenir à l'état précédent.
        /// </summary>
        private void ReprendreJeu()
        {
            if (gestionnaireEtats != null)
            {
                gestionnaireEtats.ReprendreDepuisPause();
            }
            else
            {
                Logger.Ecrire("Erreur : GestionnaireEtats non défini dans EtatPause", NiveauLog.Erreur);
                // Fallback : retour au menu
                changerEtatCallback(new EtatMenu(changerEtatCallback));
            }
        }
    }
}
