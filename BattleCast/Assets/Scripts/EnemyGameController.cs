using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGameController : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private bool movingRight;

    [SerializeField]
    private int enemyMaxHP;

    private NetworkVariable<int> currentEnemyHP;

    [SerializeField]
    private Transform groundCheckPos;

    [SerializeField]
    private Slider healthBar;


    private void Start()
    {
        if (IsServer)
        {
            currentEnemyHP.Value = enemyMaxHP;
            healthBar.minValue = 0;
            healthBar.maxValue = currentEnemyHP.Value;
            healthBar.value = currentEnemyHP.Value;
            UpdateEnemyHPClientRPC(currentEnemyHP.Value);
        }       
    }

    // Update is called once per frame // Server Side
    void Update()
    {
        //TO remove
        if (Input.GetKeyDown(KeyCode.D))
            DamageEnemy(10);

        if (IsServer)
        {
            transform.Translate(Vector2.right * Time.deltaTime * walkSpeed);

            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos.position, Vector2.down, 2f);

            if (groundInfo.collider == false)
            {
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);

                    var rotationVector = healthBar.gameObject.transform.rotation.eulerAngles;
                    rotationVector.z = 180;
                    UpdateEnemyHPBarRotationClientRPC(rotationVector);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);

                    var rotationVector = healthBar.gameObject.transform.rotation.eulerAngles;
                    rotationVector.z = 0;
                    UpdateEnemyHPBarRotationClientRPC(rotationVector);
                    movingRight = true;
                }
            }
        }


        //handles the slider bug
        if (currentEnemyHP.Value <= 7)
        {
            EndGame();
        }
    }
    public void DamageEnemy(int damageTaken)
    {

        if (IsServer)
        {
            currentEnemyHP.Value -= damageTaken;
            healthBar.value = currentEnemyHP.Value;
            UpdateEnemyHPClientRPC(currentEnemyHP.Value);
        }

    }


    [ClientRpc]
    public void UpdateEnemyHPBarRotationClientRPC(Vector3 newRotation)
    {
        healthBar.gameObject.transform.rotation = Quaternion.Euler(newRotation);
    }


    [ClientRpc]
    public void UpdateEnemyHPClientRPC(int newHP)
    {
        healthBar.value = newHP;
    }

    private void EndGame()
    {
        //TODO
        //endgame here
    }
}
