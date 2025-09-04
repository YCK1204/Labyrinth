using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMonsterController : GroundMonsterController
{
    BossMonsterData _bossData;
    MonsterAttackHitboxController _attackHitbox;
    bool attacked = false;
    protected override Vector2 destPos
    {
        get { return _destPos; }
        set
        {
            _destPos = value;
            var scale = transform.localScale;
            if (_destPos.x < transform.position.x)
            {
                _destDir = Vector2.left;
                scale.x = Mathf.Abs(scale.x);
            }
            else
            {
                _destDir = Vector2.right;
                scale.x = -Mathf.Abs(scale.x);
            }
            transform.localScale = scale;
        }
    }
    Vector2 _destDir = Vector2.zero;
    public override void StartAttack()
    {
        speed = 0;
        attacked = false;
        playHitFail = false;
    }
    bool playHitFail = false;
    public override void OnAttacked()
    {
        var audioData = Manager.Audio.Monster.GetAudiodata(MonsterAudioType.Boss);
        if (attacked) return;
        Vector2 pos = transform.position;
        var coll = _attackHitbox.Check();
        if (coll == null)
        {
            if (!playHitFail)
            {
                Manager.Audio.PlayOneShot(audioData.HitFail, pos);
                playHitFail = true;
            }
            return;
        }
        var player = coll.GetComponent<PlayerController>();
        bool isCrit = Random.Range(0f, 100f) < crit;
        var dmg = power * (100 / (100 + Mathf.Max(0, player.armor - armorPen))) * (isCrit ? critX : 1);
        dmg = Mathf.Round(dmg * 10f) / 10f;
        bool isDamage = player._TakeDamage(dmg);
        attacked = true;
        if (DamageUI.Instance != null & isDamage)
            DamageUI.Instance.Show(player.transform.position + Vector3.up * 1.0f, dmg, DamageStyle.Player, isCrit);
        if (player._rolling)
            Manager.Audio.PlayOneShot(audioData.HitSuccess[0], pos);
    }
    public override void OnAttackFinished()
    {
        if (target == null || target.hp == 0)
        {
            state = MonsterState.Idle;
            return;
        }

        destPos = target.transform.position;
        var dist = Vector2.Distance(transform.position, destPos);
        if (dist < patrol.detectionRange)
        {
            if (dist < attackRange)
                animator.Play("Attack", -1, 0f);
            else if (dist < _bossData.FastChaseArea)
            {
                state = MonsterState.ChaseRun;
                speed = _bossData.RunningSpeed;
            }
            else
            {
                state = MonsterState.Chase;
                speed = _bossData.WalkingSpeed;
            }
        }
        else
        {
            speed = _bossData.WalkingSpeed;
            state = MonsterState.Idle;
            target = null;
        }
    }
    public void AdjustLeapSpeed()
    {
        speed = _bossData.LeapSpeed;
    }
    public void AdjustRunSpeed()
    {
        speed = _bossData.LeapSpeed;
    }
    protected override void Attack()
    {
        Move();
    }
    public override void TakeDamage(float dmg)
    {
        hp = Mathf.Clamp(hp - dmg, 0, hp);
        if (hp == 0)
        {
            state = MonsterState.Die;
            var audioData = Manager.Audio.Monster.GetAudiodata(MonsterAudioType.Boss);
            Manager.Audio.PlayOneShot(audioData.Die, transform.position);
        }
    }
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        switch (state)
        {
            case MonsterState.ChaseRun:
                animator.Play("Run", -1, 0f);
                break;
        }
    }
    protected override void UpdateController()
    {
        base.UpdateController();

        switch (state)
        {
            case MonsterState.ChaseRun:
                Chase();
                break;
            case MonsterState.Attack:
                Attack();
                break;
        }
    }
    protected override void Move()
    {
        var pos = (Vector2)transform.position + speed * Time.deltaTime * _destDir;
        pos.y = detectionCollider.bounds.min.y + .01f;
        Ray ray = new Ray(pos, Vector2.down);
        var hit = Physics2D.Raycast(pos, Vector2.down, .1f, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            if (state == MonsterState.Patrol)
                state = MonsterState.Idle;
            return;
        }
        pos = spriteRenderer.bounds.center;
        pos.x = destDir.x > 0 ? spriteRenderer.bounds.max.x : spriteRenderer.bounds.min.x;
        ray = new Ray(pos, destDir);
        hit = Physics2D.Raycast(pos, destDir, .1f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            if (state == MonsterState.Patrol)
                state = MonsterState.Idle;
            return;
        }
        transform.position += speed * Time.deltaTime * (Vector3)_destDir;
    }
    protected override void Init()
    {
        base.Init();
        _bossData = _gmData as BossMonsterData;

        if (_bossData == null)
        {
            Debug.LogError("BossMonsterData is null");
            return;
        }
        speed = _bossData.WalkingSpeed;
        var fastChaseArea = new GameObject("FaseChaseArea").AddComponent<TriggerSensorController>();
        fastChaseArea.Init(transform, GetTopFloorY(), GetBottomFloorY(), _bossData.FastChaseArea, LayerMask.NameToLayer("Player"));
        fastChaseArea.SetCallback(FastChaseEnter, FastChaseExit);

        // ���� ������ MonsterAttackHitboxController ����
        var attackHitbox = new GameObject("AttackHitbox");
        _attackHitbox = attackHitbox.AddComponent<MonsterAttackHitboxController>();
        _attackHitbox.Init(attackHitboxRadius, transform, _bossData.AttackHitboxOffset, 1 << LayerMask.NameToLayer("Player"));

        startPosition = transform.position;
    }
    void FastChaseEnter(Collider2D collision)
    {
        var pc = collision.GetComponent<PlayerController>();
        if (pc == null)
            return;
        target = pc;
        speed = _bossData.RunningSpeed;
        state = MonsterState.ChaseRun;
    }
    void FastChaseExit(Collider2D collision)
    {
        var pc = collision.GetComponent<PlayerController>();
        if (pc == null)
            return;
        target = pc;
        speed = _bossData.WalkingSpeed;
        state = MonsterState.Chase;
    }
}
