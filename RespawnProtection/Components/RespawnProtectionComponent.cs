﻿using RestoreMonarchy.RespawnProtection.Models;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.RespawnProtection.Components
{
    public class RespawnProtectionComponent : MonoBehaviour
    {
        private RespawnProtectionPlugin pluginInstance => RespawnProtectionPlugin.Instance;
        private RespawnProtectionConfiguration configuration => pluginInstance.Configuration.Instance;

        public Player Player { get; private set; }
        public bool IsProtected { get; private set; }
        public bool IsHomeSpawn { get; internal set; }
        private CSteamID SteamID => Player.channel.owner.playerID.steamID;

        public List<LastAttackMessage> LastAttackMessages { get; private set; } = new();

        private Vector3 respawnPosition;

        void Awake()
        {
            Player = gameObject.GetComponent<Player>();
        }

        void Start()
        {
            Player.life.onLifeUpdated += OnLifeUpdated;
            Player.equipment.onEquipRequested += OnEquipRequested;            
        }

        void OnDestroy()
        {
            Player.life.onLifeUpdated -= OnLifeUpdated;
            Player.equipment.onEquipRequested -= OnEquipRequested;
        }

        public Coroutine ProtectionCoroutine { get; private set; }
        public Coroutine EffectCoroutine { get; private set; }

        private void OnLifeUpdated(bool isDead)
        {
            if (isDead)
            {
                DisableProtection();
            }
            else
            {
                if (!configuration.EnableHomeSpawnProtection && IsHomeSpawn)
                {
                    return;
                }

                EnableProtection();
            }
        }

        public void EnableProtection()
        {
            if (IsProtected)
            {
                return;
            }

            UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionEnabled", configuration.ProtectionDuration), pluginInstance.MessageColor);

            IsProtected = true;
            respawnPosition = Player.transform.position;
            ProtectionCoroutine = StartCoroutine(ProtectionTimer());
            EffectCoroutine = StartCoroutine(EffectTimer());
            LastAttackMessages.Clear();
        }

        public void DisableProtection()
        {
            if (!IsProtected)
            {
                return;
            }

            if (ProtectionCoroutine != null)
            {
                StopCoroutine(ProtectionCoroutine);
            }

            if (EffectCoroutine != null)
            {
                StopCoroutine(EffectCoroutine);
            }

            IsProtected = false;
            LastAttackMessages.Clear();
        }

        private IEnumerator ProtectionTimer()
        {
            yield return new WaitForSeconds(configuration.ProtectionDuration);

            DisableProtection();
            UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionDisabledExpired"), pluginInstance.MessageColor);
        }

        private IEnumerator EffectTimer()
        {
            EffectAsset effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, configuration.EffectId);
            while (IsProtected)
            {
                TriggerEffectParameters parameters = new(effectAsset)
                {
                    position = Player.transform.position
                };
                EffectManager.triggerEffect(parameters);

                yield return new WaitForSeconds(configuration.EffectTriggerRate);
            }
        }

        void FixedUpdate()
        {
            if (IsProtected && configuration.DisableOnMove)
            {
                if (Vector3.Distance(respawnPosition, Player.transform.position) > configuration.MaxMoveDistance)
                {
                    DisableProtection();
                    UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionDisabledOnMove"), pluginInstance.MessageColor);
                }
            }
        }

        private void OnEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            if (IsProtected && configuration.DisableOnEquipGun && asset != null)
            {
                if (configuration.DisableOnEquipGun && asset.type == EItemType.GUN)
                {
                    DisableProtection();
                    UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionDisabledOnEquipGun"), pluginInstance.MessageColor);
                }

                if (configuration.DisableOnEquipMelee && asset.type == EItemType.MELEE)
                {
                    DisableProtection();
                    UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionDisabledOnEquipMelee"), pluginInstance.MessageColor);
                }

                if (configuration.DisableOnEquipThrowable && asset.type == EItemType.THROWABLE)
                {
                    DisableProtection();
                    UnturnedChat.Say(SteamID, pluginInstance.Translate("SpawnProtectionDisabledOnEquipThrowable"), pluginInstance.MessageColor);
                }
            }
        }
    }
}