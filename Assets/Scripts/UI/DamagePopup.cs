using System.Collections;
using TMPro;
using UnityEngine;

public enum DamageStyle { Normal, Crit, Player, Enemy }

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] CanvasGroup cg;
    [SerializeField] float lifetime = 0.8f;
    [SerializeField] AnimationCurve yCurve = 
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 startPos;

    public void Init(Vector3 worldPos, float value, DamageStyle style, bool isCrit = false)
    {
        startPos = worldPos + new Vector3(Random.Range(-0.12f, 0.12f), 0.25f, 0);
        transform.position = startPos;

        var col = style switch {
            DamageStyle.Player=> new Color32(255, 255, 255, 255),
            DamageStyle.Enemy => new Color32(255,220,120,255),
            _                 => new Color32(255,255,255,255)
        };
        if (isCrit) col = new Color32(255, 0, 0, 255);

        label.text = value.ToString();
        label.color = col;
        label.enableAutoSizing = true;
        label.fontSizeMin = 10; label.fontSizeMax = isCrit ? 48 : 36;

        StartCoroutine(Co_Play());
    }

    IEnumerator Co_Play()
    {
        float t = 0f;
        Vector3 endPos = startPos + new Vector3(0, 1.0f, 0);

        while (t < lifetime)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / lifetime);

            transform.position = Vector3.LerpUnclamped(startPos, endPos, yCurve.Evaluate(u));
            cg.alpha = (u < 0.7f) ? 1f : Mathf.Lerp(1f, 0f, (u - 0.7f) / 0.3f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
