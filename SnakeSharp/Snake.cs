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
        void Spawn();
        void Grow();
        void Move();
        Position GetNextHead();
        public void GetUserInput(ref bool continueGame);
    }
    internal class Snake : ISnake
    {
        private readonly int __startingLength = 5;
        private IBoard __board;
        private List<Position> __positions;

        // Creation code

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

        public void Spawn()
        {
            for (int i = 0; i < __positions.Count; i++)
            {
                __board.PutObjects(__positions[i].Left, __positions[i].Top, CellType.Snake);
            }
        }

        // Grow code

        private readonly int __growthRate = 1;
        private int __growth = 0;

        public void Grow()
        {
            __growth += __growthRate;
        }

        // Movement code

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

        private SnakeDirection __direction = SnakeDirection.Left;

        public Position GetNextHead()
        {
            switch (__direction)
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
                        if (__direction != SnakeDirection.Down) __direction = SnakeDirection.Up;
                        continue;
                    case ConsoleKey.RightArrow:
                        if (__direction != SnakeDirection.Left) __direction = SnakeDirection.Right;
                        continue;
                    case ConsoleKey.DownArrow:
                        if (__direction != SnakeDirection.Up) __direction = SnakeDirection.Down;
                        continue;
                    case ConsoleKey.LeftArrow:
                        if (__direction != SnakeDirection.Right) __direction = SnakeDirection.Left;
                        continue;
                    default:
                        continue;
                }
            }
        }
    }
}
