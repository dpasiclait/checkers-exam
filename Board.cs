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
                    // when adding the row to the column the % operator shows that red slots will always be odd numbers
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
                        string rankAndColor = "MW";
                        string pieceNumberId = GetSlotNumberIdString(count++);
                        PlacePieceOnBoard(row, column, rankAndColor + pieceNumberId);
                    }
        }

        private string GetSlotNumberIdString(int id)
        {
            return (id < 10) ? "0" + id : id.ToString();
        }

        private void PlacePieceOnBoard(int row, int column, string pieceName)
        {
            board[row, column].Piece = pieceName;

            string currentPositionOnBoard = row + "," + column;
            piecePositionsCheatSheet.Add(board[row, column].Piece, currentPositionOnBoard);
        }

        private void PlaceBlackPieces()
        {
            int count = 1;
            for (int row = 5; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column].Color == SlotColor.Red)
                    {
                        string rankAndColor = "MB";
                        string pieceNumberId = GetSlotNumberIdString(count++);
                        PlacePieceOnBoard(row, column, rankAndColor + pieceNumberId);
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

            for (int columnNumber = 0; columnNumber < 8; columnNumber++)
                Console.Write("     " + columnNumber + "   ");

            Console.WriteLine("\n");
        }

        private void DrawSlots()
        {
            for (int row = 0; row < 8; row++)
            {
                DrawRowHeaderAndNumber(row);
                DrawRowSlots(row);
                DrawRowFooter();
            }
        }

        private static void DrawRowHeaderAndNumber(int rowNumber)
        {
            Console.WriteLine("\t-------------------------------------------------------------------------");
            Console.Write(rowNumber + "\t|  ");
        }

        private void DrawRowSlots(int row)
        {
            for (int column = 0; column < 8; column++)
            {
                bool isSlotOcupiedByAPiece = board[row, column].Piece != "";
                string slotText = isSlotOcupiedByAPiece ? board[row, column].Piece + "  |  " : "----  |  ";
                Console.Write(slotText);
            }
        }

        private static void DrawRowFooter()
        {
            Console.WriteLine("\n\t-------------------------------------------------------------------------");
        }

        public string WhatPossibleActionsCanBeTakenByAGivenPieceWithBasicRank(string pieceName)
        {
            if (!piecePositionsCheatSheet.Keys.Contains<string>(pieceName.ToUpper()))
                return "Invalid piece";

            if (pieceName.ToUpper().Contains('K'))
                return "This piece has a King rank";

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

            bool doesThisPieceHaveKingRank = board[row, column].Piece.Contains("K");
            bool isThisAWhitePiece = board[row, column].Piece.Contains("W");

            if (doesThisPieceHaveKingRank || isThisAWhitePiece)
                possibleMoves += TryMovingOneRowDownFromCurrentPosition(row, column);

            if (doesThisPieceHaveKingRank || !isThisAWhitePiece)
                possibleMoves += TryMovingOneRowUpFromCurrentPosition(row, column);

            char pieceColor = board[row, column].Piece[1];
            if (doesThisPieceHaveKingRank)
            {
                List<string> slotsAlreadyVisited = new List<string>();
                possibleMoves += TryJumpingWithAPieceWithKingRank(pieceColor, row, column, slotsAlreadyVisited);
            }
            else
                possibleMoves += TryJumpingWithAPieceWithBasicRank(pieceColor, row, column);

            return (possibleMoves == "NO JUMPS;" || possibleMoves == "") ? "NO MOVES" : possibleMoves;
        }

        private static bool IsPositionOutOfBounds(int row, int column)
        {
            return row < 0 || row > 7 || column < 0 || column > 7;
        }

        private string TryMovingOneRowDownFromCurrentPosition(int row, int column)
        {
            string possibleMovesDown = "";

            possibleMovesDown += CanPieceMoveToNeighboringSlot(row + 1, column - 1) ? " Move Left One Row Down;" : ""; 
            possibleMovesDown += CanPieceMoveToNeighboringSlot(row + 1, column + 1) ? " Move Right One Row Down;" : "";

            return possibleMovesDown;
        }

        private bool CanPieceMoveToNeighboringSlot(int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return false;

            return board[row, column].Piece == "";
        }

        private string TryMovingOneRowUpFromCurrentPosition(int row, int column)
        {
            string possibleMovesUp = "";

            possibleMovesUp += CanPieceMoveToNeighboringSlot(row - 1, column - 1) ? " Move Left One Row Up;" : "";
            possibleMovesUp += CanPieceMoveToNeighboringSlot(row - 1, column + 1) ? " Move Right One Row Up;" : "";

            return possibleMovesUp;
        }

        private string TryJumpingWithAPieceWithKingRank(char pieceColor, int row, int column, List<string> slotsAlreadyVisited)
        {
            if (IsPositionOutOfBounds(row, column))
                return "NO JUMPS;";

            string possibleJumps = "";

            string enemyColor = (pieceColor == 'W') ? "B" : "W";

            try
            {
                bool isLowerLetfSlotOccypiedByEnemy = board[row + 1, column - 1].Piece.Contains(enemyColor);
                bool isLowerLetfSlotBehindEnemyUnoccupiec = board[row + 2, column - 2].Piece == "";
                string lowerLetfSlotBehindEnemy = (row + 2) + "," + (column - 2);
                if (isLowerLetfSlotOccypiedByEnemy && isLowerLetfSlotBehindEnemyUnoccupiec && !slotsAlreadyVisited.Contains(lowerLetfSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + lowerLetfSlotBehindEnemy + ");";
                    slotsAlreadyVisited.Add(lowerLetfSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAPieceWithKingRank(pieceColor, row + 2, column - 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds     
            }

            try
            {
                bool isUpperLetfSlotOccypiedByEnemy = board[row - 1, column - 1].Piece.Contains(enemyColor);
                bool isUpperLetfSlotBehindEnemyUnoccupied = board[row - 2, column - 2].Piece == "";
                string upperLetfSlotBehindEnemy = (row - 2) + "," + (column - 2);
                if (isUpperLetfSlotOccypiedByEnemy && isUpperLetfSlotBehindEnemyUnoccupied && !slotsAlreadyVisited.Contains(upperLetfSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + upperLetfSlotBehindEnemy + ");";
                    slotsAlreadyVisited.Add(upperLetfSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAPieceWithKingRank(pieceColor, row - 2, column - 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds     
            }

            try
            {
                bool isLowerRightSlotOccypiedByEnemy = board[row + 1, column + 1].Piece.Contains(enemyColor);
                bool isLowerRightSlotBehindEnemyUnoccupied = board[row + 2, column + 2].Piece == "";
                string lowerRightSlotBehindEnemy = (row + 2) + "," + (column + 2);
                if (isLowerRightSlotOccypiedByEnemy && isLowerRightSlotBehindEnemyUnoccupied && !slotsAlreadyVisited.Contains(lowerRightSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + lowerRightSlotBehindEnemy + ");";
                    slotsAlreadyVisited.Add(lowerRightSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAPieceWithKingRank(pieceColor, row + 2, column + 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds 
            }

            try
            {
                bool isUpperRightSlotOccypiedByEnemy = board[row - 1, column + 1].Piece.Contains(enemyColor);
                bool isUpperRightSlotBehindEnemyUnoccupied = board[row - 2, column + 2].Piece == "";
                string upperRightSlotBehindEnemy = (row - 2) + "," + (column + 2);
                if (isUpperRightSlotOccypiedByEnemy && isUpperRightSlotBehindEnemyUnoccupied && !slotsAlreadyVisited.Contains(upperRightSlotBehindEnemy))
                {
                    possibleJumps += "Jump to (" + upperRightSlotBehindEnemy + ");";
                    slotsAlreadyVisited.Add(upperRightSlotBehindEnemy);
                    possibleJumps += TryJumpingWithAPieceWithKingRank(pieceColor, row - 2, column + 2, slotsAlreadyVisited);
                }
            }
            catch (Exception)
            {
                // we've gone out of bounds 
            }

            return (possibleJumps == "") ? "" : possibleJumps;
        }

        private string TryJumpingWithAPieceWithBasicRank(char pieceColor, int row, int column)
        {
            if (IsPositionOutOfBounds(row, column))
                return "NO JUMPS;";

            string possibleJumps = "";

            bool isLetfSlotOccypiedByEnemy;
            bool isLetfSlotBehindEnemyUnoccupied;
            string letfSlotBehindEnemy;
            bool isRightSlotOccypiedByEnemy;
            bool isRightSlotBehindEnemyUnoccupied;
            string rightSlotBehindEnemy;
            if (pieceColor == 'W')
            {
                try
                {
                    isLetfSlotOccypiedByEnemy = board[row + 1, column - 1].Piece.Contains("B");
                    isLetfSlotBehindEnemyUnoccupied = board[row + 2, column - 2].Piece == "";
                    letfSlotBehindEnemy = (row + 2) + "," + (column - 2);
                    if (isLetfSlotOccypiedByEnemy && isLetfSlotBehindEnemyUnoccupied)
                    {
                        possibleJumps += "Jump to (" + letfSlotBehindEnemy + ");";
                        possibleJumps += TryJumpingWithAPieceWithBasicRank(pieceColor, row + 2, column - 2);
                    }
                } catch (Exception)
                {
                   // we've gone out of bounds     
                }

                try
                {
                    isRightSlotOccypiedByEnemy = board[row + 1, column + 1].Piece.Contains("B");
                    isRightSlotBehindEnemyUnoccupied = board[row + 2, column + 2].Piece == "";
                    rightSlotBehindEnemy = (row + 2) + "," + (column + 2);
                    if (isRightSlotOccypiedByEnemy && isRightSlotBehindEnemyUnoccupied)
                    {
                        possibleJumps += "Jump to (" + rightSlotBehindEnemy + ");";
                        possibleJumps += TryJumpingWithAPieceWithBasicRank(pieceColor, row + 2, column + 2);
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
                    isLetfSlotBehindEnemyUnoccupied = board[row - 2, column - 2].Piece == "";
                    letfSlotBehindEnemy = (row - 2) + "," + (column - 2);
                    if (isLetfSlotOccypiedByEnemy && isLetfSlotBehindEnemyUnoccupied)
                    {
                        possibleJumps += "Jump to (" + letfSlotBehindEnemy + ");";
                        possibleJumps += TryJumpingWithAPieceWithBasicRank(pieceColor, row - 2, column - 2);
                    }
                }
                catch (Exception)
                {
                    // we've gone out of bounds 
                }

                try
                {
                    isRightSlotOccypiedByEnemy = board[row - 1, column + 1].Piece.Contains("W");
                    isRightSlotBehindEnemyUnoccupied = board[row - 2, column + 2].Piece == "";
                    rightSlotBehindEnemy = (row - 2) + "," + (column + 2);
                    if (isRightSlotOccypiedByEnemy && isRightSlotBehindEnemyUnoccupied)
                    {
                        possibleJumps += "Jump to (" + rightSlotBehindEnemy + ");";
                        possibleJumps += TryJumpingWithAPieceWithBasicRank(pieceColor, row - 2, column + 2);
                    }
                }
                catch (Exception)
                {
                    // we've gone out of bounds 
                }
            }

            return (possibleJumps == "") ? "" : possibleJumps;
        }

        public string WhatPossibleJumpsCanBeMadeByGivenPieceWithBasicRank(string pieceName)
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

            string possibleJumps;

            bool doesThisPieceHaveKingRank = board[row, column].Piece.Contains('K');
            char pieceColor = board[row, column].Piece[1];
            if (doesThisPieceHaveKingRank)
            {
                List<string> slotsAlreadyVisited = new List<string>();
                possibleJumps = TryJumpingWithAPieceWithKingRank(pieceColor, row, column, slotsAlreadyVisited);
            }
            else
                possibleJumps = TryJumpingWithAPieceWithBasicRank(pieceColor, row, column);

            return (possibleJumps == "") ? "NO JUMPS;" : possibleJumps;
        }

        public string WhichPiecesOfaAGivenColorCanBeMoved(char color)
        {
            if (color != 'W' && color != 'B')
                return "Invalid color";

            string pieces = "";

            foreach(string key in piecePositionsCheatSheet.Keys)
                if (key.Contains(color))
                {
                    bool doesThisPieceHaveKingRank = key.Contains('K');
                    string possiblemoves = "";
                    
                    if (doesThisPieceHaveKingRank)
                        possiblemoves = WhatPossibleActionsCanBeTakenByAGivenPieceWithKingRank(key);
                    else
                        possiblemoves = WhatPossibleActionsCanBeTakenByAGivenPieceWithBasicRank(key);

                    if (possiblemoves != "NO MOVES")
                    {
                        string currentPosition = piecePositionsCheatSheet[key];
                        pieces += key + " at (" + currentPosition + ");";
                    }
                }

            return (pieces == "") ? "No moves can be made;" : pieces;
        }

        public string WhatPossibleActionsCanBeTakenByAGivenPieceWithKingRank(string pieceName)
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

        public string WhatPossibleJumpsCanBeMadeByGivenKingPiece(string pieceName)
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

            string possibleJumps = PossibleJumpsFromCurrentPosition(row, column);

            return possibleJumps;
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
