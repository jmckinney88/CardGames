using System;
using System.Collections.Generic;
using Utils.IO;
using Blackjack;

namespace ConsoleBlackjack
{

    class Program
    {

        static void Main()
        {
            BlackjackGame game = new BlackjackGame(
                new System.IO.StreamReader(Console.OpenStandardInput()),
                new System.IO.StreamWriter(Console.OpenStandardOutput()),
                new ConsoleKeyReader()
            );
            Console.Title = game.Title;
            game.Run();
        }

    }
}