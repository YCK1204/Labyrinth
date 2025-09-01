using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonsterController
{
    GoblinData _data;
    BoxCollider2D _detectionRangeCollider;
    MonsterAttackHitboxController attackHitboxController;
    Vector2 attackHitboxOffset { get { return _data.AttackHitboxOffset; } }
    float maxCheckDist { get { return _data.MaxCheckDist; } }
    protected override Vector2 destDir => destPos.x < transform.position.x ? Vector2.left : Vector2.right;
    public override void OnAttacked()
    {
        Vector2 pos = transform.position;
        var coll = attackHitboxController.Check();
        if (coll == null) return;
        var player = coll.GetComponent<PlayerController>();
        if (player == null) return;
        player.TakeDamage(3);
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
                animator.Play("Idle", -1, 0f);
                break;
            case MonsterState.Patrol:
            case MonsterState.Chase:
                animator.Play("Run", -1, 0f);
                break;
            case MonsterState.Attack:
                animator.Play("Attack1");
                break;
            case MonsterState.TakeHit:
                animator.Play("TakeHit", -1, 0f);
                break;
            case MonsterState.Die:
                animator.Play("Death", -1, 0f);
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = null;
            state = MonsterState.Idle;
        }
    }
    protected override void Move()
    {
        transform.position += speed * Time.deltaTime * (Vector3)destDir;
    }
    protected override void Init()
    {
        base.Init();

        _data = monsterData as GoblinData;
        if (_data == null)
        {
            Debug.LogError("GoblinController: monsterData is not GoblinData");
            return;
        }
        _detectionRangeCollider = gameObject.AddComponent<BoxCollider2D>();
        _detectionRangeCollider.isTrigger = true;
        float yBottom = GetBottomFloorY();
        float yTop = GetTopFloorY();
        float height = yTop - yBottom;

        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;
        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_detectionRangeCollider, collision);
        collision.size = (Vector2)spriteRenderer.bounds.size;
        var offset = collision.bounds.center.y - collision.bounds.min.y;
        transform.position = new Vector2(transform.position.x, yBottom + offset);
        _detectionRangeCollider.size = new Vector2(patrol.detectionRange / transform.localScale.x, height / transform.localScale.y);
        _detectionRangeCollider.offset = new Vector2(0, ((yTop + yBottom) / 2 - transform.position.y) / transform.localScale.y);
        var attackHitbox = new GameObject("AttackHitbox");
        attackHitboxController = attackHitbox.AddComponent<MonsterAttackHitboxController>();
        attackHitboxController.Init(attackHitboxRadius, transform, attackHitboxOffset, 1 << LayerMask.NameToLayer("Player"));
    }
}
