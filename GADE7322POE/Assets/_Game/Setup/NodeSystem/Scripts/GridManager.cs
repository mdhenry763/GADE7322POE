using System.Collections.Generic;
using UnityEngine;

public class GridManager :MonoBehaviour
{
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid => grid;

    public void AddNodeToGrid(Vector2Int coordinates, Vector3 position)
    {
        var newNode = new Node (coordinates, true, position);
        grid.Add(coordinates, newNode);
    }

    public Node GetNode(Vector2Int coordinates)
    {
        return grid.GetValueOrDefault(coordinates);
    }
}