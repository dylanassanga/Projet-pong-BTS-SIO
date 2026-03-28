using System;
using Raylib_cs;

class PongGame
{
    // Dimensions de l'écran
    const int ScreenWidth = 800;
    const int ScreenHeight = 480;

    // Dimensions de la balle
    const int BallSize = 10;

    // Dimensions des raquettes
    const int PaddleWidth = 15;
    const int PaddleHeight = 100;

    // Vitesse de la balle
    const int BallSpeed = 4;

    // Vitesse des raquettes
    const int PaddleSpeed = 4;

    // Score maximum avant la fin de la partie
    const int MaxScore = 14;

    static void Main()
    {
        // Initialisation de la fenêtre de jeu
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Pong Game");
        Raylib.SetTargetFPS(60); // Fixe la fréquence à 60 images par seconde

        // Positions initiales de la balle et des raquettes
        int ballX = ScreenWidth / 2;  // Position X initiale (centre de l'écran)
        int ballY = ScreenHeight / 2; // Position Y initiale (centre de l'écran)
        int ballSpeedX = BallSpeed;  // Direction initiale horizontale de la balle
        int ballSpeedY = BallSpeed;  // Direction initiale verticale de la balle

        int player1Y = (ScreenHeight - PaddleHeight) / 2; // Position Y initiale raquette 1
        int player2Y = (ScreenHeight - PaddleHeight) / 2; // Position Y initiale raquette 2

        // Scores des joueurs
        int scorePlayer1 = 0;
        int scorePlayer2 = 0;

        // Variables pour gérer la fin du jeu
        bool gameOver = false;
        string winnerMessage = "";

        // Boucle principale du jeu
        while (!Raylib.WindowShouldClose()) // Continue tant que la fenêtre est ouverte
        {
            if (!gameOver) // Si le jeu n'est pas terminé
            {
                // Mise à jour des positions des raquettes
                UpdatePaddlePositions(ref player1Y, ref player2Y);

                // Mise à jour de la position de la balle
                ballX += ballSpeedX;
                ballY += ballSpeedY;

                // Collision avec les bords supérieur et inférieur de l'écran
                if (ballY <= 0 || ballY >= ScreenHeight - BallSize)
                {
                    ballSpeedY = -ballSpeedY; // Inverse la direction verticale de la balle
                }

                // Collision entre la balle et les raquettes
                if ((ballX <= 40 + PaddleWidth && ballY >= player1Y && ballY <= player1Y + PaddleHeight) || 
                    (ballX >= ScreenWidth - 40 - PaddleWidth - BallSize && ballY >= player2Y && ballY <= player2Y + PaddleHeight))
                {
                    ballSpeedX = -ballSpeedX; // Inverse la direction horizontale de la balle
                }

                // Gestion des points si la balle sort de l'écran
                if (ballX <= 0) // Si la balle sort à gauche
                {
                    scorePlayer2++; // Le joueur 2 marque un point
                    ResetBall(ref ballX, ref ballY, ref ballSpeedX); // Réinitialise la balle
                }
                else if (ballX >= ScreenWidth) // Si la balle sort à droite
                {
                    scorePlayer1++; // Le joueur 1 marque un point
                    ResetBall(ref ballX, ref ballY, ref ballSpeedX); // Réinitialise la balle
                }

                // Vérifie si un joueur a atteint le score maximum
                if (scorePlayer1 == MaxScore)
                {
                    gameOver = true; // Fin du jeu
                    winnerMessage = "Player 1 Wins!"; // Message de victoire
                }
                else if (scorePlayer2 == MaxScore)
                {
                    gameOver = true; // Fin du jeu
                    winnerMessage = "Player 2 Wins!"; // Message de victoire
                }
            }

            // Dessin et affichage des éléments graphiques
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black); // Nettoie l'écran avec une couleur noire

            if (!gameOver) // Si le jeu est en cours
            {
                DrawDashedLine(ScreenWidth / 2); // Dessine la ligne centrale en pointillés
                DrawScores(scorePlayer1, scorePlayer2); // Affiche les scores
                Raylib.DrawCircle(ballX, ballY, BallSize, Color.White); // Dessine la balle
                Raylib.DrawRectangle(40, player1Y, PaddleWidth, PaddleHeight, Color.Brown); // Raquette 1
                Raylib.DrawRectangle(ScreenWidth - 40 - PaddleWidth, player2Y, PaddleWidth, PaddleHeight, Color.Brown); // Raquette 2
            }
            else // Si le jeu est terminé
            {
                // Affiche le message de victoire
                Raylib.DrawText(winnerMessage, ScreenWidth / 2 - 150, ScreenHeight / 2 - 50, 40, Color.Yellow);
                Raylib.DrawText("Press [Enter] to Exit", ScreenWidth / 2 - 150, ScreenHeight / 2 + 10, 20, Color.White);

                // Quitte le jeu si le joueur appuie sur Entrée
                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    break;
                }
            }

            Raylib.EndDrawing();
        }

        // Ferme la fenêtre de jeu
        Raylib.CloseWindow();
    }

    // Met à jour les positions des raquettes selon les touches pressées
    static void UpdatePaddlePositions(ref int player1Y, ref int player2Y)
    {
        if (Raylib.IsKeyDown(KeyboardKey.Z)) player1Y -= PaddleSpeed; // Joueur 1 monte
        if (Raylib.IsKeyDown(KeyboardKey.S)) player1Y += PaddleSpeed; // Joueur 1 descend

        if (Raylib.IsKeyDown(KeyboardKey.Up)) player2Y -= PaddleSpeed; // Joueur 2 monte
        if (Raylib.IsKeyDown(KeyboardKey.Down)) player2Y += PaddleSpeed; // Joueur 2 descend

        // Contraint les raquettes à rester dans les limites de l'écran
        player1Y = Math.Clamp(player1Y, 0, ScreenHeight - PaddleHeight);
        player2Y = Math.Clamp(player2Y, 0, ScreenHeight - PaddleHeight);
    }

    // Dessine une ligne centrale en pointillés
    static void DrawDashedLine(int centerX)
    {
        int dashHeight = 10; // Taille d'un segment
        int gapHeight = 5;   // Taille de l'écart

        for (int y = 0; y < ScreenHeight; y += dashHeight + gapHeight)
        {
            Raylib.DrawRectangle(centerX - 1, y, 2, dashHeight, Color.White); // Segment blanc
        }
    }

    // Affiche les scores des deux joueurs
    static void DrawScores(int scorePlayer1, int scorePlayer2)
    {
        Raylib.DrawText(scorePlayer1.ToString(), ScreenWidth / 2 - 100, 20, 40, Color.White); // Score joueur 1
        Raylib.DrawText(scorePlayer2.ToString(), ScreenWidth / 2 + 50, 20, 40, Color.White); // Score joueur 2
    }

    // Réinitialise la position et la direction de la balle
    static void ResetBall(ref int ballX, ref int ballY, ref int ballSpeedX)
    {
        ballX = ScreenWidth / 2; // Centre horizontal
        ballY = ScreenHeight / 2; // Centre vertical
        ballSpeedX = -ballSpeedX; // Change la direction initiale
    }
}
