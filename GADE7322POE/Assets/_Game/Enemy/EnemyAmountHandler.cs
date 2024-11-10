using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyAmountHandler
{
    private static readonly List<EnemyData> _enemies = new List<EnemyData>();

    public static void AddEnemy(EnemyData enemy)
    {
        _enemies.Add(enemy);
    }

    public static void ChaseEnemy(Transform ally, float speed)
    {
        var enemyPos = GetClosestEnemy(ally).transform.position;
        ally.position = Vector3.Lerp(ally.position, enemyPos, Time.deltaTime * speed * 1.1f);
        var direction = enemyPos - ally.position;
        ally.rotation = Quaternion.LookRotation(direction);
    }

    public static List<GameObject> GetAllNearestEnemies(float radius)
    {
        List<GameObject> nearbyEnemies = new List<GameObject>();

        return nearbyEnemies;
    }

    public static void ChangeAnim(Animator anim, int hashCode)
    {
        anim.CrossFade(hashCode, 0.1f);
    }

    public static AllyType GetTypeToSpawn()
    {
        //TODO:
        //Determine which ally to spawn and then return that ally

        var numGoblins = _enemies.Count(enemy => enemy.EnemyType == EEnemy.Goblin);
        var numDragons = _enemies.Count(enemy => enemy.EnemyType == EEnemy.Dragon);
        var numGolems = _enemies.Count(enemy => enemy.EnemyType == EEnemy.Golem);

        var total = numGolems + numDragons + numGoblins;

        var percentageGoblin = numGoblins / total;
        var percentageDragons = numDragons / total;
        var percentageGolems = numGolems / total;
        
        Debug.Log($"Num Goblins: {numGoblins}, percentage Goblins: {percentageGoblin}");

        if (numGoblins >= 5 && percentageGoblin > 0.5f)
        {
            return AllyType.Bomber;
        }

        if (numDragons > 4 && percentageDragons > 0.3f)
        {
            return AllyType.Archer;
        }

        if (numGolems > 2 && percentageGoblin > 0.6f)
        {
            return AllyType.Knight;
        }
        
        
        //Default to Archer
        return AllyType.Knight;

    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if(_enemies.Count == 0) return;

        foreach (var data in _enemies.Where(data => data.Prefab == enemy))
        {
            _enemies.Remove(data);
            return;
        }
    }

    public static GameObject GetClosestEnemy(Transform from)
    {
        if (_enemies.Count == 0) return null;
   
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var data in _enemies)
        {
            var enemy = data.Prefab;
            var distance = Vector3.Distance(from.position, enemy.transform.position);

            if (!(distance < closestDistance)) continue;
            closestDistance = distance;
            closestEnemy = enemy;
        }

        return closestEnemy;
    }
}

public enum AllyType
{
    Bomber,
    Archer,
    Knight
}