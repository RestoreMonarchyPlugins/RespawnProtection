using RestoreMonarchy.RespawnProtection.Components;
using Rocket.API;
using Rocket.Unturned.Chat;
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
                UnturnedChat.Say(caller, pluginInstance.Translate("SpawnProtectionCommandFormat"), pluginInstance.MessageColor);
                return;
            }

            UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
            if (target == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("PlayerNotFound"), pluginInstance.MessageColor);
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
                    UnturnedChat.Say(target, pluginInstance.Translate("SpawnProtectionDisabledWithCommand"), pluginInstance.MessageColor);
                }

                UnturnedChat.Say(caller, pluginInstance.Translate("SpawnProtectionCommandDisabled", target.DisplayName), pluginInstance.MessageColor);
            }
            else
            {
                component.EnableProtection();
                string duration = pluginInstance.Configuration.Instance.ProtectionDuration.ToString("N0");
                if (pluginInstance.Configuration.Instance.SendProtectionEnabledMessage)
                {
                    UnturnedChat.Say(target, pluginInstance.Translate("SpawnProtectionEnabled", duration), pluginInstance.MessageColor);
                }

                UnturnedChat.Say(caller, pluginInstance.Translate("SpawnProtectionCommandEnabled", target.DisplayName, duration), pluginInstance.MessageColor);
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
