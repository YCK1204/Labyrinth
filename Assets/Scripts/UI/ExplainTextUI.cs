using UnityEngine;
using TMPro;

public class ExplainTextUI : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    void Start()
    {
        if (!textUI) return;

        if (gameObject.layer == LayerMask.NameToLayer("MonsterCollision"))
        {
            textUI.text = "공격 : LeftClick";
        }
        else
        {
            textUI.text = "상호작용 : F키";
        }
    }

    void Update()
    {
        if (!textUI) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        textUI.transform.position = screenPos;
    }
}
