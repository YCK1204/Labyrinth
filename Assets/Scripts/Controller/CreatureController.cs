using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    [SerializeField]
    protected CreatureData creatureData;

    protected float hp;
    protected float speed;
    protected float armor;
    protected float power;
    protected float crit;
    protected int lv;
    protected float kbResist;
    protected float critX;
    protected float armorPen;
    protected float atkSpeed;

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
