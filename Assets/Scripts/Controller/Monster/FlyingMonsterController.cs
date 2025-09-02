using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonsterController : MonsterController
{
    protected FlyingMonsterData _fmData;
    public override void OnAttacked()
    {
        Vector2 pos = transform.position;
        var coll = Physics2D.OverlapCircle(pos, attackHitboxRadius, LayerMask.GetMask("Player"));
        if (coll == null) return;
        var player = coll.GetComponent<PlayerController>();
        if (player == null) return;
        var dmg = power * (100 / (100 + Mathf.Max(0, player.armor - armorPen))) * (Random.Range(0f, 100f) < crit ? critX : 1);
        player.TakeDamage(dmg);
    }
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        switch (state)
        {
            case MonsterState.Idle:
                animator.Play("Idle", -1, 0f);
                break;
            case MonsterState.Patrol:
            case MonsterState.Chase:
                animator.Play("Flying", -1, 0f);
                break;
        }
    }
    override protected void Init()
    {
        base.Init();

        _fmData = monsterData as FlyingMonsterData;
        detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        (detectionCollider as CircleCollider2D).radius = patrol.detectionRange;
        startPosition = transform.position;
        InitCollisionChild();
    }
}
