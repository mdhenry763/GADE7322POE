using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
     [Header("Wave Controller")] 
     public EnemyWaveController waveController;
     
     [Header("Goblin Spawn")] 
     public EnemyData[] enemies;
     public float spawnInterval = 5;
     public float spawnIntervalDecreaseOverTime = 1f;
     public float goblinSpeed = 1;
     public Vector3 offset = new Vector3(0, 0.5f, 0);
     
     [Header("Enemy Path")]
     public SplineContainer splineContainer;
     public NavMeshSurface surface;

     private Spline spline;
     private List<Vector3> path;
     private int _mNumEnemiesSpawned;
     private bool _isSpawning;

     private void OnEnable()
     {
          EnemyWaveController.OnWaveIncreased += HandleWaveIncrease;
     }

     private void HandleWaveIncrease(int obj)
     {
          _isSpawning = false;
          StartCoroutine(DelaySpawnAfterWave());
     }

     public void SpawnEnemies(Dictionary<int, List<Vector3>> paths)
     {
          //Spawn Trails and GetNavMesh for when enemy off path
          splineContainer.GetComponent<SplineInstantiate>();
          surface.BuildNavMesh();

          _isSpawning = true;
          StartCoroutine(SpawnEnemiesOnInterval(paths)); //Spawn interval
     }

     /// <summary>
     /// Spawn enemies on the paths based on the interval timer
     /// </summary>
     /// <param name="paths"></param>
     /// <returns></returns>
     IEnumerator SpawnEnemiesOnInterval(Dictionary<int, List<Vector3>> paths)
     {
          while (true)
          {
               if (_isSpawning)
               {
                    var index = Random.Range(0, paths.Count);
                    path = paths[index];
               
                    var enemyType = waveController.GetRandomEnemy();
                    var goblinPrefab = enemies.FirstOrDefault((enemy) => enemy.EnemyType == enemyType).Prefab;

                    if (goblinPrefab == null)
                    {
                         goblinPrefab = enemies[0].Prefab;
                    }
                    
                    //Spawn enemy on random path
                    var enemyObj = Instantiate(goblinPrefab, this.transform);
                    var enemyController = enemyObj.GetComponent<GeneralEnemyController>();
                    //Initialise enemy
                    enemyController.InitEnemy(path, offset, goblinSpeed);

                    _mNumEnemiesSpawned++;
                    DecreaseInterval();
                    yield return new WaitForSeconds(spawnInterval);
               }
               else
               {
                    yield return null;
               }
               
          }
     }

     IEnumerator DelaySpawnAfterWave()
     {
          Debug.Log("New Wave");
          yield return new WaitForSeconds(3f);
          _isSpawning = true;
          Debug.Log("Start Wave");
     }
     
     private void DecreaseInterval()
     {
          if (_mNumEnemiesSpawned % 15 != 0) return;
          
          if (spawnInterval > 2)
          { 
               spawnInterval--;
          }
     }
     
}

[Serializable]
public struct EnemyData
{
     public EEnemy EnemyType;
     public GameObject Prefab;
}

public enum EEnemy
{
     Goblin,
     Dragon,
     Golem,
}
