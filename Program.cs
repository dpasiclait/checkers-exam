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

            Console.WriteLine("W01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W01"));
            Console.WriteLine("B01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b12"));
            Console.WriteLine("301: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("301"));
            Console.WriteLine("W12: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W12"));
            Console.WriteLine("B01: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b01"));
            Console.WriteLine("W11: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("W11"));
            Console.WriteLine("B03: " + gameboard.WhatPossibleActionsCanBeTakenByGivenPiece("b03"));

            Console.WriteLine("Yolo!");
        }
    }
}
