using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : MonsterController
{
    Vector2 _startAttackPos;
    FlyingeyeData _data;
    Coroutine _coMoveAttack = null;
    float OnAttackDuration { get { return _data.OnAttackDuration; } }
    float FinishAttackDuration { get { return _data.FinishAttackDuration; } }
    CircleCollider2D detectionRangeCollider;
    public override void OnAttacked()
    {
        Vector2 pos = transform.position;
        var coll = Physics2D.OverlapCircle(pos, attackHitboxRadius, LayerMask.GetMask("Player"));
        if (coll == null) return;
        var player = coll.GetComponent<PlayerController>();
        if (player == null) return;
        player.TakeDamage(power);
    }
    public override void OnAttackReturn()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        _coMoveAttack = StartCoroutine(CoMoveAttack(_startAttackPos, FinishAttackDuration));
    }
    public override void OnAttackFinished()
    {
        if (target == null)
        {
            state = MonsterState.Idle;
            return;
        }

        var dist = Vector2.Distance(transform.position, target.transform.position);
        if (dist > patrol.detectionRange)
        {
            state = MonsterState.Idle;
            target = null;
        }
        else if (dist > attackRange)
        {
            state = MonsterState.Chase;
        }
        else
        {
            animator.Play("Attack1");
        }
    }
    IEnumerator CoMoveAttack(Vector2 destPos, float duration)
    {
        Vector2 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector2.Lerp(startPos, destPos, t);
            yield return null;
        }
        transform.position = destPos;
        _coMoveAttack = null;
    }
    public override void StartAttack()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }

        _startAttackPos = transform.position;
        _coMoveAttack = StartCoroutine(CoMoveAttack(target.transform.position, OnAttackDuration));
    }
    protected override void UpdateAnimation()
    {
        switch (state)
        {
            case MonsterState.Idle:
            case MonsterState.Patrol:
            case MonsterState.Chase:
                animator.Play("Flight");
                break;
            case MonsterState.Attack:
                animator.Play("Attack1");
                break;
            case MonsterState.TakeHit:
                animator.Play("TakeHit");
                break;
            case MonsterState.Die:
                animator.Play("Death");
                break;
        }
    }
    protected override void UpdateController()
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
    protected override Vector2 GenRandomPosition()
    {
        var range = Random.Range(0, patrol.range);
        var ranX = Random.Range(-1f, 1f);
        var ranY = Random.Range(-1f, 1f);
        var nextDir = new Vector2(ranX, ranY).normalized;

        return (Vector2)transform.position + nextDir * range;
    }
    protected override void Init()
    {
        base.Init();
        _data = monsterData as FlyingeyeData;
        detectionRangeCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionRangeCollider.isTrigger = true;
        detectionRangeCollider.radius = patrol.detectionRange;
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;
        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(detectionRangeCollider, collision);
        collision.size = (Vector2)spriteRenderer.bounds.size;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = collision.gameObject;
            state = MonsterState.Chase;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = null;
            state = MonsterState.Idle;
        }
    }
}
