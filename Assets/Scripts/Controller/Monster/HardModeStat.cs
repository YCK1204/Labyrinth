using UnityEngine;

public static class HardModeStat
{
    private static float GetMultiplier(PlayerData playerData, bool isBoss)
    {
        if (playerData == null || playerData.IsHardMode == false)
            return 1f;
        
        return isBoss ? 1.2f : 1.5f;
    }
    public static void ApplyAll(
        PlayerData playerData,
        bool isBoss,
        CreatureData data,
        out float hp, out float power, out float atkSpeed,
        out float armor, out float crit, out float speed,
        out float kbResist, out float critX, out float armorPen)
    {
        float mul = GetMultiplier(playerData, isBoss);
        hp       = data.HP       * mul;
        power    = data.Power    * mul;
        atkSpeed = data.AtkSpeed * mul;
        armor    = data.Armor    * mul;
        crit     = Mathf.Clamp(data.Crit * mul, 0f, 100f);
        speed    = data.Speed    * mul;
        kbResist = data.KBResist * mul;
        critX    = data.CritX    * mul;
        armorPen = data.ArmorPen * mul;
    }
}
