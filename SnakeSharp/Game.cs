namespace SnakeSharp
{
    internal class Game
    {
        // Scores

        private int __score = 0;
        private int __scoreIncrement = 1;

        // Dependencies

        private IBoard __board;
        private ISnake __snake;
        private IFood __food;

        // Creation code

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
            __snake.Spawn();
            __food.MoveDelay = __frameInterval;
            __food.Create();
            Thread getUserInputThread = new Thread(() =>
            {
                __snake.GetUserInput(ref __continue);
            });
            getUserInputThread.Start();
            __PrintScore();
            __PrintFrameInterval();
            do
            {
                __snake.Move();
                __food.Move(__snake.GetNextHead(), __frameInterval);
                Thread.Sleep(__frameInterval);
                __hasSnakeHitBorder = __board.IsPositionBorder(__snake.GetNextHead().Left, __snake.GetNextHead().Top);
                __hasSnakeHitSelf = __board.IsPositionSnake(__snake.GetNextHead().Left, __snake.GetNextHead().Top);
                __continue = !__hasSnakeHitBorder && !__hasSnakeHitSelf;
                if (__board.IsPositionFood(__snake.GetNextHead().Left, __snake.GetNextHead().Top))
                {
                    __food.Eaten();
                    __snake.Grow();
                    __score += __scoreIncrement;
                    __IncreaseSpeed();
                    __PrintScore();
                    __PrintFrameInterval();
                }
            }
            while (__continue);
            __PrintGameOver();
        }

        // Difficulty increase

        private int __frameInterval = 200;

        private void __IncreaseSpeed()
        {
            if (__frameInterval > 1)
            {
                __frameInterval = __frameInterval * 14 /15;
                __food.MoveDelay = __frameInterval;
            }
        }

        // Game ending flags

        private bool __continue = true;
        private bool __hasSnakeHitBorder = false;
        private bool __hasSnakeHitSelf = false;

        //  Print endgame messages
        
        const string __GAMEOVERMSG = "GAME OVER!";
        const string __HITWALLMG = "YOU HIT A WALL";
        const string __HITSELFMG = "YOU HIT YOURSELF";

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
        
        private void __PrintFrameInterval()
        {
            string frameIntervals = $"Time between frames: {__frameInterval} ms";
            Console.SetCursorPosition(__board.Width - frameIntervals.Length, __board.Height);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(frameIntervals);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
