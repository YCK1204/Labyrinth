using UnityEngine;

[CreateAssetMenu(menuName = "Equip/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public enum EquipmentType
    {
        Weapon, Armor,
    }

    public enum StatType
    {
        // 무기
        Power,
        AtkSpeed,
        ArmorPen,
        Crit,
        CritX,

        // 방어구
        Armor,
        Hp,
        Energy,
        Speed,
        KbResist
    }

    [System.Serializable]
    public struct StatValue
    {
        public StatType stat;
        public float value;
    }

    [Header("아이템 정보")]
    public string itemName;
    public EquipmentType type;

    [Header("스탯")]
    public StatValue[] stats;
}
