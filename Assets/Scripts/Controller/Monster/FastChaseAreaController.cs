using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FastChaseAreaController : MonoBehaviour
{
    Action<Collider2D> _onTriggerEnterCallback;
    Action<Collider2D> _onTriggerExitCallback;
    LayerMask _targetLayer;
    public void Init(UnityEngine.Transform tf, float yTop, float yBottom, float xSize, LayerMask targetLayer)
    {
        transform.parent = tf;
        transform.localPosition = Vector2.zero;

        var fastChaseDetectionCollider = gameObject.AddComponent<BoxCollider2D>();
        fastChaseDetectionCollider.isTrigger = true;

        float height = yTop - yBottom;
        fastChaseDetectionCollider.size = new Vector2(xSize / tf.localScale.x, height / tf.localScale.y);
        fastChaseDetectionCollider.offset = new Vector2(0, ((yTop + yBottom) / 2 - tf.position.y) / tf.localScale.y);

        _targetLayer = targetLayer;

        transform.localScale = Vector3.one;
    }
    public void SetCallback(Action<Collider2D> enterCallback, Action<Collider2D> exitCallback)
    {

        _onTriggerEnterCallback = enterCallback;
        _onTriggerExitCallback = exitCallback;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _targetLayer)
        {
            _onTriggerEnterCallback.Invoke(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _targetLayer)
        {
            _onTriggerExitCallback.Invoke(collision);
        }
    }
}
