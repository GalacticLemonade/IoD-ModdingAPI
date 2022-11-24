using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace IoDModdingAPI
{
    
    class Core
    {
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern IntPtr CreateRemoteThread(IntPtr hProcess,
        IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    // privileges
    const int PROCESS_CREATE_THREAD = 0x0002;
    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int PROCESS_VM_OPERATION = 0x0008;
    const int PROCESS_VM_WRITE = 0x0020;
    const int PROCESS_VM_READ = 0x0010;

    // used for memory allocation
    const uint MEM_COMMIT = 0x00001000;
    const uint MEM_RESERVE = 0x00002000;
    const uint PAGE_READWRITE = 4;
        
        public static int Inject()
        {
            // the target process - I'm using a dummy process for this
            // if you don't have one, open Task Manager and choose wisely
            Process targetProcess = Process.GetProcessesByName("instruments")[0];

            // geting the handle of the process - with required privileges
            IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);

            // searching for the address of LoadLibraryA and storing it in a pointer
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            // name of the dll we want to inject
            string dllName = "ModAPI.dll";

            // alocating some memory on the target process - enough to store the name of the dll
            // and storing its address in a pointer
            IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

            // writing the name of the dll there
            UIntPtr bytesWritten;
            WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);

            // creating a thread that will call LoadLibraryA with allocMemAddress as argument
            CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

            return 0;
        }
        
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

            string installedDirectory = foundDir + "Instruments_Data/ModAPIInstalled";

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
                    using (FileStream fs = File.Create(installedDirectory))
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
            }
            catch (Exception Exception)
            {
                Console.WriteLine("Program failed to start: "+Exception.ToString());
            }

            Awake();
        }
    }
}
