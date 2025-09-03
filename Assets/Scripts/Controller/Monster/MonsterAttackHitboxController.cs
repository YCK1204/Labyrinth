using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackHitboxController : MonoBehaviour
{
    float _radius;
    LayerMask _layer;
    public void Init(float radius, Transform parent, Vector2 pos, LayerMask layer)
    {
        transform.SetParent(parent);
        transform.localPosition = pos / parent.localScale.x;
        _radius = radius;
        _layer = layer;
    }
    public virtual Collider2D Check()
    {
        return Physics2D.OverlapCircle(transform.position, _radius, _layer);
    }
}
