using RestoreMonarchy.RespawnProtection.Components;
using RestoreMonarchy.RespawnProtection.Models;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Linq;

namespace RestoreMonarchy.RespawnProtection
{
    public class RespawnProtectionPlugin : RocketPlugin<RespawnProtectionConfiguration>
    {
        public static RespawnProtectionPlugin Instance { get; private set; }
        public UnityEngine.Color MessageColor { get; set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.yellow);

            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            DamageTool.damagePlayerRequested += DamagedPlayerRequested;
            PlayerLife.OnSelectingRespawnPoint += OnSelectRespawnPoint;

            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            DamageTool.damagePlayerRequested -= DamagedPlayerRequested;
            PlayerLife.OnSelectingRespawnPoint -= OnSelectRespawnPoint;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "SpawnProtectionEnabled", "Spawn protection enabled for {0} seconds." },
            { "SpawnProtectionDisabledExpired", "Your spawn protection has expired." },
            { "SpawnProtectionDisabledOnMove", "Spawn protection disabled because you moved too far." },
            { "SpawnProtectionDisabledOnEquipGun", "Spawn protection disabled because you equipped a gun." },
            { "SpawnProtectionDisabledOnEquipMelee", "Spawn protection disabled because you equipped a melee weapon." },
            { "SpawnProtectionDisabledOnEquipThrowable", "Spawn protection disabled because you equipped a throwable." },
            { "SpawnProtectionDisabledOnAttack", "Spawn protection disabled because you attacked." },
            { "SpawnProtectionDisabledWithCommand", "Spawn protection disabled by command." },
            { "PlayerHasProtection", "You can't hurt {0} because they have spawn protection." },
            { "SpawnProtectionCommandFormat", "You must specify player name." },
            { "PlayerNotFound", "Player not found." },
            { "SpawnProtectionCommandDisabled", "Spawn protection disabled for {0}." },
            { "SpawnProtectionCommandEnabled", "Spawn protection enabled for {0} for {1} seconds." }
        };

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            RespawnProtectionComponent component = player.Player.gameObject.AddComponent<RespawnProtectionComponent>();
            if (Configuration.Instance.EnableJoinSpawnProtection)
            {
                component.EnableProtection();
                if (Configuration.Instance.SendProtectionEnabledMessage)
                {
                    string duration = Configuration.Instance.ProtectionDuration.ToString("N0");
                    UnturnedChat.Say(player, Translate("SpawnProtectionEnabled", duration), MessageColor);
                }
            }
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            RespawnProtectionComponent component = player.Player.gameObject.GetComponent<RespawnProtectionComponent>();
            if (component != null)
            {
                Destroy(component);
            }
        }

        private void DamagedPlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            Player killer = PlayerTool.getPlayer(parameters.killer);

            if (killer == parameters.player)
            {
                return;
            }

            RespawnProtectionComponent component = parameters.player.gameObject.GetComponent<RespawnProtectionComponent>();
            if (component != null && component.IsProtected)
            {
                if (killer == null)
                {
                    if (Configuration.Instance.ProtectFromPVE)
                    {
                        shouldAllow = false;
                    }
                } else
                {
                    if (!component.LastAttackMessages.Any(x => x.Player == killer && (DateTime.Now - x.DateTime) <= TimeSpan.FromSeconds(Configuration.Instance.AttackMessageRate)))
                    {
                        component.LastAttackMessages.Add(new LastAttackMessage
                        {
                            Player = killer,
                            DateTime = DateTime.Now
                        });

                        UnturnedChat.Say(parameters.killer, Translate("PlayerHasProtection", parameters.player.channel.owner.playerID.characterName), MessageColor);
                    }
                    shouldAllow = false;
                }
            }

            if (killer != null && Configuration.Instance.DisableOnAttack)
            {
                RespawnProtectionComponent killerComponent = killer.gameObject.GetComponent<RespawnProtectionComponent>();
                if (killerComponent != null && killerComponent.IsProtected)
                {
                    killerComponent.DisableProtection();
                    if (Configuration.Instance.SendProtectionDisabledOtherMessage)
                    {
                        UnturnedChat.Say(parameters.killer, Translate("SpawnProtectionDisabledOnAttack"), MessageColor);
                    }                    
                }
            }            
        }

        private void OnSelectRespawnPoint(PlayerLife sender, bool wantsToSpawnAtHome, ref UnityEngine.Vector3 position, ref float yaw)
        {
            RespawnProtectionComponent component = sender.gameObject.GetComponent<RespawnProtectionComponent>();
            if (component != null)
            {
                component.IsHomeSpawn = wantsToSpawnAtHome;
            }
        }
    }
}