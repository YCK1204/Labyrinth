using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class MonsterStatInitializer : MonoBehaviour
{
    [SerializeField] private CreatureData creatureData;
    [SerializeField] private bool isBoss = false;

    private MonsterController _controller;

    void Awake()
    {
        _controller = GetComponent<MonsterController>();
        if (creatureData == null)
        {
            Debug.LogError($"[{name}] MonsterStatInitializer: CreatureData가 연결되지 않았습니다!");
            return;
        }

        PlayerData pd = Manager.Game != null ? Manager.Game.PlayerData : null;

        HardModeStat.ApplyAll(
            pd, isBoss, creatureData,
            out _controller.hp,
            out _controller.power,
            out _controller.atkSpeed,
            out _controller.armor,
            out _controller.crit,
            out _controller.speed,
            out _controller.kbResist,
            out _controller.critX,
            out _controller.armorPen
        );
    }
}
