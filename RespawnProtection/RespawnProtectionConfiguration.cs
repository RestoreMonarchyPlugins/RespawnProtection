using Rocket.API;

namespace RestoreMonarchy.RespawnProtection
{
    public class RespawnProtectionConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; } = "yellow";
        public string MessageIconUrl { get; set; } = "https://i.imgur.com/Di3NWF0.png";
        public float ProtectionDuration { get; set; }
        public bool EnableHomeSpawnProtection { get; set; }
        public bool EnableJoinSpawnProtection { get; set; }
        public bool EnableArenaSpawnProtection { get; set; } = true;
        public float MaxMoveDistance { get; set; }
        public bool ProtectFromPVE { get; set; }
        public bool DisableOnEquipGun { get; set; }
        public bool DisableOnEquipMelee { get; set; }
        public bool DisableOnEquipThrowable { get; set; }
        public bool DisableOnAttack { get; set; }
        public bool DisableOnMove { get; set; }
        public ushort EffectId { get; set; }
        public float EffectTriggerRate { get; set; }
        public float AttackMessageRate { get; set; }
        public bool SendProtectionEnabledMessage { get; set; } = true;
        public float ProtectionEnabledMessageDelay { get; set; } = 0f;
        public bool SendProtectionDisabledExpiredMessage { get; set; } = true;
        public bool SendProtectionDisabledOtherMessage { get; set; } = true;

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            MessageIconUrl = "https://i.imgur.com/Di3NWF0.png";
            ProtectionDuration = 10;
            EnableHomeSpawnProtection = false;
            EnableJoinSpawnProtection = false;
            EnableArenaSpawnProtection = true;
            MaxMoveDistance = 10f;
            ProtectFromPVE = true;
            DisableOnEquipGun = true;
            DisableOnEquipMelee = true;
            DisableOnEquipThrowable = true;
            DisableOnAttack = true;
            DisableOnMove = true;
            EffectId = 132;
            EffectTriggerRate = 0.1f;
            AttackMessageRate = 2f;
            SendProtectionEnabledMessage = true;
            SendProtectionDisabledExpiredMessage = true;
            SendProtectionDisabledOtherMessage = true;
        }
    }
}
