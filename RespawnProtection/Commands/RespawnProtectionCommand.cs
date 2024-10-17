using RestoreMonarchy.RespawnProtection.Components;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace RestoreMonarchy.RespawnProtection.Commands
{
    public class RespawnProtectionCommand : IRocketCommand
    {
        private RespawnProtectionPlugin pluginInstance => RespawnProtectionPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                pluginInstance.SendMessageToPlayer(caller, "SpawnProtectionCommandFormat");
                return;
            }

            UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
            if (target == null)
            {
                pluginInstance.SendMessageToPlayer(caller, "PlayerNotFound");
                return;
            }

            RespawnProtectionComponent component = target.Player.gameObject.GetComponent<RespawnProtectionComponent>();
            if (component == null)
            {
                component = target.Player.gameObject.AddComponent<RespawnProtectionComponent>();
            }

            if (component.IsProtected)
            {
                component.DisableProtection();
                if (pluginInstance.Configuration.Instance.SendProtectionDisabledExpiredMessage)
                {
                    pluginInstance.SendMessageToPlayer(target, "SpawnProtectionDisabledWithCommand");
                }
                pluginInstance.SendMessageToPlayer(caller, "SpawnProtectionCommandDisabled", target.DisplayName);
            }
            else
            {
                component.EnableProtection();
                string duration = pluginInstance.Configuration.Instance.ProtectionDuration.ToString("N0");
                if (pluginInstance.Configuration.Instance.SendProtectionEnabledMessage)
                {
                    pluginInstance.SendMessageToPlayer(target, "SpawnProtectionEnabled", duration);
                }
                pluginInstance.SendMessageToPlayer(caller, "SpawnProtectionCommandEnabled", target.DisplayName, duration);
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "respawnprotection";
        public string Help => "";
        public string Syntax => "[player] [duration]";
        public List<string> Aliases => new();
        public List<string> Permissions => new();
    }
}