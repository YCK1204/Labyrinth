using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    [SerializeField]
    protected CreatureData creatureData;

    [HideInInspector]
    public float hp;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float armor;
    [HideInInspector]
    public float power;
    [HideInInspector]
    public float crit;
    [HideInInspector]
    public int lv;
    [HideInInspector]
    public float kbResist;
    [HideInInspector]
    public float critX;
    [HideInInspector]
    public float armorPen;
    [HideInInspector]
    public float atkSpeed;

    public abstract void TakeDamage(float dmg);
    protected abstract void OnDied();
    protected abstract void Attack();
    protected abstract void Move();
    private void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        if (creatureData == null)
        {
            Debug.LogError("CreatureData is not assigned in " + gameObject.name);
            return;
        }

        hp = creatureData.HP;
        speed = creatureData.Speed;
        armor = creatureData.Armor;
        power = creatureData.Power;
        crit = creatureData.Crit;
        lv = creatureData.LV;
        kbResist = creatureData.KBResist;
        critX = creatureData.CritX;
        armorPen = creatureData.ArmorPen;
        atkSpeed = creatureData.AtkSpeed;
    }
}
