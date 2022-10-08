using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IoDModdingAPI
{
    class Program
    {
        static string LogPathWithEnv = @"%USERPROFILE%\AppData\LocalLow\Radiangames\Instruments\Player.log";
        static string RealFilePath = Environment.ExpandEnvironmentVariables(LogPathWithEnv);
        static void PrepareConsole()
        {
            Console.Title = "IoD Modding API";
            Console.WriteLine("Locating game...");

            string[] lines = System.IO.File.ReadAllLines(RealFilePath);

            string foundDir = lines[1].Substring(44, 73);

            string finalDir = foundDir + "Instruments.exe";

            Console.WriteLine("Game found! Directory: "+ finalDir);
            System.Threading.Thread.Sleep(500);
            Console.WriteLine("Starting game...");

            Process.Start(finalDir);
            System.Threading.Thread.Sleep(1000000000);
        }

        static void Main()
        {
            PrepareConsole();
        }
    }
}
