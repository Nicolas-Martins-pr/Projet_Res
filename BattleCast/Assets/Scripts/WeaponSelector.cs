using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



public class WeaponSelector : NetworkBehaviour
{

    string weaponCollide;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            UpdateServer();
        }

    }
    private void UpdateServer()
    {
        if(weaponCollide!=null){
            Debug.Log("serv");
            GameObject weap = GameObject.Find(weaponCollide);
            weap.transform.SetParent(this.transform, this.transform);
            weap.GetComponent<BoxCollider2D>().enabled = false;
            
            weaponCollide = null; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision0");
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log("collision");
            UpdateClientWeaponServerRPC(collision.gameObject.name);
        }
        
    }
    [ServerRpc]
    public void UpdateClientWeaponServerRPC(string collide)
    {
        Debug.Log("rpc");
        weaponCollide = collide;
    }
}


