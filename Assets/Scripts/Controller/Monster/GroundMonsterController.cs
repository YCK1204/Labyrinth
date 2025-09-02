using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMonsterController : MonsterController
{
    protected GroundMonsterData _gmData;
    protected override Vector2 destDir => destPos.x < transform.position.x ? Vector2.left : Vector2.right;

    public float GetTopFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.up, _gmData.MaxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        return hit.collider != null ? hit.point.y : transform.position.y + _gmData.MaxCheckDist;
    }
    public float GetBottomFloorY()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, _gmData.MaxCheckDist, 1 << LayerMask.NameToLayer("Ground"));
        return hit.collider != null ? hit.point.y : transform.position.y - _gmData.MaxCheckDist;
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
                animator.Play("Walk", -1, 0f);
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

        _gmData = monsterData as GroundMonsterData;
        // 몬스터 파격용 박스콜라이더 생성

        // 플레이어 감지용 박스콜라이더 생성
        detectionCollider = gameObject.AddComponent<BoxCollider2D>();
        detectionCollider.isTrigger = true;

        // 현재 위 아래 플랫폼 사이 간격에 맞게 감지 콜라이더 크기 조정
        float yTop = GetTopFloorY();
        float yBottom = GetBottomFloorY();
        float height = yTop - yBottom;

        (detectionCollider as BoxCollider2D).size = new Vector2(patrol.detectionRange / transform.localScale.x, height / transform.localScale.y);
        detectionCollider.offset = new Vector2(0, ((yTop + yBottom) / 2 - transform.position.y) / transform.localScale.y);

        InitCollisionChild();
        var collision = transform.FindChild<BoxCollider2D>(name:"Collision");
        var offset = collision.bounds.center.y - collision.bounds.min.y;
        transform.position = new Vector2(transform.position.x, yBottom + offset);
        Physics2D.IgnoreCollision(detectionCollider, collision);
        startPosition = transform.position;
    }
}
