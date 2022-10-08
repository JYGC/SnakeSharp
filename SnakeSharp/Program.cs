namespace SnakeSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Game game = new Game(board, new Snake(board), new Food(board));
            game.Start();
        }
    }
}