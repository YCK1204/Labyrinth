using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldCountText;
    [SerializeField] private TextMeshProUGUI _gemCountText;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _energyBar;
    [SerializeField] private TextMeshProUGUI _levelCountText;
    [SerializeField] private Image _levelBar;

    private PlayerDataUI _playerDataUI;

    void Start()
    {
        _playerDataUI = PlayerDataUI.Instance;
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (Manager.UI == null) return;

        _goldCountText.text = Manager.UI.Gold.ToString();
        _gemCountText.text = Manager.UI.Gem.ToString();
        _levelCountText.text = Manager.UI.Level.ToString();

        _healthBar.fillAmount = Manager.UI.CurrentHP / Manager.UI.MaxHP;
        _energyBar.fillAmount = Manager.UI.CurrentEnergy / Manager.UI.MaxEnergy;
        _levelBar.fillAmount = Manager.UI.CurrentEXP / Manager.UI.MaxEXP;
    }
}
