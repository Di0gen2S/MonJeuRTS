using Raylib_cs;

namespace MonJeuRTS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialisation de la fenêtre
            Raylib.InitWindow(800, 600, "Test Raylib - Mon RTS");
            Raylib.SetTargetFPS(60);

            // Boucle principale
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGreen);
                Raylib.DrawText("Raylib fonctionne !", 250, 280, 30, Color.White);
                Raylib.EndDrawing();
            }

            // Fermeture
            Raylib.CloseWindow();
        }
    }
}