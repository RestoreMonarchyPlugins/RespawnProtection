using SDG.Unturned;
using System;

namespace RestoreMonarchy.RespawnProtection.Models
{
    public class LastAttackMessage
    {
        public Player Player { get; set; }
        public DateTime DateTime { get; set; }
    }
}
