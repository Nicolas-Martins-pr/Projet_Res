
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

    [SerializeField]
    private UnityEngine.UI.Text endGameText;

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



    public void EndGame()
    {
        endGameText.gameObject.SetActive(true);

        //COMPARER LES SCORES
        endGameText.text = "Le gagnant est : " ;

        UpdateEndGameTextClientRPC(endGameText.text);
        StartCoroutine(WaitAndEndGame());
    }

    [ClientRpc]
    public void UpdateEndGameTextClientRPC(string endGameMessage)
    {

        endGameText.gameObject.SetActive(true);
        endGameText.text = endGameMessage;
    }


    IEnumerator WaitAndEndGame()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        EndGameClientRPC();

        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(2);

        Application.Quit();

    }

    [ClientRpc]
    public void EndGameClientRPC()
    {
        Application.Quit();
    }
}