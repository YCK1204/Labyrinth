using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
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

    protected abstract void TakeDamage(float dmg);
    protected abstract void OnDied();
    protected abstract void Attack();
    protected abstract void Move();
}
