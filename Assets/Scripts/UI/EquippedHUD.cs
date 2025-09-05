using UnityEngine;
using UnityEngine.UI;

public class EquippedHUD : MonoBehaviour
{
    [SerializeField] Image weaponIcon;
    [SerializeField] Image armorIcon;
    void OnEnable()
    {
        var equip = FindObjectOfType<PlayerEquipment>();
        if (equip) Refresh(equip);
    }
    public void Refresh(PlayerEquipment equip)
    {
        if (!equip) return;

        // 무기
        if (weaponIcon)
        {
            if (equip.weapon && equip.weapon.icon)
            {
                weaponIcon.sprite = equip.weapon.icon;
                SetAlpha(weaponIcon, 1f);
            }
            else
            {
                SetAlpha(weaponIcon, 0f);
            }
        }

        // 방어구
        if (armorIcon)
        {
            if (equip.armor && equip.armor.icon)
            {
                armorIcon.sprite = equip.armor.icon;
                SetAlpha(armorIcon, 1f);
            }
            else
            {
                SetAlpha(armorIcon, 0f);
            }
        }
    }

    void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }
}
