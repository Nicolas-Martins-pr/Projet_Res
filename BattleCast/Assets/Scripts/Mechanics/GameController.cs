
using UnityEngine;


public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform enemySpawnPoint;

    public void StartGame()
    {
        //instanciate enemy
        Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        //Make it move / attack
        //
    }
}