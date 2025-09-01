using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonsterController
{
    [SerializeField]
    float Speed;
    [SerializeField]
    float AttackRange;
    BoxCollider2D _detectionRangeCollider;
    Rigidbody2D _rigidbody2D;

    [SerializeField]
    float backdumblingDuration;
    [SerializeField]
    float attack2OnAttackDuration;
    [SerializeField]
    float attack2FinishAttackDuration;
    [SerializeField]
    float maxCheckDist;
    Vector2 _startAttackPos;
    Coroutine _coMoveAttack = null;
    public override void OnAttacked()
    {
    }
    public override void StartAttack()
    {
        _startAttackPos = transform.position;
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        var position = (Vector2)transform.position;
        Vector2 dest = position + (spriteRenderer.flipX ? Vector2.right : Vector2.left);
        destPos = target.transform.position;
        _coMoveAttack = StartCoroutine(CoMoveAttack(dest, backdumblingDuration));
    }
    public void OnBackdumblingFinished()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        _coMoveAttack = StartCoroutine(CoMoveAttack(new (destPos.x, transform.position.y), attack2OnAttackDuration));
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
            AttackAnimNum = Random.Range(1, attackAnimCount + 1);
            animator.Play("Attack" + AttackAnimNum.ToString());
        }
    }
    public override void OnAttackReturn()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        _coMoveAttack = StartCoroutine(CoMoveAttack(_startAttackPos, backdumblingDuration));
    }
    IEnumerator CoMoveAttack(Vector2 destPos, float duration)
    {
        float time = 0;
        Vector2 startPos = transform.position;
        while (time < duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            transform.position = Vector2.Lerp(startPos, destPos, t);
            yield return null;
        }
        transform.position = destPos;
        _coMoveAttack = null;
    }
    protected override Vector2 GenRandomPosition()
    {
        var directions = patrol.directions;
        var ranInt = Random.Range(0, directions.Length);
        var dir = directions[ranInt];
        var range = Random.Range(0, patrol.range);

        return transform.position + (Vector3)dir * range;
    }

    float GetTopFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.up, maxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position, Vector2.up * maxCheckDist, Color.red, 10f);
        return hit.collider != null ? hit.point.y : transform.position.y + maxCheckDist;
    }
    float GetBottomFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, maxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * maxCheckDist, Color.red, 10f);
        return hit.collider != null ? hit.point.y : transform.position.y - maxCheckDist;
    }
    protected override void UpdateAnimation()
    {
        switch (state)
        {
            case MonsterState.Idle:
                animator.Play("Idle");
                break;
            case MonsterState.Patrol:
            case MonsterState.Chase:
                animator.Play("Run");
                break;
            case MonsterState.Attack:
                AttackAnimNum = Random.Range(1, attackAnimCount + 1);
                animator.Play("Attack" + AttackAnimNum.ToString());
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = collision.gameObject;
            state = MonsterState.Chase;
        }
    }
    protected override void Init()
    {
        base.Init();
        patrol.directions = new Vector2[] { Vector2.left, Vector2.right };
        _detectionRangeCollider = gameObject.AddComponent<BoxCollider2D>();
        _detectionRangeCollider.isTrigger = true;
        float yBottom = GetBottomFloorY();
        float yTop = GetTopFloorY();
        float height = yTop - yBottom;
        _detectionRangeCollider.size = new Vector2(patrol.detectionRange, height);
        _detectionRangeCollider.offset = new Vector2(0, (yTop + yBottom) / 2 - transform.position.y);
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;
        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_detectionRangeCollider, collision);
        Debug.Log(spriteRenderer.bounds.size);
        collision.size = (Vector2)spriteRenderer.bounds.size;
        _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        speed = Speed;
        attackRange = AttackRange;
    }
}
