using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private bool movingRight;

    [SerializeField]
    private Transform groundCheckPos;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * walkSpeed);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos.position, Vector2.down, 2f);

        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            } else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

}
