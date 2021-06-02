using System;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Board gameboard = new Board();
            gameboard.DrawBoard();

            Console.WriteLine("301: " + gameboard.WhatPossibleActionsCanBeTakenByAGivenPieceWithBasicRank("301") + "\n");

            gameboard.MovePiece("MB03", 4, 3);
            gameboard.MovePiece("MW09", 3, 2);
            gameboard.MovePiece("MB12", 4, 7);
            gameboard.MovePiece("MB10", 3, 0);

            Console.WriteLine("MW09 can: " + gameboard.WhatPossibleActionsCanBeTakenByAGivenPieceWithBasicRank("MW09") + "\n");
            Console.WriteLine("MB03 can: " + gameboard.WhatPossibleActionsCanBeTakenByAGivenPieceWithBasicRank("MB03") + "\n");

            Console.WriteLine("MW09 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPieceWithBasicRank("MW09") + "\n");
            Console.WriteLine("MB03 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPieceWithBasicRank("MB03") + "\n");
            Console.WriteLine("MB12 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPieceWithBasicRank("MB12") + "\n");

            Console.WriteLine("White pieces that can move: \n" + gameboard.WhichPiecesOfaAGivenColorCanBeMoved('W') + "\n");
            Console.WriteLine("\nBlack pieces that can move: \n" + gameboard.WhichPiecesOfaAGivenColorCanBeMoved('B') + "\n");

            Console.WriteLine("MW09 can: " + gameboard.WhatPossibleActionsCanBeTakenByAGivenPieceWithKingRank("MW09") + "\n");
            Console.WriteLine("MW09 can: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenKingPiece("MW09") + "\n");
        }
    }
}
