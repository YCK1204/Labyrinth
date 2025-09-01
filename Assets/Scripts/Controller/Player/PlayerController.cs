using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : CreatureController
{
    [Header("Stats")]
    [SerializeField] private PlayerStats PlayerStats;

    [Header("Refs")]
    [SerializeField] private SimpleSensor2D GroundSensor;
    [SerializeField] private Transform AttackPoint;     // 칼끝 기준점
    [SerializeField] private float AttackRadius = 0.6f; // 원 범위 반경
    [SerializeField] private LayerMask EnemyLayer;

    [Header("Force")]
    [SerializeField] private float JumpForce = 7.5f;
    [SerializeField] private float RollForce = 10f;
    [SerializeField] private float RollDuration = 0.5f;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Playeranimator _anim;
    private ComboController _combo;
    private Vector2 _attackPointDefault;

    private bool grounded, rolling;
    private int facing = 1;
    private float rollTimer;

    private float inputX;
    private bool jump, roll, attackHold, blockDown, blockUp;

    private void Awake()
    {
        // 초기화: SO로 스탯 초기화
        if (PlayerStats != null)
        {
            hp = PlayerStats.hp;
            speed = PlayerStats.speed;
            armor = PlayerStats.armor;
            power = PlayerStats.power;
            crit = PlayerStats.crit;
            lv = PlayerStats.lv;
            kbResist = PlayerStats.kbResist;
            critX = PlayerStats.critX;
            armorPen = PlayerStats.armorPen;
            atkSpeed = PlayerStats.atkSpeed;
        }
        if (AttackPoint) _attackPointDefault = AttackPoint.localPosition;

        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = new Playeranimator(GetComponent<Animator>());
        _rb.freezeRotation = true;

        float baseMinGap = 0.5f;
        float minGap = baseMinGap / Mathf.Max(0.01f, atkSpeed);
        float resetGap = 1.0f;
        _combo = new ComboController(minGap, resetGap);
    }

    private void Update()
    {
        ReadInput();
        _combo.Tick(Time.deltaTime);
        SenseAndAnimate();
        HandleActions();
    }

    private void FixedUpdate()
    {
        Move();
    }
    // 이동 처리
    protected override void Move()
    {
        if (!rolling)
            _rb.velocity = new Vector2(inputX * speed, _rb.velocity.y);
    }
    // 공격 처리(몬스터 스탯이 생기면 데미지 처리 수정)
    protected override void Attack()
    {
        if (!_combo.TryNext(out var step)) return;

        _anim.TrgAttack(step);

        if (AttackRadius <= 0f) return;
        Vector2 center   = AttackPoint ? (Vector2)AttackPoint.position : (Vector2)transform.position;

        var hits = Physics2D.OverlapCircleAll(center, AttackRadius, EnemyLayer);

        bool anyHit = false;
        foreach (var h in hits)
        {
            var monster = h.GetComponentInParent<MonsterController>();
            if (monster == null) continue;

            float dmg = CalcFinalDamage(power, 0f);
            monster.TakeDamage(dmg);
            anyHit = true;

            Debug.Log($"{monster.name}에게 {dmg} 피해!");
        }
        if (!anyHit)
            Debug.Log("[ATTACK MISS] 히트 없음");
    }


    // 피격 처리
    public override void TakeDamage(float atk)
    {
        float dmg = CalcFinalDamage(atk, armor);
        hp -= dmg;

        if (hp <= 0f)
        {
            hp = 0f;
            OnDied();
        }
        else
        _anim.TrgHurt();

    }
    // 사망 처리
    protected override void OnDied()
    {
        _anim.TrgDeath();
        enabled = false;
    }
    // 구르기 시작
    private void OnRoll()
    {
        rolling = true;
        rollTimer = 0f;
        _anim.TrgRoll();
        _rb.velocity = new Vector2(facing * RollForce, _rb.velocity.y);
    }
    // 점프 시작
    private void OnJump()
    {
        _anim.TrgJump();
        grounded = false;
        _anim.SetGrounded(false);
        _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
        GroundSensor?.DisableFor(0.2f);
    }
    // 감지 & 애니메이션 동기화
    private void SenseAndAnimate()
    {
        bool nowGrounded = GroundSensor && GroundSensor.IsOn;
        if (grounded != nowGrounded)
        {
            grounded = nowGrounded;
            _anim.SetGrounded(grounded);
        }

        if (inputX > 0f)
        {
            facing = 1; if (_sr) _sr.flipX = false;
            UpdateAttackPointSide();
        }
        else if (inputX < 0f)
        {
            facing = -1; if (_sr) _sr.flipX = true;
            UpdateAttackPointSide();
        }

        _anim.SetAirY(_rb.velocity.y);
        _anim.SetState(Mathf.Abs(inputX) > Mathf.Epsilon ? 1 : 0);
    }
    // 액션 처리: 공격(홀드), 막기 토글, 구르기/점프
    private void HandleActions()
    {
        if (attackHold && !rolling)
            Attack();

        if (roll && !rolling) OnRoll();
        if (jump && grounded && !rolling) OnJump();

        if (rolling)
        {
            rollTimer += Time.deltaTime;
            if (rollTimer > RollDuration) rolling = false;
        }
    }
    //키입력
    private void ReadInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        jump = Input.GetKeyDown(KeyCode.Space);
        roll = Input.GetKeyDown(KeyCode.LeftShift);
        attackHold = Input.GetMouseButton(0);
        blockDown = Input.GetMouseButtonDown(1);
        blockUp = Input.GetMouseButtonUp(1);
    }
    //방향전환시 검 콜라이더 위치 조정
    private void UpdateAttackPointSide()
    {
        if (!AttackPoint) return;
        var pos = _attackPointDefault;
        pos.x = Mathf.Abs(pos.x) * (facing >= 0 ? 1 : -1);
        AttackPoint.localPosition = pos;
    }
    //피해량 계산식
    float CalcFinalDamage(float atk, float targetArmor)
    {
        float effArmor = Mathf.Max(0f, targetArmor - armorPen);
        float reducMul = 100f / (100f + effArmor);
        float critMul = (Random.value < crit) ? critX : 1f;
        return atk * reducMul * critMul;
    }
    private void OnDrawGizmosSelected()
{
    if (!AttackPoint) return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
}
}
