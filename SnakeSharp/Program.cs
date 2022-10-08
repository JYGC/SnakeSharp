namespace SnakeSharp
{
    internal class Program
    {
        private static readonly int __windowWidth = 60;
        private static readonly int __windowHeight = 30;

        static void Main(string[] args)
        {
            Console.SetWindowSize(__windowWidth, __windowHeight);
            Board board = new Board();
            Game game = new Game(board, new Snake(board), new Food(board));
            game.Start();
        }
    }
}