using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PalWebControl
{
    public static class PalWorldSettingsValidator
    {
        public static bool IsValid(string propertyName, string value)
        {
            switch (propertyName)
            {
                case "Difficulty":
                    return new[] { "Casual", "Normal", "Hard" }.Contains(value);

                case "DayTimeSpeedRate":
                case "NightTimeSpeedRate":
                case "ExpRate":
                case "PalCaptureRate":
                case "PalSpawnNumRate":
                case "PalDamageRateAttack":
                case "PalDamageRateDefense":
                case "PlayerDamageRateAttack":
                case "PlayerDamageRateDefense":
                case "PlayerStomachDecreaceRate":
                case "PlayerStaminaDecreaceRate":
                case "PlayerAutoHPRegeneRate":
                case "PlayerAutoHpRegeneRateInSleep":
                case "PalStomachDecreaceRate":
                case "PalStaminaDecreaceRate":
                case "PalAutoHPRegeneRate":
                case "PalAutoHpRegeneRateInSleep":
                case "BuildObjectDamageRate":
                case "CollectionDropRate":
                case "CollectionObjectHpRate":
                case "CollectionObjectRespawnSpeedRate":
                case "EnemyDropItemRate":
                    return IsInRange(value, 0.1f, 5.0f);

                case "BuildObjectDeteriorationDamageRate":
                    return IsInRange(value, 0.0f, 10.0f);

                case "DeathPenalty":
                    return new[] { "None", "Item", "ItemAndEquipment", "All" }.Contains(value);

                case "GuildPlayerMaxNum":
                    return IsInRange(value, 1, 100);

                case "PalEggDefaultHatchingTime":
                    return IsInRange(value, 0, 240);

                // 对于布尔值或其他不需要范围检查的属性，返回 true
                case "bEnablePlayerToPlayerDamage":
                case "bEnableFriendlyFire":
                case "bEnableInvaderEnemy":
                    // ... 其他布尔值属性
                    return true;

                // 默认情况下，假设输入有效
                default:
                    return true;
            }
        }


        private static bool IsInRange(string value, float min, float max)
        {
            if (float.TryParse(value, out float number))
            {
                if (number >= min && number <= max)
                {
                    // 检查小数位数是否不超过6位
                    string[] parts = value.Split('.');
                    if (parts.Length == 2 && parts[1].Length <= 6)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsInRange(string value, int min, int max)
        {
            if (int.TryParse(value, out int number))
            {
                return number >= min && number <= max;
            }
            return false;
        }
    }
}
