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

    [SerializeField]
    float backdumblingDuration;
    [SerializeField]
    float attack2OnAttackDuration;
    [SerializeField]
    float attack2FinishAttackDuration;
    [SerializeField]
    float maxCheckDist;
    protected override Vector2 destDir => destPos.x < transform.position.x ? Vector2.left : Vector2.right;
    public override void OnAttacked()
    {
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
            spriteRenderer.flipX = (target.transform.position.x < transform.position.x);
            animator.Play("Attack1", -1, 0f);
        }
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
                spriteRenderer.flipX = (target.transform.position.x < transform.position.x);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = collision.gameObject;
            state = MonsterState.Chase;
        }
    }
    protected override void Move()
    {
        transform.position += speed * Time.deltaTime * (Vector3)destDir;
        spriteRenderer.flipX = (destDir.x < 0);
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
        var offset = spriteRenderer.size.y / 2;
        transform.position = new Vector2(transform.position.x, yBottom + offset);
        _detectionRangeCollider.size = new Vector2(patrol.detectionRange, height);
        _detectionRangeCollider.offset = new Vector2(0, (yTop + yBottom) / 2 - transform.position.y);
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;
        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_detectionRangeCollider, collision);
        collision.size = (Vector2)spriteRenderer.bounds.size;
        speed = Speed;
        attackRange = AttackRange;
    }
}
