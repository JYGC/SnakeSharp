using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSharp
{
    internal interface IFood
    {
        Position Position { get; set; }
        void Create();
        void Put();
        void Eaten();
    }
    internal class Food : IFood
    {
        private IBoard __board;
        public Position Position { get; set; }
        public Food(IBoard board)
        {
            __board = board;
        }
        public void Create()
        {
            bool tryCreateFood = true;
            while (tryCreateFood)
            {
                Position = __CreateRandomPosition();
                tryCreateFood = !__board.IsPositionEmpty(Position.Left, Position.Top);
            }
        }
        public void Put()
        {
            __board.PutObjects(Position.Left, Position.Top, CellType.Food);
        }
        public void Eaten()
        {
            Position oldPosition = Position;
            bool tryCreateFood = true;
            while (tryCreateFood)
            {
                Position = __CreateRandomPosition();
                tryCreateFood = !__board.IsPositionEmpty(Position.Left, Position.Top) || (Position.Left == oldPosition.Left && Position.Top == oldPosition.Top);
            }
            // Old cell will be automatically overwritten by snake no need to call __board.PutObjects(...)
        }
        private Position __CreateRandomPosition()
        {
            Random random = new Random();
            Position position = new Position();
            position.Left = random.Next(1, __board.Width - 1);
            position.Top = random.Next(1, __board.Height - 1);
            return position;
        }
    }
}
