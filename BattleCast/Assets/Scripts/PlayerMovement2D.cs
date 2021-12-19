using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement2D : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.01f;

    [SerializeField]
    private float jumpheight = 5f;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-2, 2);


    [SerializeField]
    private NetworkVariable<float> XYPosition = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<bool> isJumping = new NetworkVariable<bool>();

    private Vector3 movement;

    public NetworkVariable<bool> flip=new NetworkVariable<bool>(false);

    WeaponSelector weapS = null;
    GameObject weapO = null;
    bool weapTrue = false;

    // client caching
    private float oldXYPosition;

    // Start is called before the first frame update
   
    private void Start()
    {
        transform.position = new Vector2(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0);
        weapS = this.GetComponent<WeaponSelector>();
      

    }

    // Update is called once per frame
    private void Update()
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
        if(weapS.weap!=null && weapTrue == false)
        {
            weapO = weapS.weap;
            weapTrue = true;
        }

        if (XYPosition.Value < 0 && flip.Value || XYPosition.Value > 0 && !flip.Value)
        {
            transform.GetComponent<SpriteRenderer>().flipX = flip.Value;
            if (weapO != null)
                weapO.transform.GetComponent<SpriteRenderer>().flipX = flip.Value;
            flip.Value = !flip.Value;
        }
        transform.position = new Vector2(transform.position.x + XYPosition.Value, transform.position.y);

        // Debug.Log(isJumping.Value);
        if (isJumping.Value)
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpheight, ForceMode2D.Impulse);

    
    }

    private void UpdateClient()
    {
        float XY = 0;
        bool jumping = false;

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) < 0.001f)
        {
            jumping = true;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            XY -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            XY += moveSpeed;
        }

        if (IsOwner)
        {
            //update the server
            UpdateClientPositionServerRPC(XY, jumping);
        }

    }


    [ServerRpc(RequireOwnership = true)]
    public void UpdateClientPositionServerRPC(float leftRight, bool jumping)
    {
        XYPosition.Value = leftRight;
        isJumping.Value = jumping;

    }
}
