using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Slot
    {
        private char color;
        private int row;
        private int column;
        private string piece = null;

        public Slot(char color, int row, int column, string piece)
        {
            this.color = color;
            this.row = row;
            this.column = column;
            this.piece = piece;
        }

        public char Color { get => color; set => color = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public string Piece { get => piece; set => piece = value; }

        public void SetPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
