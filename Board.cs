using System;
using System.Collections.Generic;
using System.Linq;

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
                    // when adding the row to the column the % shows red slots will always be odd numbers
                    bool shouldSlotBeRed = (row + column) % 2 == 1;

                    SlotColor slotColor = shouldSlotBeRed ? SlotColor.Red : SlotColor.White;
                    
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
                    if (board[row, column].Color == SlotColor.Red)
                    {
                        board[row, column].Piece = "MW" + GetSlotNumberIdString(count++);
                        piecePositionsCheatSheet.Add(board[row, column].Piece, row + "," + column);
                    }
        }

        private string GetSlotNumberIdString(int id)
        {
            return (id < 10) ? "0" + id: id.ToString();
        }

        private void PlaceBlackPieces()
        {
            int count = 1;
            for (int row = 5; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column].Color == SlotColor.Red)
                    {
                        board[row, column].Piece = "MB" + GetSlotNumberIdString(count++);
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
                Console.Write("     " + column + "   ");
            }

            Console.WriteLine("\n");
        }

        private void DrawSlots()
        {
            for (int row = 0; row < 8; row++)
            {
                Console.WriteLine("\t-------------------------------------------------------------------------");
                Console.Write(row + "\t|  ");

                for (int column = 0; column < 8; column++)
                {
                    if (board[row, column].Piece != "")
                        Console.Write(board[row, column].Piece + "  |  ");
                    else
                        Console.Write("----  |  ");
                }

                Console.WriteLine("\n\t-------------------------------------------------------------------------");
            }

            //foreach(string key in piecePositionsCheatSheet.Keys)
            //{
            //    Console.WriteLine(key + " at position " + piecePositionsCheatSheet[key]);
            //}
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

            if (board[row, column].Piece.Contains("K") || board[row, column].Piece.Contains("W"))
            {
                string move = TryMovingLeft(row + 1, column - 1);
                move += (move != "") ? " One Row Down;": "";
                possibleMoves += move;

                move = TryMovingRight(row + 1, column + 1);
                move += (move != "") ? " One Row Down;" : "";
                possibleMoves += move;
            }

            if (board[row, column].Piece.Contains("K") || board[row, column].Piece.Contains("B"))
            {
                string move = TryMovingLeft(row - 1, column - 1);
                move += (move != "") ? " One Row Up;" : "";
                possibleMoves +=  move;

                move = TryMovingRight(row - 1, column + 1);
                move += (move != "") ? " One Row Up;" : "";
                possibleMoves +=  move;
            }

            char pieceRank = board[row, column].Piece[0];
            char pieceColor = board[row, column].Piece[1];
            if (pieceRank == 'M')
            {
                possibleMoves += TryJumpingWithAManRankPiece(pieceColor, row, column);
            }
            else
            {
                List<string> slotsAlreadyVisited = new List<string>();
                possibleMoves += TryJumpingWithAKingRankPiece(pieceColor, row, column, slotsAlreadyVisited);
            }

            return (possibleMoves == "NO JUMPS;" || possibleMoves == "") ? "NO MOVES" : possibleMoves;
        }

        private static bool IsPositionOutOfBounds(int row, int column)
        {
            return row < 0 || row > 7 || column < 0 || column > 7;
        }

        private string TryMovingLeft(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            return (board[row, column].Piece == "") ? "Move Left" : "";
        }

        private string TryMovingRight(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "";

            return (board[row, column].Piece == "") ? "Move Right" : "";
        }

        private string TryJumpingWithAManRankPiece(char pieceColor, int row, int column)
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
                        possibleJumps += TryJumpingWithAManRankPiece(pieceColor, row + 2, column - 2);
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
                        possibleJumps += TryJumpingWithAManRankPiece(pieceColor, row + 2, column + 2);
                    }
                } catch (Exception)
                {
                    // we've gone out of bounds 
                }
            }
            
            if(pieceColor == 'B')
            {
                try
                {
                    isLetfSlotOccypiedByEnemy = board[row - 1, column - 1].Piece.Contains("W");
                    isLetfSlotBehindEnemyOccupied = board[row - 2, column - 2].Piece == "";
                    if (isLetfSlotOccypiedByEnemy && isLetfSlotBehindEnemyOccupied)
                    {
                        possibleJumps += "Jump to (" + (row - 2) + "," + (column - 2) + ");";
                        possibleJumps += TryJumpingWithAManRankPiece(pieceColor, row - 2, column - 2);
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
                        possibleJumps += TryJumpingWithAManRankPiece(pieceColor, row - 2, column + 2);
                    }
                }
                catch (Exception)
                {
                    // we've gone out of bounds 
                }
            }

            return (possibleJumps == "") ? "" : possibleJumps;
        }

        private string TryJumpingWithAKingRankPiece(char pieceColor, int row, int column, List<string> slotsAlreadyVisited)
        {
            if (IsPositionOutOfBounds(row, column))
                return "NO JUMPS;";

            string possibleJumps = "";

            string enemyColor = (pieceColor == 'W') ? "B" : "W";

            try
            {
                bool isLowerLetfSlotOccypiedByEnemy = board[row + 1, column - 1].Piece.Contains(enemyColor);
                bool isLowerLetfSlotBehindEnemyOccupied = board[row + 2, column - 2].Piece == "";
                string lowerLetfSlotBehindEnemy = (row + 2) + "," + (column - 2);
                if (isLowerLetfSlotOccypiedByEnemy && isLowerLetfSlotBehindEnemyOccupied && !slotsAlreadyVisited.Contains(lowerLetfSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + (row + 2) + "," + (column - 2) + ");";
                    slotsAlreadyVisited.Add(lowerLetfSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAKingRankPiece(pieceColor, row + 2, column - 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds     
            }

            try
            {
                bool isUpperLetfSlotOccypiedByEnemy = board[row - 1, column - 1].Piece.Contains(enemyColor);
                bool isUpperLetfSlotBehindEnemyOccupied = board[row - 2, column - 2].Piece == "";
                string upperLetfSlotBehindEnemy = (row - 2) + "," + (column - 2);
                if (isUpperLetfSlotOccypiedByEnemy && isUpperLetfSlotBehindEnemyOccupied && !slotsAlreadyVisited.Contains(upperLetfSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + (row - 2) + "," + (column - 2) + ");";
                    slotsAlreadyVisited.Add(upperLetfSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAKingRankPiece(pieceColor, row - 2, column - 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds     
            }

            try
            {
                bool isLowerRightSlotOccypiedByEnemy = board[row + 1, column + 1].Piece.Contains(enemyColor);
                bool isLowerRightSlotBehindEnemyOccupied = board[row + 2, column + 2].Piece == "";
                string lowerRightSlotBehindEnemy = (row + 2) + "," + (column + 2);
                if (isLowerRightSlotOccypiedByEnemy && isLowerRightSlotBehindEnemyOccupied && !slotsAlreadyVisited.Contains(lowerRightSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + (row + 2) + "," + (column + 2) + ");";
                    slotsAlreadyVisited.Add(lowerRightSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAKingRankPiece(pieceColor, row + 2, column + 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds 
            }

            try
            {
                bool isUpperRightSlotOccypiedByEnemy = board[row - 1, column + 1].Piece.Contains(enemyColor);
                bool isUpperRightSlotBehindEnemyOccupied = board[row - 2, column + 2].Piece == "";
                string upperRightSlotBehindEnemy = (row - 2) + "," + (column + 2);
                if (isUpperRightSlotOccypiedByEnemy && isUpperRightSlotBehindEnemyOccupied && !slotsAlreadyVisited.Contains(upperRightSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + (row - 2) + "," + (column + 2) + ");";
                    slotsAlreadyVisited.Add(upperRightSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAKingRankPiece(pieceColor, row - 2, column + 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds 
            }

            return (possibleJumps == "") ? "" : possibleJumps;
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

            char pieceColor = board[row, column].Piece[1];
            string possibleJumps = TryJumpingWithAManRankPiece(pieceColor, row, column);

            return (possibleJumps == "") ? "NO JUMPS;" : possibleJumps;
        }

        public string WhichPiecesOfAColorCanBeMoved(char color)
        {
            string pieces = "";
            foreach(string key in piecePositionsCheatSheet.Keys)
                if (key.Contains(color))
                {
                    string possiblemoves = WhatPossibleActionsCanBeTakenByGivenPiece(key);
                    if (possiblemoves != "NO MOVES")
                        pieces += key + " at (" + piecePositionsCheatSheet[key] + ");";
                }

            return (pieces == "") ? "No moves can be made;" : pieces;
        }

        public string WhatPossibleActionsCanBeTakenByGivenKingPiece(string pieceName)
        {
            if (!piecePositionsCheatSheet.Keys.Contains<string>(pieceName.ToUpper()))
                return "Invalid piece";

            #region Only for testing movement with kning pieces without playing a full game
            //string value = piecePositionsCheatSheet[pieceName];
            //piecePositionsCheatSheet.Remove(pieceName);
            //pieceName = "K" + pieceName.Substring(1);
            //piecePositionsCheatSheet.Add(pieceName, value);
            #endregion

            if (!pieceName.ToUpper().Contains('K'))
                return "This piece is not a King piece";

            string currentPiecePosition = piecePositionsCheatSheet[pieceName.ToUpper()];
            int row = Int32.Parse(currentPiecePosition.Split(',')[0]);
            int column = Int32.Parse(currentPiecePosition.Split(',')[1]);

            #region Only for testing movement with kning pieces without playing a full game
            //board[row, column].Piece = pieceName;
            #endregion

            string possibleActions = PossibleMovesFromCurrentPosition(row, column);

            return possibleActions;
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
