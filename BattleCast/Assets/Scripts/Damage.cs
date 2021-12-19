using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; 


public class Damage : NetworkBehaviour
{
    HealthManager hM=null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ennemy")
        {
            hM = collision.gameObject.GetComponent<HealthManager>();
            hM.HealthDamage(5);
        }

    }
}
