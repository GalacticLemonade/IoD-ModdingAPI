using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace IoDModdingAPI
{
    class Core
    {
        static string LogPathWithEnv = @"%USERPROFILE%\AppData\LocalLow\Radiangames\Instruments\Player.log";
        static string RealFilePath = Environment.ExpandEnvironmentVariables(LogPathWithEnv);

        static void StartProgramLoop()
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName("instruments");
                if (processes.Length == 0)
                {
                    Console.WriteLine("Game stopped, stopping API");
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(0);
                }
            }
        }

        static void Awake()
        {
            System.Threading.Thread.Sleep(1000);
            Awake();
        }

        static void Main()
        {
            Console.Title = "IoD Modding API";
            Console.WriteLine("Locating game...");

            string[] lines = System.IO.File.ReadAllLines(RealFilePath);

            string foundDir = lines[1].Substring(44, 73);

            string installedDirectory = foundDir + "Instruments_Data/API/";

            string installedFile = installedDirectory + "Installed";

            string finalDir = foundDir + "Instruments.exe";

            Console.WriteLine("Game found! Directory: " + foundDir);
            try
            {
                if (File.Exists(installedDirectory))
                {
                    Console.WriteLine("API located!");
                }
                else
                {
                    using (FileStream fs = File.Create(installedFile))
                    {

                        Byte[] text = new UTF8Encoding(true).GetBytes("DO NOT DELETE THIS!!");
                        fs.Write(text, 0, text.Length);
                        Console.WriteLine("Created and installed!");
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
                    using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream("PipeMain"))
                    {
                        namedPipeServer.WaitForConnection();
                        namedPipeServer.WriteByte(1);
                        int byteFromClient = namedPipeServer.ReadByte();
                        Console.WriteLine(byteFromClient);
                    }
                }
                catch (Exception Exception)
                {
                    Console.WriteLine("DLL Failed to inject: " + Exception.ToString() + "Please contact GalacticLemonade#7367 on discord with this same error message.");
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
