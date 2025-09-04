using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : FlyingMonsterController
{
    Vector2 _startAttackPos;
    FlyingeyeData _data;
    Coroutine _coMoveAttack = null;
    float OnAttackDuration { get { return _data.OnAttackDuration; } }
    float FinishAttackDuration { get { return _data.FinishAttackDuration; } }
    public override void OnAttacked()
    {
        var audioData = Manager.Audio.Monster.GetAudiodata(MonsterAudioType.Bat);
        Vector2 pos = transform.position;
        bool isCrit = Random.Range(0f, 100f) < crit;
        var coll = Physics2D.OverlapCircle(pos, attackHitboxRadius, LayerMask.GetMask("Player"));
        if (coll == null) return;
        var player = coll.GetComponent<PlayerController>();
        if (player == null) return;
        var dmg = power * (100 / (100 + Mathf.Max(0, player.armor - armorPen))) * (isCrit ? critX : 1);
        dmg = Mathf.Round(dmg * 10f) / 10f;
        player.TakeDamage(dmg);
        if (DamageUI.Instance != null)
            DamageUI.Instance.Show(player.transform.position + Vector3.up * 1.0f, dmg, DamageStyle.Player, isCrit);
        if (player._rolling)
            Manager.Audio.PlayOneShot(audioData.HitSuccess[0], pos);
    }
    public override void OnAttackReturn()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }
        _coMoveAttack = StartCoroutine(CoMoveAttack(_startAttackPos, FinishAttackDuration));
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
            animator.Play("Attack");
        }
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
    public override void StartAttack()
    {
        if (_coMoveAttack != null)
        {
            StopCoroutine(_coMoveAttack);
            _coMoveAttack = null;
        }

        _startAttackPos = transform.position;
        _coMoveAttack = StartCoroutine(CoMoveAttack(target.transform.position, OnAttackDuration));
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
        _data = monsterData as FlyingeyeData;
    }
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        if (hp == 0)
        {
            var data = Manager.Audio.Monster.GetAudiodata(MonsterAudioType.Bat);
            Manager.Audio.PlayOneShot(data.Die, transform.position);
        }
    }
}
