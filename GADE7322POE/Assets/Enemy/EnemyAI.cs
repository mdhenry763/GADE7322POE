using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
     public void InitializeEnemy(Dictionary<int, List<Vector3>> paths)
     {
          //Create enemy based off random path
          //Enemy create get obj then obj. init

          var index = Random.Range(0, paths.Count);
          var path = paths[index];
          
     }
}
