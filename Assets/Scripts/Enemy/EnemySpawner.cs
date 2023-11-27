using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private float SpawnInterval = 5f;


    [SerializeField]
    private bool canSpawn = true;

    [SerializeField]
    private Transform position;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnInterval);
        while (canSpawn)
        {
            yield return wait;
            GameObject EnemyToSpawn = EnemyPrefab;

            Instantiate(EnemyToSpawn, position);
        }
    }





}
