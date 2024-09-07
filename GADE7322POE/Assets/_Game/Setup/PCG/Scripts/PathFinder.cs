using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PathFinder
{
    private GridManager _gridManager;
    private Node _startNode;
    private Node _destinationNode;
    private Node _currentSearchNode;

    private Queue<Node> frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down, };
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public static event Action<List<Vector3>> onPathGenerated;

    public PathFinder(GridManager gridManager, Node startNode, Node endNode)
    {
        _gridManager = gridManager;
        _startNode = startNode;
        _destinationNode = endNode;

        grid = gridManager.Grid;
    }

    /// <summary>
    /// Check for different nodes in all direction up, right, down, left
    /// </summary>
    void ExploreNeighbours()
    {
        //Initialize neighbours
        List<Node> neighbours = new List<Node>();
    
        //Get neighbours
        foreach (var direction in directions)
        {
            var coords = _currentSearchNode.coordinates + direction;
            if (grid.TryGetValue(coords, out var value))
            {
                neighbours.Add(value);
            }
        }

        foreach (var neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                //Set node connections for BFS
                neighbour.connectedTo = _currentSearchNode;
                reached.Add(neighbour.coordinates, neighbour);
                frontier.Enqueue(neighbour);
            }
        }
    }

    /// <summary>
    /// handles path creation and gives enemies a path to use
    /// </summary>
    public void BreadthFirstSearch()
    {
        _startNode.isWalkable = true;
        _destinationNode.isWalkable = true;
    
        bool isRunning = true;
        //Node start
        frontier.Enqueue(_startNode);
        reached.Add(_startNode.coordinates, _startNode);

        //Create path
        while (frontier.Count > 0 && isRunning)
        {
            _currentSearchNode = frontier.Dequeue();
            _currentSearchNode.isExplored = true;
            
            ExploreNeighbours();
            
            if (_currentSearchNode.coordinates == _destinationNode.coordinates)
            {
                isRunning = false;
                
                //Link destination node if node not set
                if (_destinationNode.connectedTo == null)
                {
                    _destinationNode.connectedTo = _currentSearchNode.connectedTo;
                }
            }
        }
    }

    /// <summary>
    ///  return a list of nodes that contains the path
    /// </summary>
    /// <returns></returns>
    public List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = _destinationNode;

        if (currentNode == null) return path;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        //Reverse generated path, end node is first node
        path.Reverse();

        //Get the position of each node
        var pathPositions = path.Select(node => node.position).ToList();

        onPathGenerated?.Invoke(pathPositions);
        return path;
    }

    
}
