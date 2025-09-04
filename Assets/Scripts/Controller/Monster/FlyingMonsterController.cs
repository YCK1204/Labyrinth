using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonsterController : MonsterController
{
    protected FlyingMonsterData _fmData;

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
