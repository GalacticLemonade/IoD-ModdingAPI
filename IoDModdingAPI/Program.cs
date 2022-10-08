using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace IoDModdingAPI
{
    class Program
    {
        static string LogPathWithEnv = @"%USERPROFILE%\AppData\LocalLow\Radiangames\Instruments\Player.log";
        static string RealFilePath = Environment.ExpandEnvironmentVariables(LogPathWithEnv);

        static void Main()
        {
            Console.Title = "IoD Modding API";
            Console.WriteLine("Locating game...");

            string[] lines = System.IO.File.ReadAllLines(RealFilePath);

            string foundDir = lines[1].Substring(44, 73);

            string installedDirectory = foundDir + "Instruments_Data/ModAPIInstalled.txt";

            string finalDir = foundDir + "Instruments.exe";

            Console.WriteLine("Game found! Directory: " + foundDir);
            try
            {
                if (File.Exists(installedDirectory))
                {
                    Console.WriteLine("API installed!");
                }
                else
                {
                    using (FileStream fs = File.Create(installedDirectory))
                    {

                        Byte[] text = new UTF8Encoding(true).GetBytes("this is a value for if it's installed or not");
                        fs.Write(text, 0, text.Length);
                        Console.WriteLine("Created and installed!");
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            Console.WriteLine("Starting application...");

            Process.Start(finalDir);
            System.Threading.Thread.Sleep(5000);
        }
    }
}
