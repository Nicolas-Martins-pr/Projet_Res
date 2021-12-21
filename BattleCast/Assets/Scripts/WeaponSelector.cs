using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;




public class WeaponSelector : NetworkBehaviour
{

    string weaponCollide;
    PlayerMovement2D player = null;
    public GameObject weap = null;
    bool playerFlipSave = true;
    NetworkVariable<float> pos;
    NetworkVariable<Vector3> mPose = new NetworkVariable<Vector3>();
    Vector3 mouse_position;
    float angle_souris;
    NetworkVariable<bool> has_shot= new NetworkVariable<bool>(false);
    [SerializeField] GameObject douille;
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
        if (IsClient)
        {
            UpdateClient();
        }

    }
    private void UpdateServer()
    {
        if (weaponCollide != null)
        {

            if (weap != null)
            {
                Destroy(weap.gameObject);
                Debug.Log(weap);
            }

            weap = GameObject.Find(weaponCollide);

            UpdateClientWeaponClientRPC(weaponCollide);

            weap.transform.SetParent(this.transform, this.transform);
            weap.GetComponent<BoxCollider2D>().enabled = false;

            weaponCollide = null;
        }
        if (weap != null && player.flip.Value != playerFlipSave)
        {
            weap.GetComponent<SpriteRenderer>().flipX = playerFlipSave;
            UpdateWeaponFlipClientRPC(playerFlipSave);
            playerFlipSave = player.flip.Value;
            float direction = player.flip.Value ? 1 : -1;
            weap.transform.localPosition = new Vector2(direction * 0.2f, -0.25f);
            
        }
        if (weap != null && pos != null) {
            
            weap.transform.rotation = Quaternion.Euler(new Vector3(0F, 0F, pos.Value));
            UpdateWeaponRotateClientRPC(pos.Value);

        }
        if (has_shot.Value != false && mPose.Value!=null )
        {       
            GameObject shot;
           // float direction = player.flip.Value ? 1 : -1;
            shot = Instantiate(douille,new Vector2(weap.transform.position.x, weap.transform.position.y),weap.transform.rotation);
            shot.GetComponent<Rigidbody2D>().AddForce(mPose.Value.normalized*1000);
            UpdateClientShotClientRPC();
            has_shot.Value = false;
        }
    }
    private void UpdateClient()
    {
        mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float direction = player.flip.Value ? 1 : -1;
        angle_souris = Mathf.Atan2(direction * mouse_position.y, direction * mouse_position.x) * Mathf.Rad2Deg;
        if (angle_souris > 45)
            angle_souris = 45;
        else if (angle_souris < -45)
            angle_souris = -45;
       
        if (IsOwner)
            UpdateClientWeaponServerRPC(angle_souris);
        if (Input.GetMouseButtonDown(0) && weap != null)
        {
            UpdateClientShotServerRPC(mouse_position);   
        }
    }

    [ClientRpc]
    public void UpdateWeaponRotateClientRPC(float val )
    {
        weap.transform.rotation = Quaternion.Euler(new Vector3(0F, 0F, val));
    }
    [ClientRpc]
    public void UpdateWeaponFlipClientRPC(bool value)
    {
        weap.GetComponent<SpriteRenderer>().flipX = value;
        float direction = value ? 1 : -1;
        weap.transform.localPosition = new Vector2(direction * 0.2f, -0.25f);
    }

    [ClientRpc]
    public void UpdateClientWeaponClientRPC(string value)
    {
        weap = GameObject.Find(value);
    }


    [ServerRpc]
    public void UpdateClientShotServerRPC(Vector3 value)
    {
        has_shot.Value = true;
        mPose.Value=value;
    }
    [ClientRpc]
    public void UpdateClientShotClientRPC()
    {
        GameObject test;
        test = Instantiate(douille, new Vector2(weap.transform.position.x , weap.transform.position.y), weap.transform.rotation);
        test.GetComponent<Rigidbody2D>().AddForce(mPose.Value.normalized * 1000);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            weaponCollide = collision.gameObject.name;
        }
        
    }

    [ServerRpc]
    public void UpdateClientWeaponServerRPC(float newMouseAngle)
    {
        if (weap != null)
        {
            pos.Value = newMouseAngle;
        }
    } 
}


