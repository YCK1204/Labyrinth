using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class Patrol
{
    public float range;
    public float interval;
    public float detectionRange;
    public Vector2[] directions;
    public float lastPatrolTime;
}

public abstract class MonsterController : CreatureController
{
    protected MonsterData monsterData;
    protected Patrol patrol = new Patrol();
    protected float attackHitboxRadius { get { return monsterData.AttackHitboxRadius; } }
    protected float attackRange { get { return monsterData.AttackRange; } }
    protected Vector2 startPosition;
    protected bool isDead;
    public bool IsAlive => !isDead;

    protected PlayerController target;
    protected Vector2 _destPos = Vector2.zero;
    protected virtual Vector2 destPos
    {
        get { return _destPos; }
        set
        {
            _destPos = value;
            var scale = transform.localScale;
            if (_destPos.x < transform.position.x)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    protected virtual Vector2 destDir { get { return (destPos - (Vector2)transform.position).normalized; } }
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected Collider2D detectionCollider;
    private float fadeoutTime = 1f;
    protected enum MonsterState
    {
        Idle,
        Patrol,
        Chase,
        ChaseRun,
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
            if (_state != value && _state != MonsterState.Die)
            {
                _state = value;
                UpdateAnimation();
            }
        }
    }
    protected override void OnDied()
    {
        StartCoroutine(FadeOut());
        Manager.Game.Player.GainExp(monsterData.Exp);
        Manager.Game.Player.GainGold(monsterData.Gold);
    }
    protected void InitCollisionChild()
    {
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;
        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(detectionCollider, collision);
        collision.size = (Vector2)spriteRenderer.bounds.size;
    }
    protected override void Init()
    {
        base.Init();
        monsterData = creatureData as MonsterData;
        if (monsterData == null)
        {
            Debug.LogError("Unassigned MonsterData to " + gameObject.name);
            return;
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = monsterData.SpriteForCollisionSize;
        patrol.range = monsterData.Patrol.range;
        patrol.interval = monsterData.Patrol.interval;
        patrol.detectionRange = monsterData.Patrol.detectionRange;
        patrol.directions = monsterData.Patrol.directions;
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
        if (state == MonsterState.Die)
            return;
        UpdateController();
    }
    protected override void Move()
    {
        transform.position += speed * Time.deltaTime * (Vector3)destDir;
    }
    protected virtual void Idle()
    {
        if (Time.time - patrol.lastPatrolTime > patrol.interval)
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
        if (target == null || target.hp == 0)
        {
            state = MonsterState.Idle;
            return;
        }

        destPos = target.transform.position;

        var dist = Vector2.Distance(transform.position, destPos);
        if (dist < attackRange)
            state = MonsterState.Attack;
        else
            Move();
    }
    protected virtual void Die() { }
    protected virtual void Patrol()
    {
        var dist = Vector2.Distance(transform.position, destPos);
        var targetDist = Vector2.Distance(destPos, transform.position);

        if (dist < 0.1f)
        {
            patrol.lastPatrolTime = Time.time;
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
        if (isDead) return;

        hp = Mathf.Clamp(hp - dmg, 0, hp);
        if (hp <= 0) {
            isDead = true;  
            state = MonsterState.Die;
        }
        else
            state = MonsterState.TakeHit;
    }
    protected virtual Vector2 GenRandomPosition()
    {
        var directions = patrol.directions;
        var ranInt = Random.Range(0, directions.Length);
        var dir = directions[ranInt];
        var range = Random.Range(0, patrol.range);

        return startPosition + dir * range;
    }
    public virtual void StartAttack() { }
    public virtual void OnAttackReturn() { }
    public virtual void OnAttackFinished() { }
    public virtual void OnAttacked() { }
    protected virtual void UpdateAnimation()
    {
        switch (state)
        {
            case MonsterState.Attack:
                animator.Play("Attack");
                break;
            case MonsterState.TakeHit:
                animator.Play("TakeHit", -1, 0f);
                break;
            case MonsterState.Die:
                animator.Play("Death", -1, 0f);
                break;
        }
    }
    protected virtual void UpdateController()
    {
        switch (state)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Chase:
                Chase();
                break;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc == null)
                return;
            target = pc;
            state = MonsterState.Chase;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc == null)
                return;
            target = null;
            state = MonsterState.Idle;
        }
    }
}
