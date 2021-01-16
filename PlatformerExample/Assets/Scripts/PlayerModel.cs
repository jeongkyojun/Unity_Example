using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float speed = 8f;
    public float jumpPower = 7f;
    public float dashSpeed = 16f;

    public float recentJumpTime = 0.0f;

    public int jumpCnt = 2;
    public int dashCnt = 4;

    public bool isGrounded = false;
    public bool isJump = false;
    public bool isDash = false;

    public bool isOnPlatform = false;
}
