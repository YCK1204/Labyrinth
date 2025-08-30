using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Patrol
{
    public float range;
    public float interval;
    public float nextPatrolTime;
    public float detectionRange;
    public float detectionInterval;
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
    [SerializeField]
    protected int attackAnimCount;

    protected GameObject target;
    protected float lastPatrolTime;
    protected float lastChaseTime;
    protected float attackRange;
    protected Vector2 destPos = Vector2.zero;
    protected Vector2 destDir { get { return (destPos - (Vector2)transform.position).normalized; } }
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected CircleCollider2D detectionRangeCollider;

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
    int _attackAnimNum = 1;
    protected virtual int AttackAnimNum
    {
        get { return _attackAnimNum; }
        set
        {
            if (_attackAnimNum != value)
            {
                _attackAnimNum = value;
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
        detectionRangeCollider = gameObject.GetComponent<CircleCollider2D>();
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
    public abstract void OnAttacked();
    protected abstract void UpdateController();
    protected abstract void Idle();
    protected abstract void Chase();
    protected virtual void Die() { }
    protected abstract void Patrol();
    protected abstract void UpdateAnimation();
    protected virtual void DestroySelf()
    {
        Object.Destroy(gameObject);
    }
    protected abstract Vector2 GenRandomPosition();
    public abstract void OnTakeDamaged();
}
