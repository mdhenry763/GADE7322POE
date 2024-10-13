using System;
using System.Collections;
using System.Collections.Generic;
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
     [Header("Goblin Spawn")] 
     public EnemyData[] enemies;
     public GameObject goblinPrefab;
     public float spawnInterval = 5;
     public float spawnIntervalDecreaseOverTime = 1f;
     public float goblinSpeed = 1;
     public Vector3 offset = new Vector3(0, 0.5f, 0);
     
     [Header("Enemy Path")]
     public SplineContainer splineContainer;
     public NavMeshSurface surface;

     private Spline spline;

     private List<Vector3> path;
     
     public void SpawnEnemies(Dictionary<int, List<Vector3>> paths)
     {
          //Spawn Trails and GetNavMesh for when enemy off path
          splineContainer.GetComponent<SplineInstantiate>();
          surface.BuildNavMesh();

          StartCoroutine(SpawnEnemiesOnInterval(paths)); //Spawn interval
          StartCoroutine(SpawnIntervalDecrease());
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
               var index = Random.Range(0, paths.Count);
               path = paths[index];

               //Spawn enemy on random path
               var enemyObj = Instantiate(goblinPrefab, this.transform);
               var enemyController = enemyObj.GetComponent<GeneralEnemyController>();
               //Initialise enemy
               enemyController.InitEnemy(path, offset, goblinSpeed);
               
               yield return new WaitForSeconds(spawnInterval);
          }
     }

     IEnumerator SpawnIntervalDecrease() //Over timer increase the spawn rate
     {
          yield return new WaitForSeconds(30f);

          if (spawnInterval > 1)
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
