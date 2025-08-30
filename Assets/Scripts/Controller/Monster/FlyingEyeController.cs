using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : MonsterController
{
    Vector2 _startAttackPos;
    Coroutine _coMoveAttack = null;
    [SerializeField]
    float attack1OnAttackDuration;
    [SerializeField]
    float attack1FinishAttackDuration;
    [SerializeField]
    float attack2OnAttackDuration;
    [SerializeField]
    float attack2FinishAttackDuration;


    [SerializeField]
    float Speed;
    [SerializeField]
    float AttackRange;
    public override void OnAttacked()
    {
        // 공격 범위 탐색 후 takedamege 호출 결정
    }
    public void OnAttackReturn()
    {
        float duration = (AttackAnimNum == 1) ? attack1FinishAttackDuration : attack2FinishAttackDuration;

        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        _coMoveAttack = StartCoroutine(CoMoveAttack(_startAttackPos, duration));
    }
    public void OnAttackFinished()
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

    protected override void Attack()
    {

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
    public void StartAttack()
    {
        if (target == null)
        {
            state = MonsterState.Idle;
            return;
        }
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }

        float duration = (AttackAnimNum == 1) ? attack1OnAttackDuration : attack2OnAttackDuration;
        _startAttackPos = transform.position;
        _coMoveAttack = StartCoroutine(CoMoveAttack(target.transform.position, duration));
    }
    protected override void Chase()
    {
        if (target == null)
        {
            state = MonsterState.Idle;
            return;
        }

        if (Time.time - lastChaseTime > patrol.detectionInterval)
        {
            destPos = target.transform.position;
            lastChaseTime = Time.time;
        }

        var dist = Vector2.Distance(transform.position, target.transform.position);
        if (dist > patrol.detectionRange)
        {
            state = MonsterState.Idle;
            target = null;
            return;
        }

        if (dist < attackRange)
        {
            state = MonsterState.Attack;
        }
        else
        {
            Move();
        }
    }

    protected override void Die()
    {
    }
    protected override void Idle()
    {
        if (Time.time - lastPatrolTime > patrol.interval)
        {
            destPos = GenRandomPosition();
            state = MonsterState.Patrol;
        }
    }

    protected override void Move()
    {
        transform.position += speed * Time.deltaTime * (Vector3)destDir;
        spriteRenderer.flipX = (destDir.x < 0);
    }

    protected override void Patrol()
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
    public override void OnTakeDamaged()
    {
        state = MonsterState.Chase;
    }

    protected override void TakeDamage(float dmg)
    {
        hp = Mathf.Clamp(hp - dmg, 0, hp);
        if (hp == 0)
            state = MonsterState.Die;
        else
            state = MonsterState.TakeHit;
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
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Die:
                Die();
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
        detectionRangeCollider.radius = patrol.detectionRange;
        speed = Speed;
        attackRange = AttackRange;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = collision.gameObject;
            state = MonsterState.Chase;
        }
    }
}
