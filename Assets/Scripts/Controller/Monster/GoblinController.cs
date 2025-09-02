using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonsterController, IPaltformAwareMonster
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
        var dmg = power * (100 / (100 + Mathf.Max(0, player.armor - armorPen))) * (Random.Range(0f, 100f) < crit ? critX : 1);
        player.TakeDamage(dmg);
    }
    public override void OnAttackFinished()
    {
        if (target == null || target.hp == 0)
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
    protected override void Init()
    {
        base.Init();

        _data = monsterData as GoblinData;
        if (_data == null)
        {
            Debug.LogError("GoblinController: monsterData is not GoblinData");
            return;
        }
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;

        _detectionRangeCollider = gameObject.AddComponent<BoxCollider2D>();
        _detectionRangeCollider.isTrigger = true;

        float yTop = (this as IPaltformAwareMonster).GetTopFloorY(transform, maxCheckDist);
        float yBottom = (this as IPaltformAwareMonster).GetBottomFloorY(transform, maxCheckDist);
        float height = yTop - yBottom;

        _detectionRangeCollider.size = new Vector2(patrol.detectionRange / transform.localScale.x, height / transform.localScale.y);
        _detectionRangeCollider.offset = new Vector2(0, ((yTop + yBottom) / 2 - transform.position.y) / transform.localScale.y);

        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        collision.size = (Vector2)spriteRenderer.bounds.size;
        var offset = collision.bounds.center.y - collision.bounds.min.y;
        transform.position = new Vector2(transform.position.x, yBottom + offset);

        var attackHitbox = new GameObject("AttackHitbox");
        attackHitboxController = attackHitbox.AddComponent<MonsterAttackHitboxController>();
        attackHitboxController.Init(attackHitboxRadius, transform, attackHitboxOffset, 1 << LayerMask.NameToLayer("Player"));

        Physics2D.IgnoreCollision(_detectionRangeCollider, collision);
    }
}
