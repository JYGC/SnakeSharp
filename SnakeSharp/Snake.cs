using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeSharp
{
    internal enum SnakeDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    internal interface ISnake
    {
        void Put();
        void Grow();
        void Move();
        Position GetHead();
        SnakeDirection Direction { get; set; }
        Position GetNextHead();
        public void GetUserInput(ref bool continueGame);
    }
    internal class Snake : ISnake
    {
        private readonly int __startingLength = 5;

        private IBoard __board;
        private List<Position> __positions;
        public Snake(IBoard board)
        {
            __board = board;
            __positions = new List<Position>();
            for (int i = 0; i < __startingLength; i++)
            {
                __positions.Add(new Position
                {
                    Left = __board.Width / 2 - __startingLength / 2 + i,
                    Top = __board.Height / 2,
                });
            }
        }
        public void Put()
        {
            for (int i = 0; i < __positions.Count; i++)
            {
                __board.PutObjects(__positions[i].Left, __positions[i].Top, CellType.Snake);
            }
        }
        private readonly int __growthRate = 1;
        private int __growth = 0;
        public void Grow()
        {
            __growth += __growthRate;
        }
        public void Move()
        {
            Position newHead = GetNextHead();
            __board.PutObjects(newHead.Left, newHead.Top, CellType.Snake);
            __positions.Insert(0, newHead);
            if (__growth > 0)
            {
                __growth--;
                return;
            }
            Position oldTail = __positions[__positions.Count() - 1];
            __board.PutObjects(oldTail.Left, oldTail.Top, CellType.Empty);
            __positions.Remove(oldTail);
        }
        public Position GetHead()
        {
            return __positions[0];
        }
        public SnakeDirection Direction { get; set; } = SnakeDirection.Left;
        public Position GetNextHead()
        {
            switch (Direction)
            {
                case SnakeDirection.Up:
                    return new Position
                    {
                        Left = __positions[0].Left,
                        Top = __positions[0].Top - 1,
                    };
                case SnakeDirection.Right:
                    return new Position
                    {
                        Left = __positions[0].Left + 1,
                        Top = __positions[0].Top,
                    };
                case SnakeDirection.Down:
                    return new Position
                    {
                        Left = __positions[0].Left,
                        Top = __positions[0].Top + 1,
                    };
                default: // SnakeDirection.Left:
                    return new Position
                    {
                        Left = __positions[0].Left - 1,
                        Top = __positions[0].Top,
                    };
            }
        }
        public void GetUserInput(ref bool continueGame)
        {
            while (continueGame)
            {
                // When GAME OVER, this line will wait for next key stroke before letting app close
                ConsoleKey keyInput = Console.ReadKey(continueGame).Key;
                switch (keyInput)
                {
                    case ConsoleKey.UpArrow:
                        if (Direction != SnakeDirection.Down) Direction = SnakeDirection.Up;
                        continue;
                    case ConsoleKey.RightArrow:
                        if (Direction != SnakeDirection.Left) Direction = SnakeDirection.Right;
                        continue;
                    case ConsoleKey.DownArrow:
                        if (Direction != SnakeDirection.Up) Direction = SnakeDirection.Down;
                        continue;
                    case ConsoleKey.LeftArrow:
                        if (Direction != SnakeDirection.Right) Direction = SnakeDirection.Left;
                        continue;
                    default:
                        continue;
                }
            }
        }
    }
}
