using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    class Game
    {
        private const int WindowHeight = 16;
        private const int WindowWidth = 32;
        private const int InitialScore = 5;
        private List<Pixel> snakeBody;
        private Pixel snakeHead;
        private Pixel berry;
        private Direction currentDirection;
        private int score;
        private bool isGameOver;
        private Random random;

        public Game()
        {
            random = new Random();
            score = InitialScore;
            snakeHead = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            berry = GenerateBerry();
            snakeBody = new List<Pixel>();
            currentDirection = Direction.Right;
            isGameOver = false;
        }

        public void Run()
        {
            while (!isGameOver)
            {
                Clear();
                isGameOver |= CheckCollision();
                DrawBorder();
                HandleBerryConsumption();
                DrawSnake();

                if (isGameOver)
                {
                    EndGame();
                    return;
                }

                MoveSnake();
            }
        }

        private Pixel GenerateBerry()
        {
            return new Pixel(random.Next(1, WindowWidth - 2), random.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
        }

        private bool CheckCollision()
        {
            return snakeHead.XPos == 0 || snakeHead.XPos == WindowWidth - 1 ||
                   snakeHead.YPos == 0 || snakeHead.YPos == WindowHeight - 1 ||
                   snakeBody.Exists(pixel => pixel.XPos == snakeHead.XPos && pixel.YPos == snakeHead.YPos);
        }

        private void HandleBerryConsumption()
        {
            if (snakeHead.XPos == berry.XPos && snakeHead.YPos == berry.YPos)
            {
                score++;
                berry = GenerateBerry();
            }
        }

        private void DrawSnake()
        {
            foreach (var pixel in snakeBody)
            {
                DrawPixel(pixel);
            }

            DrawPixel(snakeHead);
            DrawPixel(berry);
        }

        private void MoveSnake()
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= 500)
            {
                currentDirection = InputHandler.ReadMovement(currentDirection);
            }

            snakeBody.Add(new Pixel(snakeHead.XPos, snakeHead.YPos, ConsoleColor.Green));

            switch (currentDirection)
            {
                case Direction.Up:
                    snakeHead.YPos--;
                    break;
                case Direction.Down:
                    snakeHead.YPos++;
                    break;
                case Direction.Left:
                    snakeHead.XPos--;
                    break;
                case Direction.Right:
                    snakeHead.XPos++;
                    break;
            }

            if (snakeBody.Count > score)
            {
                snakeBody.RemoveAt(0);
            }
        }

        private void EndGame()
        {
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            WriteLine($"Game over, Score: {score - InitialScore}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            ReadKey();
        }

        private static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        private static void DrawBorder()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");
                SetCursorPosition(i, WindowHeight - 1);
                Write("■");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");
                SetCursorPosition(WindowWidth - 1, i);
                Write("■");
            }
        }
    }
}
