using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance { get; private set; }

    [SerializeField] DamagePopup popupPrefab;
    [SerializeField] Transform worldRoot;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        
        Instance = this;
        if (!worldRoot) worldRoot = transform;
    }

    public void Show(Vector3 worldPos, float amount, DamageStyle style, bool isCrit = false)
    {
        var pop = Instantiate(popupPrefab, worldRoot);
        pop.Init(worldPos, amount, style, isCrit);
    }
}
