using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpawnManager : MonoBehaviour
{
    [Header("Ally Prefabs")]
    public GameObject ArcherPrefab;
    public GameObject BomberPrefab;
    public GameObject KnightPrefab;

    private bool _mCanSpawnAlly;

    public void SpawnAlly(Vector3 position) //Method for responding to event
    {
        //Each Ally will have their own script
        var type = EnemyAmountHandler.GetTypeToSpawn();

        GameObject spawnedAlly = type switch
        {
            AllyType.Archer => ArcherPrefab,
            AllyType.Bomber => BomberPrefab,
            AllyType.Knight => KnightPrefab,
            _ => ArcherPrefab
        };

        var obj = Instantiate(spawnedAlly, position, Quaternion.identity);
    }
}
