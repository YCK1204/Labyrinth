using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeData : MonsterData
{
    [SerializeField]
    private float jumpForce;
    public float JumpForce { get { return jumpForce; } }
}
