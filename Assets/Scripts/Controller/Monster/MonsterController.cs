using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class Patrol
{
    public float range;
    public float interval;
    public float nextPatrolTime;
    public float detectionRange;
    public Vector2[] directions;
}

public interface IJumpable
{
    void Jump();
}

public abstract class MonsterController : CreatureController
{
    [SerializeField]
    protected Patrol patrol;

    protected GameObject target;
    protected float lastPatrolTime;
    protected float lastChaseTime;
    protected float attackRange;
    protected Vector2 destPos = Vector2.zero;
    protected virtual Vector2 destDir { get { return (destPos - (Vector2)transform.position).normalized; } }
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private float fadeoutTime = 1f;
    protected enum MonsterState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        TakeHit,
        Jump,
        Die
    }
    MonsterState _state = MonsterState.Idle;
    protected MonsterState state
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _state = value;
                UpdateAnimation();
            }
        }
    }
    private void Start()
    {
        Init();
    }
    protected override void OnDied()
    {
        StartCoroutine(FadeOut());
    }
    protected virtual void Init()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        while (elapsedTime < fadeoutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeoutTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        DestroySelf();
    }
    private void Update()
    {
        UpdateController();
    }
    protected override void Move()
    {
        transform.position += speed * Time.deltaTime * (Vector3)destDir;
        spriteRenderer.flipX = (destDir.x < 0);
    }
    protected virtual void Idle()
    {
        if (Time.time - lastPatrolTime > patrol.interval)
        {
            destPos = GenRandomPosition();
            state = MonsterState.Patrol;
        }
    }
    protected override void Attack()
    {
    }
    protected virtual void Chase()
    {
        destPos = target.transform.position;

        var dist = Vector2.Distance(transform.position, destPos);
        if (dist < attackRange)
        {
            state = MonsterState.Attack;
        }
        else
        {
            Move();
        }
    }
    protected virtual void Die() { }
    protected virtual void Patrol()
    {
        var dist = Vector2.Distance(transform.position, destPos);
        var targetDist = Vector2.Distance(destPos, transform.position);

        if (dist < 0.1f)
        {
            lastPatrolTime = Time.time;
            state = MonsterState.Idle;
        }
        else
            Move();
    }
    protected virtual void DestroySelf()
    {
        Object.Destroy(gameObject);
    }
    public virtual void OnTakeDamaged()
    {
        state = MonsterState.Chase;
    }
    public override void TakeDamage(float dmg)
    {
        hp = Mathf.Clamp(hp - dmg, 0, hp);
        if (hp == 0)
            state = MonsterState.Die;
        else
            state = MonsterState.TakeHit;
    }
    protected abstract Vector2 GenRandomPosition();
    public virtual void StartAttack() {}
    public virtual void OnAttackReturn() {}
    public virtual void OnAttackFinished() {}
    public virtual void OnAttacked() {}
    protected abstract void UpdateAnimation();
    protected abstract void UpdateController();
}
