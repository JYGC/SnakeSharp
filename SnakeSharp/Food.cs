namespace SnakeSharp
{
    internal enum FoodDirection
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }
    internal interface IFood
    {
        void Create();
        void Eaten();
        int MoveDelay { get; set; }
        void Move(Position nextSnakeHead, int frameInterval);
    }
    internal class Food : IFood
    {
        // Creation code

        private IBoard __board;
        private Position __position;

        public Food(IBoard board)
        {
            __board = board;
            __moveCountDown = MoveDelay;
        }

        public void Create()
        {
            bool tryCreateFood = true;
            while (tryCreateFood)
            {
                __position = __CreateRandomPosition();
                tryCreateFood = !__board.IsPositionEmpty(__position.Left, __position.Top);
            }
            __Put();
            __SetRandomDirection();
        }

        public void Eaten()
        {
            Position oldPosition = __position;
            bool tryCreateFood = true;
            while (tryCreateFood)
            {
                __position = __CreateRandomPosition();
                tryCreateFood = !__board.IsPositionEmpty(__position.Left, __position.Top) || (__position.Left == oldPosition.Left && __position.Top == oldPosition.Top);
            }
            // Old cell will be automatically overwritten by snake no need to call __board.PutObjects(...)
            __Put();
        }

        private void __Put()
        {
            __board.PutObjects(__position.Left, __position.Top, CellType.Food);
        }

        // Movement code

        public int MoveDelay { get; set; } = 0;
        private int __moveCountDown = 0;
        public void Move(Position nextSnakeHead, int frameInterval)
        {
            if (__moveCountDown > 0)
            {
                __moveCountDown -= frameInterval;
                return;
            }
            Random random = new Random();
            if (random.Next(100) < 20) __SetRandomDirection(); // Change direction for no reason
            bool changeDirection;
            do
            {
                changeDirection = __AvoidBorderAndSnake(nextSnakeHead);
                if (changeDirection) __SetRandomDirection();
            }
            while (changeDirection);
            __board.PutObjects(__position.Left, __position.Top, CellType.Empty);
            __position = __GetNextPosition(__position);
            __board.PutObjects(__position.Left, __position.Top, CellType.Food);
            __moveCountDown = MoveDelay;

        }

        private bool __AvoidBorderAndSnake(Position nextSnakeHead)
        {
            return !__board.IsPositionEmpty(__GetNextPosition(__position).Left, __GetNextPosition(__position).Top) || (__GetNextPosition(__position).Left == nextSnakeHead.Left && __GetNextPosition(__position).Top == nextSnakeHead.Top);
        }

        // Direction detemination code

        private FoodDirection __direction;
        private void __SetRandomDirection()
        {
            Array directions = Enum.GetValues(typeof(FoodDirection));
            Random random = new Random();
            __direction = (FoodDirection)directions.GetValue(random.Next(directions.Length));
        }

        private Position __CreateRandomPosition()
        {
            Random random = new Random();
            Position position = new Position();
            position.Left = random.Next(1, __board.Width - 1);
            position.Top = random.Next(1, __board.Height - 1);
            return position;
        }

        private Position __GetNextPosition(Position currentPosition)
        {
            switch (__direction)
            {
                case FoodDirection.Up:
                    return new Position
                    {
                        Left = currentPosition.Left,
                        Top = currentPosition.Top - 1,
                    };
                case FoodDirection.UpRight:
                    return new Position
                    {
                        Left = currentPosition.Left + 1,
                        Top = currentPosition.Top - 1,
                    };
                case FoodDirection.Right:
                    return new Position
                    {
                        Left = currentPosition.Left + 1,
                        Top = currentPosition.Top,
                    };
                case FoodDirection.DownRight:
                    return new Position
                    {
                        Left = currentPosition.Left + 1,
                        Top = currentPosition.Top + 1,
                    };
                case FoodDirection.Down:
                    return new Position
                    {
                        Left = currentPosition.Left,
                        Top = currentPosition.Top + 1,
                    };
                case FoodDirection.DownLeft:
                    return new Position
                    {
                        Left = currentPosition.Left - 1,
                        Top = currentPosition.Top + 1,
                    };
                case FoodDirection.Left:
                    return new Position
                    {
                        Left = currentPosition.Left - 1,
                        Top = currentPosition.Top,
                    };
                default: // SnakeDirection.UpLeft:
                    return new Position
                    {
                        Left = currentPosition.Left - 1,
                        Top = currentPosition.Top - 1,
                    };
            }
        }
    }
}
