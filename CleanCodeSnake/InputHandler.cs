using System;
using static System.Console;

namespace Snake
{
    internal class InputHandler
    {
        public static Direction ReadMovement(Direction movement)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && movement != Direction.Down) return Direction.Up;
                if (key == ConsoleKey.DownArrow && movement != Direction.Up) return Direction.Down;
                if (key == ConsoleKey.LeftArrow && movement != Direction.Right) return Direction.Left;
                if (key == ConsoleKey.RightArrow && movement != Direction.Left) return Direction.Right;
            }
            return movement;
        }
    }
}
