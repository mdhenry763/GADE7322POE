using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Node currentSearchNode;
    [SerializeField] private GridManager gridManager;
    
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down, };
    private Dictionary<Vector2Int, Node> grid;
    
    public void PathFind()
    {
        grid = gridManager.Grid;
        ExploreNeighbours();
    }

    void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();
        
        foreach (var direction in directions)
        {
            var coords = currentSearchNode.coordinates + direction;
            if (grid.TryGetValue(coords, out var value))
            {
                neighbours.Add(value);
                //Remove after testing
                Debug.Log($"I am a neighbour with {coords}");
            }
        }
    }

    
}
