using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PalWebControl.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IConfiguration _configuration;

        [BindProperty]
        public PalWorldSettings PalWorldSettings { get; set; }
        public Dictionary<string, string> SettingsDesc { get; set; }
        public Dictionary<string, string> SettingsDesc2 { get; set; }

        public bool hasUnsavedChanges = false;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            string path = _configuration["PalWorldSavedPath"];

            if (System.IO.File.Exists("./unsaved.ini"))
            {
                PalWorldSettings = PalWorldSettings.LoadSettingsFromFile("./unsaved.ini");
            }
            else
            {
                PalWorldSettings = PalWorldSettings.LoadSettingsFromFile(path + "\\Pal\\Saved\\Config\\WindowsServer\\PalWorldSettings.ini");
            }

            InitializeSettingsDesc();
            InitializeSettingsDesc2();

            hasUnsavedChanges = System.IO.File.Exists("./unsaved.ini");
        }

        public IActionResult OnGetCheckValid(string name, string value)
        {
            bool isValid = PalWorldSettingsValidator.IsValid(name, value);

            if (isValid)
            {
                return new OkResult(); // 返回 200 OK
            }
            else
            {
                return new BadRequestResult(); // 返回 400 Bad Request
            }
        }

        public IActionResult OnPost()
        {
            System.IO.File.Copy(_configuration["PalWorldSavedPath"] + "\\DefaultPalWorldSettings.ini", "./unsaved.ini", true);
            PalWorldSettings.SaveSettingsToFile("./unsaved.ini");
            return RedirectToPage("/Index");
        }

        private void InitializeSettingsDesc()
        {
            SettingsDesc = new Dictionary<string, string>
        {
            { "Difficulty", "游戏难度" },
            { "DayTimeSpeedRate", "游戏内白天速度" },
            { "NightTimeSpeedRate", "游戏内夜间速度" },
            { "ExpRate", "玩家跟帕鲁获得经验倍率" },
            { "PalCaptureRate", "捕获帕鲁的速度" },
            { "PalSpawnNumRate", "帕鲁生成速度" },
            { "PalDamageRateAttack", "帕鲁造成的伤害" },
            { "PalDamageRateDefense", "帕鲁受到的伤害" },
            { "PlayerDamageRateAttack", "玩家造成的伤害" },
            { "PlayerDamageRateDefense", "玩家受到的伤害" },
            { "PlayerStomachDecreaseRate", "玩家饱食度下降的速度" },
            { "PlayerStaminaDecreaseRate", "玩家体力下降的速度" },
            { "PlayerAutoHPRegeneRate", "玩家生命值自动恢复的速度" },
            { "PlayerAutoHpRegeneRateInSleep", "玩家睡眠期间自动生命值恢复的速度" },
            { "PalStomachDecreaseRate", "帕鲁饱食度下降的速度" },
            { "PalStaminaDecreaseRate", "帕鲁耐力下降的速度" },
            { "PalAutoHPRegeneRate", "帕鲁生命值自动恢复速度" },
            { "PalAutoHpRegeneRateInSleep", "帕鲁睡眠期间生命值自动恢复的速度" },
            { "BuildObjectDamageRate", "对建筑伤害倍率" },
            { "BuildObjectDeteriorationDamageRate", "建筑物的劣化速度倍率" },
            { "CollectionDropRate", "修改采集倍率" },
            { "CollectionObjectHpRate", "可采集物生命倍率" },
            { "CollectionObjectRespawnSpeedRate", "可采集物重生速度" },
            { "EnemyDropItemRate", "击杀敌人掉落倍率" },
            { "DeathPenalty", "死亡掉落（All全部，None不掉落）" },
            { "bEnablePlayerToPlayerDamage", "开启关闭玩家伤害" },
            { "bEnableFriendlyFire", "开启关闭友伤" },
            { "bEnableInvaderEnemy", "开启关闭入侵" },
            { "bActiveUNKO", "激活或停用 UNKO（不明夜间敲钟）" },
            { "bEnableAimAssistPad", "开启关闭控制器的辅助瞄准" },
            { "bEnableAimAssistKeyboard", "开启关闭键盘辅助瞄准" },
            { "DropItemMaxNum", "掉落物品的最大数量" },
            { "DropItemMaxNum_UNKO", "设置游戏中掉落的UNKO物品的最大数量" },
            { "BaseCampMaxNum", "设置最大基地数量" },
            { "BaseCampWorkerMaxNum", "帕鲁的最大工作数量" },
            { "DropItemAliveMaxHours", "掉落物品多久消失" },
            { "bAutoResetGuildNoOnlinePlayers", "自动重置没有在线玩家的工会" },
            { "AutoResetGuildTimeNoOnlinePlayers", "设置没有在线玩家的工会自动重置时间" },
            { "GuildPlayerMaxNum", "工会最大玩家数量" },
            { "PalEggDefaultHatchingTime", "帕鲁蛋孵化时间" },
            { "WorkSpeedRate", "调整游戏中的整体工作速度" },
            { "bIsMultiplay", "开启关闭多人游戏模式" },
            { "bIsPvP", "开启关闭PVP" },
            { "bCanPickupOtherGuildDeathPenaltyDrop", "开启关闭拾取其他工会玩家死亡掉落" },
            { "bEnableNonLoginPenalty", "开启关闭登录惩罚" },
            { "bEnableFastTravel", "开启关闭传送" },
            { "bIsStartLocationSelectByMap", "开启关闭地图选择起始位置" },
            { "bExistPlayerAfterLogout", "开启关闭玩家下线后角色是否在原地" },
            { "bEnableDefenseOtherGuildPlayer", "开启关闭其他工会玩家防御" },
            { "CoopPlayerMaxNum", "设置会话中合作玩家的最大数量" },
            { "ServerPlayerMaxNum", "服务器最大玩家数量" },
            { "ServerName", "服务器名字" },
            { "ServerDescription", "服务器描述" },
            { "AdminPassword", "管理员密码" },
            { "ServerPassword", "服务器密码" },
            { "PublicPort", "服务器端口" },
            { "PublicIP", "公网IP（可以不用设置）" },
            { "RCONEnabled", "是否开启远程控制台" },
            { "RCONPort", "控制台端口" },
            { "Region", "设置服务器地区" },
            { "bUseAuth", "开启关闭服务器身份认证" },
            { "BanListURL", "服务器禁止列表地址" }
        };
        }

        private void InitializeSettingsDesc2()
        {
            SettingsDesc2 = new Dictionary<string, string>
        {
            { "DayTimeSpeedRate", "0.100000-5.000000" },
            { "NightTimeSpeedRate", "0.100000-5.000000" },
            { "ExpRate", "0.100000-20.000000" },
            { "PalCaptureRate", "0.500000-2.000000" },
            { "PalSpawnNumRate", "0.500000-3.000000" },
            { "PalDamageRateAttack", "0.100000-5.000000" },
            { "PalDamageRateDefense", "0.100000-5.000000" },
            { "PlayerDamageRateAttack", "0.100000-5.000000" },
            { "PlayerDamageRateDefense", "0.100000-5.000000" },
            { "PlayerStomachDecreaceRate", "0.100000-5.000000" },
            { "PlayerStaminaDecreaceRate", "0.100000-5.000000" },
            { "PlayerAutoHPRegeneRate", "0.100000-5.000000" },
            { "PlayerAutoHpRegeneRateInSleep", "0.100000-5.000000" },
            { "PalStomachDecreaceRate", "0.100000-5.000000" },
            { "PalStaminaDecreaceRate", "0.100000-5.000000" },
            { "PalAutoHPRegeneRate", "0.100000-5.000000" },
            { "PalAutoHpRegeneRateInSleep", "0.100000-5.000000" },
            { "BuildObjectDamageRate", "0.500000-3.000000" },
            { "BuildObjectDeteriorationDamageRate", "0.000000-10.000000" },
            { "CollectionDropRate", "0.500000-3.000000" },
            { "CollectionObjectHpRate", "0.500000-3.000000" },
            { "CollectionObjectRespawnSpeedRate", "0.500000-3.000000" },
            { "EnemyDropItemRate", "0.500000-3.000000" },
            { "GuildPlayerMaxNum", "1-100" },
            { "PalEggDefaultHatchingTime", "0-240" },
        };
        }
    }
}
