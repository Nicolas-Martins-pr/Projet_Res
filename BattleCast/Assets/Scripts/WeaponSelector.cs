using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;




public class WeaponSelector : NetworkBehaviour
{

     string weaponCollide;
    PlayerMovement2D player = null;
    public GameObject weap = null;
    bool rightPos = false;
    bool playerFlipSave = true;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement2D>();
        
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
            
            weap=GameObject.Find(weaponCollide);
            weap.transform.SetParent(this.transform,this.transform);         
            weap.GetComponent<BoxCollider2D>().enabled = false;
            
            weaponCollide = null; 
        }
         if (weap!=null && player.flip.Value!=playerFlipSave )
        {
            weap.GetComponent<SpriteRenderer>().flipX = playerFlipSave;
            playerFlipSave = player.flip.Value;

            float direction = player.flip.Value ? 1 : -1;
            weap.transform.localPosition = new Vector2(direction * 0.2f, -0.25f);
        }
         
        //else { gameObject.transform.GetChild().gameObject.tag="Weapon"}
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


