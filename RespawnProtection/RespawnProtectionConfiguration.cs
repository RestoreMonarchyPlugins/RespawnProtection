using Rocket.API;

namespace RestoreMonarchy.RespawnProtection
{
    public class RespawnProtectionConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; } = "yellow";
        public float ProtectionDuration { get; set; }
        public bool EnableHomeSpawnProtection { get; set; }
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

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            ProtectionDuration = 10;
            EnableHomeSpawnProtection = false;
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
        }
    }
}
