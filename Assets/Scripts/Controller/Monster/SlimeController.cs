using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : GroundMonsterController
{
    SlimeData _slimeData;
    private MonsterAttackHitboxController _attackHitbox;
    float _speed;
    protected override void UpdateController()
    {
        base.UpdateController();

        switch (state)
        {
            case MonsterState.Attack:
                Move();
                break;
        }
    }
    #region Attack
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
                    bool isCrit = Random.Range(0f, 100f) < crit;
                    var dmg = power * (100 / (100 + Mathf.Max(0, target.armor - armorPen))) *  (isCrit ? critX : 1);
                    dmg = Mathf.Round(dmg * 10f) / 10f;
                    pc.TakeDamage(dmg);
                    if (DamageUI.Instance != null)
                        DamageUI.Instance.Show(pc.transform.position + Vector3.up * 1.0f, dmg, DamageStyle.Player, isCrit);
                    break;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    #endregion
    protected override void Move()
    {
        var pos = transform.position + speed * Time.deltaTime * (Vector3)destDir;
        pos.y = detectionCollider.bounds.min.y + .01f;
        Ray ray = new Ray(pos, Vector2.down);
        var hit = Physics2D.Raycast(pos, Vector2.down, .1f, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            if (state == MonsterState.Patrol)
                state = MonsterState.Idle;
            return;
        }
        base.Move();
    }
    protected override void Init()
    {
        base.Init();
        _slimeData = _gmData as SlimeData;
        _speed = speed;

        if (_slimeData == null)
        {
            Debug.LogError("GoblinController: monsterData is not GoblinData");
            return;
        }

        // ���� ������ MonsterAttackHitboxController ����
        _attackHitbox = new GameObject("AttackHitbox").AddComponent<MonsterAttackHitboxController>();
        _attackHitbox.transform.parent = transform;
        _attackHitbox.Init(attackHitboxRadius, transform, Vector2.zero, LayerMask.GetMask("Player"));
        startPosition = transform.position;
    }
    protected override void OnTriggerExit2D(Collider2D collision)
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
