using Unity.Netcode;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{

    [SerializeField]
    private float walkSpeed = 3.5f;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4,4);

    [SerializeField]
    private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();

    // client caching
    private float oldLeftRightPosition;

    private void Start()
    {
        transform.position = new Vector2(Random.Range(defaultPositionRange.x, defaultPositionRange.y),Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

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
        transform.position = new Vector2(transform.position.x + leftRightPosition.Value,
            transform.position.y);
    }

    private void UpdateClient()
    {
        float leftRight = 0;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            //TODO JUMP ?
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRight -= walkSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRight += walkSpeed;
        }

        if (oldLeftRightPosition != leftRight)
        {
            oldLeftRightPosition = leftRight;
            //update the server
            UpdateClientPositionServerRPC(leftRight);
        }

    }

    [ServerRpc]
    public void UpdateClientPositionServerRPC(float leftRight)
    {
        leftRightPosition.Value = leftRight;
    }
}
