using System;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Board gameboard = new Board();
            gameboard.DrawBoard();

            //Console.WriteLine("301: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("301"));

            gameboard.MovePiece("MB03", 4, 3);
            gameboard.MovePiece("MW09", 5, 4);
            gameboard.MovePiece("MB12", 4, 7);
            gameboard.MovePiece("MB10", 3, 0);

            //Console.WriteLine("MW09 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MW09"));
            //Console.WriteLine("MB03 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MB03"));

            //Console.WriteLine("MW09 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MW09"));
            //Console.WriteLine("MB03 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MB03"));
            //Console.WriteLine("MB12 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MB12"));

            //Console.WriteLine("White pieces that can move: \n" + gameboard.WhichPiecesOfAColorCanBeMoved('W'));
            //Console.WriteLine("\nBlack pieces that can move: \n" + gameboard.WhichPiecesOfAColorCanBeMoved('B'));

            Console.WriteLine("MW09 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenKingPiece("MW09"));

            Console.WriteLine("Yolo!");
        }
    }
}
