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
                return new OkResult(); // ���� 200 OK
            }
            else
            {
                return new BadRequestResult(); // ���� 400 Bad Request
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
            { "Difficulty", "��Ϸ�Ѷ�" },
            { "DayTimeSpeedRate", "��Ϸ�ڰ����ٶ�" },
            { "NightTimeSpeedRate", "��Ϸ��ҹ���ٶ�" },
            { "ExpRate", "��Ҹ���³��þ��鱶��" },
            { "PalCaptureRate", "������³���ٶ�" },
            { "PalSpawnNumRate", "��³�����ٶ�" },
            { "PalDamageRateAttack", "��³��ɵ��˺�" },
            { "PalDamageRateDefense", "��³�ܵ����˺�" },
            { "PlayerDamageRateAttack", "�����ɵ��˺�" },
            { "PlayerDamageRateDefense", "����ܵ����˺�" },
            { "PlayerStomachDecreaseRate", "��ұ�ʳ���½����ٶ�" },
            { "PlayerStaminaDecreaseRate", "��������½����ٶ�" },
            { "PlayerAutoHPRegeneRate", "�������ֵ�Զ��ָ����ٶ�" },
            { "PlayerAutoHpRegeneRateInSleep", "���˯���ڼ��Զ�����ֵ�ָ����ٶ�" },
            { "PalStomachDecreaseRate", "��³��ʳ���½����ٶ�" },
            { "PalStaminaDecreaseRate", "��³�����½����ٶ�" },
            { "PalAutoHPRegeneRate", "��³����ֵ�Զ��ָ��ٶ�" },
            { "PalAutoHpRegeneRateInSleep", "��³˯���ڼ�����ֵ�Զ��ָ����ٶ�" },
            { "BuildObjectDamageRate", "�Խ����˺�����" },
            { "BuildObjectDeteriorationDamageRate", "��������ӻ��ٶȱ���" },
            { "CollectionDropRate", "�޸Ĳɼ�����" },
            { "CollectionObjectHpRate", "�ɲɼ�����������" },
            { "CollectionObjectRespawnSpeedRate", "�ɲɼ��������ٶ�" },
            { "EnemyDropItemRate", "��ɱ���˵��䱶��" },
            { "DeathPenalty", "�������䣨Allȫ����None�����䣩" },
            { "bEnablePlayerToPlayerDamage", "�����ر�����˺�" },
            { "bEnableFriendlyFire", "�����ر�����" },
            { "bEnableInvaderEnemy", "�����ر�����" },
            { "bActiveUNKO", "�����ͣ�� UNKO������ҹ�����ӣ�" },
            { "bEnableAimAssistPad", "�����رտ������ĸ�����׼" },
            { "bEnableAimAssistKeyboard", "�����رռ��̸�����׼" },
            { "DropItemMaxNum", "������Ʒ���������" },
            { "DropItemMaxNum_UNKO", "������Ϸ�е����UNKO��Ʒ���������" },
            { "BaseCampMaxNum", "��������������" },
            { "BaseCampWorkerMaxNum", "��³�����������" },
            { "DropItemAliveMaxHours", "������Ʒ�����ʧ" },
            { "bAutoResetGuildNoOnlinePlayers", "�Զ�����û��������ҵĹ���" },
            { "AutoResetGuildTimeNoOnlinePlayers", "����û��������ҵĹ����Զ�����ʱ��" },
            { "GuildPlayerMaxNum", "��������������" },
            { "PalEggDefaultHatchingTime", "��³������ʱ��" },
            { "WorkSpeedRate", "������Ϸ�е����幤���ٶ�" },
            { "bIsMultiplay", "�����رն�����Ϸģʽ" },
            { "bIsPvP", "�����ر�PVP" },
            { "bCanPickupOtherGuildDeathPenaltyDrop", "�����ر�ʰȡ�������������������" },
            { "bEnableNonLoginPenalty", "�����رյ�¼�ͷ�" },
            { "bEnableFastTravel", "�����رմ���" },
            { "bIsStartLocationSelectByMap", "�����رյ�ͼѡ����ʼλ��" },
            { "bExistPlayerAfterLogout", "�����ر�������ߺ��ɫ�Ƿ���ԭ��" },
            { "bEnableDefenseOtherGuildPlayer", "�����ر�����������ҷ���" },
            { "CoopPlayerMaxNum", "���ûỰ�к�����ҵ��������" },
            { "ServerPlayerMaxNum", "����������������" },
            { "ServerName", "����������" },
            { "ServerDescription", "����������" },
            { "AdminPassword", "����Ա����" },
            { "ServerPassword", "����������" },
            { "PublicPort", "�������˿�" },
            { "PublicIP", "����IP�����Բ������ã�" },
            { "RCONEnabled", "�Ƿ���Զ�̿���̨" },
            { "RCONPort", "����̨�˿�" },
            { "Region", "���÷���������" },
            { "bUseAuth", "�����رշ����������֤" },
            { "BanListURL", "��������ֹ�б��ַ" }
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
