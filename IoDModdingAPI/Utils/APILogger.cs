using System;
using System.Runtime.CompilerServices;

namespace IoDModdingAPI.Utils
{
    public class APILogger
    {
        public void Info(string Input)
        {
            Console.WriteLine("INFO: "+Input);
        }

        public void Warning(string Input)
        {
            Console.WriteLine("WARNING: " + Input);
        }

        public void Error(string Input)
        {
            Console.WriteLine("ERROR: " + Input);
        }
    }
}
