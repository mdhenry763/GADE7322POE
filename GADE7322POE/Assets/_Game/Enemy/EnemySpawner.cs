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
     [Header("Goblin")] 
     public GameObject goblinPrefab;
     public float goblinSpeed = 1;
     public Vector3 offset = new Vector3(0, 0.5f, 0);
     public SplineContainer splineContainer;
     public NavMeshSurface surface;

     private Spline spline;

     private List<Vector3> path;
     
     public void InitializeEnemy(Dictionary<int, List<Vector3>> paths)
     {
          //Create enemy based off random path
          //Enemy create get obj then obj. init

          var index = Random.Range(0, paths.Count);
          path = paths[index];

          var enemyObj = Instantiate(goblinPrefab, this.transform);
          var enemyController = enemyObj.GetComponent<EnemyController>();
          enemyController.InitEnemy(path, offset, goblinSpeed);
          
          var splineIn = splineContainer.GetComponent<SplineInstantiate>();
          surface.BuildNavMesh();
          
     }
     

     

}
