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
    [SerializeField] private float RollCooldown = 0.4f; // 회피 쿨타임
    [SerializeField] private float RollIFrame   = 0.2f; // 무적 시간(초)

    [Header("Force")]
    [SerializeField] private float JumpForce = 7.5f;
    [SerializeField] private float RollForce = 10f;
    [SerializeField] private float RollDuration = 0.5f;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Playeranimator _anim;
    private ComboController _combo;
    private Vector2 _attackPointDefault;
    private bool _attackLocked;

    private bool _grounded, _rolling;
    private int _facing = 1;
    private float _rollTimer;
    private float _rollCooldownRemain;
    // private float timer = 0;

    private float _inputX;
    private bool _jump, _roll, _attack;

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
        float resetGap = 0.6f;
        _combo = new ComboController(minGap, resetGap);
    }

    private void Update()
    {
        ReadInput();
        _combo.Tick(Time.deltaTime);
        SenseAndAnimate();
        HandleActions();

        //피격 테스트용
        // timer += Time.deltaTime;
        // if (timer > 2f)
        // {
        //     timer = 0;
        //     TakeDamage(10f);
        // }
    }

    private void FixedUpdate()
    {
        Move();
    }
    // 이동 처리
    protected override void Move()
    {
        if (_rolling)
        {
            _rb.velocity = new Vector2(_facing * RollForce, _rb.velocity.y);
            return;
        }

        float vx = _rb.velocity.x;

        if (_attackLocked && _grounded)
        {
            vx = 0f;
        }
        else if (!_attackLocked)
        {
            vx = _inputX * speed;
        }

        _rb.velocity = new Vector2(vx, _rb.velocity.y);
    }
    // 공격 애니메이션
    protected override void Attack()
    {
        if (!_grounded)
        {
            if (!_combo.TryNext(out _, consumeStep: false)) return;
            _anim.TrgAttack(1);
            return;
        }

        if (!_combo.TryNext(out var step, consumeStep: true)) return;
        _anim.TrgAttack(step);
    }
    //실제 공격(몬스터 스탯이 생기면 데미지 처리 수정)
    public void OnAttackHit()
    {
        if (AttackRadius <= 0f) return;
        Vector2 center = AttackPoint ? (Vector2)AttackPoint.position : (Vector2)transform.position;

        var hits = Physics2D.OverlapCircleAll(center, AttackRadius, EnemyLayer);

        foreach (var h in hits)
        {
            var monster = h.GetComponentInParent<MonsterController>();
            if (monster == null) continue;

            var (dmg, isCrit) = CalcFinalDamage(power, 0f);
            monster.TakeDamage(dmg);

            // 데미지 UI
            int d = Mathf.RoundToInt(dmg);
            DamageUI.Instance.Show(monster.transform.position + Vector3.up * 1f, dmg ,DamageStyle.Enemy, isCrit);

            Debug.Log($"{monster.name}에게 {dmg} 피해!");
        }
    }
    public void OnAttackMoveLock()
    {
        _attackLocked = true;
    }
    public void OnAttackMoveUnlock()
    {
        _attackLocked = false;
    }
    // 피격 처리
    public override void TakeDamage(float atk)
    {
        //구르기 무적시간
        if (_rolling && _rollTimer <= RollIFrame) return;

        var (dmg, isCrit) = CalcFinalDamage(atk, armor);
        dmg = Mathf.Round(dmg * 10f) / 10f;
        hp -= dmg;

        Vector3 pos = transform.position + Vector3.up * 1.0f;
        DamageUI.Instance.Show(pos, dmg, DamageStyle.Player, isCrit);

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
        _rolling = true;
        _rollTimer = 0f;
        _anim.TrgRoll();
        _rb.velocity = new Vector2(_facing * RollForce, _rb.velocity.y);
    }
    // 점프 시작
    private void OnJump()
    {
        _anim.TrgJump();
        _grounded = false;
        _anim.SetGrounded(false);
        _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
        GroundSensor?.DisableFor(0.2f);
    }
    // 감지 & 애니메이션 동기화
    private void SenseAndAnimate()
    {
        bool nowGrounded = GroundSensor && GroundSensor.IsOn;
        if (_grounded != nowGrounded)
        {
            _grounded = nowGrounded;
            _anim.SetGrounded(_grounded);
        }

        if (_inputX > 0f)
        {
            _facing = 1; if (_sr) _sr.flipX = false;
            UpdateAttackPointSide();
        }
        else if (_inputX < 0f)
        {
            _facing = -1; if (_sr) _sr.flipX = true;
            UpdateAttackPointSide();
        }

        _anim.SetAirY(_rb.velocity.y);
        _anim.SetState(Mathf.Abs(_inputX) > Mathf.Epsilon ? 1 : 0);
    }
    // 액션 처리: 공격(홀드), 구르기/점프
    private void HandleActions()
    {
        if (_attack && !_rolling) Attack();

        if (_roll && !_rolling && _rollCooldownRemain <= 0f) OnRoll();
        if (_jump && _grounded && !_rolling) OnJump();

        if (_rolling)
        {
            _rollTimer += Time.deltaTime;
            if (_rollTimer > RollDuration)
            {
                _rolling = false;
                _rollCooldownRemain = RollCooldown;
            }
        }
        if (_rollCooldownRemain > 0f)
            _rollCooldownRemain -= Time.deltaTime;
    }
    //키입력
    private void ReadInput()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _jump = Input.GetKeyDown(KeyCode.Space);
        _roll = Input.GetKeyDown(KeyCode.LeftShift);
        _attack = Input.GetMouseButtonDown(0);
    }
    //방향전환시 검 콜라이더 위치 조정
    private void UpdateAttackPointSide()
    {
        if (!AttackPoint) return;
        var pos = _attackPointDefault;
        pos.x = Mathf.Abs(pos.x) * (_facing >= 0 ? 1 : -1);
        AttackPoint.localPosition = pos;
    }
    //피해량 계산식
    (float damage, bool isCrit) CalcFinalDamage(float atk, float targetArmor)
    {
        float effArmor = Mathf.Max(0f, targetArmor - armorPen);
        float reducMul = 100f / (100f + effArmor);
        bool  isCrit   = Random.value < crit;
        float damage   = atk * reducMul * (isCrit ? critX : 1f);
        Debug.Log(damage);
        return (damage, isCrit);
    }
}
