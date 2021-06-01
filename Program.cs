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

            //Console.WriteLine("W01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W01"));
            //Console.WriteLine("B01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b12"));
            //Console.WriteLine("301: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("301"));
            //Console.WriteLine("W12: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W12"));
            //Console.WriteLine("B01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b01"));
            //Console.WriteLine("W11: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W11"));
            //Console.WriteLine("B03: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b03"));

            gameboard.MovePiece("W09", 3, 2);
            gameboard.MovePiece("B03", 4, 3);
            gameboard.MovePiece("B12", 4, 7);
            gameboard.MovePiece("B10", 3, 0);
            Console.WriteLine("W09 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W09"));
            Console.WriteLine("B03 can: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("B03"));

            Console.WriteLine("Yolo!");
        }
    }
}
