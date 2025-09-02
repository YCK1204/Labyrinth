using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonsterController
{
    SlimeData _slimeData;
    float _jumpForce;
    protected override Vector2 GenRandomPosition()
    {
        return Vector2.down;
    }
    protected override void UpdateAnimation()
    {
    }
    protected override void UpdateController()
    {
    }
    protected override void Init()
    {
        base.Init();
        _slimeData = monsterData as SlimeData;
        _jumpForce = _slimeData.JumpForce;
        //_directions = _slimeData.Directions;
    }
}
