using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("플레이어 SO")]
    public PlayerData playerSO;

    [Header("장착중")]
    public EquipmentData weapon;
    public EquipmentData armor;

    public float Power      { get; private set; }
    public float AtkSpeed   { get; private set; }
    public float ArmorPen   { get; private set; }
    public float Crit       { get; private set; }
    public float CritX      { get; private set; }

    public float Armor      { get; private set; }
    public float Hp         { get; private set; }
    public float Energy     { get; private set; }
    public float Speed      { get; private set; }
    public float KbResist   { get; private set; }

    PlayerController pc;

    void Awake()
    {
        pc = GetComponent<PlayerController>();
        if (playerSO == null)
            Debug.LogWarning("playerSO 연결 필요");
    }

    public bool Equip(EquipmentData data)
    {
        if (data == null) return false;

        if (data.type == EquipmentData.EquipmentType.Weapon)
            weapon = data;
        else if (data.type == EquipmentData.EquipmentType.Armor)
            armor = data;
        else
            return false;

        return SyncToController();
    }

    public bool Unequip(EquipmentData.EquipmentType type)
    {
        if (type == EquipmentData.EquipmentType.Weapon) weapon = null;
        else if (type == EquipmentData.EquipmentType.Armor) armor = null;
        else return false;

        return SyncToController();
    }

    public bool SyncToController()
    {
        Recalculate();
        if (pc == null) return false;
        pc.ApplyStatsFrom(this);
        return true;
    }

    public void Recalculate()
    {
        float bPower     = playerSO ? playerSO.Power     : 0f;
        float bAtkSpeed  = playerSO ? playerSO.AtkSpeed  : 1f;
        float bArmorPen  = playerSO ? playerSO.ArmorPen  : 0f;
        float bCrit      = playerSO ? playerSO.Crit      : 0f;
        float bCritX     = playerSO ? playerSO.CritX     : 1.5f;

        float bArmor     = playerSO ? playerSO.Armor     : 0f;
        float bHp        = playerSO ? playerSO.HP        : 100f;
        float bEnergy    = playerSO ? playerSO.Energy    : 100f;
        float bSpeed     = playerSO ? playerSO.Speed     : 5f;
        float bKbResist  = playerSO ? playerSO.KBResist  : 0f;

        float addPower=0, addAtkSpeed=0, addArmorPen=0, addCrit=0, addCritX=0;
        float addArmor=0, addHp=0, addEnergy=0, addSpeed=0, addKbResist=0;

        void Acc(EquipmentData eq)
        {
            if (eq == null || eq.stats == null) return;
            foreach (var s in eq.stats)
            {
                switch (s.stat)
                {
                    case EquipmentData.StatType.Power:     addPower    += s.value; break;
                    case EquipmentData.StatType.AtkSpeed:  addAtkSpeed += s.value; break;
                    case EquipmentData.StatType.ArmorPen:  addArmorPen += s.value; break;
                    case EquipmentData.StatType.Crit:      addCrit     += s.value; break;
                    case EquipmentData.StatType.CritX:     addCritX    += s.value; break;

                    case EquipmentData.StatType.Armor:     addArmor    += s.value; break;
                    case EquipmentData.StatType.Hp:        addHp       += s.value; break;
                    case EquipmentData.StatType.Energy:    addEnergy   += s.value; break;
                    case EquipmentData.StatType.Speed:     addSpeed    += s.value; break;
                    case EquipmentData.StatType.KbResist:  addKbResist += s.value; break;
                }
            }
        }

        Acc(weapon);
        Acc(armor);

        Power     = bPower    + addPower;
        AtkSpeed  = bAtkSpeed + addAtkSpeed;
        ArmorPen  = bArmorPen + addArmorPen;
        Crit     = Mathf.Clamp(bCrit + addCrit, 0f, 100f);
        CritX     = bCritX    + addCritX;

        Armor     = bArmor    + addArmor;
        Hp        = bHp    + addHp;
        Energy    = bEnergy   + addEnergy;
        Speed     = Mathf.Max(0f, bSpeed + addSpeed);
        KbResist = Mathf.Clamp(bKbResist + addKbResist, 0f, 100f);
    }
}
