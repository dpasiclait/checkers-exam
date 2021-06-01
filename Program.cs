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

            //Console.WriteLine("MW01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MW01"));
            //Console.WriteLine("MB01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("Mb12"));
            //Console.WriteLine("M301: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("301"));
            //Console.WriteLine("MW12: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MW12"));
            //Console.WriteLine("MB01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("Mb01"));
            //Console.WriteLine("MW11: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MW11"));
            //Console.WriteLine("MB03: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("Mb03"));

            gameboard.MovePiece("MW09", 3, 2);
            gameboard.MovePiece("MB03", 4, 3);
            gameboard.MovePiece("MB12", 4, 7);
            gameboard.MovePiece("MB10", 3, 0);

            //Console.WriteLine("MW09 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MW09"));
            //Console.WriteLine("MB03 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("MB03"));

            //Console.WriteLine("MW09 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MW09"));
            //Console.WriteLine("MB03 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MB03"));
            //Console.WriteLine("MB12 has: " + gameboard.WhatPossibleJumpsCanBeMadeByGivenPiece("MB12"));

            Console.WriteLine("White pieces that can move: \n" + gameboard.WhichPiecesOfAColorCanBeMoved('W'));
            Console.WriteLine("\nBlack pieces that can move: \n" + gameboard.WhichPiecesOfAColorCanBeMoved('B'));

            Console.WriteLine("Yolo!");
        }
    }
}
