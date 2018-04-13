using System;
namespace Utils.IO
{
    public class ConsoleKeyReader : IKeyReader
    {
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }
    }
}
