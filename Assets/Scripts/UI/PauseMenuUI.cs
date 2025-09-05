using TMPro;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Stat Texts")]
    [SerializeField] private TextMeshProUGUI atkCountText;
    [SerializeField] private TextMeshProUGUI atkSpdCountText;
    [SerializeField] private TextMeshProUGUI defCountText;
    [SerializeField] private TextMeshProUGUI hpCountText;

    [Header("Refs")]
    [SerializeField] private PlayerEquipment playerEquip;
    [SerializeField] private PlayerData playerSO;

    private void Awake()
    {
        if (!playerEquip)
        {
            playerEquip = FindObjectOfType<PlayerEquipment>();
        }
    }

    private void OnEnable()
    {
        UpdatePlayerStats();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnResumeButtonClicked() => Manager.UI.HidePauseMenuUI();
    public void OnSettingButtonClicked() => Manager.UI.ShowSettingUI();
    public void OnExitButtonClicked() => Manager.UI.ShowExitUI();

    private void UpdatePlayerStats()
    {
        if (!atkCountText || !atkSpdCountText || !defCountText || !hpCountText)
        {
            Debug.LogError("PauseMenuUI Text references X");
            return;
        }
        if (!playerEquip)
        {
            Debug.LogError("playerEquip X");
            return;
        }

        playerEquip.Recalculate();

        atkCountText.SetText(playerEquip.Power.ToString("0.0"));
        atkSpdCountText.SetText(playerEquip.AtkSpeed.ToString("0.00"));
        defCountText.SetText(playerEquip.Armor.ToString("0.0"));
        hpCountText.SetText(playerEquip.Hp.ToString("0.0"));
    }
}
