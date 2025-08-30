using UnityEngine;

// 2D 트리거 충돌을 감지해서 캐릭터가 땅이나 벽에 닿아있는지 판별하는 센서
public class SimpleSensor2D : MonoBehaviour
{
    private int _contactCount;
    private float _disableTimer;
    public bool IsOn => _disableTimer <= 0f && _contactCount > 0;

    void OnEnable()
    {
        _contactCount = 0;
    }
    void Update()
    {
        if (_disableTimer > 0f) _disableTimer -= Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D _)
    {
        _contactCount++;
    }
    void OnTriggerExit2D(Collider2D _) {
        _contactCount--;
    }
    public void DisableFor(float sec)
    {
        _disableTimer = sec;
    }
}
