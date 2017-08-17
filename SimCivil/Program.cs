using System;

namespace SimCivil
{
    class Program
    {
        static void Main(string[] args)
        {
            SimCivil game = new SimCivil();
            game.Run();
            Console.Read();
        }
    }
}