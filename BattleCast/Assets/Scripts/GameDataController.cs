
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;


public class GameDataController : NetworkBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] weaponsPrefabs;

    [SerializeField]
    private Transform enemySpawnPoint;

    [SerializeField]
    private GameObject weaponsSpawnPoints;

    private int weaponsSpawned = 0;

    private void Start()
    {
        if (IsServer && IsOwner)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();

            StartCoroutine(WeaponsSpawner());
        }
    }


    IEnumerator WeaponsSpawner()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        GameObject weapon = Instantiate(weaponsPrefabs[weaponsSpawned], weaponsSpawnPoints.transform.GetChild(weaponsSpawned).position, Quaternion.identity);
        weapon.GetComponent<NetworkObject>().Spawn();

        weaponsSpawned++;

        if (weaponsSpawned < weaponsPrefabs.Length)
        {
            StartCoroutine(WeaponsSpawner());
        }

    }
}