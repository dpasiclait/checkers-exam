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
            Console.WriteLine("Yolo!");
        }
    }
}
