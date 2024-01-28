using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PalWebControl
{
    public class PalWorldSettings
    {
        public  string Difficulty { get; set; }
        public  float DayTimeSpeedRate { get; set; }
        public  float NightTimeSpeedRate { get; set; }
        public  float ExpRate { get; set; }
        public  float PalCaptureRate { get; set; }
        public  float PalSpawnNumRate { get; set; }
        public  float PalDamageRateAttack { get; set; }
        public  float PalDamageRateDefense { get; set; }
        public  float PlayerDamageRateAttack { get; set; }
        public  float PlayerDamageRateDefense { get; set; }
        public  float PlayerStomachDecreaceRate { get; set; }
        public  float PlayerStaminaDecreaceRate { get; set; }
        public  float PlayerAutoHPRegeneRate { get; set; }
        public  float PlayerAutoHpRegeneRateInSleep { get; set; }
        public  float PalStomachDecreaceRate { get; set; }
        public  float PalStaminaDecreaceRate { get; set; }
        public  float PalAutoHPRegeneRate { get; set; }
        public  float PalAutoHpRegeneRateInSleep { get; set; }
        public  float BuildObjectDamageRate { get; set; }
        public  float BuildObjectDeteriorationDamageRate { get; set; }
        public  float CollectionDropRate { get; set; }
        public  float CollectionObjectHpRate { get; set; }
        public  float CollectionObjectRespawnSpeedRate { get; set; }
        public  float EnemyDropItemRate { get; set; }
        public  string DeathPenalty { get; set; }
        public  bool bEnablePlayerToPlayerDamage { get; set; }
        public  bool bEnableFriendlyFire { get; set; }
        public  bool bEnableInvaderEnemy { get; set; }
        public  bool bActiveUNKO { get; set; }
        public  bool bEnableAimAssistPad { get; set; }
        public  bool bEnableAimAssistKeyboard { get; set; }
        public  int DropItemMaxNum { get; set; }
        public  int DropItemMaxNum_UNKO { get; set; }
        public  int BaseCampMaxNum { get; set; }
        public  int BaseCampWorkerMaxNum { get; set; }
        public  float DropItemAliveMaxHours { get; set; }
        public  bool bAutoResetGuildNoOnlinePlayers { get; set; }
        public  float AutoResetGuildTimeNoOnlinePlayers { get; set; }
        public  int GuildPlayerMaxNum { get; set; }
        public  float PalEggDefaultHatchingTime { get; set; }
        public  float WorkSpeedRate { get; set; }
        public  bool bIsMultiplay { get; set; }
        public  bool bIsPvP { get; set; }
        public  bool bCanPickupOtherGuildDeathPenaltyDrop { get; set; }
        public  bool bEnableNonLoginPenalty { get; set; }
        public  bool bEnableFastTravel { get; set; }
        public  bool bIsStartLocationSelectByMap { get; set; }
        public  bool bExistPlayerAfterLogout { get; set; }
        public  bool bEnableDefenseOtherGuildPlayer { get; set; }
        public  int CoopPlayerMaxNum { get; set; }
        public  int ServerPlayerMaxNum { get; set; }
        public  string ServerName { get; set; }
        public  string ServerDescription { get; set; }
        public  string AdminPassword { get; set; }
        public  string ServerPassword { get; set; }
        public  int PublicPort { get; set; }
        public  string PublicIP { get; set; }
        public  bool RCONEnabled { get; set; }
        public  int RCONPort { get; set; }
        public  string Region { get; set; }
        public  bool bUseAuth { get; set; }
        public  string BanListURL { get; set; }

        public static PalWorldSettings LoadSettingsFromFile(string filePath)
        {
            PalWorldSettings settings = new PalWorldSettings();
            string configFileContent = File.ReadAllText(filePath);

            // 查找包含配置的行
            string settingsLinePrefix = "OptionSettings=(";
            int settingsStart = configFileContent.IndexOf(settingsLinePrefix);
            if (settingsStart == -1) return settings; // 如果没有找到配置行，返回空的设置

            int settingsEnd = configFileContent.IndexOf(")", settingsStart);
            if (settingsEnd == -1) return settings; // 如果没有找到结束标记，返回空的设置

            string settingsLine = configFileContent.Substring(settingsStart + settingsLinePrefix.Length, settingsEnd - settingsStart - settingsLinePrefix.Length);

            // 解析配置
            string[] keyValuePairs = settingsLine.Split(',');
            foreach (var kvp in keyValuePairs)
            {
                string[] parts = kvp.Split('=');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim().Trim('\"'); // 移除可能的引号
                    SetProperty(settings, key, value);
                }
            }

            return settings;
        }

        private static void SetProperty(PalWorldSettings settings, string key, string value)
        {
            PropertyInfo propertyInfo = settings.GetType().GetProperty(key);
            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(settings, int.Parse(value));
                }
                else if (propertyInfo.PropertyType == typeof(float))
                {
                    propertyInfo.SetValue(settings, float.Parse(value));
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(settings, bool.Parse(value));
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(settings, value);
                }
            }
        }


        public void SaveSettingsToFile(string filePath)
        {
            // 读取原始配置文件的所有内容
            string originalContent = File.ReadAllText(filePath);

            // 查找配置开始和结束的位置
            string settingsLinePrefix = "OptionSettings=(";
            int settingsStart = originalContent.IndexOf(settingsLinePrefix);
            if (settingsStart == -1) return; // 如果没有找到配置行，不进行操作

            int settingsEnd = originalContent.IndexOf(")", settingsStart);
            if (settingsEnd == -1) return; // 如果没有找到结束标记，不进行操作

            // 获取设置部分的内容
            string settingsContent = originalContent.Substring(settingsStart, settingsEnd - settingsStart + 1);

            // 使用反射构建新的设置内容
            StringBuilder newSettingsContent = new StringBuilder();
            newSettingsContent.Append(settingsLinePrefix);
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(this);
                newSettingsContent.AppendFormat("{0}={1},", prop.Name, FormatValue(originalContent, value, prop));
            }

            // 移除最后一个逗号
            newSettingsContent.Remove(newSettingsContent.Length - 1, 1);
            newSettingsContent.Append(")");

            // 替换原始内容中的设置部分
            string newContent = originalContent.Replace(settingsContent, newSettingsContent.ToString());

            // 写入文件
            File.WriteAllText(filePath, newContent);
        }

        private string FormatValue(string originalContent, object value, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(bool))
            {
                return value.ToString().ToLower(); // 将布尔值转换为小写
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                return ((float)value).ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                string stringValue = value as string;

                // 检查原始内容中此属性的值是否包含引号
                if (originalContent.Contains($"{propertyInfo.Name}=\""))
                {
                    stringValue = $"\"{stringValue}\"";
                }
                return stringValue;
            }
            else
            {
                return value.ToString();
            }
        }

    }
}
