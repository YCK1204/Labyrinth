using UnityEngine;

public sealed class Playeranimator
{
    private readonly Animator anim;

    public Playeranimator(Animator a)
    {
        anim = a;
#if UNITY_EDITOR
        if (anim == null) Debug.LogError("[HeroAnimator] Animator가 null입니다.");
#endif
    }

    // Hashes
    private static readonly int H_Grounded  = Animator.StringToHash("Grounded");
    private static readonly int H_AirY      = Animator.StringToHash("AirSpeedY");
    private static readonly int H_State     = Animator.StringToHash("AnimState");
    private static readonly int H_IdleBlock = Animator.StringToHash("IdleBlock");
    private static readonly int H_WallSlide = Animator.StringToHash("WallSlide");
    private static readonly int H_NoBlood   = Animator.StringToHash("noBlood");

    private static readonly int T_Roll   = Animator.StringToHash("Roll");
    private static readonly int T_Jump   = Animator.StringToHash("Jump");
    private static readonly int T_Hurt   = Animator.StringToHash("Hurt");
    private static readonly int T_Death  = Animator.StringToHash("Death");
    private static readonly int T_Block  = Animator.StringToHash("Block");
    private static readonly int T_Atk1   = Animator.StringToHash("Attack1");
    private static readonly int T_Atk2   = Animator.StringToHash("Attack2");
    private static readonly int T_Atk3   = Animator.StringToHash("Attack3");

    public void SetGrounded(bool v) { if (anim && anim.GetBool(H_Grounded) != v) anim.SetBool(H_Grounded, v); }
    public void SetWall(bool v)     { if (anim && anim.GetBool(H_WallSlide) != v) anim.SetBool(H_WallSlide, v); }
    public void SetBlock(bool v)    { if (anim && anim.GetBool(H_IdleBlock) != v) anim.SetBool(H_IdleBlock, v); }
    public void SetNoBlood(bool v)  { if (anim && anim.GetBool(H_NoBlood)  != v) anim.SetBool(H_NoBlood,  v); }

    public void SetAirY(float y)
    {
        if (!anim) return;
        if (Mathf.Abs(anim.GetFloat(H_AirY) - y) > 0.0001f) anim.SetFloat(H_AirY, y);
    }

    public void SetState(int s)
    {
        if (anim && anim.GetInteger(H_State) != s) anim.SetInteger(H_State, s);
    }

    public void TrgRoll()   { if (anim) anim.SetTrigger(T_Roll); }
    public void TrgJump()   { if (anim) anim.SetTrigger(T_Jump); }
    public void TrgHurt()   { if (anim) anim.SetTrigger(T_Hurt); }
    public void TrgDeath()  { if (anim) anim.SetTrigger(T_Death); }
    public void TrgBlock()  { if (anim) anim.SetTrigger(T_Block); }

    public void TrgAttack(int step)
    {
        if (!anim) return;
        switch (step)
        {
            case 1: anim.SetTrigger(T_Atk1); break;
            case 2: anim.SetTrigger(T_Atk2); break;
            default: anim.SetTrigger(T_Atk3); break;
        }
    }
}
