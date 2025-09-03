using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldCountText;
    public TextMeshProUGUI GoldCountText
    { get => _goldCountText; set => _goldCountText = value; }

    [SerializeField] private TextMeshProUGUI _gemCountText;
    public TextMeshProUGUI GemCountText
    { get => _gemCountText; set => _gemCountText = value; }

    [SerializeField] private Image _healthBar;
    public Image HealthBar
    { get => _healthBar; set => _healthBar = value; }

    [SerializeField] private Image _energyBar;
    public Image EnergyBar
    { get => _energyBar; set => _energyBar = value; }

    [SerializeField] private TextMeshProUGUI _levelCountText;
    public TextMeshProUGUI LevelCountText
    { get => _levelCountText; set => _levelCountText = value; }

    [SerializeField] private Image _levelBar;
    public Image LevelBar
    { get => _levelBar; set => _levelBar = value; }

    private PlayerDataUI _playerDataUI;

    void Start()
    {
        _playerDataUI = PlayerDataUI.Instance;

        UpdateUI();
    }

    void Update()
    {
        
    }

    private void UpdateUI()
    {
        if (_playerDataUI == null) return;

        _goldCountText.text = _playerDataUI.Gold.ToString();
        _gemCountText.text = _playerDataUI.Gem.ToString();
        _levelCountText.text = _playerDataUI.Level.ToString();

        _healthBar.fillAmount = _playerDataUI.CurrentHP / _playerDataUI.MaxHP;
        _energyBar.fillAmount = _playerDataUI.CurrentEnergy / _playerDataUI.MaxEnergy;
        _levelBar.fillAmount = _playerDataUI.currentEXP / _playerDataUI.MaxEXP;
    }
}
