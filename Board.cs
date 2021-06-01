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

        private Dictionary<string, string> piecePositionsCheatSheet = new Dictionary<string, string>();

        public Board()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            InitializeSlots();
            PlacePieces();
        }

        private void InitializeSlots()
        {
            for (int row = 0; row < 8; row++)
                for(int column = 0; column < 8; column++)
                {
                    bool shouldSlotBeRed = (row + column) % 2 == 1;

                    char slotColor = shouldSlotBeRed ? 'R' : 'W';
                    
                    board[row, column] = new Slot(slotColor, row, column);
                }
        }
        private void PlacePieces()
        {
            PlaceWhitePieces();
            PlaceBlackPieces();
        }

        private void PlaceWhitePieces()
        {
            int count = 1;
            for (int row = 0; row < 3; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column].Color == 'R')
                    {
                        board[row, column].Piece = "W" + GetSlotNumberIdString(count++);
                        piecePositionsCheatSheet.Add(board[row, column].Piece, row + "," + column);
                    }
        }

        private string GetSlotNumberIdString(int id)
        {
            return id < 10 ? "0" + id: id.ToString();
        }

        private void PlaceBlackPieces()
        {
            int count = 1;
            for (int row = 5; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column].Color == 'R')
                    {
                        board[row, column].Piece = "B" + GetSlotNumberIdString(count++);
                        piecePositionsCheatSheet.Add(board[row, column].Piece, row + "," + column);
                    }
        }

        public void DrawBoard()
        {
            DrawColumnHeaderIndex();
            DrawSlots();
        }
        private void DrawColumnHeaderIndex()
        {
            Console.Write("\t");

            for (int column = 0; column < 8; column++)
            {
                Console.Write("    " + column + "   ");
            }

            Console.WriteLine("\n");
        }

        private void DrawSlots()
        {
            for (int row = 0; row < 8; row++)
            {
                Console.WriteLine("\t-----------------------------------------------------------------");
                Console.Write(row + "\t|  ");

                for (int column = 0; column < 8; column++)
                {
                    if (board[row, column].Piece != "")
                        Console.Write(board[row, column].Piece + "  |  ");
                    else
                        Console.Write("---  |  ");
                }

                Console.WriteLine("\n\t-----------------------------------------------------------------");
            }
        }

    }
}
