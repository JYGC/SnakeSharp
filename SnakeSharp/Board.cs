namespace SnakeSharp
{
    internal interface IBoard
    {
        int Width { get; }
        int Height { get; }
        void Create();
        void PutObjects(int left, int top, CellType cellType);
        bool IsPositionEmpty(int left, int top);
        bool IsPositionBorder(int left, int top);
        bool IsPositionSnake(int left, int top);
        bool IsPositionFood(int left, int top);
    }
    internal class Board : IBoard
    {
        // Board creation
        public int Width { get; } = Console.WindowWidth;
        public int Height { get; } = Console.WindowHeight - 1;
        private Cell[,] __grid;
        public Board()
        {
            __grid = new Cell[Width, Height];
        }

        // Placement and painting code

        public void Create()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    __grid[i, j] = new Cell { Left = i, Top = j };
                    __SetBorder(__grid[i, j]);
                }
            }
        }

        private void __SetBorder(Cell cell)
        {
            if (cell.Left == 0 || cell.Left == Width - 1 || cell.Top == 0 || cell.Top == Height - 1)
            {
                PutObjects(cell.Left, cell.Top, CellType.Border);
            }
        }

        public void PutObjects(int left, int top, CellType cellType)
        {
            __grid[left, top].Type = cellType;
            __grid[left, top].PaintConsole();
        }

        // Object identification code

        public bool IsPositionEmpty(int left, int top)
        {
            return __grid[left, top].Type == CellType.Empty;
        }

        public bool IsPositionBorder(int left, int top)
        {
            return __grid[left, top].Type == CellType.Border;
        }

        public bool IsPositionSnake(int left, int top)
        {
            return __grid[left, top].Type == CellType.Snake;
        }

        public bool IsPositionFood(int left, int top)
        {
            return __grid[left, top].Type == CellType.Food;
        }
    }
}
