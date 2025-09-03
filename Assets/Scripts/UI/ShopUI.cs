using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerSO;
    [SerializeField] private GameObject shopPanel;
    
    [Header("List UI")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private List<EquipmentData> shopItems = new();
    [SerializeField] private TMP_Text playerGoldText;
    PlayerEquipment equipRef;


    [Header("Confirm UI")]
    [SerializeField] private GameObject buySellPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject cantBuyPanel;
    [SerializeField] private Button confirmButton;

    [Header("Detail UI")]
    [SerializeField] private GameObject itemDetailPanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button closeButton;

    [Header("Detail Stats")]
    [SerializeField] private TMP_Text[] statLabels;
    [SerializeField] private TMP_Text[] statValues;

    [Header("Select / Double-Click")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Color selectionOutlineColor = new Color(0.2f, 0.6f, 1f, 1f);
    [SerializeField] private float doubleClickThreshold = 0.3f;

    readonly List<GameObject> rows = new();
    readonly Dictionary<EquipmentData, GameObject> rowMap = new();
    readonly HashSet<EquipmentData> purchased = new();
    readonly Dictionary<EquipmentData, GameObject> badgeMap = new();

    enum PendingAction { None, Buy, Sell }
    PendingAction pending = PendingAction.None;

    GameObject selectedRow;
    EquipmentData selectedItem;
    GameObject lastClickedRow;
    float lastClickTime;

    void Start()
    {
        equipRef = FindObjectOfType<PlayerEquipment>();

        Refresh();
        if (itemDetailPanel) itemDetailPanel.SetActive(false);
        if (buySellPanel) buySellPanel.SetActive(false);
        if (closeButton) closeButton.onClick.AddListener(() => itemDetailPanel.SetActive(false));
        if (buyButton) { buyButton.onClick.RemoveAllListeners(); buyButton.onClick.AddListener(() => OpenConfirm(PendingAction.Buy)); }
        if (sellButton) { sellButton.onClick.RemoveAllListeners(); sellButton.onClick.AddListener(() => OpenConfirm(PendingAction.Sell)); }
        if (equipButton) { equipButton.onClick.RemoveAllListeners(); equipButton.onClick.AddListener(ToggleEquip); }
        if (yesButton) yesButton.onClick.AddListener(OnConfirmYes);
        if (noButton) noButton.onClick.AddListener(CloseConfirm);
        if (cantBuyPanel) cantBuyPanel.SetActive(false);
        if (confirmButton) confirmButton.onClick.AddListener(() => cantBuyPanel.SetActive(false));
        if (exitButton)
        {
            exitButton.onClick.AddListener(() =>
            {
                shopPanel.SetActive(false);
                Time.timeScale = 1f;
            });
        }


        UpdateGoldUI();
        UpdateBuyButtonState();
        UpdateSellButtonState();
        UpdateEquipButtonState();

        // 장착 배지 초기 동기화
        if (equipRef) SyncBadges(equipRef);
    }


    public void Refresh()
    {
        foreach (var r in rows) Destroy(r);
        rows.Clear();
        rowMap.Clear();
        selectedRow = null;
        selectedItem = null;
        lastClickedRow = null;
        lastClickTime = 0f;
        UpdateBuyButtonState();

        foreach (var item in shopItems)
        {
            var row = Instantiate(rowPrefab, content);
            rows.Add(row);
            rowMap[item] = row;

            var images = row.GetComponentsInChildren<Image>(true);
            var texts = row.GetComponentsInChildren<TMP_Text>(true);
            var badge = row.transform.Find("Item_StateBadge");
            if (badge) badge.gameObject.SetActive(false);
            badgeMap[item] = badge ? badge.gameObject : null;
            if (images.Length > 0) images[^1].sprite = item.icon;

            if (texts.Length >= 5)
            {
                texts[1].text = item.itemName;
                texts[2].text = item.type == EquipmentData.EquipmentType.Weapon ? "무기"
                              : item.type == EquipmentData.EquipmentType.Armor ? "방어구"
                              : item.type.ToString();

                if (item.stats != null && item.stats.Length > 0)
                {
                    var st = item.stats[0].stat;
                    string stName = st == EquipmentData.StatType.Power ? "공격력"
                                   : st == EquipmentData.StatType.Armor ? "방어력"
                                   : st.ToString();
                    texts[3].text = $"{stName} {item.stats[0].value}";
                }

                texts[4].text = purchased.Contains(item) ? "구매완료" : ("Gold " + item.price);
            }

            EnsureOutline(row, false);

            var btn = row.GetComponent<Button>();
            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnRowClicked(row, item));
            }
        }
        if (equipRef) SyncBadges(equipRef);
    }

    void OnRowClicked(GameObject row, EquipmentData item)
    {
        if (selectedRow != row)
        {
            if (selectedRow)
            {
                var oldOutline = selectedRow.GetComponent<Outline>();
                if (oldOutline) oldOutline.enabled = false;
            }
            selectedRow = row;
            var outline = row.GetComponent<Outline>() ?? row.AddComponent<Outline>();
            outline.effectColor = selectionOutlineColor;
            outline.effectDistance = new Vector2(2f, -2f);
            outline.enabled = true;
        }

        selectedItem = item;

        float now = Time.unscaledTime;
        if (lastClickedRow == row && (now - lastClickTime) <= doubleClickThreshold)
            ShowDetail(item);

        lastClickedRow = row;
        lastClickTime = now;
        UpdateBuyButtonState();
        UpdateSellButtonState();
        UpdateEquipButtonState();
    }

    void EnsureOutline(GameObject row, bool enabled)
    {
        var img = row.GetComponent<Image>();
        if (!img) { img = row.AddComponent<Image>(); img.color = new Color(1, 1, 1, 0); }
        var outline = row.GetComponent<Outline>() ?? row.AddComponent<Outline>();
        outline.effectColor = selectionOutlineColor;
        outline.effectDistance = new Vector2(2f, -2f);
        outline.enabled = enabled;
    }

    void BuySelected()
    {
        if (selectedItem == null || purchased.Contains(selectedItem) || playerSO == null) return;

        if (playerSO.Gold < selectedItem.price)
        {
            if (cantBuyPanel) cantBuyPanel.SetActive(true);
            return;
        }

        playerSO.Gold -= selectedItem.price;
        UpdateGoldUI();
        purchased.Add(selectedItem);

        if (priceText) priceText.text = "구매완료";
        if (rowMap.TryGetValue(selectedItem, out var row))
        {
            var texts = row.GetComponentsInChildren<TMP_Text>(true);
            if (texts.Length >= 5) texts[4].text = "구매완료";
        }

        UpdateBuyButtonState();
        UpdateSellButtonState();
        UpdateEquipButtonState();
    }

    void SellSelected()
    {
        if (selectedItem == null || !purchased.Contains(selectedItem) || playerSO == null) return;
        if (equipRef && (equipRef.weapon == selectedItem || equipRef.armor == selectedItem)) return;

        int sellPrice = Mathf.FloorToInt(selectedItem.price * 0.7f);
        playerSO.Gold += sellPrice;
        UpdateGoldUI();

        purchased.Remove(selectedItem);

        if (priceText) priceText.text = "Gold " + selectedItem.price;
        if (rowMap.TryGetValue(selectedItem, out var row2))
        {
            var texts = row2.GetComponentsInChildren<TMP_Text>(true);
            if (texts.Length >= 5) texts[4].text = "Gold " + selectedItem.price;
        }

        UpdateBuyButtonState();
        UpdateSellButtonState();
        UpdateEquipButtonState();
    }

    void ToggleEquip()
    {
        if (selectedItem == null || !purchased.Contains(selectedItem)) return;
        if (!equipRef) return;

        if (selectedItem.type == EquipmentData.EquipmentType.Weapon)
        {
            if (equipRef.weapon == selectedItem) { equipRef.Unequip(EquipmentData.EquipmentType.Weapon); }
            else { if (equipRef.weapon) equipRef.Unequip(EquipmentData.EquipmentType.Weapon); equipRef.Equip(selectedItem); }
        }
        else if (selectedItem.type == EquipmentData.EquipmentType.Armor)
        {
            if (equipRef.armor == selectedItem) { equipRef.Unequip(EquipmentData.EquipmentType.Armor); }
            else { if (equipRef.armor) equipRef.Unequip(EquipmentData.EquipmentType.Armor); equipRef.Equip(selectedItem); }
        }

        SyncBadges(equipRef);
        UpdateEquipButtonState();
        UpdateSellButtonState();
    }



    void UpdateBuyButtonState()
    {
        if (!buyButton) return;
        buyButton.interactable = selectedItem != null && !purchased.Contains(selectedItem);
    }

    void UpdateSellButtonState()
    {
        if (!sellButton) return;
        bool isEquipped = equipRef &&
            ((equipRef.weapon == selectedItem) || (equipRef.armor == selectedItem));
        bool canSell = selectedItem != null && purchased.Contains(selectedItem) && !isEquipped;

        sellButton.interactable = canSell;
        var colors = sellButton.colors;
        colors.normalColor = canSell ? Color.white : new Color(0.7f, 0.7f, 0.7f);
        sellButton.colors = colors;
    }



    void UpdateEquipButtonState()
    {
        if (!equipButton) return;
        bool canEquip = selectedItem != null && purchased.Contains(selectedItem);
        equipButton.interactable = canEquip;
        var colors = equipButton.colors;
        colors.normalColor = canEquip ? Color.white : new Color(0.7f, 0.7f, 0.7f);
        equipButton.colors = colors;
    }

    void ShowDetail(EquipmentData item)
    {
        if (!itemDetailPanel) return;
        itemDetailPanel.SetActive(true);

        if (nameText) nameText.text = item.itemName;
        if (typeText) typeText.text =
            item.type == EquipmentData.EquipmentType.Weapon ? "무기" :
            item.type == EquipmentData.EquipmentType.Armor ? "방어구" : item.type.ToString();

        for (int i = 0; i < statLabels.Length; i++)
        {
            statLabels[i].text = "-";
            statValues[i].text = "-";
        }

        if (item.stats != null)
        {
            for (int i = 0; i < item.stats.Length && i < statLabels.Length; i++)
            {
                var sv = item.stats[i];
                string label =
                    sv.stat == EquipmentData.StatType.Power ? "공격력" :
                    sv.stat == EquipmentData.StatType.AtkSpeed ? "공격속도" :
                    sv.stat == EquipmentData.StatType.ArmorPen ? "방어관통" :
                    sv.stat == EquipmentData.StatType.Crit ? "치명타확률" :
                    sv.stat == EquipmentData.StatType.CritX ? "치명타배수" :
                    sv.stat == EquipmentData.StatType.Armor ? "방어력" :
                    sv.stat == EquipmentData.StatType.Hp ? "체력" :
                    sv.stat == EquipmentData.StatType.Energy ? "에너지" :
                    sv.stat == EquipmentData.StatType.Speed ? "이동속도" :
                    sv.stat == EquipmentData.StatType.KbResist ? "넉백저항" :
                    sv.stat.ToString();

                statLabels[i].text = label + " : ";
                statValues[i].text = sv.value.ToString();
            }
        }

        if (priceText) priceText.text = purchased.Contains(item) ? "구매완료" : item.price.ToString();
        UpdateBuyButtonState();
    }

    void OpenConfirm(PendingAction action)
    {
        if (selectedItem == null) return;
        pending = action;
        if (buySellPanel) buySellPanel.SetActive(true);
    }

    void CloseConfirm()
    {
        pending = PendingAction.None;
        if (buySellPanel) buySellPanel.SetActive(false);
    }

    void OnConfirmYes()
    {
        if (pending == PendingAction.Buy) BuySelected();
        else if (pending == PendingAction.Sell) SellSelected();
        CloseConfirm();
    }

    void UpdateGoldUI()
    {
        if (playerGoldText && playerSO)
            playerGoldText.text = $"보유 골드 : {playerSO.Gold}";
    }

    void SyncBadges(PlayerEquipment equip)
    {
        foreach (var kv in badgeMap)
        {
            var item = kv.Key;
            var badgeGO = kv.Value;
            if (!badgeGO) continue;
            bool equipped = (equip.weapon == item) || (equip.armor == item);
            badgeGO.SetActive(equipped);
        }
    }
}
