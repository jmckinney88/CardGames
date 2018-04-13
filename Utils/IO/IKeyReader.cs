using System;
namespace Utils.IO
{
    public interface IKeyReader
    {
        ConsoleKeyInfo ReadKey(bool intercept);
    }
}
