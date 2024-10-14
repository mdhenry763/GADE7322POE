using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefendersController
{
    private static List<GameObject> allDefenders = new List<GameObject>();

    public static void AddDefender(GameObject defender)
    {
        allDefenders.Add(defender);
        Debug.Log($"Added Defender: {defender.name} , defender Count: {allDefenders.Count}");
    }

    public static void RemoveDefender(GameObject defender)
    {
        allDefenders.Remove(defender);
        Debug.Log($"Removed Defender: {defender.name} , defender Count: {allDefenders.Count}");
    }

    public static GameObject GetClosestDefender(Transform from)
    {
        if (allDefenders.Count == 0) return null;
        
        GameObject closestDefender = null;
        float closestDistance = float.MaxValue;

        foreach (var defender in allDefenders)
        {
            var distance = Vector3.Distance(from.position, defender.transform.position);

            if (!(distance < closestDistance)) continue;
            closestDistance = distance;
            closestDefender = defender;
        }

        return closestDefender;

    }

    public static void ClearDefenders()
    {
        allDefenders.Clear();
    }
}
