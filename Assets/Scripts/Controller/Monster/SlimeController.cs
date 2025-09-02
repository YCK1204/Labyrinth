using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonsterController
{
    SlimeData _slimeData;
    float _maxCheckDist { get { return _slimeData.MaxCheckDist; } }

    private BoxCollider2D detectionCollider;
    protected override Vector2 destDir => destPos.x < transform.position.x ? Vector2.left : Vector2.right;
    private MonsterAttackHitboxController _attackHitbox;
    float _speed;
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
                animator.Play("Walk", -1, 0f);
                break;
            case MonsterState.Attack:
                animator.Play("Attack", -1, 0f);
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
            case MonsterState.Attack:
                Move();
                break;
        }
    }
    public override void StartAttack()
    {
        speed = 0;
    }
    public void StartJumpAttack()
    {
        speed = _speed * 5f;
        StartCoroutine(CoAttacking());
    }
    public void EndJumpAttack()
    {
        speed = 0;
        StopCoroutine(CoAttacking());
    }
    public override void OnAttackFinished()
    {
        speed = _speed;
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
            destPos = target.transform.position;
            animator.Play("Attack", -1, 0f);
        }
    }
    IEnumerator CoAttacking()
    {
        while (true)
        {
            var collider = _attackHitbox.Check();
            if (collider != null)
            {
                var pc = collider.GetComponent<PlayerController>();
                if (pc != null)
                {
                    var dmg = power * (100 / (100 + Mathf.Max(0, target.armor - armorPen))) * (Random.Range(0f, 100f) < crit ? critX : 1);
                    pc.TakeDamage(dmg);
                    break;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    float GetTopFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.up, _maxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        return hit.collider != null ? hit.point.y : transform.position.y + _maxCheckDist;
    }
    float GetBottomFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, _maxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        return hit.collider != null ? hit.point.y : transform.position.y - _maxCheckDist;
    }
    protected override void Init()
    {
        base.Init();
        _slimeData = monsterData as SlimeData;
        _speed = speed;

        if (_slimeData == null)
        {
            Debug.LogError("GoblinController: monsterData is not GoblinData");
            return;
        }
        var child = new GameObject("Collision");
        child.transform.parent = transform;
        child.layer = LayerMask.NameToLayer("MonsterCollision");
        child.transform.localPosition = Vector2.zero;

        detectionCollider = gameObject.AddComponent<BoxCollider2D>();
        detectionCollider.isTrigger = true;

        float yBottom = GetBottomFloorY();
        float yTop = GetTopFloorY();
        float height = yTop - yBottom;

        detectionCollider.size = new Vector2(patrol.detectionRange / transform.localScale.x, height / transform.localScale.y);
        detectionCollider.offset = new Vector2(0, ((yTop + yBottom) / 2 - transform.position.y) / transform.localScale.y);

        var collision = child.gameObject.AddComponent<BoxCollider2D>();
        collision.size = (Vector2)spriteRenderer.bounds.size;
        var offset = collision.bounds.center.y - collision.bounds.min.y;
        transform.position = new Vector2(transform.position.x, yBottom + offset);

        Physics2D.IgnoreCollision(detectionCollider, collision);

        _attackHitbox = new GameObject("AttackHitbox").AddComponent<MonsterAttackHitboxController>();
        _attackHitbox.transform.parent = transform;
        _attackHitbox.Init(attackHitboxRadius, transform, Vector2.zero, LayerMask.GetMask("Player"));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc == null)
                return;
            speed = _speed;
            target = null;
            state = MonsterState.Idle;
        }
    }
}
