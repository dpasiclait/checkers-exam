using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Board
    {
        private Slot[,] board = new Slot[8,8];

        public Board()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            // TODO: setup pieces
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            for (int row = 0; row < 8; row++)
                for(int column = 0; column < 8; column++)
                {
                    bool shouldSlotBeRed = (row + column) % 2 == 1;

                    char slotColor = shouldSlotBeRed ? 'R' : 'W';
                    
                    board[row, column] = new Slot(slotColor, row, column, null);
                }
        }

        public void DrawBoard()
        {
            DrawColumnHeaderIndex();
            DrawSlots();
        }
        private static void DrawColumnHeaderIndex()
        {
            Console.Write("\t");

            for (int column = 0; column < 8; column++)
            {
                Console.Write("   " + column + "  ");
            }

            Console.WriteLine("\n");
        }

        private void DrawSlots()
        {
            for (int row = 0; row < 8; row++)
            {
                Console.WriteLine("\t-------------------------------------------------");
                Console.Write(row + "\t|  ");

                for (int column = 0; column < 8; column++)
                {
                    Console.Write(board[row, column].Color + "  |  ");
                }

                Console.WriteLine("\n\t-------------------------------------------------");
            }
        }

    }
}
