namespace SnakeSharp
{
    enum CellType
    {
        Empty,
        Food,
        Snake,
        Border
    }

    internal class Position
    {
        public int Left { get; set; }
        public int Top { get; set; }
    }

    internal class Cell : Position
    {
        public CellType Type { get; set; } = CellType.Empty;

        private ConsoleColor __GetColor()
        {
            switch (Type)
            {
                case CellType.Border:
                    return ConsoleColor.Gray;
                case CellType.Food:
                    return ConsoleColor.DarkYellow;
                case CellType.Snake:
                    return ConsoleColor.Green;
                default:
                    return ConsoleColor.Black;
            }
        }

        public void PaintConsole()
        {
            Console.SetCursorPosition(Left, Top);
            Console.BackgroundColor = __GetColor();
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
