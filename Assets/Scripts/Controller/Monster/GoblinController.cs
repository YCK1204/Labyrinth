using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : GroundMonsterController
{
    GoblinData _data;
    MonsterAttackHitboxController _attackHitbox;

    public override void OnAttacked()
    {
        Vector2 pos = transform.position;
        bool isCrit = Random.Range(0f, 100f) < crit;
        var coll = _attackHitbox.Check();
        if (coll == null) return;
        var player = coll.GetComponent<PlayerController>();
        if (player == null) return;
        var dmg = power * (100 / (100 + Mathf.Max(0, player.armor - armorPen))) * (isCrit ? critX : 1);
        dmg = Mathf.Round(dmg * 10f) / 10f;
        player.TakeDamage(dmg);
        if (DamageUI.Instance != null)
            DamageUI.Instance.Show(player.transform.position + Vector3.up * 1.0f, dmg, DamageStyle.Player, isCrit);
        
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
            animator.Play("Attack", -1, 0f);
        }
    }
    protected override void Init()
    {
        base.Init();

        _data = _gmData as GoblinData;
        if (_data == null)
        {
            Debug.LogError("GoblinController: monsterData is not GoblinData");
            return;
        }

        // ���� ������ MonsterAttackHitboxController ����
        var attackHitbox = new GameObject("AttackHitbox");
        _attackHitbox = attackHitbox.AddComponent<MonsterAttackHitboxController>();
        _attackHitbox.Init(attackHitboxRadius, transform, _gmData.AttackHitboxOffset, 1 << LayerMask.NameToLayer("Player"));

        startPosition = transform.position;
    }
}
