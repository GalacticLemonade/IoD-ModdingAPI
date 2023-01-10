using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace IoDModdingAPI
{
    class Core
    {
        static string LogPathWithEnv = @"%USERPROFILE%\AppData\LocalLow\Radiangames\Instruments\Player.log"; // Get Player.log path
        static string RealFilePath = Environment.ExpandEnvironmentVariables(LogPathWithEnv); // Expand env variables

        static void StartProgramLoop()
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName("instruments");
                if (processes.Length == 0)
                {
                    // If game is not open, close API
                    Console.WriteLine("Game stopped, stopping API");
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(0);
                }
            }
        }

        static void Awake()
        {
            // Keep the console application running.
            System.Threading.Thread.Sleep(1000);
            Awake();
        }

        static void Main()
        {
            Console.Title = "IoD Modding API";
            Console.WriteLine("Locating game...");

            string[] lines = System.IO.File.ReadAllLines(RealFilePath);

            string foundDir = lines[1].Substring(44, 73); // Get game directory

            string installedDirectory = foundDir + "Instruments_Data/Modding_API/";

            string installedFile = installedDirectory + "IsInstalled";
            string modFolder = foundDir + "Mods/";

            string finalDir = foundDir + "Instruments.exe";

            Console.WriteLine("Game found! Directory: " + foundDir);
            try
            {
                if (File.Exists(installedDirectory))
                {
                    Console.WriteLine("API located!"); // API was already found, continue
                }
                else
                {
                    Console.WriteLine("Creating required directories...");
                    
                    Directory.CreateDirectory(Path.GetDirectoryName(modFolder)); // Create modFolder dir
                    
                    using (FileStream fs = File.Create(installedFile))
                    {

                        Byte[] text = new UTF8Encoding(true).GetBytes("hi there"); // Create installed flag
                        fs.Write(text, 0, text.Length);
                        
                    }
                }
            }
            catch (Exception Exception)
            {
                Console.WriteLine(Exception.ToString());
            }

            Console.WriteLine("Starting application...");

            try
            {
                Process.Start(finalDir);
                Console.WriteLine("Game started!");
                StartProgramLoop();
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine("Attempting to inject...");
                try
                {
                    
                }
                catch (Exception Exception)
                {
                    Console.WriteLine("DLL Failed to inject: " + Exception.ToString() + " Please contact GalacticLemonade#7367 on discord with this message, or open an issue on github, GalaacticLemonade/IoD-ModdingAPI");
                }
                
            }
            catch (Exception Exception)
            {
                Console.WriteLine("Program failed to start: "+Exception.ToString());
            }

            Awake();
        }
    }
}
