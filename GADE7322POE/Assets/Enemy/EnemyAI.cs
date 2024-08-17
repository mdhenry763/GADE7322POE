using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Splines;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{
     [Header("Goblin")] 
     public GameObject goblinPrefab;
     public float speed = 1;
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

          var enemy = Instantiate(goblinPrefab, this.transform);
          StartCoroutine(FollowPath(enemy));

          //spline = splineContainer.Spline;
          var splineIn = splineContainer.GetComponent<SplineInstantiate>();
          //BakeSplinePath();
          
     }
     
     /*private void BakeSplinePath()
     {
          var spline = splineContainer.Spline;
          var splineInstantiate = splineContainer.GetComponent<SplineInstantiate>();

          // Specify the interval for instantiation (e.g., every 0.1 units of spline length)
          float interval = 0.1f;

          // Loop through the spline and instantiate items at regular intervals
          for (float t = 0; t <= spline.GetLength(); t += interval)
          {
               Vector3 position = spline.EvaluatePosition(t / spline.GetLength());

               foreach (var instantiableItem in splineInstantiate.itemsToInstantiate)
               {
                    var obj = Instantiate(instantiableItem.Prefab, position, Quaternion.identity);
               }
          }

          // Optional: Adjust the NavMeshSurface bounds if necessary
          surface.transform.position = splineContainer.transform.position;
          surface.transform.rotation = splineContainer.transform.rotation;
     }*/

     IEnumerator FollowPath(GameObject enemy)
     {
          yield return new WaitForSeconds(0.5f);
          enemy.transform.position = path[0];

          for (int i = 1; i < path.Count; i++)
          {
               var currentPosition = path[i - 1];
               var nextPosition = path[i];

               float elapsedTime = 0f;
               float journeyLength = Vector3.Distance(currentPosition, nextPosition);

               while (elapsedTime < journeyLength / speed)
               {
                    // Calculate the interpolation factor (t)
                    float t = elapsedTime * speed / journeyLength;

                    // Interpolate position
                    enemy.transform.position = Vector3.Lerp(currentPosition, nextPosition, t);

                    // Increase the elapsed time
                    elapsedTime += Time.deltaTime;

                    yield return null; // Wait for the next frame
               }

               // Ensure the enemy reaches the exact position
               enemy.transform.position = nextPosition;
          }
     }

}
