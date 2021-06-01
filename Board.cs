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

        public string WhatPossibleActionsCanBeTakenByGivenPiece(string pieceName)
        {
            if (!piecePositionsCheatSheet.Keys.Contains<string>(pieceName.ToUpper()))
                return "Invalid piece";

            string currentPiecePosition = piecePositionsCheatSheet[pieceName.ToUpper()];
            int row = Int32.Parse(currentPiecePosition.Split(',')[0]);
            int column = Int32.Parse(currentPiecePosition.Split(',')[1]);

            string possibleActions = PossibleMovesFromCurrentPosition(row, column);

            return possibleActions;
        }

        private string PossibleMovesFromCurrentPosition(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            string possibleMoves = "";

            if (board[row, column].Piece.Contains("W"))
            {
                possibleMoves += TryMovingLeft(row + 1, column - 1);
                possibleMoves += TryMovingRight(row + 1, column + 1);
            }
            else
            {
                possibleMoves += TryMovingLeft(row - 1, column - 1);
                possibleMoves += TryMovingRight(row - 1, column + 1);
            }

            possibleMoves += TryJumping(board[row, column].Piece[0], row, column, "");

            return possibleMoves == "NO JUMPS;" ? "NO MOVES" : possibleMoves;
        }

        private static bool IsPositionOutOfBounds(int row, int column)
        {
            return row < 0 || row > 7 || column < 0 || column > 7;
        }

        private string TryMovingLeft(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            return board[row, column].Piece == "" ? "Move Left;" : "";
        }

        private string TryMovingRight(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            return board[row, column].Piece == "" ? "Move Right;" : "";
        }

        private string TryJumping(char pieceColor, int row, int column, string initalRoute)
        {
            if (IsPositionOutOfBounds(row, column))
                return "NO JUMPS;";

            string possibleJumps = "";

            bool isLetfSlotOccypiedByEnemy;
            bool isLetfSlotBehindEnemyOccupied;
            bool isRightSlotOccypiedByEnemy;
            bool isRightSlotBehindEnemyOccupied;
            if (pieceColor == 'W')
            {
                try
                {
                    isLetfSlotOccypiedByEnemy = board[row + 1, column - 1].Piece.Contains("B");
                    isLetfSlotBehindEnemyOccupied = board[row + 2, column - 2].Piece == "";
                    if (isLetfSlotOccypiedByEnemy && isLetfSlotBehindEnemyOccupied)
                    {
                        possibleJumps += "Jump to (" + (row + 2) + "," + (column - 2) + ");";
                        possibleJumps += TryJumping(pieceColor, row + 2, column - 2, possibleJumps);
                    }
                } catch (Exception)
                {
                   // we've gone out of bounds     
                }

                try
                {
                    isRightSlotOccypiedByEnemy = board[row + 1, column + 1].Piece.Contains("B");
                    isRightSlotBehindEnemyOccupied = board[row + 2, column + 2].Piece == "";
                    if (isRightSlotOccypiedByEnemy && isRightSlotBehindEnemyOccupied)
                    {
                        possibleJumps += "Jump to (" + (row + 2) + "," + (column + 2) + ");";
                        possibleJumps += TryJumping(pieceColor, row + 2, column + 2, possibleJumps);
                    }
                } catch (Exception)
                {
                    // we've gone out of bounds 
                }
            }
            else
            {
                try
                {
                    isLetfSlotOccypiedByEnemy = board[row - 1, column - 1].Piece.Contains("W");
                    isLetfSlotBehindEnemyOccupied = board[row - 2, column - 2].Piece == "";
                    if (isLetfSlotOccypiedByEnemy && isLetfSlotBehindEnemyOccupied)
                    {
                        possibleJumps += "Jump to (" + (row - 2) + "," + (column - 2) + ");";
                        possibleJumps += TryJumping(pieceColor, row - 2, column - 2, possibleJumps);
                    }
                }
                catch (Exception)
                {
                    // we've gone out of bounds 
                }

                try
                {
                    isRightSlotOccypiedByEnemy = board[row - 1, column + 1].Piece.Contains("W");
                    isRightSlotBehindEnemyOccupied = board[row - 2, column + 2].Piece == "";
                    if (isRightSlotOccypiedByEnemy && isRightSlotBehindEnemyOccupied)
                    {
                        possibleJumps += "Jump to (" + (row - 2) + "," + (column + 2) + ");";
                        possibleJumps += TryJumping(pieceColor, row - 2, column + 2, possibleJumps);
                    }
                }
                catch (Exception)
                {
                    // we've gone out of bounds 
                }
            }

            return possibleJumps == "" ? "" : possibleJumps;
        }

        public string WhatPossibleJumpsCanBeMadeByGivenPiece(string pieceName)
        {
            if (!piecePositionsCheatSheet.Keys.Contains<string>(pieceName.ToUpper()))
                return "Invalid piece";

            string currentPiecePosition = piecePositionsCheatSheet[pieceName.ToUpper()];
            int row = Int32.Parse(currentPiecePosition.Split(',')[0]);
            int column = Int32.Parse(currentPiecePosition.Split(',')[1]);

            string possibleJumps = PossibleJumpsFromCurrentPosition(row, column);

            return possibleJumps;
        }

        private string PossibleJumpsFromCurrentPosition(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            string possibleJumps = TryJumping(board[row, column].Piece[0], row, column, "");

            return possibleJumps == "" ? "NO JUMPS;" : possibleJumps;
        }

        public void MovePiece(string pieceName, int newRow, int newColumn)
        {
            if (IsPositionOutOfBounds(newRow, newColumn))
                return;

            if (!piecePositionsCheatSheet.Keys.Contains<string>(pieceName.ToUpper()))
                return;

            string currentPiecePosition = piecePositionsCheatSheet[pieceName.ToUpper()];
            int row = Int32.Parse(currentPiecePosition.Split(',')[0]);
            int column = Int32.Parse(currentPiecePosition.Split(',')[1]);

            board[newRow, newColumn].Piece = board[row, column].Piece;
            board[row, column].Piece = "";

            piecePositionsCheatSheet[pieceName.ToUpper()] = newRow + "," + newColumn;

            Console.Clear();
            DrawBoard();
        }
    }
}
