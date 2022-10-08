using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeSharp
{
    internal class Game
    {
        const string __GAMEOVERMSG = "GAME OVER!";
        const string __HITWALLMG = "YOU HIT A WALL";
        const string __HITSELFMG = "YOU HIT YOURSELF";

        private IBoard __board;
        private ISnake __snake;
        private IFood __food;
        private int __score = 0;
        private int __scoreIncrement = 1;
        private bool __continue = true;
        private bool __hasSnakeHitBorder = false;
        private bool __hasSnakeHitSelf = false;
        private int __frameInterval = 100;
        public Game(IBoard board, ISnake snake, IFood food)
        {
            __board = board;
            __snake = snake;
            __food = food;
        }

        public void Start()
        {
            Console.CursorVisible = false;
            __board.Create();
            __snake.Put();
            __food.Create();
            __food.Put();
            Thread getUserInputThread = new Thread(() =>
            {
                __snake.GetUserInput(ref __continue);
            });
            getUserInputThread.Start();
            __PrintScore();
            do
            {
                __snake.Move();
                Thread.Sleep(__frameInterval);
                __hasSnakeHitBorder = __board.IsPositionBorder(__snake.GetNextHead().Left, __snake.GetNextHead().Top);
                __hasSnakeHitSelf = __board.IsPositionSnake(__snake.GetNextHead().Left, __snake.GetNextHead().Top);
                __continue = !__hasSnakeHitBorder && !__hasSnakeHitSelf;
                if (__board.IsPositionFood(__snake.GetNextHead().Left, __snake.GetNextHead().Top))
                {
                    __food.Eaten();
                    __food.Put();
                    __snake.Grow();
                    __score += __scoreIncrement;
                    __PrintScore();
                }
            }
            while (__continue);
            __PrintGameOver();
        }

        static void GetUserInput(ISnake snake, ref bool continueGame)
        {
            while (continueGame)
            {
                // When GAME OVER, this line will wait for next key stroke before letting app close
                ConsoleKey keyInput = Console.ReadKey(continueGame).Key;
                switch (keyInput)
                {
                    case ConsoleKey.UpArrow:
                        if (snake.Direction != SnakeDirection.Down) snake.Direction = SnakeDirection.Up;
                        continue;
                    case ConsoleKey.RightArrow:
                        if (snake.Direction != SnakeDirection.Left) snake.Direction = SnakeDirection.Right;
                        continue;
                    case ConsoleKey.DownArrow:
                        if (snake.Direction != SnakeDirection.Up) snake.Direction = SnakeDirection.Down;
                        continue;
                    case ConsoleKey.LeftArrow:
                        if (snake.Direction != SnakeDirection.Right) snake.Direction = SnakeDirection.Left;
                        continue;
                    default:
                        continue;
                }
            }
        }

        private void __PrintGameOver()
        {
            string errorMsg = __hasSnakeHitSelf ? __HITSELFMG : __HITWALLMG;
            errorMsg = $"{__GAMEOVERMSG} {errorMsg}";
            Console.SetCursorPosition(__board.Width / 2 - errorMsg.Length / 2, __board.Height / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(errorMsg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void __PrintScore()
        {
            Console.SetCursorPosition(0, __board.Height);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Score: {__score}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
