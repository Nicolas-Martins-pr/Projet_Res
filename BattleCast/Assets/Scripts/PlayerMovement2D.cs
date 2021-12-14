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

    private Vector3 movement;

    // client caching
    private float oldXYPosition;

    // Start is called before the first frame update
    private void Start()
    {
        transform.position = new Vector2(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0);
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


        /* Jump();
        Rotate();   
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;*/
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) < 0.001f)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpheight, ForceMode2D.Impulse);
        }
    }

    private void Rotate()
    {
        if (!Mathf.Approximately(0, movement.x))
        {
            transform.rotation = movement.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
    }



    private void UpdateServer()
    {
        transform.position = new Vector2(transform.position.x + XYPosition.Value,
            transform.position.y);
    }

    private void UpdateClient()
    {
        float XY = 0;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) < 0.001f)
        {
            //TODO JUMP ?
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            XY -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            XY += moveSpeed;
        }

        if (oldXYPosition != XY)
        {
            oldXYPosition = XY;
            //update the server
            UpdateClientPositionServerRPC(XY);
        }


    }

    [ServerRpc]
    public void UpdateClientPositionServerRPC(float leftRight)
    {
        XYPosition.Value = leftRight;
    }
}
