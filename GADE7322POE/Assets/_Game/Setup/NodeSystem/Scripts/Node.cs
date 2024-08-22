using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public Vector3 position;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool isWalkable, Vector3 position)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
        this.position = position;
    }

}