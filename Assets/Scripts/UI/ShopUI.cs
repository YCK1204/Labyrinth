using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("List UI")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private List<EquipmentData> shopItems = new();

    [Header("Detail UI")]
    [SerializeField] private GameObject itemDetailPanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button closeButton;

    [Header("Detail Stats")]
    [SerializeField] private TMP_Text[] statLabels;
    [SerializeField] private TMP_Text[] statValues;

    readonly List<GameObject> rows = new();

    void Start()
    {
        Refresh();
        if (itemDetailPanel) itemDetailPanel.SetActive(false);
        if (closeButton) closeButton.onClick.AddListener(() => itemDetailPanel.SetActive(false));
    }

    public void Refresh()
    {
        foreach (var r in rows) Destroy(r);
        rows.Clear();

        foreach (var item in shopItems)
        {
            var row = Instantiate(rowPrefab, content);
            rows.Add(row);

            var images = row.GetComponentsInChildren<Image>(true);
            var texts = row.GetComponentsInChildren<TMP_Text>(true);
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
                texts[4].text = "Gold " + item.price;
            }

            var btn = row.GetComponent<Button>();
            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => ShowDetail(item));
            }
        }
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

        if (priceText) priceText.text = item.price.ToString();
    }
}
