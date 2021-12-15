
using Unity.Netcode;
using UnityEngine;


public class GameDataController : NetworkBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform enemySpawnPoint;

    public void StartGame()
    {
        //instanciate enemy
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<NetworkObject>().Spawn();
        //Make it move / attack
        //
    }

}