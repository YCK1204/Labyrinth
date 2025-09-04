using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AttackPracticeNPC : MonoBehaviour
{
    private Animator _anim;
    private static readonly int HitHash = Animator.StringToHash("Hit");

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(float dmg)
    {

        if (_anim != null)
        {
            _anim.ResetTrigger(HitHash);
            _anim.SetTrigger(HitHash);
        }
    }
}
