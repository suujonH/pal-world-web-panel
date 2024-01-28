using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PalWebControl.RCon;
using System.Diagnostics;
using System.Management;

namespace PalWebControl.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class RestartServerModel : PageModel
    {
        private readonly ILogger<RestartServerModel> _logger;
        public IConfiguration _configuration { get; }
        public List<string> ClientList { get; set; } = new List<string>();

        public ulong PalServiceMem = 0;
        public ulong TotalPhysicalMemory = 0;
        public ulong AvailablePhysicalMemory = 0;

        public bool hasUnsavedChanges = false;

        public RestartServerModel(ILogger<RestartServerModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            hasUnsavedChanges = System.IO.File.Exists("./unsaved.ini");
            Process[] processes = Process.GetProcessesByName(_configuration["PalServerProcessName"]);

            if (processes.Length > 0)
            {
                string path = _configuration["PalWorldSavedPath"];
                PalWorldSettings settings = PalWorldSettings.LoadSettingsFromFile(path + "\\Pal\\Saved\\Config\\WindowsServer\\PalWorldSettings.ini");

                RConClient rconClient = new RConClient();
                rconClient.Connect("localhost", Convert.ToInt32(settings.RCONPort));

                if (rconClient.Authenticate(settings.AdminPassword))
                {
                    String playerList = rconClient.Run("showPlayers");
                    ClientList.AddRange(playerList.Split("\n"));
                    ClientList.Remove("");
                }

                foreach (var process in processes)
                {
                    PalServiceMem += (ulong)process.WorkingSet64;
                }
            }

            // 获取系统总内存和可用内存
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                TotalPhysicalMemory = (ulong)result["TotalVisibleMemorySize"];
                AvailablePhysicalMemory = (ulong)result["FreePhysicalMemory"];
            }
        }

        public IActionResult OnPost()
        {
            // 从 IConfiguration 读取目录
            string targetPath = _configuration["PalWorldSavedPath"];
            string destFile = (targetPath + @"\\Pal\Saved\Config\WindowsServer\\PalWorldSettings.ini");
            Process[] processes = Process.GetProcessesByName(_configuration["PalServerProcessName"]);
            bool successSaved = false || processes.Length <= 0;

            PalWorldSettings settings = PalWorldSettings.LoadSettingsFromFile(destFile);

            if (processes.Length > 0)
            {
                RConClient rconClient = new RConClient();
                rconClient.Connect("localhost", Convert.ToInt32(settings.RCONPort));

                if (rconClient.Authenticate(settings.AdminPassword))
                {

                    String ret = rconClient.Run("save");
                    successSaved = ret.Contains("Complete Save");
                }
            }


            if (successSaved)
            {
                if (System.IO.File.Exists("./unsaved.ini"))
                {
                    // 复制文件
                    System.IO.File.Copy("./unsaved.ini", destFile, true);
                }

                // 结束进程
                foreach (var process in Process.GetProcessesByName(_configuration["PalServerProcessName"]))
                {
                    process.Kill();
                }

                // 运行 SteamCMD 更新命令
                ProcessStartInfo steamCmdStartInfo = new ProcessStartInfo
                {
                    FileName = _configuration["SteamCmdPath"],
                    Arguments = "+login anonymous +app_update 2394010 validate +quit",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                using (var steamCmdProcess = Process.Start(steamCmdStartInfo))
                {
                    steamCmdProcess.WaitForExit();
                }

                Process.Start(_configuration["PalServerPath"]);

                System.IO.File.Delete("./unsaved.ini");
            }

            return RedirectToPage("/RestartServer");
        }
    }
}
