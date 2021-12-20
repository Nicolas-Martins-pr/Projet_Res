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
        if (Input.GetKeyDown(KeyCode.M) && IsServer)
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
                    rotationVector.z = -180;
                    rotationVector.x = 0;
                    rotationVector.y = 0;
                    healthBar.gameObject.transform.rotation = Quaternion.Euler(rotationVector);
                    UpdateEnemyHPBarRotationClientRPC(rotationVector);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);

                    var rotationVector = healthBar.gameObject.transform.rotation.eulerAngles;
                    rotationVector.z = 0;
                    rotationVector.x = 0;
                    rotationVector.y = 0;
                    healthBar.gameObject.transform.rotation = Quaternion.Euler(rotationVector);
                    UpdateEnemyHPBarRotationClientRPC(rotationVector);
                    movingRight = true;
                }
            }

            //handles the slider bug
            if (currentEnemyHP.Value <= 7)
            {
                GameObject.Find("GameController").GetComponent<GameDataController>().EndGame();
            }

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
        //healthBar.gameObject.transform.rotation.Set(0, 0, newRotation,0);
        healthBar.gameObject.transform.rotation = Quaternion.Euler(newRotation);
    }


    [ClientRpc]
    public void UpdateEnemyHPClientRPC(int newHP)
    {
        healthBar.value = newHP;
    }

}
