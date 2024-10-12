# Respawn Protection
Gives players protection from other players damaging them after respawning.

## Features
- Protects players from damage for a configurable amount of time after spawning
- Conditions to disable protection:
  - Moving too far
  - Equipping a gun
  - Equipping a melee weapon
  - Equipping a throwable
  - Attacking
- Protection can be disabled for home spawns, so players can't abuse it to protect their base
- Protection can be disabled for PVE damage
- Configurable effect to show when protection is active

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<RespawnProtectionConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>yellow</MessageColor>
  <ProtectionDuration>10</ProtectionDuration>
  <EnableHomeSpawnProtection>false</EnableHomeSpawnProtection>
  <EnableJoinSpawnProtection>false</EnableJoinSpawnProtection>
  <MaxMoveDistance>10</MaxMoveDistance>
  <ProtectFromPVE>true</ProtectFromPVE>
  <DisableOnEquipGun>true</DisableOnEquipGun>
  <DisableOnEquipMelee>true</DisableOnEquipMelee>
  <DisableOnEquipThrowable>true</DisableOnEquipThrowable>
  <DisableOnAttack>true</DisableOnAttack>
  <DisableOnMove>true</DisableOnMove>
  <EffectId>132</EffectId>
  <EffectTriggerRate>0.1</EffectTriggerRate>
  <AttackMessageRate>2</AttackMessageRate>
  <SendProtectionEnabledMessage>true</SendProtectionEnabledMessage>
  <SendProtectionDisabledExpiredMessage>true</SendProtectionDisabledExpiredMessage>
  <SendProtectionDisabledOtherMessage>true</SendProtectionDisabledOtherMessage>
</RespawnProtectionConfiguration>
```

## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="SpawnProtectionEnabled" Value="Spawn protection enabled for {0} seconds." />
  <Translation Id="SpawnProtectionDisabledExpired" Value="Your spawn protection has expired." />
  <Translation Id="SpawnProtectionDisabledOnMove" Value="Spawn protection disabled because you moved too far." />
  <Translation Id="SpawnProtectionDisabledOnEquipGun" Value="Spawn protection disabled because you equipped a gun." />
  <Translation Id="SpawnProtectionDisabledOnEquipMelee" Value="Spawn protection disabled because you equipped a melee weapon." />
  <Translation Id="SpawnProtectionDisabledOnEquipThrowable" Value="Spawn protection disabled because you equipped a throwable." />
  <Translation Id="SpawnProtectionDisabledOnAttack" Value="Spawn protection disabled because you attacked." />
  <Translation Id="SpawnProtectionDisabledWithCommand" Value="Spawn protection disabled by command." />
  <Translation Id="PlayerHasProtection" Value="You can't hurt {0} because they have spawn protection." />
  <Translation Id="SpawnProtectionCommandFormat" Value="You must specify player name." />
  <Translation Id="PlayerNotFound" Value="Player not found." />
  <Translation Id="SpawnProtectionCommandDisabled" Value="Spawn protection disabled for {0}." />
  <Translation Id="SpawnProtectionCommandEnabled" Value="Spawn protection enabled for {0} for {1} seconds." />
</Translations>
```