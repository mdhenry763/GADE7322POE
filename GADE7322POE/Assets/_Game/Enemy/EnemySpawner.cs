using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
     [Header("Goblin Spawn")] 
     public GameObject goblinPrefab;
     public float spawnInterval = 5;
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

          StartCoroutine(SpawnEnemiesOnInterval(paths));
     }

     IEnumerator SpawnEnemiesOnInterval(Dictionary<int, List<Vector3>> paths)
     {
          while (true)
          {
               var index = Random.Range(0, paths.Count);
               path = paths[index];

               var enemyObj = Instantiate(goblinPrefab, this.transform);
               var enemyController = enemyObj.GetComponent<EnemyController>();
               enemyController.InitEnemy(path, offset, goblinSpeed);
               
               yield return new WaitForSeconds(spawnInterval);
          }
     }
     

}
