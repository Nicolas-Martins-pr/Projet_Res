using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Damage : NetworkBehaviour
{
    NetworkVariable<bool> collide=new NetworkVariable<bool>(false);
    NetworkVariable<int> enTag = new NetworkVariable<int>();
    GameObject coll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
        void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ennemy")
        {
            EnemyGameController en = collision.gameObject.GetComponent<EnemyGameController>();
            en.DamageEnemy(5);
            Destroy(gameObject);

        }
        else if (collision.gameObject.tag == "Ground")
            Destroy(gameObject);

    }

    
}
