using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : CreatureController
{
    [Header("Stats")]
    [SerializeField] private PlayerStats PlayerStats;

    [Header("Refs")]
    [SerializeField] private SimpleSensor2D GroundSensor;
    [SerializeField] private SimpleSensor2D WallR1, WallR2, WallL1, WallL2;

    [Header("Force")]
    [SerializeField] private float JumpForce = 7.5f;
    [SerializeField] private float RollForce = 6f;
    [SerializeField] private float RollDuration = 8f / 14f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Playeranimator anim;
    private ComboController combo;

    private bool grounded, rolling, wallSlide;
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

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = new Playeranimator(GetComponent<Animator>());
        rb.freezeRotation = true;

        float baseMinGap = 0.5f;
        float minGap = baseMinGap / Mathf.Max(0.01f, atkSpeed);
        float resetGap = 1.0f;
        combo = new ComboController(minGap, resetGap);
    }

    private void Update()
    {
        ReadInput();
        combo.Tick(Time.deltaTime);
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
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
    }
    // 공격 처리
    protected override void Attack()
    {
        if (combo.TryNext(out var step))
        {
            anim.TrgAttack(step);
        }
    }
    // 피격 처리
    protected override void TakeDamage(float dmg)
    {
        float effective = Mathf.Max(0f, dmg - Mathf.Max(0f, armor - armorPen));
        hp -= effective;

        if (hp <= 0f) { hp = 0f; OnDied(); }
        else anim.TrgHurt();
    }
    // 사망 처리
    protected override void OnDied()
    {
        anim.TrgDeath();
        enabled = false;
    }
    // 구르기 시작
    private void OnRoll()
    {
        rolling = true;
        rollTimer = 0f;
        anim.TrgRoll();
        rb.velocity = new Vector2(facing * RollForce, rb.velocity.y);
    }
    // 점프 시작
    private void OnJump()
    {
        anim.TrgJump();
        grounded = false;
        anim.SetGrounded(false);
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        GroundSensor?.DisableFor(0.2f);
    }
     // 감지 & 애니메이션 동기화
    private void SenseAndAnimate()
    {
        bool onR = WallR1 && WallR2 && WallR1.IsOn && WallR2.IsOn;
        bool onL = WallL1 && WallL2 && WallL1.IsOn && WallL2.IsOn;
        wallSlide = onR || onL;
        anim.SetWall(wallSlide);

        bool nowGrounded = GroundSensor && GroundSensor.IsOn;
        if (grounded != nowGrounded)
        {
            grounded = nowGrounded;
            anim.SetGrounded(grounded);
        }

        if (inputX > 0f)
        {
            facing = 1; if (sr) sr.flipX = false;
        }
        else if (inputX < 0f)
        {
            facing = -1; if (sr) sr.flipX = true;
        }

        anim.SetAirY(rb.velocity.y);
        anim.SetState(Mathf.Abs(inputX) > Mathf.Epsilon ? 1 : 0);
    }
      // 액션 처리: 공격(홀드), 막기 토글, 구르기/점프
    private void HandleActions()
    {
        if (attackHold && !rolling)
            Attack();

        if (blockDown && !rolling)
        {
            anim.TrgBlock(); anim.SetBlock(true);
        }
        if (blockUp)
        {
            anim.SetBlock(false);
        }

        if (roll && !rolling && !wallSlide) OnRoll();
        if (jump && grounded && !rolling)   OnJump();

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
}
