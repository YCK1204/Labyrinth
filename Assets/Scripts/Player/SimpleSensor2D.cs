using UnityEngine;

public class SimpleSensor2D : MonoBehaviour
{
    [Header("Ground Ray Settings")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength = 0.08f;
    [SerializeField] private float rayWidth = 0.4f;
    [SerializeField] private int rayCount = 3;

    private float _disableTimer;
    private bool _fullyGrounded;
    public bool IsOn => _disableTimer <= 0f && _fullyGrounded;

    private void OnEnable()
    {
        _disableTimer = 0f;
        _fullyGrounded = false;
    }

    private void FixedUpdate()
    {
        if (_disableTimer > 0f) _disableTimer -= Time.fixedDeltaTime;

        Vector2 basePos = transform.position;
        bool allHit = true;

        for (int i = 0; i < Mathf.Max(1, rayCount); i++)
        {
            float t = (rayCount <= 1) ? 0f : (i / (float)(rayCount - 1) - 0.5f);
            Vector2 origin = basePos + new Vector2(t * rayWidth, 0f);

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, groundMask);

            if (!hit) allHit = false;
        }

        _fullyGrounded = allHit;
    }
    public void DisableFor(float sec)
    {
        _disableTimer = sec;
    }

}
